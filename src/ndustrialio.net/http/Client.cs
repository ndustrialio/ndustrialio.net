using System;


namespace com.ndustrialio.api.http
{
    public class Client
    {
        private String _accessToken;

        public Client(String accessToken)
        {
            _accessToken = accessToken;

        }

        public APIResponse execute(APIRequest request)
        {
            if (request.authorize())
            {
                request.headers("Authorization", "Bearer " + _accessToken);
            }

            APIResponse response = request.execute();

            if (!response.isSuccess())
            {
                throw new Exception(response.responseData);
            }

            return response;

        }
    }
}
