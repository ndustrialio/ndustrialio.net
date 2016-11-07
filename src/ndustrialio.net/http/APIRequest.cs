using System;
using System.Collections.Generic;
using System.Text;
//using System.Net.Sockets;
using System.Net.Http; 
using System.Threading.Tasks;


namespace com.ndustrialio.api.http
{
    public abstract class APIRequest
    {

        public const string URLENCODED_CONTENT_TYPE = "application/x-www-form-urlencoded";
        public const string JSON_CONTENT_TYPE = "application/json";

        protected string _uri, _contentType, _baseURL;
        protected string _body;
        protected bool _authorize = true, _version = true;
         
        protected HttpMethod _method;
        protected Dictionary<string, string> _params, _headers;

        protected HttpClient _client;

        public APIRequest(String uri=null, 
            Dictionary<String, String> parameters=null, 
            string body=null, 
            string content_type=null)
        {
            _uri = uri;
            _body = body;
            _params = parameters;
            _contentType = content_type;

            _headers = new Dictionary<string, string>();

            _client = new HttpClient();
        }

        public APIRequest contentType(string content_type)
        {
            _contentType = content_type;
            return this;
        }

        public string contentType()
        {
            return _contentType;
        }

        public APIRequest baseURL(string base_url)
        {
            _baseURL = base_url;
            _client.BaseAddress = new Uri(_baseURL);
            return this;
        }

        public string baseURL()
        {
            return _baseURL;
        }

        public APIRequest version(bool version)
        {
            _version = version;
            return this;
        }

        public bool version()
        {
            return _version;
        }
        public APIRequest authorize(bool authorize)
        {
            _authorize = authorize;
            return this;
        }

        public bool authorize()
        {
            return _authorize;
        }

        public APIRequest headers(string key, string value)
        {
            _headers.Add(key, value);

            return this;
        }

        public Dictionary<string, string> headers()
        {
            return _headers;
        }

        public APIRequest method(HttpMethod method)
        {
            _method = method;
            return this;
        }

        public HttpMethod method()
        {
            return _method;
        }

        protected string buildURI()
        {
            // Build query string
            StringBuilder sb = new StringBuilder();

            if (_version)
            {
                sb.Append("/v1/");
            }

            if (_uri != null)
            {
                // Add URI extension
                sb.Append(_uri);
            }

            if (_params != null && _params.Count > 0)
            {
                sb.Append("?");
                
                sb.Append(urlEncode(_params));
            }

            return sb.ToString();

        }

        protected HttpRequestMessage toHttpRequestMessage()
        {
            HttpRequestMessage ret = new HttpRequestMessage(_method, buildURI());

            foreach(KeyValuePair<string, string> header in _headers)
            {
                ret.Headers.Add(header.Key, header.Value);
            }

            return ret;
        }

        public abstract Task<HttpResponseMessage> executeImpl();

        private String urlEncode(Dictionary<String, String> args)
        {
            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<String, String> entry in args)
            {
                sb.Append(entry.Key + '=' + entry.Value);
                sb.Append("&");
            }

            // Remove trailing "&"
            sb.Length--;

            return sb.ToString();
        }
    }



    public class GET : APIRequest
    {
        public GET(String uri=null, 
            Dictionary<String, String> parameters=null, 
            string body=null, 
            string content_type=null) : base(uri, parameters, body, content_type) 
            {
                method(HttpMethod.Get);
            }

        public override Task<HttpResponseMessage> executeImpl()
        {
            return _client.SendAsync(this.toHttpRequestMessage());
        }
    }

    public class POST : APIRequest
    {
        public POST(String uri=null, 
            Dictionary<String, String> parameters=null, 
            string body=null, 
            string content_type=null) : base(uri, parameters, body, content_type) 
            {
                method(HttpMethod.Post);

                // this can be changed later 
                contentType(JSON_CONTENT_TYPE);
            }

        public override Task<HttpResponseMessage> executeImpl()
        {
            HttpRequestMessage message = this.toHttpRequestMessage();

            message.Content = new StringContent(_body, Encoding.UTF8, contentType());

            return _client.SendAsync(message);
        }
    }

    public class PUT : APIRequest
    {
        public PUT(String uri=null, 
            Dictionary<String, String> parameters=null, 
            string body=null, 
            string content_type=null) : base(uri, parameters, body, content_type) 
            {
                // this can be changed later 
                contentType(JSON_CONTENT_TYPE);

                method(HttpMethod.Put);
            }

        public override Task<HttpResponseMessage> executeImpl()
        {
            HttpRequestMessage message = this.toHttpRequestMessage();

            message.Content = new StringContent(_body, Encoding.UTF8, contentType());

            return _client.SendAsync(message);
        }
    }

    public class DELETE : APIRequest
    {
        public DELETE(String uri=null, 
            Dictionary<String, String> parameters=null, 
            string body=null, 
            string content_type=null) : base(uri, parameters, body, content_type) 
            {
                method(HttpMethod.Delete);
            }


        public override Task<HttpResponseMessage> executeImpl()
        {
            return _client.SendAsync(this.toHttpRequestMessage());
        }
    }
}
