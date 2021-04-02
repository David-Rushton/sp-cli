using System;
using System.Collections.Generic;


namespace Stands4.Grammar
{
    public class ApiCredentials
    {
        public ApiCredentials(string userId, string tokenId)
        {
            var exceptionMessages = new List<string>();

            if(userId == string.Empty)
                exceptionMessages.Add("UserId required");

            if(tokenId == string.Empty)
                exceptionMessages.Add("TokenId required");

            if(exceptionMessages.Count > 0)
                throw new ApiCredentialsInvalidException(string.Join('\n', exceptionMessages));


            (UserId, TokenId) = (userId, tokenId);
        }


        internal string UserId { get; init; }

        internal string TokenId { get; init; }
    }


    public class ApiCredentialsInvalidException : Exception
    {
        public ApiCredentialsInvalidException(string message) : base(message)
        { }
    }
}
