namespace Improbable.Unity.EditorTools.Util.Validators
{
    public class NotEmpty : Validator<string>
    {
        public NotEmpty(string errorMessage) : base(errorMessage) {}

        public override bool IsConditionMet(string value)
        {
            return !string.IsNullOrEmpty(value);
        }
    }
}