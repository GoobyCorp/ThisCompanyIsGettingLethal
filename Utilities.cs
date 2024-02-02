using System.IO.Compression;

namespace ThisCompanyIsGettingLethal
{
    internal class Utilities
    {
        internal static void ExtractZipEntriesWithPrefix(string zipPath, string prefix, string outPath) {
            using (ZipArchive archive = ZipFile.OpenRead(zipPath)) {
                foreach (var entry in archive.Entries) {
                    if (String.IsNullOrEmpty(entry.Name))
                        continue;
                    if (!entry.FullName.StartsWith(prefix))
                        continue;

                    string finalPath = Path.Combine(outPath, Path.GetRelativePath(prefix, entry.FullName));
                    string finalDir = Path.GetDirectoryName(finalPath);

                    if (String.IsNullOrEmpty(finalDir))
                        continue;

                    if (!Directory.Exists(finalDir))
                        Directory.CreateDirectory(finalDir);

                    entry.ExtractToFile(finalPath, true);
                }
            }
        }

        internal static async Task DownloadFile(string url, string path) {
            using (var hc = new HttpClient()) {
                Stream httpStream = await hc.GetStreamAsync(url);
                using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                    httpStream.CopyTo(fs, 8192);
            }
        }
    }
}
