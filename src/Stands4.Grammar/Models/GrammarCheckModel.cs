using System.Collections.Generic;


namespace Stands4.Models
{
    public record GrammarCheckModel
    (
        string Software,
        GrammarWarning Warnings,
        GrammarLanguage Language,
        List<GrammarMatch> Matches
    ) : IGrammarCheckResult;
}
