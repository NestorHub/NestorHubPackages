using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using HomeController.Common.Api;
using HomeController.Sentinels.Api;
using HomeController.Sentinels.Api.Domain;
using Nestor.Clock.Interfaces;
using Nestor.Clock.Messages;

namespace Nestor.Clock.Class
{
    public class NestorService : INestorService
    {
        private const string HomeControllerServerAddress = "nestor-controller.ddns.net";
        private const int HomeControllerServerPort = 8006;
        private HomeConnectionServer _homeConnectionServer;
        private SubscriptionHost _subscriptionHost;
        private Dictionary<ControlType, List<StateValueKey>> _subscriptions = new Dictionary<ControlType, List<StateValueKey>>();
        public NestorService()
        {
            RunSentinel();
        }

        public async void SubscribeOnStateValue(string sentinelName, string packageName, string valueName, ControlType control)
        {
            if (await _subscriptionHost.IsOnline())
            {
                var subscribeId = await _subscriptionHost.Subscribe(sentinelName, packageName, valueName);
                if (subscribeId != null)
                {
                    if (_subscriptions.ContainsKey(control))
                    {
                        var subscriptionsValue = _subscriptions[control];
                        subscriptionsValue.Add(subscribeId.StateValueKey);
                    }
                    else
                    {
                        _subscriptions.Add(control, new List<StateValueKey>() { subscribeId.StateValueKey });
                    }
                }
            }
        }

        private void RunSentinel()
        {
            var sentinelHost = new SentinelHost();

            sentinelHost.Run("Marc.Clock", "Nestor.Clock");

            _homeConnectionServer = HomeConnectionServer.CreateConnection(HomeControllerServerAddress, HomeControllerServerPort);

            _subscriptionHost = new SubscriptionHost(sentinelHost, _homeConnectionServer);
            _subscriptionHost.SubscriptionValueChanged += SubscriptionHostOnSubscriptionValueChanged;
        }

        private void SubscriptionHostOnSubscriptionValueChanged(StateValueKey stateValueKey, StateValue stateValue)
        {
            var subscriptions = _subscriptions.Where(s => s.Value.Contains(stateValueKey));
            foreach (var subscription in subscriptions)
            {
                if (subscription.Key == ControlType.Main)
                {
                    Messenger.Default.Send<SubscriptionToMainControlChanged>(new SubscriptionToMainControlChanged(stateValueKey, stateValue));
                }
                else
                {
                    
                }
            }
        }
    }
}
