using Stands4.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace Stands4
{
    public class DefinitionCheckFailedException : Exception
    {
        internal DefinitionCheckFailedException(string message)
            : base(message)
        { }

        internal DefinitionCheckFailedException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }


    /// <summary>
    /// TODO: fill me in
    /// </summary>
    public class DefinitionClient
    {
        const string BaseAddress = "https://www.stands4.com/services/v2/defs.php";

        readonly ApiCredentials _credentials;

        readonly HttpClient _client = new();

        readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };


        internal DefinitionClient(ApiCredentials credentials) => (_credentials) = (credentials);


        public async Task<DefinitionModel> CheckDefinition(string word)
        {
            if(word.Length == 0)
                throw new ArgumentException("Please provide a word to lookup", nameof(word))
            ;


            var uri = new ClientUriBuilder(BaseAddress)
                .AddCredentials(_credentials)
                .SetFormat("json")
                .SetWord(word)
                .Build()
            ;


            try
            {
                var result = await _client.GetFromJsonAsync<DefinitionModelInternal>(uri, _jsonOptions);


                if(result is null)
                    throw new DefinitionCheckFailedException("Definition not found");

                if( ! string.IsNullOrEmpty(result.ErrorMessage) )
                    throw new DefinitionCheckFailedException(result.ErrorMessage);


                return result.GetDefinitionModel();
            }
            catch(DefinitionCheckFailedException)
            {
                throw;
            }
            catch(Exception e)
            {
                throw new DefinitionCheckFailedException("Cannot download definition", e);
            }
        }
    }
}
