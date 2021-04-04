using System;


namespace SpCli.Views
{
    public static class StringExtensions
    {
        public static string AddVerticalPadding(this string @string, int verticalPadding) =>
            string.Format
            (
                "{0}{1}{2}",
                new string('\n', verticalPadding),
                @string,
                new string('\n', verticalPadding)
            )
        ;
    }
}
