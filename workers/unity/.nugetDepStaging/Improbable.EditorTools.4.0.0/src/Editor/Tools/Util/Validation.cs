using System.Collections.Generic;
using System.Linq;
using Improbable.Unity.EditorTools.Util.Validators;

namespace Improbable.Unity.EditorTools.Util
{
    public static class Validation<T>
    {
        public static IEnumerable<string> GetViolations(IEnumerable<Validator<T>> validators, T value)
        {
            return from validator in validators
                   where !validator.IsConditionMet(value)
                   select validator.ErrorMessage;
        }
    }
}