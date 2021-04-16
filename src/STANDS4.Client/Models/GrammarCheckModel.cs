using System.Collections.Generic;


namespace Stands4.Models
{
    public record GrammarCheck
    (
        string Software,
        GrammarWarning Warnings,
        GrammarLanguage Language,
        List<GrammarMatch> Matches
    );


    // ⬇⬇⬇ remaining records are all part of the GrammarCheck object graph ⬇⬇⬇


    public record GrammarWarning(bool IncompleteResults);


    public record GrammarLanguage
    (
        string Name,
        string Code,
        GrammarLanguageDetected languageDetected
    );


    public record GrammarLanguageDetected
    (
        string Name,
        string Code,
        decimal Confidence
    );


    public record GrammarMatch
    (
        string Message,
        string ShortMessage,
        List<GrammarReplacement> Replacements,
        int Offset,
        int Length,
        GrammarContext Context,
        string Sentence,
        GrammarType Type,
        GrammarRule rule,
        bool IgnoreForIncompleteSentence,
        int ContextForSureMatch
    );


    public record GrammarReplacement(string Value, string ShortDescription = "");


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


    public record GrammarType(string TypeName);


    public record GrammarRule
    (
        string Id,
        string Description,
        string IssueType,
        GrammarCategory Category
    );


    public record GrammarCategory(string Id, string Name);
}
