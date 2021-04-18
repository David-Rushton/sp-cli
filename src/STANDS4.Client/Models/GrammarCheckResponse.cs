using System.Collections.Generic;


namespace Stands4.Models
{
    public enum GrammarCheckResponseStatus
    {
        RequestFailed,
        RequestSuccessful
    }


    public class GrammarCheckResponse
    {
        internal GrammarCheckResponse(GrammarCheck check) =>
            (Status, Check) = (GrammarCheckResponseStatus.RequestSuccessful, check)
        ;

        internal GrammarCheckResponse()
        {
            Status = GrammarCheckResponseStatus.RequestFailed;
            Check = new GrammarCheck
            (
                Software: "",
                Warnings: new GrammarWarning(IncompleteResults: true),
                Language: new GrammarLanguage(Name: "", Code: "", new GrammarLanguageDetected(Name: "", Code: "", Confidence: 0)),
                Matches: new List<GrammarMatch>()
            );
        }


        public GrammarCheckResponseStatus Status { get; init; }

        public GrammarCheck Check { get; init; }

        public string ErrorMessage { get; init; } = string.Empty;
    }
}
