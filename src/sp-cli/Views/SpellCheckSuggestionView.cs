using System;
using System.Collections.Generic;
using Spectre.Console;


namespace SpCli.Views
{
    public class SpellCheckSuggestionView
    {
        public string Show(string context, string word, List<string> suggestions, string hint)
        {
            AnsiConsole.Render
            (
                new Table()
                    .AddColumn
                    (
                        new TableColumn(hint)
                            .Padding(2, 0)
                            .NoWrap()
                    )
                    //.Width(Console.BufferWidth > 40 ? 40 : Console.BufferWidth)
                    .Border(TableBorder.Rounded)
                    .AddRow(context.AddVerticalPadding(1))
            );


            return PromptForReplacement(word, suggestions);
        }


        private string PromptForReplacement(string word, List<string> suggestions)
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
    }
}
