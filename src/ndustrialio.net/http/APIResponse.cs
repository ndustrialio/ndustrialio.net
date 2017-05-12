using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections;

namespace com.ndustrialio.api.http
{
    public class PagedResponse<T>
    {
        public class MetaData
        {
            public int offset {get; set;}
            public int totalRecords {get; set;}
        }
        public List<T> records {get; set;}

        public MetaData _metadata {get; set;}


    }


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
                throw new Exception(ToString());
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


        public bool isSuccess()
        {
            return (_statusCode < 400);
        }

    }

    
}
