using System.Linq;

namespace Improbable.Unity.EditorTools.Util.Validators
{
    public class OnlyAlphanumeric : Validator<string>
    {
        public OnlyAlphanumeric(string errorMessage) : base(errorMessage) {}

        public override bool IsConditionMet(string value)
        {
            return value.All(char.IsLetterOrDigit);
        }
    }
}