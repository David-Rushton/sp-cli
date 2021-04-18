using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpCli.Commands.Settings;
using SpCli.Options;
//using SpCli.Views;
using Stands4;
using Stands4.Models;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;




namespace SpCli.Commands
{
    public class DefinitionCommand : AsyncCommand<DefinitionSettings>
    {
        readonly ILogger<DefinitionCommand> _logger;

        // TODO: bad name
        readonly GrammarOptions _grammarOptions;


        public DefinitionCommand(
            ILogger<DefinitionCommand> logger,
            IOptions<GrammarOptions> grammarOptions
        )
        {
            _logger = logger;
            _grammarOptions = grammarOptions.Value;
        }


        public async override Task<int> ExecuteAsync(CommandContext context, DefinitionSettings settings)
        {
            try
            {
                var response = await ClientBuilder
                    .AddCredentials(_grammarOptions.UserId, _grammarOptions.TokenId)
                    .BuildDefinitionClient()
                    .TryGetDefinition(settings.Word)
                ;


                if(response.Status == DefinitionResponseStatus.RequestFailed)
                    throw new Exception(response.ErrorMessage);


                if(response.Status == DefinitionResponseStatus.RequestSuccessfulWithSuggestions)
                {
                    AnsiConsole.MarkupLine
                    (
                        string.Format
                        (
                            "[blue]Did you mean?[/]\n{0}\n",
                            string.Join('\n', response.Suggestions)
                        )
                    );
                }


                foreach(var word in GetWords(response))
                {
                    var display = $"[blue invert]   { word.Term.EscapeMarkup() }   [/]\n\n";

                    foreach(var entry in word.Entries)
                    {
                        var definitionIndex = 1;
                        display += $"[blue]{ entry.PartOfSpeech.EscapeMarkup() }[/]\n";

                        foreach(var definition in entry.Definitions)
                        {
                            display +=
                            string.Format
                            (
                                "[grey]{0}[/] {1}\n[grey italic]{2}[/]\n\n",
                                definitionIndex++,
                                definition.Definition.EscapeMarkup(),
                                definition.Example.EscapeMarkup()
                            );
                        }
                    }


                    AnsiConsole.MarkupLine(display);
                }


                return 0;
            }
            catch
            {
                // errors are caught, formatted and displayed by Spectre
                throw;
            }
        }


        private IEnumerable<Word> GetWords(DefinitionResponse response) =>
            from definition in response.Definitions
            group definition by definition.Term into termGroup
            select new Word
            (
                Term: termGroup.Key,
                Entries:
                    from term in termGroup
                    group term by term.PartOfSpeech into posGroup
                    select new Entry
                    (
                        PartOfSpeech: posGroup.Key == string.Empty ? "definition" : posGroup.Key,
                        Definitions: posGroup.Select(dd => (dd.Definition, dd.Example))
                    )
            )
        ;


        private record Word
        (
            string Term,
            IEnumerable<Entry> Entries
        );

        private record Entry
        (
            string PartOfSpeech,
            IEnumerable<(string Definition, string Example)> Definitions
        );
    }
}
