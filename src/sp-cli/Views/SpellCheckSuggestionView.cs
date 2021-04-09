using System.Collections.Generic;
using Spectre.Console;


namespace SpCli.Views
{
    public class SpellCheckView
    {
        public void Show(string contextMarkup, string hint) =>
            AnsiConsole.Render
            (
                new Table()
                    .AddColumn
                    (
                        new TableColumn(FormatHint(hint))
                            .Padding(2, 0)
                            .NoWrap()
                    )
                    .Border(TableBorder.Rounded)
                    .AddRow(contextMarkup.AddVerticalPadding(1))
            )
        ;

        public string PromptForReplacement(string word, List<string> suggestions)
        {
            const string Skip = "<skip>";
            const string ManuallyCorrect = "<manullay correct>";
            var replacement = string.Empty;


            while(replacement == string.Empty)
            {
                replacement = AnsiConsole.Prompt
                (
                    new SelectionPrompt<string>()
                        .Title("Suggestions")
                        .AddChoices(suggestions)
                        .AddChoice(Skip)
                        .AddChoice(ManuallyCorrect)
                );
            }


            return replacement switch
            {
                Skip            => word,
                ManuallyCorrect => AnsiConsole.Prompt(new TextPrompt<string>("Enter Correction")),
                _               => replacement
            };
        }

        private string FormatHint(string hint)
        {
            if(hint[^1] == '.')
                hint = hint.Remove(hint.Length - 1);

            hint = hint.ToLower();

            hint = $"[blue]{ hint }[/]";


            return hint;
        }
    }
}
