using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace com.ndustrialio.api.http
{
    public abstract class APIRequest
    {

        public const string URLENCODED_CONTENT_TYPE = "application/x-www-form-urlencoded";
        public const string JSON_CONTENT_TYPE = "application/json";

        protected string _uri, _method, _contentType, _baseURL;
        protected object _body;
        protected bool _authorize = true, _version = true;
         
        protected Dictionary<string, string> _params, _headers;

        public APIRequest(String uri=null, 
            Dictionary<String, String> parameters=null, 
            object body=null, 
            string content_type=null)
        {
            _uri = uri;
            _body = body;
            _params = parameters;
            _contentType = content_type;

            _headers = new Dictionary<string, string>();
        }

        public APIRequest contentType(string content_type)
        {
            _headers.Add("Content-Type", content_type);
            return this;
        }

        public string contentType()
        {
            string content_type = null;

            _headers.TryGetValue("Content-Type", out content_type);

            return content_type;
        }

        public APIRequest baseURL(string base_url)
        {
            _baseURL = base_url;
            return this;
        }

        public string baseURL()
        {
            return _baseURL;
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

        public APIRequest method(string method)
        {
            _method = method;
            return this;
        }

        public string method()
        {
            return _method;
        }

        protected string buildURI()
        {
            // Build query string
            StringBuilder sb = new StringBuilder();

            if (_version)
            {
                sb.Append("/v1");
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

        private APIResponse readResponse(Socket socket)

        {
            // Are we done reading HTTP response header yet?
            bool finished = false;
            // Content length of response body
            int content_length = 0;

            // This will be the HTTP response header
            string headerString = "";

            string sentinel = "bbbb";

            while(!finished)
            {
                // Read response byte by byte
                byte[] buffer = new byte[1];
                socket.Receive(buffer, 0, 1, 0);

                string headerBit = Encoding.UTF8.GetString(buffer);
                // Build header string
                headerString += headerBit;

                // Keep track of sentinel
                // Kinda like a fifo, we are looking for 
                // \r\n\r\n
                sentinel = sentinel.Substring(1);
                sentinel += headerBit;

                if (sentinel.Equals("\r\n\r\n"))
                {                    
                    // Header read is complete, 
                    finished = true;
                }
            }

            // Parse header
            string[] header_chunks = headerString.Split(new string[] {"\r\n"}, StringSplitOptions.None);

            // First line is status
            string[] status_chunks = header_chunks[0].Split(' ');
            int status_code = int.Parse(status_chunks[1]);
            string status_description = status_chunks[2];


            // Iterate over remaining header lines
            foreach (string header_chunk in header_chunks)
            {
                if (header_chunk.StartsWith("Content-Lenth"))
                {
                    content_length = int.Parse(header_chunk.Split(':')[1].Trim());
                }
            }


            // Find and parse content length
            // Regex content_regex = new Regex("\\\r\nContent-Length: (.*?)\\\r\n");
            // Match m = content_regex.Match(headerString);
            // content_length = int.Parse(m.Groups[1].ToString());

            // Read body and encode to string
            byte[] body_buffer = new byte[content_length];
            socket.Receive(body_buffer, 0, content_length, 0);
            string body_string = Encoding.UTF8.GetString(body_buffer);
            



            return new APIResponse(status_code, status_description, body_string);
        }

        protected abstract string getRawHTTP();

        public APIResponse execute()
        {
            string rawHTTP = getRawHTTP();

            // Create and connect socket, will automatically dispose of it
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                socket.Connect(_baseURL, 80);

                // SEND HTTP REQUEST
                socket.Send(Encoding.UTF8.GetBytes(rawHTTP));

                // DECODE RESPONSE
                var response = readResponse(socket);

                return response;
            }

        }

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
            object body=null, 
            string content_type=null) : base(uri, parameters, body, content_type) 
            {
                method("GET");
            }

        protected override string getRawHTTP()
        {
            return RawHTTPRequest.GET(buildURI(), _baseURL, _headers);
        }
    }

    public class POST : APIRequest
    {
        public POST(String uri=null, 
            Dictionary<String, String> parameters=null, 
            object body=null, 
            string content_type=null) : base(uri, parameters, body, content_type) 
            {
                method("POST");

                // this can be overridden 
                contentType(JSON_CONTENT_TYPE);
            }

        protected override string getRawHTTP()
        {
            throw new NotImplementedException();
        }
    }

    public class PUT : APIRequest
    {
        public PUT(String uri=null, 
            Dictionary<String, String> parameters=null, 
            object body=null, 
            string content_type=null) : base(uri, parameters, body, content_type) 
            {
                method("PUT");
            }

        protected override string getRawHTTP()
        {
            throw new NotImplementedException();
        }
    }

    public class DELETE : APIRequest
    {
        public DELETE(String uri=null, 
            Dictionary<String, String> parameters=null, 
            object body=null, 
            string content_type=null) : base(uri, parameters, body, content_type) 
            {
                method("DELETE");
            }

        protected override string getRawHTTP()
        {
            throw new NotImplementedException();
        }
    }


}
