using Q42.HueApi;
using Q42.HueApi.Interfaces;

namespace Nestor.Hue.Lights.Interfaces
{
    public interface IClient
    {
        IHueClient GetClient();
    }
}