using Stands4.Converters;
using Stands4.DTO;
using Stands4.Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;


namespace Stands4
{
    /// <summary>
    /// Download word definitions from the STANDS4 dictionary definition API.
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


        public async Task<DefinitionResponse> TryGetDefinition(string word)
        {
            try
            {
                if(string.IsNullOrEmpty(word))
                    throw new Exception("Word is required");


                var uri = new ClientUriBuilder(BaseAddress)
                    .AddCredentials(_credentials)
                    .SetFormat("json")
                    .SetWord(word)
                    .Build()
                ;
                var response = await _client.GetFromJsonAsync<DictionaryDTO>(uri, _jsonOptions);

                if(response is null)
                    throw new Exception("Definition download failed");


                return new DictionaryDtoConvertor().ConvertToModel(response);
            }
            catch(Exception e)
            {
                return new DefinitionResponse
                {
                    Status = DefinitionResponseStatus.RequestFailed,
                    ErrorMessage = e.Message
                };
            }
        }
    }
}
