using System.Collections.Generic;

namespace NestorHub.Netatmo.Common
{
    public static class TokenPayload
    {
        public static List<KeyValuePair<string, string>> GetFirstTokenPayload(string clientId, string clientSecret, string username, string password, string scope)
        {
            var list = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("scope", scope)
            };
            return list;
        }

        public static List<KeyValuePair<string, string>> GetRefreshTokenPayload(string clientId, string clientSecret, string refresh_token)
        {
            var list = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refresh_token)
            };
            return list;
        }
    }
}