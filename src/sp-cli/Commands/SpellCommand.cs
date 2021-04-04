using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpCli.Commands.Settings;
using SpCli.Options;
using SpCli.Views;
using Stands4.Grammar;
using Stands4.Grammar.Models;
using Spectre.Console.Cli;
using System;
using System.Threading.Tasks;


namespace SpCli.Commands
{
    public class SpellCommand : AsyncCommand<SpellSettings>
    {
        readonly ILogger<SpellCommand> _logger;

        readonly GrammarOptions _grammarOptions;

        readonly ApiCredentials _apiCredentials;

        readonly GrammarCheckClient _grammarClient;


        // public SpellCommand(
        //     ILogger<SpellCommand> logger,
        //     IOptions<GrammarOptions> grammarOptions,
        //     ApiCredentials apiCredentials,
        //     GrammarCheckClient grammarClient
        // )
        // {
        //     _logger = logger;
        //     _grammarOptions = grammarOptions.Value;
        //     _apiCredentials = apiCredentials;
        //     _grammarClient = grammarClient;
        // }


        public override Task<int> ExecuteAsync(CommandContext context, SpellSettings settings)
        {


            return Task.Run(() => 0);
            throw new NotImplementedException();
        }
    }
}
