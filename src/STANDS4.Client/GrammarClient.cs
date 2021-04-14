using Stands4.Grammar.Exceptions;
using Stands4.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;


namespace Stands4
{
    /// <summary>
    /// TODO:...
    /// <exception cref=nameof(GrammarCheckEmptyResultException)>description</exception>
    /// <exception cref=nameof(GrammarCheckFailedException)>description</exception>
    /// <exception cref=nameof(GrammarCheckValidationException)>description</exception>
    /// </summary>
    public class GrammarClient
    {
        const string NoTextToCheckValidationMessage = "Text to check is required";

        const string RequestFormat = "json";

        const string DefaultLanguageCode = "en-US";

        const string BaseAddress = "https://www.stands4.com/services/v2/grammar.php";

        readonly ApiCredentials _credentials;

        readonly string _languageCode;

        readonly HttpClient _client = new();

        readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };


        public GrammarClient(ApiCredentials credentials)
            : this(credentials, DefaultLanguageCode)
        { }

        public GrammarClient(ApiCredentials credentials, string languageCode) =>
            (_credentials, _languageCode) = (credentials, languageCode)
        ;


        public async Task<GrammarCheckModel> CheckGrammar(string textToCheck)
        {
            if(textToCheck.Length == 0)
                throw new GrammarCheckValidationException(NoTextToCheckValidationMessage);


            try
            {
                var uri = new ClientUriBuilder(BaseAddress)
                    .AddCredentials(_credentials)
                    .SetLanguage(_languageCode)
                    .SetFormat(RequestFormat)
                    .Build()
                ;

                var json = await GetJsonOrThrow(uri);
                var result = JsonSerializer.Deserialize<GrammarCheckModel>(json, _jsonOptions);


                return result ?? throw new GrammarCheckEmptyResultException();
            }
            catch(Exception e)
            {
                // wrap this error as the client isn't interested in implementation issues
                if(e is JsonException)
                    throw new GrammarCheckEmptyResultException(e);

                throw;
            }
        }


        private async Task<string> GetJsonOrThrow(Uri uri)
        {
            const int streamEmpty = -1;
            const int streamContainsXml = 60; // char 60 is "<".  Payload isn't JSON.  STANDS4 returns errors in XML (<?XML...>).
            using var streamReader = new StreamReader( await _client.GetStreamAsync(uri) );


            return streamReader.Peek() switch
            {
                streamEmpty         => throw new GrammarCheckEmptyResultException(),
                streamContainsXml   => throw new GrammarCheckFailedException(await streamReader.ReadToEndAsync()),
                _                   => await streamReader.ReadToEndAsync()
            };
        }
    }
}