# STAND4 Client

A simple .Net client library for accessing selected STANDS4 web services:

- [Definitions API](https://www.definitions.net/definitions_api.php)
- [Grammar API](https://www.grammar.com/grammar_api.php)
- [Synonyms API](https://www.synonyms.com/synonyms_api.php)

## Getting Starting

```csharp

// TODO: this example is outdated and does not include definitions

var userId = "<YOUR-USER-ID>";
var tokenId = "<YOUR-TOKEN-ID>";

var client = ClientBuilder
    .AddCredentials(userId, tokenId)
    .BuildGrammarClient()
;

var issues = await client.TryCheckGrammar("<TEXT-TO-CHECK>");

foreach(var match in issues.Matches)
{
    Console.WriteLine($"{match.Message}: {match.Context.Section}");

    foreach(var replacement in match.Replacements)
    {
        Console.WriteLine($"\t{replacement.Value}");
    }
}

```
