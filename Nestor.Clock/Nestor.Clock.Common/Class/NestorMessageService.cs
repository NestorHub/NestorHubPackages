using System.Collections.Generic;
using Nestor.Clock.Common.Enums;

namespace Nestor.Clock.Common.Class
{
    public static class NestorMessageService
    {
        private static Dictionary<NestorMessageType, NestorMessage> _messages = new Dictionary<NestorMessageType, NestorMessage>()
        {
            [NestorMessageType.Sleeping] = new NestorMessage("Il sera bientot l'heure de dormir", NestorEmoticon.Sleep),
            [NestorMessageType.GoodMorning] = new NestorMessage("Bonjour {0}, il est temps de se lever", NestorEmoticon.GoodMorning)
        };

        public static NestorMessage GetMessageAccordingToMessageType(NestorMessageType nestorMessageType)
        {
            return _messages[nestorMessageType];
        }
    }
}