using System.Collections.Generic;
using System.Text;

namespace com.ndustrialio.api.http
{

    public static class RawHTTPRequest
    {
        public static string USER_AGENT="ndustrialio.net HTTP client/0.1";

        public static string GET(string uri, string base_url, 
                                    Dictionary<string, string> headers)
        {
            StringBuilder sb = new StringBuilder("GET ");
            sb.Append(uri);
            sb.Append("HTTP 1.1\r\n");
            sb.Append("Host: " + base_url + "\r\n");
            sb.Append("User-Agent: " + USER_AGENT + "\r\n");
            foreach(KeyValuePair<string, string> header in headers)
            {
                sb.Append(header.Key + ": " + header.Value + "\r\n");
            }
            sb.Append("Content-Length: 0\r\n");
            sb.Append("\r\n");

            return sb.ToString();
        }

        public static string POST(string uri, string base_url, 
                                    Dictionary<string, string> headers, 
                                    string body)
        {
            int content_length = Encoding.UTF8.GetByteCount(body);

            StringBuilder sb = new StringBuilder("POST ");
            sb.Append(uri);
            sb.Append("HTTP 1.1\r\n");
            sb.Append("Host: " + base_url + "\r\n");
            sb.Append("User-Agent: " + USER_AGENT + "\r\n");
            foreach(KeyValuePair<string, string> header in headers)
            {
                sb.Append(header.Key + ": " + header.Value + "\r\n");
            }
            sb.Append("Content-Length: "+content_length+"\r\n");
            sb.Append("\r\n");

            sb.Append(body);

            return sb.ToString();
        }

    }



}