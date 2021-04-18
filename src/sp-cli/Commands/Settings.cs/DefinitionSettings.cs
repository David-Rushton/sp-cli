using Spectre.Console.Cli;


namespace SpCli.Commands.Settings
{
    public class DefinitionSettings : CommandSettings
    {
        [CommandArgument(0, "<word>")]
        public string Word { get; set; } = string.Empty;
    }
}
