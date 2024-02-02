using System.Text.Json;
using System.IO.Compression;

namespace ThisCompanyIsGettingLethal
{
    internal static class Program {
        internal const string CONFIG_FILE = "config.json";
        internal const string DOWNLOAD_DIR = "Downloads";
        internal const string MODS_DIR = "Mods";
        internal const string MODPACK_FILE = "modpack.zip";

        static int Main(string[] args) {
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
                goto Finished;
            }

            Console.WriteLine("Fetching download links and files...");
            List<Task> allTasks = new List<Task>();
            entries.ToList().ForEach(e => allTasks.Add(Task.Run(() => { FindDownloadExtractPackage(e.Creator, e.Mod); })));
            Task.WaitAll(allTasks.ToArray());

            Console.WriteLine("Creating modpack...");
            using (var fs = File.Open(MODPACK_FILE, FileMode.Create, FileAccess.Write, FileShare.None))
                ZipFile.CreateFromDirectory(MODS_DIR, fs, CompressionLevel.Optimal, false);
            Console.WriteLine("Done!");

        Finished:
#if DEBUG
            Console.WriteLine("Press ENTER to exit...");
            Console.ReadKey();
#endif

            return 0;
        }

        internal static void FindDownloadExtractPackage(string creator, string mod) {
            Task<PackageExperimental> t0 = ThunderstoreAPI.PackageExperimental(creator, mod);
            t0.Wait();
            var pe = t0.Result;
            string path = Path.Combine(DOWNLOAD_DIR, pe.Latest.FullName + ".zip");

            Console.WriteLine($"Downloading \"{pe.Namespace}/{pe.Name}\" version \"{pe.Latest.VersionNumber}\" to \"{path}\"...");

            Task t1 = Utilities.DownloadFile(pe.Latest.DownloadUrl, path);
            t1.Wait();
            Task t2 = Task.Run(() => {
                Utilities.ExtractZipEntriesWithPrefix(path, "BepInEx/plugins/", MODS_DIR);
            });
            t2.Wait();
        }
    }
}