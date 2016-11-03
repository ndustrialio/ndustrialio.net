

namespace com.ndustrialio.api.http
{
    public class APIResponse
    {
        public int statusCode;
        public string statusDescription;
        public string responseData;
        

        public APIResponse(int status, string description, string data)
        {
            statusCode = status;
            statusDescription = description;
            responseData = data;
        }

        public override string ToString()
        {
            return responseData;
        }

        public bool isSuccess()
        {
            return (statusCode < 400);
        }

    }

    
}
