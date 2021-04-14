namespace Stands4.Models
{
    public record GrammarExceptionModel
    (
        string Error
    ) : IGrammarCheckResult;
}
