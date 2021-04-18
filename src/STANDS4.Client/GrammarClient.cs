using Stands4.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;


namespace Stands4
{
    /// <summary>
    /// Spelling and Grammar check powered by the STANDS4 Grammar API.
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


        internal GrammarClient(ApiCredentials credentials)
            : this(credentials, DefaultLanguageCode)
        { }

        internal GrammarClient(ApiCredentials credentials, string languageCode) =>
            (_credentials, _languageCode) = (credentials, languageCode)
        ;


        public async Task<GrammarCheckResponse> TryCheckGrammar(string text)
        {
            try
            {
                if(text.Length == 0)
                    throw new Exception(NoTextToCheckValidationMessage);


                var uri = new ClientUriBuilder(BaseAddress)
                    .AddCredentials(_credentials)
                    .SetLanguage(_languageCode)
                    .SetFormat(RequestFormat)
                    .SetText(text)
                    .Build()
                ;


                var json = await GetJsonOrThrow(uri);
                var result = JsonSerializer.Deserialize<GrammarCheck>(json, _jsonOptions);

                if(result is null || result.Matches is null)
                    throw new Exception("Unable to download grammar check results");


                return new GrammarCheckResponse(result);
            }
            catch(Exception e)
            {
                return new GrammarCheckResponse()
                {
                    ErrorMessage = e.Message
                };
            }
        }


        private async Task<string> GetJsonOrThrow(Uri uri)
        {
            const int streamEmpty = -1;
            const int streamContainsXml = 60; // char 60 is "<".  Payload isn't JSON.  STANDS4 returns errors in XML (<?XML...>).
            using var streamReader = new StreamReader( await _client.GetStreamAsync(uri) );


            return streamReader.Peek() switch
            {
                streamEmpty         => throw new Exception("Unable to download grammar check results"),
                streamContainsXml   => throw new Exception(await streamReader.ReadToEndAsync()),
                _                   => await streamReader.ReadToEndAsync()
            };
        }
    }
}
