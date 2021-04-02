using System;
using System.Text.RegularExpressions;


namespace Stands4.Grammar.Exceptions
{
    public class GrammarCheckFailedException : Exception
    {
        public GrammarCheckFailedException(string message) : base(ExtractErrorMessage(message))
        { }


        private static string ExtractErrorMessage(string xml)
        {

            const string fallbackMessage = "Unknown Grammar Api error";
            const string errorPattern = "<error>(.*)</error>";


            // ⚠ ⚠ ⚠ HACK: don't use regex to parse xml ⚠ ⚠ ⚠
            // https://stackoverflow.com/a/1732454/2572928
            var match = Regex.Match(xml, errorPattern);

            return
                // error is contained in the 2nd group
                match.Groups.Count == 2
                ? match.Groups[1].Value
                : fallbackMessage
            ;
        }
    }
}
