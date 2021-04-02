# STAND4 Grammar

A simple client library for the [Grammar API](https://www.grammar.com/grammar_api.php) from the [STANDS4 Network](https://www.grammar.com/about.php).

## Getting Starting

```csharp
var userId = "<YOUR-USER-ID>";
var tokenId = "<YOUR-TOKEN-ID>";
var credentials = new ApiCredentials(userId, tokenId);
var client = new GrammerCheckClient(credentials);
var issues = await client.CheckGrammar("<TEXT-TO-CHECK>");

foreach(var match in issues.Matches)
{
    Console.WriteLine($"{match.Message}: {match.Context.Section}");

    foreach(var replacement in match.Replacements)
    {
        Console.WriteLine($"\t{replacement.Value}");
    }
}
```
