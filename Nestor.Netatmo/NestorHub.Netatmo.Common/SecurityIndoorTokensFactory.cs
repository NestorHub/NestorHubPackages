namespace NestorHub.Netatmo.Common
{
    public class SecurityIndoorTokensFactory : BaseTokensFactory
    {
        public SecurityIndoorTokensFactory(string username, string password, string clientId, string clientSecret) : 
            base("read_camera", username, password, clientId, clientSecret)
        {}
    }
}
