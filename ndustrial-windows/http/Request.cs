using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace com.ndustrialio.api.http
{
    public class Request
    {

        public const string BASE_URL = "https://api.ndustrial.io";
        public const string URLENCODED_CONTENT_TYPE = "application/x-www-form-urlencoded";
        public const string JSON_CONTENT_TYPE = "application/json";

        private string _uri, _method, _contentType, _baseURL = Request.BASE_URL;
        private object _body;
        private bool _authorize = true, _version;
         
        public Dictionary<String, String> _params;

        public Request(String uri=null, 
            Dictionary<String, String> parameters=null, 
            object body=null, 
            string content_type=null,
            bool version = true)
        {
            _uri = uri;
            _body = body;
            _params = parameters;
            _contentType = content_type;
            _version = version;
        }

        public string ContentType
        {
            get { return _contentType; }
            set { _contentType = value; }
        }

        public string BaseURL
        {
            get { return _baseURL; }
            set { _baseURL = value; }
        }

        public bool Authorize
        {
            get { return _authorize; }
            set { _authorize = value; }
        }

        public string Method
        {
            get { return _method; }
            set { _method = value; }
        }

        public HttpWebRequest generate()
        {
            // Build query string
            StringBuilder sb = new StringBuilder(_baseURL);

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

            HttpWebRequest ret = (HttpWebRequest)WebRequest.Create(sb.ToString());

            // Set verb
            ret.Method = _method;

            // Handle body
            if (_body != null)
            {
                ret.ContentType = _contentType;

                String bodyString;

                if (_contentType == Request.URLENCODED_CONTENT_TYPE)
                {
                    bodyString = urlEncode((Dictionary<String, String>)_body);
                } else
                {
                    bodyString = (String)_body;
                }

                UTF8Encoding encoding = new UTF8Encoding();

                byte[] bodyBytes = encoding.GetBytes(bodyString);

                ret.ContentLength = bodyBytes.Length;

                Stream s = ret.GetRequestStream();

                s.Write(bodyBytes, 0, bodyBytes.Length);

                s.Close();
            }

            return ret;
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



}
