using System.Collections.Generic;
using System.Threading.Tasks;
using NestorHub.Netatmo.Common;

namespace NestorHub.Netatmo.Security.Class
{
    public class SecurityIndoorDevicesManager
    {
        private readonly SecurityIndoorTokensFactory _securityIndoorTokensFactory;

        public SecurityIndoorDevicesManager(SecurityIndoorTokensFactory securityIndoorTokensFactory)
        {
            _securityIndoorTokensFactory = securityIndoorTokensFactory;
        }

        public async Task<IEnumerable<Home>> GetHomes(string homeId = "", int numberOfEvents = -1)
        {
            return await HomeDataService.GetData(homeId, numberOfEvents, await _securityIndoorTokensFactory.GetToken());
        }
    }
}
