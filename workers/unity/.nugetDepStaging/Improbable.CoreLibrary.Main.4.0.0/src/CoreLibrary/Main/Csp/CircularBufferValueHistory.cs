using System;
using System.Collections.Generic;
using Improbable.Corelib.Interpolation;
using Improbable.Corelib.Util;

namespace Improbable.Corelib.Csp
{
    public class CircularBufferValueHistory<T> : IValueHistory<T>
    {
        private readonly int capacity;
        private readonly List<TimestampedValue<T>> timestampedValues;
        private readonly IValueInterpolation<T> valueInterpolation;
        private int absoluteIndexOfOldestSlot;

        public CircularBufferValueHistory(int capacity, IValueInterpolation<T> valueInterpolation)
        {
            timestampedValues = InitialiseTimestampPositionBuffer(capacity);
            this.capacity = capacity;
            this.valueInterpolation = valueInterpolation;
        }

        public int Count { get; private set; }

        public TimestampedValue<T> this[int index]
        {
            get { return timestampedValues[ToCircularIndex((index))]; }
            set { timestampedValues[ToCircularIndex(index)] = value; }
        }

        public T ValueAtTime(float absoluteTimeInSeconds)
        {
            if (Count == 0)
            {
                throw new ArgumentException("Could not find the value for given time. There is no recorded value history.");
            }
            if (absoluteTimeInSeconds <= this[0].Timestamp)
            {
                return this[0].Value;
            }
            for (int i = 0; i < Count - 1; i++)
            {
                var currentTimestampedPosition = this[i];
                var nextTimestampedPosition = this[i + 1];
                if (absoluteTimeInSeconds < nextTimestampedPosition.Timestamp)
                {
                    return InterpolatedValue(absoluteTimeInSeconds, currentTimestampedPosition, nextTimestampedPosition);
                }
            }
            return this[Count - 1].Value;
        }

        public void Reset(T initialValue, float currentTimeInSeconds)
        {
            absoluteIndexOfOldestSlot = 0;
            Count = 1;
            this[0] = new TimestampedValue<T>(initialValue, currentTimeInSeconds);
        }

        public void RecordValue(T value, float absoluteTimeInSeconds)
        {
            AssertNewValueIsFresherThanLastOne(absoluteTimeInSeconds);
            MakeSpaceForNewElement();
            this[Count - 1] = new TimestampedValue<T>(value, absoluteTimeInSeconds);
        }

        public void ApplyCorrection(IHistoryCorrection<T> correctionToApply, float absoluteSecondsInPast)
        {
            for (int i = Count - 1; i >= 0; --i)
            {
                var currentTimestampedPosition = this[i];
                if (currentTimestampedPosition.Timestamp >= absoluteSecondsInPast)
                {
                    correctionToApply.CorrectValue(ref currentTimestampedPosition.Value, currentTimestampedPosition.Timestamp);
                    this[i] = currentTimestampedPosition;
                }
            }
        }

        private void AssertNewValueIsFresherThanLastOne(float absoluteTimeInSeconds)
        {
            if (Count != 0 && absoluteTimeInSeconds < this[Count - 1].Timestamp)
            {
                throw new ArgumentException("Cannot record a value older than the last recorded value. Only new values can be recorded.");
            }
        }

        private int ToCircularIndex(int flatIndex)
        {
            return (absoluteIndexOfOldestSlot + flatIndex) % capacity;
        }

        private T InterpolatedValue(float absoluteTimeInSeconds, TimestampedValue<T> currentTimestampedPosition, TimestampedValue<T> nextTimestampedPosition)
        {
            float progressInSeconds = absoluteTimeInSeconds - currentTimestampedPosition.Timestamp;
            float entireTimespan = nextTimestampedPosition.Timestamp - currentTimestampedPosition.Timestamp;
            return valueInterpolation.InterpolatedValue(ref currentTimestampedPosition.Value, ref nextTimestampedPosition.Value, progressInSeconds / entireTimespan);
        }

        private void MakeSpaceForNewElement()
        {
            if (Count < capacity)
            {
                ++Count;
            }
            else
            {
                absoluteIndexOfOldestSlot = ToCircularIndex(1);
            }
        }

        private static List<TimestampedValue<T>> InitialiseTimestampPositionBuffer(int capacity)
        {
            var timestampedValueBuffer = new List<TimestampedValue<T>>(capacity);
            for (int i = 0; i < capacity; i++)
            {
                timestampedValueBuffer.Add(new TimestampedValue<T>());
            }
            return timestampedValueBuffer;
        }
    }
}