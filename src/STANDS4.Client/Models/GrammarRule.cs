namespace Stands4.Models
{
    public record GrammarRule
    (
        string Id,
        string Description,
        string IssueType,
        GrammarCategory Category
    );
}
