using System;
using System.IO;
using Newtonsoft.Json.Linq;
using com.ndustrialio.api.services;
using com.ndustrialio.api.http;

namespace com.ndustrialio.api
{
	public class NdustrialIoApi
	{
        private Client _client;

		// Services
		private Service _feedService, 
            _oauthService,
            _nGestService;
		
		public NdustrialIoApi(string accessToken=null,
            string refreshToken=null,
            string clientID=null,
            string clientSecret=null)
		{
            // Can provide token via argument, environment, or file
            if (accessToken == null)
            {
                Console.WriteLine("Checking environment for token..");

                // Check environment
                accessToken = Environment.GetEnvironmentVariable("ACCESS_TOKEN");

                if (accessToken == null)
                {
                    Console.WriteLine("Checking for token file..");

                    try
                    {
                        JObject tokenObject = JObject.Parse(File.ReadAllText("tokenfile"));

                        accessToken = (String)tokenObject["access_token"];
                        refreshToken = (String)tokenObject["refresh_token"];
                        clientID = (String)tokenObject["client_id"];
                        clientSecret = (String)tokenObject["client_secret"];
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Unable to read token data from file, " + e.Message);

                        Environment.Exit(1);
                    }

                } else
                {
                    refreshToken = Environment.GetEnvironmentVariable("REFRESH_TOKEN");
                    clientID = Environment.GetEnvironmentVariable("CLIENT_ID");
                    clientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET");
                }
            }

            // Instantiate client.  Service objects will share this. 
            _client = new Client(accessToken, refreshToken, clientID, clientSecret);


			_feedService = new Feeds(_client);
            _oauthService = new Oauth(_client);
            _nGestService = new Ngest(_client);
		}

        public Feeds FEEDS
        {
            get { return (Feeds)_feedService; }
        }

        public Oauth OAUTH
        {
            get { return (Oauth)_oauthService; }
        }

        public Ngest NGEST
        {
            get { return (Ngest)_nGestService; }
        }
		
	
	}	
}

