using System;
using System.Collections.Generic;
using System.Linq;


namespace Stands4
{
    internal enum QueryStringKey
    {
        UID,
        TokenId,
        Format,
        Lang,
        Text,
    }


    internal class GrammarUriBuilder
    {
        const string _baseAddress = "https://www.stands4.com/services/v2/grammar.php";

        readonly Dictionary<QueryStringKey, string> _queryString = new()
        {
            { QueryStringKey.UID,     string.Empty },
            { QueryStringKey.TokenId, string.Empty },
            { QueryStringKey.Format,  "json"       },
            { QueryStringKey.Lang,    "en-US"      },
            { QueryStringKey.Text,    string.Empty },
        };


        internal GrammarUriBuilder AddCredentials(ApiCredentials credentials) =>
            this
                .AddUserId(credentials.UserId)
                .AddTokenId(credentials.TokenId)
        ;

        internal GrammarUriBuilder AddUserId(string uId) => SetQueryString(QueryStringKey.UID, uId);

        internal GrammarUriBuilder AddTokenId(string tokenId) => SetQueryString(QueryStringKey.TokenId, tokenId);

        internal GrammarUriBuilder AddText(string text) => SetQueryString(QueryStringKey.Text, text);

        internal GrammarUriBuilder SetLanguage(string language) => SetQueryString(QueryStringKey.Lang, language);

        internal GrammarUriBuilder SetFormat(string format) => SetQueryString(QueryStringKey.Format, format);

        internal Uri Build() => new Uri($"{_baseAddress}?{GetQueryString()}");


        private string GetQueryString()
        {
            ThrowIfAnyKeysEmpty();
            return string.Join('&', _queryString.Select(qs => ConvertFromKeyValuePair(qs)));


            string ConvertFromKeyValuePair(KeyValuePair<QueryStringKey, string> kvp) =>
                $"{ kvp.Key.ToString().ToLower() }={ kvp.Value }"
            ;

            void ThrowIfAnyKeysEmpty()
            {
                var exceptions = new List<string>();

                foreach(var (k, v) in _queryString)
                    if(v == string.Empty)
                        exceptions.Add($"Required parameter {k} missing");

                if(exceptions.Count > 0)
                    throw new MissingParametersException(string.Join('\n', exceptions));
            }
        }

        private GrammarUriBuilder SetQueryString(QueryStringKey key, string value)
        {
            _queryString[key] = Uri.EscapeDataString(value);
            return this;
        }
    }


    public class MissingParametersException : Exception
    {
        public MissingParametersException(string message) : base(message)
        { }
    }
}
