using System;


namespace Stands4.Grammar.Exceptions
{
    public class GrammarCheckValidationException : Exception
    {
        public GrammarCheckValidationException(string message) : base(message)
        { }
    }
}
