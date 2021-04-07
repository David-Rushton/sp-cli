using System.Collections.Generic;


namespace Stands4.Models
{
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
}
