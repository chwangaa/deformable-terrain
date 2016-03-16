using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Core;

namespace Improbable.Unity.EditorTools.Util
{
    public static class CliArgsUtils
    {
        public const string CLI_LIST_ARG_VALUE_SEPARATOR = ",";
        private static readonly char[] CLI_LIST_ARG_VALUE_SEPARATOR_CHAR_ARRAY = CLI_LIST_ARG_VALUE_SEPARATOR.ToCharArray();

        public static string ToCommaSeparatedList(IEnumerable<string> targetNames)
        {
            return String.Join(CLI_LIST_ARG_VALUE_SEPARATOR, targetNames.ToArray());
        }

        public static IList<string> FromCommaSeparatedList(string commaSeparatedList)
        {
            if (string.IsNullOrEmpty(commaSeparatedList))
            {
                return new string[] { };
            }
            return commaSeparatedList.Split(CLI_LIST_ARG_VALUE_SEPARATOR_CHAR_ARRAY, StringSplitOptions.None);
        }

        public static IEnumerable<T> GetListArgWithDefault<T>(this CommandLineArguments commandLineArguments, string argName, Func<string, T> stringToValueConverter, IEnumerable<string> defaultValues)
        {
            string cliValues;
            if (!commandLineArguments.TryGetConfigValue(argName, out cliValues))
            {
                return defaultValues.Select(stringToValueConverter);
            }
            return FromCommaSeparatedList(cliValues).Select(stringToValueConverter);
        }
    }
}