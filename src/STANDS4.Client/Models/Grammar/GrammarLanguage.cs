namespace Stands4.Models
{
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
}
