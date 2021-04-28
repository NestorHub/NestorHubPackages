using System.Collections.Generic;

namespace Nestor.Clock.Interfaces
{
    public interface IClockSettings
    {
        T GetSetting<T>(string settingName);
        void SaveSetting<T>(string settingName, T value);
        object GetCompositeSetting(string settingName);
        void SaveCompositeSetting(string settingName, List<KeyValuePair<string, object>> compositeValues);
    }
}