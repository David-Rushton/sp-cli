using System;


namespace Stands4.Grammar.Exceptions
{
    public class GrammarCheckEmptyResultException : Exception
    {
        public GrammarCheckEmptyResultException() : base("Grammar API failed to return a result")
        { }

        public GrammarCheckEmptyResultException(Exception innerException)
            : base("Grammar API failed to return a result", innerException)
        { }
    }
}
