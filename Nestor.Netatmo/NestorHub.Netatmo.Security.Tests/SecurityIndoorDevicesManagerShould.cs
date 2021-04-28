using System;
using System.Linq;
using NestorHub.Netatmo.Common;
using NestorHub.Netatmo.Security.Class;
using NFluent;
using Xunit;

namespace NestorHub.Netatmo.Security.Tests
{
    public class SecurityIndoorDevicesManagerShould
    {
        private readonly SecurityIndoorTokensFactory _securityIndoorTokensFactory;

        public SecurityIndoorDevicesManagerShould()
        {
            var clientId = "587790346d1dbd5e1c8ba445";
            var clientSecret = "zvUT8N2M6yeARhswSYhCn0THSRM2KsKMQP9afV";
            var username = "mplessis@hotmail.com";
            var password = "zru*p387";

            _securityIndoorTokensFactory = new SecurityIndoorTokensFactory(username, password, clientId, clientSecret);
        }

        [Fact]
        public async void return_homes()
        {
            var securityIndoorManager = new SecurityIndoorDevicesManager(_securityIndoorTokensFactory);
            var homes = await securityIndoorManager.GetHomes();
            Check.That(homes.Count()).IsEqualTo(1);

            var home = homes.First();
            Check.That(home.Id).IsEqualTo("5e9ed68aa91a646ece15abed");
            Check.That(home.Name).IsNotEmpty();
        }

        [Fact]
        public async void return_cameras()
        {
            var securityIndoorManager = new SecurityIndoorDevicesManager(_securityIndoorTokensFactory);
            var homes = await securityIndoorManager.GetHomes();

            var home = homes.First();
            var camera = home.Cameras.First();

            Check.That(home.Cameras.Count()).IsStrictlyPositive();
            Check.That(camera.Id).IsNotEmpty();
            Check.That(camera.Type).IsNotEmpty();
            Check.That(camera.Status).IsNotEmpty();
            Check.That(camera.IsLocal).IsFalse();
            Check.That(camera.SdStatus).IsNotEmpty();
            Check.That(camera.AlimStatus).IsNotEmpty();
            Check.That(camera.Name).IsNotEmpty();
        }

        [Fact]
        public async void return_events()
        {
            var securityIndoorManager = new SecurityIndoorDevicesManager(_securityIndoorTokensFactory);
            var homes = await securityIndoorManager.GetHomes();

            var home = homes.First();
            var cameraEvent = home.Events.First();

            Check.That(home.Events.Count()).IsStrictlyPositive();
            Check.That(cameraEvent.Id).IsNotEmpty();
            Check.That(cameraEvent.Type).IsNotEmpty();
            Check.That(cameraEvent.Time).IsInstanceOf<TimeSpan>();
            Check.That(cameraEvent.CameraId).IsNotEmpty();
        }

        [Fact]
        public async void return_no_events()
        {
            var securityIndoorManager = new SecurityIndoorDevicesManager(_securityIndoorTokensFactory);
            var homes = await securityIndoorManager.GetHomes("", 0);

            var home = homes.First();

            Check.That(home.Events.Count()).IsEqualTo(0);

        }
    }
}
