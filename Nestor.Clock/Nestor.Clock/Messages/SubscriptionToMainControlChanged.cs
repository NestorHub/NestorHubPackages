using HomeController.Common.Api;

namespace Nestor.Clock.Messages
{
    public class SubscriptionToMainControlChanged
    {
        public StateValueKey StateValueKey { get; }
        public StateValue StateValue { get; }

        public SubscriptionToMainControlChanged(StateValueKey stateValueKey, StateValue stateValue)
        {
            StateValueKey = stateValueKey;
            StateValue = stateValue;
        }
    }
}
