using System.Collections.Generic;


namespace Stands4.Grammar.Models
{
    public record GrammarCheckModel
    (
        string Software,
        GrammarWarning Warnings,
        GrammarLanguage Language,
        List<GrammarMatch> Matches
    ) : IGrammarCheckResult;
}
