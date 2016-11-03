
using com.ndustrialio.api.http;


namespace com.ndustrialio.api.services
{

    public abstract class APIService
    {
        protected Client _client;

        public abstract string baseURL();

        public APIResponse execute(APIRequest request)
        {
            // Set base url
            request.baseURL(baseURL());

            return _client.execute(request);
        }
        
    }

}
