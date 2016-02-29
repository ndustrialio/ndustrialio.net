
using System.Net;
using System.IO;


namespace com.ndustrialio.api.http
{
    public class Response
    {
        public HttpStatusCode statusCode;
        public string statusDescription;
        public string responseData;
        

        public Response(HttpStatusCode status, string description, string data)
        {
            statusCode = status;
            statusDescription = description;
            responseData = data;
        }

        public override string ToString()
        {
            return responseData;
        }

    }

    
}
