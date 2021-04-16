using Stands4.DTO;
using Stands4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;


namespace Stands4.Converters
{
    internal class DictionaryDtoConvertor
    {
        internal DefinitionResponse ConvertToModel(DictionaryDTO dto)
        {
            var statusCode = DefinitionResponseStatus.RequestSuccessful;
            var errorMessage = string.Empty;


            if(dto.ErrorMessage.Length > 0)
            {
                statusCode = DefinitionResponseStatus.RequestFailed;
                errorMessage = dto.ErrorMessage;
            }

            if(dto.Suggestions.Count > 0)
                statusCode = DefinitionResponseStatus.RequestSuccessfulWithSuggestions;


            return new DefinitionResponse
            {
                Status = statusCode,
                Definitions = ConvertFromDTO(dto),
                Suggestions = dto.Suggestions,
                ErrorMessage = errorMessage
            };
        }


        private List<DictionaryDefinition> ConvertFromDTO(object? dto)
        {
            /// System.Text.Json and the object converter <see cref="ObjectJsonConverter"/> should
            /// always populate the data transfer object with:
            ///
            ///   - null            Call to dictionary API failed or search term was not a valid word
            ///   - JsonElement     Call to dictionary API returned one or more dictionary definitions.
            ///
            /// JsonElement may contain an object (word has one definitions) or an array (word has
            /// multiple definitions).


            if(dto is JsonElement jsonElement)
            {
                var json = jsonElement.GetRawText();


                if(jsonElement.ValueKind == JsonValueKind.Array)
                {
                    var definitions = JsonSerializer.Deserialize<List<DictionaryDefinitionDTO>>(json)!;
                    return definitions.Select(d => d.ConvertToModel()).ToList();
                }


                if(jsonElement.ValueKind == JsonValueKind.Object)
                {
                    var definition = JsonSerializer.Deserialize<DictionaryDefinitionDTO>(json)!;
                    return new List<DictionaryDefinition>
                    {
                        definition.ConvertToModel()
                    };
                }
            }


            if(dto is null)
                return new List<DictionaryDefinition>();
            ;


            // I do not think it is possible to reach this point.
            throw new Exception($"Unexpected definition type: { dto.GetType().FullName }");
        }
    }
}
