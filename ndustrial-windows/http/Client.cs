using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using com.ndustrialio.api.errors;
using com.ndustrialio.api.services;
using Newtonsoft.Json;

namespace com.ndustrialio.api.http
{
    public class Client
    {
        private String _accessToken, _refreshToken, _clientID, _clientSecret;

        private bool _autoRefresh;

        public Client(String accessToken, String refreshToken=null,
            String clientID=null, String clientSecret=null)
        {
            _accessToken = accessToken;

            _refreshToken = refreshToken;

            _clientID = clientID;

            _clientSecret = clientSecret;

            if (_refreshToken != null && _clientID != null && _clientSecret != null)
            {
                Console.WriteLine("Automatic token refresh enabled");

                _autoRefresh = true;
            }
            else
            {
                _autoRefresh = false;
            }
        }

        public Response execute(Request request)
        {
            HttpWebRequest webRequest = request.generate();

            if (request.Authorize)
            {
                // Add authorization
                webRequest.Headers.Add("Authorization", "Bearer " + _accessToken);
            }

            try
            {
                // Get response, making sure to dispose of it properly
                HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();

                // Process response
                processResponse(response);

                String data;

                // Get response data
                using (var reader = new StreamReader(response.GetResponseStream(),
                    new System.Text.UTF8Encoding()))
                {
                    data = reader.ReadToEnd();
                }

                response.Close();

                return new Response(response.StatusCode,
                    response.StatusDescription, data);

            } catch (WebException e)
            {
                processResponse((HttpWebResponse)e.Response);
            }


        }

        public bool AutoRefresh
        {
            get { return _autoRefresh; }
        }

        public void refreshToken()
        {
            // Create request 
            Request request = new Request(Oauth.URI, body:new Dictionary<String, String>() {
                    { "refresh_token", _refreshToken },
                    {"client_id", _clientID },
                    {"client_secret", _clientSecret }
                });

            request.Method = WebRequestMethods.Http.Post;

            // Execute
            Response response = execute(request);

            // Get response and fill in values
            TokenData ret = JsonConvert.DeserializeObject<TokenData>(response.ToString());

            _refreshToken = ret.RefreshToken;

            _accessToken = ret.AccessToken;
        }

        private void processResponse(HttpWebResponse response)
        {
            if (response.StatusCode == HttpStatusCode.OK || 
                response.StatusCode == HttpStatusCode.Created ||
                response.StatusCode == HttpStatusCode.NoContent)
            {
                // A-OK!
                return;
            } else if (response.StatusCode == HttpStatusCode.ServiceUnavailable ||
                response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new InvalidAcccessTokenException(response.StatusDescription);
            } else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new InternalServerErrorException(response.StatusDescription);
            } else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new BadRequestException(response.StatusDescription);
            } else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException(response.StatusDescription);
            }
            else
            {
                throw new Exception(response.StatusDescription);
            }
        }

    }
}
