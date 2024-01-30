using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ThisCompanyIsGettingLethal
{
    internal class ConfigEntry
    {
        public ConfigEntry(string creator, string mod) {
            Creator = creator;
            Mod = mod;
        }

        [JsonPropertyName("creator"), ReadOnly(true)]
        public string Creator { get; set; }

        [JsonPropertyName("mod"), ReadOnly(true)]
        public string Mod { get; set; }
    }
}
