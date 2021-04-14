namespace Stands4.Models
{
    public class GrammarErrorModel
    {
        public GrammarError Results { get; init; } = new();
    }


    public class GrammarError
    {
        public string Error { get; init; } = string.Empty;
    }
}
