using System.Net.Http.Json;

namespace ThisCompanyIsGettingLethal
{
    internal class ThunderstoreAPI
    {
        internal const string API_BASE = "https://thunderstore.io";

        internal static async Task<PackageExperimental> PackageExperimental(string creator, string mod) {
            using (var hc = new HttpClient()) {
                var pe = await hc.GetFromJsonAsync<PackageExperimental>($"{API_BASE}/api/experimental/package/{creator}/{mod}/");
                if (pe == null)
                    return null;
                if (String.IsNullOrEmpty(pe.Latest.DownloadUrl) || String.IsNullOrEmpty(pe.Latest.FullName))
                    return null;
                return pe;
            }
        }
    }
}
