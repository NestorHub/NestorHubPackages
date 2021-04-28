using Newtonsoft.Json;

namespace NestorHub.Netatmo.Security.Class
{
    public class Camera
    {
        public string Id { get; }
        public string Type { get; }
        public string Status { get; }
        public string VpnUrl { get; }
        public bool IsLocal { get; }
        public string SdStatus { get; }
        public string AlimStatus { get; }
        public string Name { get; }

        [JsonConstructor]
        public Camera(string id, string type, string status, string vpn_url, bool is_local, string sd_status, string alim_status, string name)
        {
            Id = id;
            Type = type;
            Status = status;
            VpnUrl = vpn_url;
            IsLocal = is_local;
            SdStatus = sd_status;
            AlimStatus = alim_status;
            Name = name;
        }
    }
}