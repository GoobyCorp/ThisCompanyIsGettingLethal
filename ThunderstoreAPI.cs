using System.Net.Http.Json;

namespace ThisCompanyIsGettingLethal
{
    internal class ThunderstoreAPI
    {
        internal static async Task<PackageExperimental> Package(string creator, string mod) {
            using (var hc = new HttpClient()) {
                var pe = await hc.GetFromJsonAsync<PackageExperimental>($"https://thunderstore.io/api/experimental/package/{creator}/{mod}/");
                if (pe == null)
                    return null;
                if (String.IsNullOrEmpty(pe.latest.download_url) || String.IsNullOrEmpty(pe.latest.full_name))
                    return null;
                return pe;
            }
        }

        internal static async Task Download(string url, string path) {
            using (var hc = new HttpClient()) {
                Stream httpStream = await hc.GetStreamAsync(url);
                using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                    httpStream.CopyTo(fs, 8192);
            }
        }
    }
}
