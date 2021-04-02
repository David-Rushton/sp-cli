namespace Stands4.Grammar.Models
{
    public record GrammarExceptionModel
    (
        string Error
    ) : IGrammarCheckResult;
}
