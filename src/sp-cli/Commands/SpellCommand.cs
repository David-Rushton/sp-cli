using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpCli.Commands.Settings;
using SpCli.Options;
using SpCli.Views;
using Stands4;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace SpCli.Commands
{
    public class SpellCommand : AsyncCommand<SpellSettings>
    {
        readonly ILogger<SpellCommand> _logger;

        readonly GrammarOptions _grammarOptions;

        readonly DocumentFactory _documentFactory;

        readonly SpellCheckSuggestionView _suggestionView;


        public SpellCommand(
            ILogger<SpellCommand> logger,
            IOptions<GrammarOptions> grammarOptions,
            DocumentFactory documentFactory,
            SpellCheckSuggestionView suggestionView
        )
        {
            _logger = logger;
            _grammarOptions = grammarOptions.Value;
            _documentFactory = documentFactory;
            _suggestionView = suggestionView;
        }


        public async override Task<int> ExecuteAsync(CommandContext context, SpellSettings settings)
        {
            var doc = _documentFactory.New(settings.TextToCheck);
            var client = GrammarApi
                .AddCredentials(_grammarOptions.UserId, _grammarOptions.TokenId)
                .GetGrammarClient()
            ;
            var checkResult = await client.CheckGrammar(doc.OriginalContent);




            // TODO: Consider clear when not enough lines
            ConsoleEx.ClearNextLines(25);



            ConsoleEx.SaveCursorPosition();

            // var bookmark = Console.GetCursorPosition();
            // bookmark.Top -= 30;


            foreach(var match in checkResult.Matches)
            {
                // Console.Clear();
                ConsoleEx.RestoreCursorPosition();
                ConsoleEx.ClearFromCursor();
                // PrintConsoleConfig();


                var (text, offset, length, section) = match.Context;
                var prettyContext = doc.PrettyPrintCorrectedContent(offset, length);
                var suggestions = match.Replacements.Select(r => r.Value).ToList();
                var correction = _suggestionView.Show(prettyContext, section, suggestions, match.Message);

                doc.AddCorrection(offset, length, correction);
            }


            // Console.Clear();
            ConsoleEx.RestoreCursorPosition();
            ConsoleEx.ClearFromCursor();

            //Console.SetCursorPosition(bookmark.Left, bookmark.Top);
            AnsiConsole.MarkupLine(doc.PrettyPrintCorrectedContent());
            AnsiConsole.WriteLine();


            // TODO: Consider returning corrected text within table.
            _suggestionView.Show(doc.PrettyPrintCorrectedContent(), "ABC", new System.Collections.Generic.List<string>(), "Complete");

            return 0;
        }
    }
}
