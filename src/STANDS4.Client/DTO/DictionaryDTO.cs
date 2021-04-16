using Stands4.Models;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace Stands4.DTO
{
    /// <summary>
    /// Used to deserialize results from the STANDS4 dictionary definitions api.
    /// </summary>
    public class DictionaryDTO
    {
        // The dictionary definitions returns a dynamic schema.  You cannot predict which fields
        // will be included in the resultset.  Each request will include one - and only one - of the
        // following fields:
        //
        // field    type        contents
        // ---------------------------------
        // result   object      Word has a single definition.
        // result   array       Word has two or more definitions.
        // related  object      Unsure if this is an option.  Not supported.  Would throw.
        // related  array       Word not recognised.  Array of suggestions returned.
        // error    string      Describes wy the API failed.


        // result may contain a DictionaryDefinitionDTO or a list of DictionaryDefinitionDTOs
        // see above
        [JsonPropertyName("result")]
        public object? Definitions { get; init; } = new();

        [JsonPropertyName("related")]
        public List<Suggestion> Suggestions { get; init; } = new();

        [JsonPropertyName("error")]
        public string ErrorMessage { get; init; } = string.Empty;
    }
}
