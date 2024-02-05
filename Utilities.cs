using System.IO.Compression;

namespace ThisCompanyIsGettingLethal
{
    internal class Utilities
    {
        internal static async Task ExtractZipEntriesWithPrefixAsync(string zipPath, string prefix, string outPath) {
            using (ZipArchive archive = ZipFile.OpenRead(zipPath)) {
                foreach (var entry in archive.Entries) {
                    if (String.IsNullOrEmpty(entry.Name))
                        continue;
                    if (!entry.FullName.StartsWith(prefix))
                        continue;

                    string finalPath = Path.Combine(outPath, Path.GetRelativePath(prefix, entry.FullName));
                    if (String.IsNullOrEmpty(finalPath))
                        continue;
                    string finalDir = Path.GetDirectoryName(finalPath);
                    if (String.IsNullOrEmpty(finalDir))
                        continue;

                    if (!Directory.Exists(finalDir))
                        Directory.CreateDirectory(finalDir);

                    using (var fs = new FileStream(finalPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    using (Stream zs = entry.Open())
                        await zs.CopyToAsync(fs);
                }
            }
        }

        internal static async Task DownloadFileAsync(string url, string path) {
            using (var hc = new HttpClient())
            using (var hs = await hc.GetStreamAsync(url))
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                await hs.CopyToAsync(fs, 8192);
        }
    }
}
