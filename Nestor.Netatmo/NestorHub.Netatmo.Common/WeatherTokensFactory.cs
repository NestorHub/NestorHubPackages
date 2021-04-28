namespace NestorHub.Netatmo.Common
{
    public class WeatherTokensFactory : BaseTokensFactory
    {
        public WeatherTokensFactory(string username, string password, string clientId, string clientSecret) : 
            base("read_station", username, password, clientId, clientSecret)
        {}
    }
}
