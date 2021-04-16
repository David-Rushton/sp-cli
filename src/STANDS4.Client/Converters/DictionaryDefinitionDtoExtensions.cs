using Stands4.DTO;
using Stands4.Models;


namespace Stands4.Converters
{
    public static class DictionaryDefinitionDtoExtensions
    {
        public static DictionaryDefinition ConvertToModel(this DictionaryDefinitionDTO dto) =>
            new DictionaryDefinition
            (
                Term: dto.Term?.ToString()                  ?? string.Empty,
                Definition: dto.Definition?.ToString()      ?? string.Empty,
                Example: dto.Example?.ToString()            ?? string.Empty,
                PartOfSpeech: dto.PartOfSpeech?.ToString()  ?? string.Empty
            )
        ;
    }
}
