namespace PracticeRecord.Data
{
    using Dropbox.Api;
    using Dropbox.Api.Files;
    using Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public class DropboxAccess
    {
        private const string AccessToken =
            "qr0GWTxnqoQAAAAAAAAAAY324Zfa4cI9aut-qgxw6BRaFpz816-T3Ex4ijEiPDjD";

        private readonly string databasePath;
        private readonly string folderPath;
        private const string DatabaseName = "PracticeRecord.db3";

        public DropboxAccess()
        {
            this.folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            this.databasePath = Path.Combine(this.folderPath, DatabaseName);
        }

        public bool FetchDatabaseFile()
        {
            try
            {
                using var dropboxClient = GetClient();

                var downloadResponse = dropboxClient.Files.DownloadAsync($"/{DatabaseName}").Result;
                File.WriteAllBytes(this.databasePath, downloadResponse.GetContentAsByteArrayAsync().Result);
                Debug.WriteLine($"Success writing {this.databasePath}");
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            return false;
        }

        public bool SaveDatabaseFile()
        {
            try
            {
                var fileList = Directory.EnumerateFiles(this.folderPath);
                var enumerable = fileList as string[] ?? fileList.ToArray();
                Debug.WriteLine(enumerable.Length);
                var fileName = enumerable.First(x => x.Contains("PracticeRecord.db3"));
                using var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                using var dropboxClient = GetClient();
                var success =
                    dropboxClient.Files.UploadAsync(
                        $"/{DatabaseName}", // /PracticeRecord/
                        WriteMode.Overwrite.Instance,
                        body: fileStream).Result;

                Debug.WriteLine(success.IsFile);
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            return false;
        }

        public List<PracticeItem> DownloadSerializedFile()
        {
            using var dropboxClient = GetClient();
            var downloadResponse = dropboxClient.Files.DownloadAsync("/practiceRecord.txt").Result;
            var content = downloadResponse.GetContentAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<PracticeItem>>(content);
        }

        public void UploadSerializedFile(List<PracticeItem> practiceItems)
        {
            var payload = JsonConvert.SerializeObject(practiceItems, Formatting.None);

            using var fileStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(payload));
            using var dropboxClient = GetClient();
            var success = dropboxClient.Files.UploadAsync("/practiceRecord.txt", WriteMode.Overwrite.Instance, body: fileStream).Result;
            Debug.WriteLine(success.IsFile);
        }

        private static DropboxClient GetClient()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                return new DropboxClient(AccessToken, new DropboxClientConfig { HttpClient = new HttpClient(new HttpClientHandler()) });
            }

            return new DropboxClient(AccessToken);
        }
    }
}