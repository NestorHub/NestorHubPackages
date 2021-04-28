using Kopigi.Utils.Helpers;
using Nestor.Clock.Common.Enums;

namespace Nestor.Clock.Common.Class
{
    public class NestorMessage
    {
        public string Message { get; set; }
        public string Emoticon { get; set; }

        public NestorMessage(string message, NestorEmoticon nestorEmoticon)
        {
            Message = message;
            Emoticon = StringEnum.GetStringValue(nestorEmoticon);
        }
    }
}