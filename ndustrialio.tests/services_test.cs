using System;
using NUnit.Framework;
using com.ndustrialio.api.services;

namespace com.ndustrialio.tests.services
{
    [TestFixture]
    public class ContxtAuth
    {
        private string client_id = Environment.GetEnvironmentVariable("CLIENT_ID");
        private string client_secret = Environment.GetEnvironmentVariable("CLIENT_SECRET");
        private string audience = "1HD1NG1VTBtkqRt2HRRj3E3hdmqmwzoz";

        private string GetToken()
        {
            ContxtAuthService test_service = new ContxtAuthService();
            var access_token = test_service.getAccessToken(client_id, client_secret, audience);
            return access_token;
        }

        [Test]
        public void IsValidToken()
        {
            Assert.IsNotEmpty(GetToken());
        }
    }
}