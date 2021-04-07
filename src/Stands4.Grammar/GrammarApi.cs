using System;
using System.Collections.Generic;


namespace Stands4
{
    public class GrammarApi
    {
        static GrammarApi _instance = new GrammarApi();

        static ApiCredentials? _credentials;


        public static GrammarApi AddCredentials(string userId, string tokenId)
        {
            ValidateArgs();

            _credentials = new ApiCredentials(userId, tokenId);

            return _instance;


            void ValidateArgs()
            {
                var exceptions = new List<string>();

                if(userId == string.Empty)
                    exceptions.Add("User id is required");

                if(tokenId == string.Empty)
                    exceptions.Add("Token id is required");

                if(exceptions.Count > 0)
                    throw new GrammarApiException(string.Join('\n', exceptions));
            }
        }


        public GrammarClient GetGrammarClient()
        {
            if(_credentials is null)
                throw new GrammarApiException("User and token ids are required");


            return new GrammarClient(_credentials);
        }
    }


    public class GrammarApiException: Exception
    {
        public GrammarApiException(string message) : base(message)
        { }
    }
}
