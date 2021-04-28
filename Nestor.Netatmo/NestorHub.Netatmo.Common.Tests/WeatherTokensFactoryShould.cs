using NestorHub.Netatmo.Common.Exceptions;
using NFluent;
using Xunit;

namespace NestorHub.Netatmo.Common.Tests
{
    public class WeatherTokensFactoryShould
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _username;
        private readonly string _password;

        public WeatherTokensFactoryShould()
        {
            _clientId = "587790346d1dbd5e1c8ba445";
            _clientSecret = "zvUT8N2M6yeARhswSYhCn0THSRM2KsKMQP9afV";
            _username = "mplessis@hotmail.com";
            _password = "zru*p387";
        }

        [Fact]
        public void throw_argument_exception_if_username_and_password_are_empty()
        {
            Check.ThatCode(() => { var token1 = new WeatherTokensFactory("", "password", _clientId, _clientSecret); }).Throws<AuthenticationArgumentException>();
            Check.ThatCode(() => { var token2 = new WeatherTokensFactory("username", "", _clientId, _clientSecret); }).Throws<AuthenticationArgumentException>();
        }

        [Fact]
        public async void return_token_is_not_empty()
        {
            var tokensManager = new WeatherTokensFactory(_username, _password, _clientId, _clientSecret);
            var accessToken = await tokensManager.GetToken();
            Check.That(accessToken).IsNotEmpty();
        }
    }
}
