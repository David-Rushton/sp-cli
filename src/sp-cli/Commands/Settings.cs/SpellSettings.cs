using Spectre.Console.Cli;


namespace SpCli.Commands.Settings
{
    public class SpellSettings : CommandSettings
    {
        [CommandArgument(0, "[text to check]")]
        public string TextToCheck { get; set; } = string.Empty;
    }
}
