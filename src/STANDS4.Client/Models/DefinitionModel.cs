using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Stands4.Models
{

    internal class WordDefinitionJsonConverter : JsonConverter<object>
    {
        public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if(reader.TokenType == JsonTokenType.StartObject)
            {
                reader.Skip();
                return string.Empty;
            }


            return reader.GetString();
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options) =>
            writer.WriteStringValue
            (
                value?.ToString() ?? string.Empty
            )
        ;
    }


    public record DefinitionModel
    (
        // TODO: something has to renamed; DefinitionModel.Definitions is terrible
        List<WordDefinition> Definitions,
        List<string> Suggestions,
        string ErrorMessage
    );


    public class DefinitionModelInternal
    {
        [JsonPropertyName("result")]
        public List<WordDefinitionInternal> Definitions { get; set; } = new();

        [JsonPropertyName("related")]
        public List<RelatedWord> Suggestions { get; set; } = new();

        [JsonPropertyName("error")]
        public string ErrorMessage { get; set; } = string.Empty;


        public DefinitionModel GetDefinitionModel() =>
            new DefinitionModel
            (
                Definitions:  this.Definitions.Select(d => d.GetWord()).ToList(),
                Suggestions:  this.Suggestions.Select(s => s.Term).ToList(),
                ErrorMessage: this.ErrorMessage
            )
        ;
    }


    public class WordDefinitionInternal
    {
        [JsonPropertyName("term")]
        [JsonConverter(typeof(WordDefinitionJsonConverter))]
        public object? Term { get; set; }

        [JsonPropertyName("definition")]
        [JsonConverter(typeof(WordDefinitionJsonConverter))]
        public object? Definition { get; set; }

        [JsonPropertyName("example")]
        [JsonConverter(typeof(WordDefinitionJsonConverter))]
        public object? Example { get; set; }

        [JsonPropertyName("partOfSpeech")]
        [JsonConverter(typeof(WordDefinitionJsonConverter))]
        public object? PartOfSpeech { get; set; }


        public WordDefinition GetWord() =>
            new WordDefinition
            (
                Term:           Term?.ToString()         ?? string.Empty,
                Definition:     Definition?.ToString()   ?? string.Empty,
                Example:        Example?.ToString()      ?? string.Empty,
                PartOfSpeech:   PartOfSpeech?.ToString() ?? string.Empty
            )
        ;
    }


    public record WordDefinition
    (
        string Term,
        string Definition,
        string Example,
        string PartOfSpeech
    );



    public record RelatedWord
    (
        string Term
    );


}
