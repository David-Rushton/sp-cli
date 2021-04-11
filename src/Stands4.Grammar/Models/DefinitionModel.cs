using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Stands4.Models
{

    internal class ObjectJsonConverter : JsonConverter<object>
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


    internal record DefinitionModel
    (
        List<WordDefinition>? Result //,
        // List<RelatedWord>? Related,
        // string? Error
    );


    internal record WordDefinitionRaw
    (
        object Term,
        object? Definition,
        object? Example,
        object? PartOfSpeech
    );

    public class WordDefinition
    {
        [JsonConverter(typeof(ObjectJsonConverter))]
        public object? Term { get; set; }

        [JsonConverter(typeof(ObjectJsonConverter))]
        public object? Definition { get; set; }

        [JsonConverter(typeof(ObjectJsonConverter))]
        public object? Example { get; set; }

        [JsonConverter(typeof(ObjectJsonConverter))]
        public object? PartOfSpeech { get; set; }
    }
    // );
    // {
    //     internal WordDefinition(WordDefinitionRaw word)
    //     : this
    //     (
    //         Term: word?.Term?.ToString() ?? " - ",
    //         Definition: word?.Definition?.ToString() ?? " - ",
    //         Example: word?.Example?.ToString() ?? " - ",
    //         PartOfSpeech: word?.PartOfSpeech?.ToString() ?? " - "
    //     )
    //     { }


    //     private string ExtractValue(object? item)
    //     {
    //         if(item is string s)
    //             return s;

    //         return string.Empty;
    //     }
    // }


    internal record RelatedWord
    (
        string Term
    );


    internal record ErrorWord
    (
        string Error
    );
}
