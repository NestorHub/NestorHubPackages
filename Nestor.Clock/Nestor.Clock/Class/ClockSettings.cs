using Nestor.Clock.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Nestor.Clock.Class
{
    public class ClockSettings : IClockSettings
    {
        readonly ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;

        public T GetSetting<T>(string settingName)
        {
            if (_localSettings.Values.ContainsKey(settingName))
            {
                return (T)_localSettings.Values[settingName];
            }
            return default(T);
        }

        public void SaveSetting<T>(string settingName, T value)
        {
            if (_localSettings.Values.ContainsKey(settingName))
            {
                _localSettings.Values[settingName] = value;
            }
            else
            {
                _localSettings.Values.Add(settingName, value);
            }
        }

        public object GetCompositeSetting(string settingName)
        {
            var composite = GetSetting<ApplicationDataCompositeValue>(settingName);
            if (composite != null && composite.Count > 0)
            {
                var items = composite.Values.Cast<object>().ToArray();
                var parameterTypes = Enumerable.Repeat(typeof(object), composite.Values.Count).ToArray();

                var createMethod = typeof(ValueTuple)
                                       .GetMethods()
                                       .SingleOrDefault(m => m.Name == "Create" && m.GetParameters().Length == items.Length) ?? throw new NotSupportedException("ValueTuple.Create method not found.");

                var createGenericMethod = createMethod.MakeGenericMethod(parameterTypes);

                var valueTuple = createGenericMethod.Invoke(null, items);
                return valueTuple;
            }

            return null;
        }

        public void SaveCompositeSetting(string settingName, List<KeyValuePair<string, object>> compositeValues)
        {
            var composite = new ApplicationDataCompositeValue();

            foreach (var (keySetting, valueSetting) in compositeValues)
            {
                composite[keySetting] = valueSetting;
            }
            
            SaveSetting(settingName, composite);
        }
    }
}
