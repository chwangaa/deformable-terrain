namespace Improbable.Corelib.Interpolation
{
    /// <summary>
    ///     These interpolators may be used when new values are added on the fly (new values are streamed).
    ///     Typically, a streaming interpolator will add an artificial delay so that the current value can be
    ///     interpolated to the new values smoothly.
    ///     <para>
    ///         <b>Important</b>: the <see cref="Reset" /> function must be called before the interpolator can be used.
    ///     </para>
    /// </summary>
    /// <typeparam name="TValue">the type of the interpolated value (e.g.: <c>Vector3</c>).</typeparam>
    public interface IStreamingValueInterpolator<TValue>
    {
        /// <summary>
        ///     Initializes or restarts value interpolation afresh (discards all pending values).
        /// </summary>
        void Reset(TValue initialValue, float initialValueAbsoluteTime);

        /// <summary>
        ///     Adds a new value to which to interpolate.
        /// </summary>
        void AddValue(TValue newValue, float newValueAbsoluteTime);

        /// <summary>
        ///     Returns a new value that is interpolated between the current actual value and the next one
        ///     stored by this streaming interpolator.
        /// </summary>
        TValue GetInterpolatedValue(float deltaTimeToAdvance);
    }
}