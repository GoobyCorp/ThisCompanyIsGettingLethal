using System.Text.Json;
using System.IO.Compression;

namespace ThisCompanyIsGettingLethal
{
    internal static class Program {
        internal const string CONFIG_FILE = "config.json";
        internal const string DOWNLOAD_DIR = "Downloads";
        internal const string MODS_DIR = "Mods";
        internal const string MODPACK_FILE = "modpack.zip";
        internal const string PLUGIN_PREFIX = "BepInEx/plugins/";

        static async Task<int> Main(string[] args) {
            if(!File.Exists(CONFIG_FILE))
                File.WriteAllText(CONFIG_FILE, "[]");

#if DEBUG
            Console.WriteLine("DEBUG - Removing old directories...");

            if (Directory.Exists(DOWNLOAD_DIR))
                Directory.Delete(DOWNLOAD_DIR, true);

            if (Directory.Exists(MODS_DIR))
                Directory.Delete(MODS_DIR, true);

            if (File.Exists(MODPACK_FILE))
                File.Delete(MODPACK_FILE);
#endif

            if (!Directory.Exists(DOWNLOAD_DIR))
                Directory.CreateDirectory(DOWNLOAD_DIR);

            if (!Directory.Exists(MODS_DIR))
                Directory.CreateDirectory(MODS_DIR);

            Console.WriteLine("Reading config...");
            ConfigEntry[] entries = JsonSerializer.Deserialize<ConfigEntry[]>(File.ReadAllText(CONFIG_FILE));
            if (entries == null || entries.Length == 0) {
                Console.WriteLine("No config entries found, aborting...");
                goto Finished;
            }

            Console.WriteLine("Fetching download links and files...");
            List<Task> tl = entries.ToList().Select((e) => FindDownloadExtractPackage(e.Creator, e.Mod)).ToList();
            await Task.WhenAll(tl);

            Console.WriteLine("Creating modpack...");
            await CreateModPackageAsync(MODPACK_FILE);

            Console.WriteLine("Done!");

        Finished:
#if DEBUG
            Console.WriteLine("Press ENTER to exit...");
            Console.ReadKey();
#endif
            return 0;
        }

        internal static async Task FindDownloadExtractPackage(string creator, string mod) {
            var pe = await ThunderstoreAPI.PackageExperimental(creator, mod);
            string path = Path.Combine(DOWNLOAD_DIR, pe.Latest.FullName + ".zip");
            Console.WriteLine($"Downloading \"{pe.Namespace}/{pe.Name}\" version \"{pe.Latest.VersionNumber}\" to \"{path}\"...");
            await Utilities.DownloadFileAsync(pe.Latest.DownloadUrl, path);
            await ExtractModPackageAsync(path);
        }

        internal static async Task ExtractModPackageAsync(string path) {
            await Utilities.ExtractZipEntriesWithPrefix(path, PLUGIN_PREFIX, MODS_DIR);
        }

        internal static async Task CreateModPackageAsync(string path) {
            await Task.Run(() => {
                using (var fs = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.None))
                    ZipFile.CreateFromDirectory(MODS_DIR, fs, CompressionLevel.Optimal, false);
            });
        }
    }
}