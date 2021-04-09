using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpCli.Options;
using SpCli.Views;
using System.IO;


namespace SpCli
{
    public static class Startup
    {
        public static IConfigurationBuilder AddOptions(this IConfigurationBuilder config) =>
            config
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables("SP")
        ;


        public static IServiceCollection AddViews(this IServiceCollection serviceCollection) =>
            serviceCollection
                .AddTransient<SpellCheckView>()
        ;


        public static IServiceCollection AddOptions(
            this IServiceCollection serviceCollection,
            IConfigurationRoot config
        ) =>
            serviceCollection
                .Configure<LocalOptions>(config.GetSection("Local"))
                .Configure<GrammarOptions>(config.GetSection("STANDS4"))
        ;


        public static IServiceCollection AddDocumentFactory(
            this IServiceCollection serviceCollection
        ) =>
            serviceCollection
                .AddTransient<DocumentFactory>()
        ;
    }
}
