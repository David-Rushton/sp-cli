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
        static async Task<int> Main(string[] args) => await Bootstrap().RunAsync(args);


        static CommandApp Bootstrap()
        {
            Console.OutputEncoding = Encoding.UTF8;


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

                    config
                        .AddCommand<DefinitionCommand>("def")
                            .WithDescription("Dictionary")
                            .WithExample( new[] {"def", "word"} )
                    ;
                }
            );


            return app;
        }


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
