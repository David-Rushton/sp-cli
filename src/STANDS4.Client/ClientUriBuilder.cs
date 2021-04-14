using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace Stands4
{
    public class EmptyParameterException : Exception
    {
        internal EmptyParameterException(string message)
            : base(message)
        { }
    }


    internal class ClientUriBuilder
    {
        readonly string _baseAddress;

        readonly Dictionary<string, string> _queryStringItems = new();


        internal ClientUriBuilder(string baseAddress) => (_baseAddress) = (baseAddress);


        internal ClientUriBuilder AddCredentials(ApiCredentials credentials)
        {
            _queryStringItems.Add("uid", credentials.UserId);
            _queryStringItems.Add("tokenid", credentials.TokenId);

            return this;
        }

        internal ClientUriBuilder SetFormat(string format)
        {
            Debug.Assert(format is "json" or "xml", $"format {format} not supported");

            AddQueryStringItem("format", format);

            return this;
        }

        internal ClientUriBuilder SetLanguage(string languageCode)
        {
            AddQueryStringItem("lang", languageCode);
            return this;
        }

        internal ClientUriBuilder SetText(string text)
        {
            AddQueryStringItem("text", text);
            return this;
        }

        internal ClientUriBuilder SetWord(string word)
        {
            AddQueryStringItem("word", word);
            return this;
        }

        internal Uri Build() => new Uri($"{ _baseAddress }?{ GetQueryString() }");


        private void AddQueryStringItem(string key, string value)
        {
            // public facing methods to ensure these assertions never fail
            // TODO: a test project would help enforce this
            Debug.Assert(key.Length > 0, "Parameter key required but not provided");
            Debug.Assert(value.Length > 0, $"Parameter {key} value required but not provided");


            _queryStringItems.Add(key, Uri.EscapeDataString(value));
        }

        private string GetQueryString() =>
            string.Join
            (
                '&',
                from kvp in _queryStringItems
                select $"{ kvp.Key }={ kvp.Value }"
            )
        ;
    }
}
