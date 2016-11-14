using System;
using Newtonsoft.Json.Linq;

namespace com.ndustrialio.api.http
{
    public class APIResponse
    {
        protected int _statusCode;
        protected string _statusDescription;
        protected string _responseData;

        
        

        public APIResponse(int status, string description, string data, bool exceptOnError)
        {
            _statusCode = status;
            _statusDescription = description;
            _responseData = data;

            // Throw exception on HTTP error if we need to
            if (exceptOnError && !isSuccess())
            {
                throw new Exception(this.toJSON());
            }
        }

        public APIResponse(int status, string description, string data) : this(status, description, data, true) {}

        public int StatusCode
        {
            get { return _statusCode;}
        }

        public string StatusDescription
        {
            get { return _statusDescription;}
        }

        public string ResponseData
        {
            get { return _responseData;}
        }

        public override string ToString()
        {
            return _responseData;
        }

        public string toJSON()
        {
            return new JObject(new JProperty("status_code", StatusCode),
                                new JProperty("response_data", ResponseData),
                                new JProperty("status_description", StatusDescription))
                                .ToString();
        }


        public bool isSuccess()
        {
            return (_statusCode < 400);
        }

    }

    
}
