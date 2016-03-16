namespace Improbable.Unity.EditorTools.Util.Validators
{
    public abstract class Validator<T>
    {
        protected Validator(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; private set; }

        public abstract bool IsConditionMet(T value);
    }
}