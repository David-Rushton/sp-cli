using Stands4.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace Stands4
{
    public class DefinitionClient
    {
        const string BaseAddress = "https://www.stands4.com/services/v2/defs.php";

        readonly ApiCredentials _credentials;

        readonly HttpClient _client = new();

        readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };


        public DefinitionClient(ApiCredentials credentials) => (_credentials) = (credentials);


        public async Task<List<WordDefinition>> CheckDefinition(string word)
        {
            if(word.Length == 0)
            {
                // TODO: custom exception is convention
                Debug.Assert(false, "Word required");
            }


            var uri = GetUri(word);
            var json = _client.GetStreamAsync(uri);
            var result = await JsonSerializer.DeserializeAsync<DefinitionModel>(await json, _jsonOptions);



            // TODO: should a library throw of return error details?  public (error, result) Get...


            // if(result is null || result.Result is null || result.Result.Count > 0)
            // {
            //     Debug.Assert(false, "Unexpected API result");
            // }

            //var words = result.Result.Select(r => new WordDefinition(r));



            return result.Result;
        }



        private Uri GetUri(string word) =>
            new Uri
            (
                string.Format
                (
                    "{0}?uid={1}&tokenid={2}&word={3}&format=json",
                    BaseAddress,
                    _credentials.UserId,
                    _credentials.TokenId,
                    Uri.EscapeDataString(word)
                )
            )
        ;
    }
}
