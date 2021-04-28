using Nestor.Clock.Class;

namespace Nestor.Clock.Interfaces
{
    public interface INestorService
    {
        void SubscribeOnStateValue(string sentinelName, string packageName, string valueName, ControlType control);
    }
}