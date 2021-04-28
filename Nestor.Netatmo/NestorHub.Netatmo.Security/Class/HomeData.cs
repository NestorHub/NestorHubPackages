using System.Collections.Generic;

namespace NestorHub.Netatmo.Security.Class
{
    public class HomeData
    {
        public IEnumerable<Home> Homes { get; }

        public HomeData(IEnumerable<Home> homes)
        {
            Homes = homes;
        }
    }
}
