using System.Linq;

namespace Improbable.Unity.EditorTools.Util.Validators
{
    public class FirstLetterUppercase : Validator<string>
    {
        public FirstLetterUppercase(string errorMessage) : base(errorMessage) {}

        public override bool IsConditionMet(string value)
        {
            return !string.IsNullOrEmpty(value) && char.IsUpper(value.First());
        }
    }
}