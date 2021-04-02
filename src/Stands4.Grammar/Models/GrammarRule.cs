namespace Stands4.Grammar.Models
{
    public record GrammarRule
    (
        string Id,
        string Description,
        string IssueType,
        GrammarCategory Category
    );
}
