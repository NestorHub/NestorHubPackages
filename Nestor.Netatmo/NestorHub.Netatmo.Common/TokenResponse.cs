using Newtonsoft.Json;

namespace NestorHub.Netatmo.Common
{
    public class TokenResponse
    {
        public string AccessToken { get; }
        public long ExpiresIn { get; }
        public string RefreshToken { get; }

        [JsonConstructor]
        public TokenResponse(string access_token, string expires_in, string refresh_token)
        {
            AccessToken = access_token;
            if (long.TryParse(expires_in, out var expireData))
            {
                ExpiresIn = expireData;
            }

            RefreshToken = refresh_token;
        }
    }
}