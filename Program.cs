using System.IO;
using System.Text.Json;

namespace ThisCompanyIsGettingLethal
{
    internal static class Program {
        internal const string CONFIG_FILE = "config.json";
        internal const string DOWNLOAD_DIR = "Downloads";
        internal const string MODS_DIR = "Mods";

        static int Main(string[] args) {
            if(!File.Exists(CONFIG_FILE))
                File.WriteAllText(CONFIG_FILE, "[]");

            if (!Directory.Exists(DOWNLOAD_DIR))
                Directory.CreateDirectory(DOWNLOAD_DIR);

            if (!Directory.Exists(MODS_DIR))
                Directory.CreateDirectory(MODS_DIR);

            ConfigEntry[] entries = JsonSerializer.Deserialize<ConfigEntry[]>(File.ReadAllText(CONFIG_FILE));
            if (entries == null || entries.Length == 0) {
                Console.WriteLine("No config entries found, aborting...");
                goto Finished;
            }

            List<Task> allTasks = new List<Task>();
            entries.ToList().ForEach(e => allTasks.Add(Task.Run(() => { FindDownloadExtractPackage(e.Creator, e.Mod); })));
            Task.WaitAll(allTasks.ToArray());

Finished:
#if DEBUG
            Console.WriteLine("Press ENTER to exit...");
            Console.ReadKey();
#endif

            return 0;
        }

        internal static void FindDownloadExtractPackage(string creator, string mod) {
            Task<PackageExperimental> t0 = ThunderstoreAPI.Package(creator, mod);
            t0.Wait();
            var pe = t0.Result;
            string path = Path.Combine(DOWNLOAD_DIR, pe.latest.full_name + ".zip");

            Console.WriteLine($"Downloading \"{pe._namespace}/{pe.name}\" version \"{pe.latest.version_number}\" to \"{path}\"...");

            Task t1 = ThunderstoreAPI.Download(pe.latest.download_url, path);
            t1.Wait();
            Task t2 = Task.Run(() => {
                Utilities.ExtractZipEntriesWithPrefix(path, "BepInEx/plugins/", MODS_DIR);
            });
            t2.Wait();
        }
    }
}