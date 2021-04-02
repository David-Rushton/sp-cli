namespace Stands4.Grammar.Models
{
    public record GrammarContext
    (
        string Text,
        int Offset,
        int Length
    )
    {
        public string Section => this.Text.Substring(this.Offset, this.Length);
    };
}
