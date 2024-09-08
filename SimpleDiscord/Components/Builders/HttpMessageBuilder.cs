using System;
using System.Net.Http;
using System.Text;

namespace SimpleDiscord.Components.Builders
{
    internal class HttpMessageBuilder()
    {
        private readonly HttpRequestMessage requestMessage = new();

        public HttpMessageBuilder SetUri(string uri)
        {
            requestMessage.RequestUri = new(uri);
            return this;
        }

        public HttpMessageBuilder SetUri(Uri uri)
        {
            requestMessage.RequestUri = uri;
            return this;
        }

        public HttpMessageBuilder SetHeader(string name, string value)
        {
            requestMessage.Headers.TryAddWithoutValidation(name, value);
            return this;
        }

        public HttpMessageBuilder SetContent(HttpContent content)
        {
            requestMessage.Content = content;
            return this;
        }

        public HttpMessageBuilder SetJsonContent(string json)
        {
            requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return this;
        }

        public HttpMessageBuilder SetMethod(string method)
        {
            requestMessage.Method = new(method);
            return this;
        }

        public HttpMessageBuilder SetMethod(HttpMethod method)
        {
            requestMessage.Method = method;
            return this;
        }

        public static HttpMessageBuilder New() => new();

        public static implicit operator HttpRequestMessage(HttpMessageBuilder builder) => builder.requestMessage;
    }
}
