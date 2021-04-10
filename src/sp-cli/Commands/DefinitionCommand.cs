using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpCli.Commands.Settings;
using SpCli.Options;
using SpCli.Views;
using Stands4;
using Stands4.Models;
using Spectre.Console;
using Spectre.Console.Cli;
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
            var credentials = new ApiCredentials(_grammarOptions.UserId, _grammarOptions.TokenId);
            var client = new DefinitionClient(credentials);
            var definitions = await client.CheckDefinition(settings.Word);

            foreach(var definition in definitions)
            {
                AnsiConsole.MarkupLine
                (
                    string.Format
                    (
                        "[bold blue]{0}[/] [italic grey]{1}[/]\n{2}\n{3}\n",
                        definition.Term,
                        definition.PartOfSpeech,
                        definition.Definition,
                        definition.Example
                    )
                );
            }


            // TODO: non-zero on fail
            return 0;
        }
    }
}
