namespace Stands4.Models
{
    public record GrammarContext
    (
        string Text,
        int Offset,
        int Length
    )
    {
        public string Section => this.Text.Substring(this.Offset, this.Length);

        public void Deconstruct(out string text, out int offset, out int length, out string section) =>
            (text, offset, length, section) = (Text, Offset, Length, Section)
        ;
    };
}
