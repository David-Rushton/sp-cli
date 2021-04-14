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
                var model = await ClientBuilder
                    .AddCredentials(_grammarOptions.UserId, _grammarOptions.TokenId)
                    .BuildDefinitionClient()
                    .CheckDefinition(settings.Word)
                ;


                if(model.Suggestions.Count > 0)
                {
                    AnsiConsole.MarkupLine
                    (
                        string.Format
                        (
                            "[blue]Did you mean?[/]\n{0}\n",
                            string.Join('\n', model.Suggestions)
                        )
                    );
                }


                foreach(var definition in model.Definitions)
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
            catch(Exception e)
            {
                AnsiConsole.MarkupLine($"[red]{ e.Message }[/]");
                throw;
                return 1;
            }
        }
    }
}