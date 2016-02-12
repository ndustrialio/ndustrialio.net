
using System.Net;
using System.IO;


namespace com.ndustrialio.api.http
{
    public class Response
    {
        public HttpStatusCode statusCode;
        public string statusDescription;
        public Stream responseStream;
        

        public Response(HttpStatusCode status, string description, Stream stream)
        {
            statusCode = status;
            statusDescription = description;
            responseStream = stream;
        }

        public override string ToString()
        {
            using (var reader = new StreamReader(responseStream))
            {
                return reader.ReadToEnd();
            }
        }

    }

    
}
