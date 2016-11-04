
using System;
using System.Threading.Tasks;
using com.ndustrialio.api.http;



namespace com.ndustrialio.api.services
{

    public abstract class APIService
    {
        protected string _accessToken;

        public abstract string baseURL();


        public APIResponse execute(APIRequest request)
        {
            // Set baseURL
            request.baseURL(baseURL());

            // Set headers
            if (request.authorize())
            {
                request.headers("Authorization", "Bearer " + _accessToken);
            }

            // Synchronous execute
            var httpResponse = request.executeImpl().Result;

            APIResponse response = new APIResponse((int)httpResponse.StatusCode,
                                                httpResponse.ReasonPhrase, 
                                                httpResponse.Content.ReadAsStringAsync().Result);

            if (!response.isSuccess())
            {
                throw new Exception(response.responseData);
            } else
            {
                return response;
            }                         

        }

        public async Task<APIResponse> executeAsync(APIRequest request)
        {
            // Set authorization headers
            if (request.authorize())
            {
                request.headers("Authorization", "Bearer " + _accessToken);
            }

            var httpResponse = await request.executeImpl();


            return new APIResponse((int)httpResponse.StatusCode,
                        httpResponse.ReasonPhrase, 
                        httpResponse.Content.ReadAsStringAsync().Result);
        }
        
    }

}
