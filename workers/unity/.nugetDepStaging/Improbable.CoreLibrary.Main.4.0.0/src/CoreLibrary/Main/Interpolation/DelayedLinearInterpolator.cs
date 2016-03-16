namespace Improbable.Corelib.Interpolation
{
    /// <summary>
    ///     <para>
    ///         Interpolates values of type <see cref="TValue" /> based on the <see cref="CurrentTime" /> and
    ///         the times of pending values.
    ///     </para>
    ///     <para>
    ///         Each pending value has an associated timestamp (the target time). The timespan between the target time and the
    ///         <see cref="CurrentTime" />
    ///         should ideally equal to <see cref="InterpolationDelaySeconds" />. However, due to network errors, message-queue buffering, and other
    ///         sources of clock drifting this timespan can grow. We use rubber-banding to decrease the adverse
    ///         effects of clock drifting. Network lag, however, has to be estimated and included in the interpolation
    ///         calculation. Interpolation delay together with network lag is used to synchronise the server simulation with
    ///         the
    ///         client's view.
    ///     </para>
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public abstract class DelayedLinearInterpolator<TValue> : IStreamingValueInterpolator<TValue>
    {
        public const float DEFAULT_INTERPOLATION_DELAY_SECONDS = 0.1f;
        public const float DEFAULT_EASING_FACTOR = 0.2f;
        public readonly Collections.CircularFifoQueue<TimestampedValue<TValue>> pendingValues = new Collections.CircularFifoQueue<TimestampedValue<TValue>>(5);
        private TValue currentValue;

        protected DelayedLinearInterpolator(float interpolationDelaySeconds)
        {
            InterpolationDelaySeconds = interpolationDelaySeconds;
        }

        protected DelayedLinearInterpolator() : this(DEFAULT_INTERPOLATION_DELAY_SECONDS) {}

        /// <summary>
        ///     The time used to calculate the current interpolated value.
        /// </summary>
        public float CurrentTime { get; private set; }

        public float InterpolationDelaySeconds { get; private set; }

        /// <summary>
        ///     <para>
        ///         Indicates whether there are any values to which we should interpolate as the <see cref="CurrentTime" />
        ///         increases.
        ///     </para>
        /// </summary>
        public bool HasPendingValues
        {
            get { return pendingValues.Count > 0; }
        }

        /// <summary>
        ///     This is the last value we've already reached. Its timestamp is absolute and corresponds to
        ///     <see cref="CurrentTime" />.
        /// </summary>
        public TimestampedValue<TValue> LastPastValue { get; set; }

        public void Reset(TValue initialValue, float initialValueAbsoluteTime)
        {
            pendingValues.Clear();
            CurrentTime = initialValueAbsoluteTime - InterpolationDelaySeconds;
            LastPastValue = new TimestampedValue<TValue>(initialValue, CurrentTime);
            currentValue = initialValue;
            EnqueueNewValue(initialValue, initialValueAbsoluteTime);
        }

        public void AddValue(TValue newValue, float newValueAbsoluteTime)
        {
            if (IsCurrentTimeAheadOf(newValueAbsoluteTime))
            {
                Reset(newValue, newValueAbsoluteTime);
            }
            else
            {
                EnqueueNewValue(newValue, newValueAbsoluteTime);
            }
        }

        public TValue GetInterpolatedValue(float deltaTimeToAdvance)
        {
            var previousTime = CurrentTime;
            CurrentTime += deltaTimeToAdvance;
            DiscardOutdatedValues();
            currentValue = HasPendingValues ? InterpolateToNextValue(previousTime) : LastPastValue.Value;
            return currentValue;
        }

        protected abstract TValue Interpolate(TValue currentValue, TValue nextValue, float progressRatio);

        private void DiscardOutdatedValues()
        {
            var nextFrameTime = CurrentTime;
            while (pendingValues.Count > 0 && nextFrameTime > pendingValues.Peek().Timestamp)
            {
                LastPastValue = pendingValues.Dequeue();
            }
        }

        private TValue InterpolateToNextValue(float previousTime)
        {
            var pendingTimestampedValue = pendingValues.Peek();
            ApplyTimeDriftCorrection(pendingTimestampedValue.Timestamp);
            var elapsedTime = CurrentTime - previousTime;
            var timeUntilNextValue = pendingTimestampedValue.Timestamp - previousTime;
            var transitionRatio = elapsedTime / timeUntilNextValue;
            return Interpolate(currentValue, pendingTimestampedValue.Value, transitionRatio);
        }

        private void ApplyTimeDriftCorrection(float timestamp)
        {
            var timeToTargetBeforeCorrection = timestamp - CurrentTime;
            if (timeToTargetBeforeCorrection > InterpolationDelaySeconds)
            {
                CurrentTime += timeToTargetBeforeCorrection * 0.1f;
            }
        }

        private void EnqueueNewValue(TValue newValue, float absoluteTime)
        {
            pendingValues.Enqueue(new TimestampedValue<TValue>(newValue, absoluteTime));
        }

        private bool IsCurrentTimeAheadOf(float newValueAbsoluteTime)
        {
            return CurrentTime > newValueAbsoluteTime;
        }
    }
}