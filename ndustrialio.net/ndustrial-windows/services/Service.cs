
using System.Net;
using com.ndustrialio.api.errors;
using com.ndustrialio.api.http;

namespace com.ndustrialio.api.services
{

    // Base class for data returned from services
    public class ApiData {}


    public abstract class Service
    {
        private Client _client;

        public Service(Client client)
        {
            _client = client;
        }

        protected Response _get(Request req)
        {
            req.Method = WebRequestMethods.Http.Get;

            return performRequest(req);
        }

        protected Response _put(Request req)
        {
            req.Method = WebRequestMethods.Http.Put;

            return performRequest(req);
        }

        protected Response _post(Request req)
        {
            req.Method = WebRequestMethods.Http.Post;

            return performRequest(req);
        }

        protected Response _delete(Request req)
        {
            // Microsoft does not have an enumeration for DELETE 
            req.Method = "DELETE";

            return performRequest(req);
        }

        private Response performRequest(Request request)
        {
            try
            {
                return _client.execute(request);
            }
            catch (InvalidAcccessTokenException e)
            {
                if (_client.AutoRefresh)
                {
                    // Attempt token refresh
                    _client.refreshToken();

                    // We won't catch any exceptions here
                    return _client.execute(request);
                } else
                {
                    // Unable to refresh token
                    throw e;
                }
            }
        }

    }

}