using System;
using Kopigi.Utils.Helpers;
using Nestor.Clock.Common.Class;
using Nestor.Clock.Common.Enums;
using NFluent;
using Xunit;

namespace Nestor.Clock.Common.Tests
{
    public class NestorMessageServiceShould
    {
        [Fact]
        public void return_face_sleep_string_character()
        {
            Check.That(NestorMessageService.GetMessageAccordingToMessageType(NestorMessageType.Sleeping).Emoticon).IsEqualTo("\U0001F634");
        }

        [Fact]
        public void return_face_happy_string_character()
        {
            Check.That(NestorMessageService.GetMessageAccordingToMessageType(NestorMessageType.GoodMorning).Emoticon).IsEqualTo("\U0001F60E");
        }
    }
}
