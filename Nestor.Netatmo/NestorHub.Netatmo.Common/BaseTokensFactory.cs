using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;
using NestorHub.Netatmo.Common.Class;
using NestorHub.Netatmo.Common.Exceptions;

namespace NestorHub.Netatmo.Common
{
    public class BaseTokensFactory
    {
        private readonly string _scope;
        private readonly string _username;
        private readonly string _password;
        private readonly string _clientId;
        private readonly string _clientSecret;

        private string _accessToken;
        private long _expiresIn;
        private string _refreshToken;
        private Timer _expiresInTimer;
        private bool _timeToRefresh;

        protected BaseTokensFactory(string scope, string username, string password, string clientId, string clientSecret)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new AuthenticationArgumentException();
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new AuthenticationArgumentException();
            }

            _scope = scope;
            _username = username;
            _password = password;
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        public async Task<string> GetToken()
        {
            if (!string.IsNullOrEmpty(_accessToken))
            {
                return _accessToken;
            }

            var response = _timeToRefresh ? await GetRefreshToken() : await GetFirstAccessToken();

            var tokensResponse = response.ContentAs<TokenResponse>();
            if (!string.IsNullOrEmpty(tokensResponse.AccessToken))
            {
                _accessToken = tokensResponse.AccessToken;
                _expiresIn = tokensResponse.ExpiresIn;
                _refreshToken = tokensResponse.RefreshToken;

                StartExpiresInTimer();

                return _accessToken;
            }
            return string.Empty;
        }

        /// <remarks>
        /// On supprime le token environ 60 secondes avant la fin de l'expiration pour être certain de ne pas utiliser un token jusqu'à la derniére seconde
        /// </remarks>
        private void StartExpiresInTimer()
        {
            if (_expiresIn > 0)
            {
                _expiresInTimer = new Timer(_expiresIn - 60);
                _expiresInTimer.Elapsed += ExpiresInTimerOnElapsed;
                _expiresInTimer.AutoReset = false;
                _expiresInTimer.Start();
            }
        }

        private void ExpiresInTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            _accessToken = string.Empty;
            _timeToRefresh = true;
            _expiresInTimer.Stop();
        }

        private async Task<HttpResponseMessage> GetFirstAccessToken()
        {
            var payload = TokenPayload.GetFirstTokenPayload(_clientId, _clientSecret, _username, _password, _scope);
            var response = await GetAuthenticationTokenOnline(payload);
            return response;
        }

        private async Task<HttpResponseMessage> GetRefreshToken()
        {
            var payload = TokenPayload.GetRefreshTokenPayload(_clientId, _clientSecret, _refreshToken);
            var response = await GetAuthenticationTokenOnline(payload);
            return response;
        }

        private static async Task<HttpResponseMessage> GetAuthenticationTokenOnline(List<KeyValuePair<string, string>> payload)
        {
            var client = HttpClientFactory.Create();
            var response = await client.PostAsync("https://api.netatmo.com/oauth2/token", new FormUrlEncodedContent(payload));
            return response;
        }
    }
}