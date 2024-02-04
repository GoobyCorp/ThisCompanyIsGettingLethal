using System.Text.Json;
using System.IO.Compression;

namespace ThisCompanyIsGettingLethal
{
    internal static class Program {
        internal const string CONFIG_FILE = "config.json";
        internal const string DOWNLOAD_DIR = "Downloads";
        internal const string MODS_DIR = "Mods";
        internal const string MODPACK_FILE = "modpack.zip";

        static async Task<int> Main(string[] args) {
            if(!File.Exists(CONFIG_FILE))
                File.WriteAllText(CONFIG_FILE, "[]");

            if (!Directory.Exists(DOWNLOAD_DIR))
                Directory.CreateDirectory(DOWNLOAD_DIR);

            if (!Directory.Exists(MODS_DIR))
                Directory.CreateDirectory(MODS_DIR);

            Console.WriteLine("Reading config...");
            ConfigEntry[] entries = JsonSerializer.Deserialize<ConfigEntry[]>(File.ReadAllText(CONFIG_FILE));
            if (entries == null || entries.Length == 0) {
                Console.WriteLine("No config entries found, aborting...");
                Finished();
                return 0;
            }

            Console.WriteLine("Fetching download links and files...");
            Task[] t = entries.ToList().Select((e) => FindDownloadExtractPackage(e.Creator, e.Mod)).ToArray();
            await Task.WhenAll(t);

            Console.WriteLine("Creating modpack...");
            using (var fs = File.Open(MODPACK_FILE, FileMode.Create, FileAccess.Write, FileShare.None))
                ZipFile.CreateFromDirectory(MODS_DIR, fs, CompressionLevel.Optimal, false);
            Console.WriteLine("Done!");

            Finished();
            return 0;
        }

        internal static void Finished() {
#if DEBUG
            Console.WriteLine("Press ENTER to exit...");
            Console.ReadKey();
#endif
        }

        internal static async Task FindDownloadExtractPackage(string creator, string mod) {
            await ThunderstoreAPI.PackageExperimental(creator, mod).ContinueWith(async (e0) => {
                var pe = e0.Result;
                string path = Path.Combine(DOWNLOAD_DIR, pe.Latest.FullName + ".zip");
                Console.WriteLine($"Downloading \"{pe.Namespace}/{pe.Name}\" version \"{pe.Latest.VersionNumber}\" to \"{path}\"...");
                await Utilities.DownloadFileAsync(pe.Latest.DownloadUrl, path).ContinueWith(async (e1) => {
                    await ExtractPackageAsync(path).ContinueWith(async (e2) => {
                        return e2;
                    });
                });
            });
        }

        internal static async Task ExtractPackageAsync(string path) {
            await Task.Run(() => {
                Utilities.ExtractZipEntriesWithPrefix(path, "BepInEx/plugins/", MODS_DIR);
            });
        }
    }
}