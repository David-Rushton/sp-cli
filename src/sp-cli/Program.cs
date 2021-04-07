using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SpCli.Commands;
using Spectre.Console.Cli;
using System;
using System.Text;
using System.Threading.Tasks;


namespace SpCli
{
    class Program
    {
        // TODO: should be an option
        static string BritishEnglishLanguageCode = "en-GB";


        static async Task<int> Main(string[] args) => await Bootstrap().RunAsync(args);


        static CommandApp Bootstrap()
        {
            Console.OutputEncoding = Encoding.Unicode;


            var configuration = new ConfigurationBuilder()
                .AddOptions()
                .Build()
            ;


            var serviceCollection = new ServiceCollection()
                .AddLogging(config => config.AddSimpleConsole())
                .AddOptions(configuration)
                .AddDocumentFactory()
                .AddViews()
            ;


            var app = new CommandApp(new TypeRegistrar(serviceCollection));
            app.Configure
            (
                config =>
                {
                    config
                        .SetApplicationName("Spelling and Grammar Check")
                        .CaseSensitivity(CaseSensitivity.None)
                        .ValidateExamples()
                        .PropagateExceptions()
                    ;

                    config
                        .AddCommand<SpellCommand>("sp")
                        .WithDescription("Spelling and grammar check")
                        .WithExample( new[] {"sp", "\"text to check\""} )
                    ;
                }
            );

            // app.SetDefaultCommand<SpellCommand>();

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
