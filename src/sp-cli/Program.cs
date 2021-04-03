using Microsoft.Bing.SpellCheck;
using Microsoft.Bing.SpellCheck.Models;
using Spectre.Console;
using Spectre.Console.Cli;
using Stands4.Grammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace SpCli
{
    class Program
    {
        static string BritishEnglishLanguageCode = "en-GB";


        static async Task Main(string[] args)
        {
            var credentials = new ApiCredentials(args[0], args[1]);
            var client = new GrammarCheckClient(credentials, BritishEnglishLanguageCode);
            var issues = await client.CheckGrammar(args[2]);
            var document = new Document(args[2]);


            foreach(var match in issues.Matches)
            {

                Console.Clear();

                var context = match.Context;
                var prompt = context.Text;

                prompt = prompt.Insert(context.Offset + context.Length, "[/]");
                prompt = prompt.Insert(context.Offset, "[yellow]");


                // TODO: rule should be title case
                AnsiConsole.WriteLine(match.rule.Category.Name);
                AnsiConsole.WriteLine(match.rule.Description);

                // this is probably the message to display
                AnsiConsole.WriteLine(match.Message);
                AnsiConsole.Render(new Rule("text"));

                AnsiConsole.Render
                (
                    new Panel
                    (
                        document.PrettyPrintCorrectedContent(match.Offset, match.Length)
                    )
                    .Border(BoxBorder.Rounded)
                );
                // AnsiConsole.MarkupLine(prompt);

                if(match.Replacements.Count > 0)
                {
                    var selectedReplacement = AnsiConsole.Prompt
                    (
                        new SelectionPrompt<string>()
                            .Title("Select")
                            .AddChoices(match.Replacements.Select(r => r.Value))
                            .AddChoice(string.Empty)
                            .AddChoice("<Retain Original Text>")
                            .AddChoice("<Enter Replacement Text>")
                    );



                    document.AddCorrection(match.Offset, match.Length, selectedReplacement);
            //        AnsiConsole.MarkupLine(document.PrettyPrintCorrectedContent());
                }
            }



            Console.Clear();
            AnsiConsole.Render
            (
                new Panel
                (
                    document.PrettyPrintCorrectedContent()
                )
                .Border(BoxBorder.Rounded)
            );



        // TODO: Exceptions to implement
        //   catch(Exception e)
        //     {
        //         e switch
        //         {
        //             MissingParametersException ex => throw,
        //             GrammarCheckEmptyResultException ex => throw,
        //             GrammarCheckFailedException ex => throw,
        //             GrammarCheckValidationException ex => throw,
        //             _ => throw
        //         };
        //     }


        }
    }
}
