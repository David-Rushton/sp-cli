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


                foreach(var definition in response.Definitions)
                {
                    AnsiConsole.MarkupLine
                    (
                        string.Format
                        (
                            "[bold blue]{0}[/] [italic grey]{1}[/]\n{2}\n[italic]{3}[/]\n",
                            definition.Term.EscapeMarkup(),
                            definition.PartOfSpeech.EscapeMarkup(),
                            definition.Definition.EscapeMarkup(),
                            definition.Example.EscapeMarkup()
                        )
                    );
                }


                return 0;
            }
            catch
            {
                // errors are caught, formatted and displayed by Spectre
                throw;
            }
        }
    }
}
