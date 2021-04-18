using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Stands4.Converters
{

    // TODO: move to converters namespace
    internal class ObjectJsonConverter : JsonConverter<object>
    {
        public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Dictionary definition API returns an empty object, as opposed to null/empty string.
            // Converting to an empty string simplifies the process of returning consistent data
            // types from this client.

            if(reader.TokenType == JsonTokenType.StartObject)
            {
                reader.Skip();
                return string.Empty;
            }


            return reader.GetString();
        }


        // implementation required by JsonConverter
        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options) =>
            Debug.Assert(false, "Json serializer support not implemented")
        ;
    }
}
