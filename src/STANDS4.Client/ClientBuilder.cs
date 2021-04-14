using System;


namespace Stands4
{
    /// <summary>
    /// Builds the STANDS4 clients.
    /// </summary>
    public class ClientBuilder
    {
        readonly ApiCredentials _credentials;


        internal ClientBuilder(ApiCredentials credentials) => (_credentials) = (credentials);


        /// <summary>
        /// Returns a new client builder.
        /// </summary>
        /// <param name="userId">Your API user id</param>
        /// <param name="tokenId">Your API token id</param>
        /// <returns>ClientBuilder</returns>
        public static ClientBuilder AddCredentials(string userId, string tokenId) =>
            new ClientBuilder( new ApiCredentials(userId, tokenId) )
        ;


        public DefinitionClient BuildDefinitionClient() => new DefinitionClient(_credentials);

        public GrammarClient BuildGrammarClient() => new GrammarClient(_credentials);

        public GrammarClient BuildGrammarClient(string languageCode) => new GrammarClient(_credentials, languageCode);

        // TODO:
        public void BuildSynonymsClient() => throw new NotImplementedException();
    }
}
