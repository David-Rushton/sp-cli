using System.Collections.Generic;


namespace Stands4.Models
{
    internal record DefinitionModel
    (
        List<WordDefinition>? Result //,
        // List<RelatedWord>? Related,
        // string? Error
    );


    public record WordDefinition
    (
        object Term,
        object? Definition,
        object? PartOfSpeech,
        object? Example
    );


    public record RelatedWord
    (
        string Term
    );
}
