using System.Collections.Generic;


namespace Stands4.Models
{
    public enum DefinitionResponseStatus
    {
        RequestFailed,
        RequestSuccessful,
        RequestSuccessfulWithSuggestions
    }


    public class DefinitionResponse
    {
        internal DefinitionResponse()
        { }


        public DefinitionResponseStatus Status { get; init; } = DefinitionResponseStatus.RequestSuccessful;

        public List<DictionaryDefinition> Definitions { get; init; } = new();

        public List<Suggestion> Suggestions { get; init; } = new();

        public string ErrorMessage { get; init; } = string.Empty;
    }
}
