using System;

namespace Assets.Improbable.Core.Physical
{
    public class ThresholdBasedUpdateNotifier<T>
    {
        public event Action<float, T> ShouldUpdate;

        private readonly float updatePeriodThreshold;
        private readonly Func<T, T, bool> isValuePastThreshold;
        private float lastUpdateTime;
        private T lastValue;

        public ThresholdBasedUpdateNotifier(float updatePeriodThreshold, Func<T, T, bool> isValuePastThreshold, T initialValue)
        {
            this.updatePeriodThreshold = updatePeriodThreshold;
            this.isValuePastThreshold = isValuePastThreshold;
            lastValue = initialValue;
        }

        public void RegisterNewValue(float currentTime, T newValue)
        {
            if (IsLastUpdateTimePastThreshold(currentTime) && isValuePastThreshold(lastValue, newValue))
            {
                OnShouldUpdate(currentTime, newValue);
            }
        }

        private void OnShouldUpdate(float currentTime, T newValue)
        {
            if (ShouldUpdate != null)
            {
                lastValue = newValue;
                ShouldUpdate(currentTime - lastUpdateTime, newValue);
                lastUpdateTime = currentTime;
            }
        }

        private bool IsLastUpdateTimePastThreshold(float currentTime)
        {
            return (currentTime - lastUpdateTime) >= updatePeriodThreshold;
        }
    }
}
