using Microsoft.Bing.SpellCheck;
using Microsoft.Bing.SpellCheck.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SpCli.Commands;
using SpCli.Options;
using SpCli.Views;
using Spectre.Cli.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;
using Stands4.Grammar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace SpCli
{
    class Program
    {
        // TODO: should be an option
        static string BritishEnglishLanguageCode = "en-GB";


        static async Task<int> Main(string[] args) => await Bootstrap().RunAsync(args);
        // {

        //     var credentials = new ApiCredentials(args[0], args[1]);
        //     //var client = new GrammarCheckClient(credentials, BritishEnglishLanguageCode);
        //     //var issues = await client.CheckGrammar(args[2]);
        //     var document = new Document(args[2]);


        //     // foreach(var match in issues.Matches)
        //     // {



        //         Console.OutputEncoding = System.Text.Encoding.UTF8;


        //         var view = new Views.SpellCheckSuggestionView();
        //         view.Show("stu", "stu", new List<string>(new [] { "stuf", "stuff", "stuffx" }), "typo");
        //         Environment.Exit(0);



        //     //     Console.Clear();

        //     //     var context = match.Context;
        //     //     var prompt = context.Text;

        //     //     prompt = prompt.Insert(context.Offset + context.Length, "[/]");
        //     //     prompt = prompt.Insert(context.Offset, "[yellow]");


        //     //     // TODO: rule should be title case
        //     //     AnsiConsole.WriteLine(match.rule.Category.Name);
        //     //     AnsiConsole.WriteLine(match.rule.Description);

        //     //     // this is probably the message to display
        //     //     AnsiConsole.WriteLine(match.Message);
        //     //     AnsiConsole.Render(new Rule("text"));

        //     //     AnsiConsole.Render
        //     //     (
        //     //         new Panel
        //     //         (
        //     //             document.PrettyPrintCorrectedContent(match.Offset, match.Length)
        //     //         )
        //     //         .Border(BoxBorder.Rounded)
        //     //     );
        //     //     // AnsiConsole.MarkupLine(prompt);

        //     //     if(match.Replacements.Count > 0)
        //     //     {
        //     //         var selectedReplacement = AnsiConsole.Prompt
        //     //         (
        //     //             new SelectionPrompt<string>()
        //     //                 .Title("Select")
        //     //                 .AddChoices(match.Replacements.Select(r => r.Value))
        //     //                 .AddChoice(string.Empty)
        //     //                 .AddChoice("<Retain Original Text>")
        //     //                 .AddChoice("<Enter Replacement Text>")
        //     //         );



        //     //         document.AddCorrection(match.Offset, match.Length, selectedReplacement);
        //     // //        AnsiConsole.MarkupLine(document.PrettyPrintCorrectedContent());
        //     //
        // }


        static CommandApp Bootstrap()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables("SP")
                .Build()
            ;


            var serviceCollection = new ServiceCollection()
                .AddLogging
                (
                    config =>
                    {
                        config.AddSimpleConsole();
                    }
                )
                .Configure<GrammarOptions>(configuration.GetSection("STANDS4"))
                .AddTransient<SpellCheckSuggestionView>()
                .AddTransient<Document>()
                .AddTransient<ApiCredentials>
                (
                    sp =>
                    {
                        var options = sp.GetService<GrammarOptions>();
                        return new ApiCredentials(options.UserId, options.TokenId);
                    }
                )
                .AddTransient<GrammarCheckClient>()
            ;


            using var registrar = new DependencyInjectionRegistrar(serviceCollection);
            // var app = new CommandApp(registrar);
            var app = new CommandApp();
            app.Configure
            (
                config =>
                {
                    config
                        .SetApplicationName("Spelling and Grammar Check")
                        .CaseSensitivity(CaseSensitivity.None)
                        .ValidateExamples()
                    ;

                    config
                        .AddCommand<SpellCommand>("check")
                        .WithDescription("Spelling and grammar check")
                        .WithExample( new[] {"check", "\"text to check\""} )
                    ;
                }
            );

            app.SetDefaultCommand<SpellCommand>();

            return app;
        }


        // Console.Clear();
        // AnsiConsole.Render
        // (
        //     new Panel
        //     (
        //         document.PrettyPrintCorrectedContent()
        //     )
        //     .Border(BoxBorder.Rounded)
        // );





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


        // }
    }
}
