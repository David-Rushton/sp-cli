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
        public DefinitionModelInternal()
        { }

        public DefinitionModelInternal(List<WordDefinitionInternal> result)
        {
            Definitions = result;
        }

        // [JsonConstructorAttribute]
        public DefinitionModelInternal(WordDefinitionInternal result)
        {
            Definitions = new List<WordDefinitionInternal>
            {
                result
            };
        }

        public DefinitionModelInternal(List<RelatedWord> related)
        {
            Suggestions = related;
        }

        public DefinitionModelInternal(RelatedWord related)
        {
            Suggestions = new List<RelatedWord>
            {
                related
            };
        }

        public DefinitionModelInternal(string error)
        {
            ErrorMessage = error;
        }


        /// <summary>
        /// In the case where a word has a single definition STANDS4 will return an object.
        /// When multiple definitions are returned this object will contain an array.
        /// </summary>
        /// <returns>WordDefinitionInternal or List of WordDefinitionInternal</returns>
        [JsonPropertyName("result")]
        public object? Definitions { get; set; } = new();


        [JsonPropertyName("related")]
        public List<RelatedWord> Suggestions { get; set; } = new();

        [JsonPropertyName("error")]
        public string ErrorMessage { get; set; } = string.Empty;


        public DefinitionModel GetDefinitionModel() =>
            new DefinitionModel
            (
                Definitions:  GetDefinitionModels()!,
                Suggestions:  this.Suggestions.Select(s => s.Term).ToList(),
                ErrorMessage: this.ErrorMessage
            )
        ;


        // TODO: maybe this is a class?
        private List<WordDefinition>? GetDefinitionModels()
        {
            if(Definitions is null)
                return null
            ;


            if(Definitions is JsonElement jsonElement)
            {
                var json = jsonElement.GetRawText();


                if(jsonElement.ValueKind == JsonValueKind.Array)
                {
                    var definitions = JsonSerializer.Deserialize<List<WordDefinitionInternal>>(json)!;
                    return definitions.Select(d => d.GetWord()).ToList();
                }


                if(jsonElement.ValueKind == JsonValueKind.Object)
                {
                    var definition = JsonSerializer.Deserialize<WordDefinitionInternal>(json)!;
                    return new List<WordDefinition>
                    {
                        definition.GetWord()
                    };
                }
            }


            throw new Exception($"Unexpected definition type: { Definitions.GetType().FullName }");
        }
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
