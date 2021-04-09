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
    public class SpellCommand : AsyncCommand<SpellSettings>
    {
        const byte ClearNumberOfLines = 20;

        const string SpellAndGrammarCheckCompleteMessage = "Spell and grammar check complete";

        readonly ILogger<SpellCommand> _logger;

        readonly LocalOptions _localOptions;

        readonly GrammarOptions _grammarOptions;

        readonly DocumentFactory _documentFactory;

        readonly SpellCheckView _spellCheckView;


        public SpellCommand(
            ILogger<SpellCommand> logger,
            IOptions<LocalOptions> localOptions,
            IOptions<GrammarOptions> grammarOptions,
            DocumentFactory documentFactory,
            SpellCheckView spellCheckView
        )
        {
            _logger = logger;
            _localOptions = localOptions.Value;
            _grammarOptions = grammarOptions.Value;
            _documentFactory = documentFactory;
            _spellCheckView = spellCheckView;
        }


        public async override Task<int> ExecuteAsync(CommandContext context, SpellSettings settings)
        {
            var doc = _documentFactory.New(settings.TextToCheck);
            var client = GrammarApi
                .AddCredentials(_grammarOptions.UserId, _grammarOptions.TokenId)
                .SetLanguageCode(_localOptions.LanguageCode)
                .GetGrammarClient()
            ;
            var checkResult = await client.CheckGrammar(doc.OriginalContent);


            PrepareConsole();
            ReadCorrections(checkResult, doc);
            ShowCorrectedText(doc.PrettyPrintCorrectedContent());


            return 0;
        }


        private void ShowCorrectedText(string prettyContext)
        {
            RestoreCursorPositionAndClear();
            _spellCheckView.Show(prettyContext, SpellAndGrammarCheckCompleteMessage);
        }

        private void ReadCorrections(GrammarCheckModel model, Document doc)
        {
            foreach(var match in model.Matches)
            {
                var (text, offset, length, section) = match.Context;
                var prettyContext = doc.PrettyPrintCorrectedContent(offset, length);

                RestoreCursorPositionAndClear();

                _spellCheckView.Show(prettyContext, match.Message);

                doc.AddCorrection
                (
                    offset,
                    length,
                    ReadCorrection(match.Context.Section, match.Replacements)
                );
            }
        }

        private string ReadCorrection(string word, List<GrammarReplacement> replacements)
        {
            var suggestions = replacements.Select(r => r.Value).ToList();
            return _spellCheckView.PromptForReplacement(word, suggestions);
        }

        private void PrepareConsole()
        {
            // ensures there is enough space at the bottom of the console
            ConsoleEx.ClearNextLines(ClearNumberOfLines);
            ConsoleEx.SaveCursorPosition();
        }

        private void RestoreCursorPositionAndClear()
        {
            ConsoleEx.RestoreCursorPosition();
            ConsoleEx.ClearFromCursor();
        }
    }
}
