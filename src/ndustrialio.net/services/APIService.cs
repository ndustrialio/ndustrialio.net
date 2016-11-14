
using System;
using System.Threading.Tasks;
using com.ndustrialio.api.http;



namespace com.ndustrialio.api.services
{

    public abstract class APIService
    {
        protected string _accessToken;

        // By default, throw exception on HTTP error
        protected bool _exceptOnError = true;

        public abstract string BaseURL
        {
            get;
        }


        public bool ExceptionOnHttpError
        {
            get {return _exceptOnError;}
            set {_exceptOnError = value;}
        }

        public virtual APIResponse execute(APIRequest request)
        {
            // Set baseURL
            request.baseURL(this.BaseURL);

            // Set headers
            if (request.authorize())
            {
                request.headers("Authorization", "Bearer " + _accessToken);
            }

            // Synchronous execute
            var httpResponse = request.executeImpl().Result;

            APIResponse response = new APIResponse((int)httpResponse.StatusCode,
                                                httpResponse.ReasonPhrase, 
                                                httpResponse.Content.ReadAsStringAsync().Result, 
                                                _exceptOnError);                     

            return response;
        }

        public virtual async Task<APIResponse> executeAsync(APIRequest request)
        {
            // Set authorization headers
            if (request.authorize())
            {
                request.headers("Authorization", "Bearer " + _accessToken);
            }

            var httpResponse = await request.executeImpl();


            return new APIResponse((int)httpResponse.StatusCode,
                        httpResponse.ReasonPhrase, 
                        httpResponse.Content.ReadAsStringAsync().Result,
                        _exceptOnError);
        }
        
    }

}
