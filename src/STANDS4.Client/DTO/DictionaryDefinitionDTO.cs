using Stands4.Models;
using Stands4.Converters;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace Stands4.DTO
{
    /// <summary>
    /// Used to deserialize results from the STANDS4 dictionary definitions api.
    /// </summary>
    public class DictionaryDefinitionDTO
    {
        // All of the below fields are returned by the dictionary definitions api.  They will
        // either contain a string value or an empty object.  To handle both of these cases we
        // deserialize to type of object?, using the WordDefinitionJsonConverter.


        [JsonPropertyName("term")]
        [JsonConverter(typeof(ObjectJsonConverter))]
        public object? Term { get; set; }

        [JsonPropertyName("definition")]
        [JsonConverter(typeof(ObjectJsonConverter))]
        public object? Definition { get; set; }

        [JsonPropertyName("example")]
        [JsonConverter(typeof(ObjectJsonConverter))]
        public object? Example { get; set; }

        [JsonPropertyName("partOfSpeech")]
        [JsonConverter(typeof(ObjectJsonConverter))]
        public object? PartOfSpeech { get; set; }
    }
}
