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
    using Xamarin.Forms;

    public class DropboxAccess
    {
        private const string AccessToken =
            "qr0GWTxnqoQAAAAAAAAAAY324Zfa4cI9aut-qgxw6BRaFpz816-T3Ex4ijEiPDjD";

        private readonly string folderPath;
        private readonly string databaseName;
        private readonly string repertoireJsonName;
        private readonly string databaseFullPath;


        public DropboxAccess(string folderPath, string databaseName = "PracticeRecord.db3", string repertoireJsonName = "GlassPieces.json")
        {
            this.folderPath = folderPath;
            this.databaseName = databaseName;
            this.repertoireJsonName = repertoireJsonName;
            this.databaseFullPath = Path.Combine(folderPath, databaseName);
            this.LocalUpdateTime = File.GetLastWriteTime(this.databaseFullPath).ToUniversalTime();
        }

        public DateTime LocalUpdateTime { get; private set; }

        public DateTime RemoteUpdateTime { get; private set; }

        public bool FetchDatabaseFile()
        {
            try
            {
                using var dropboxClient = GetClient();

                var downloadResponse = dropboxClient.Files.DownloadAsync($"/{this.databaseName}").Result;
                this.RemoteUpdateTime =
                    (downloadResponse.Response.ClientModified > downloadResponse.Response.ServerModified
                        ? downloadResponse.Response.ClientModified
                        : downloadResponse.Response.ServerModified).ToUniversalTime();
                if (this.RemoteUpdateTime > this.LocalUpdateTime)
                {
                    File.WriteAllBytes(this.databaseFullPath, downloadResponse.GetContentAsByteArrayAsync().Result);
                    Debug.WriteLine($"Success writing {this.databaseFullPath}");
                    this.LocalUpdateTime = this.RemoteUpdateTime;
                    return true;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            return false;
        }

        public List<GlassPiece> FetchGlassPiecesFile()
        {
            try
            {
                using var dropboxClient = GetClient();
                var downloadResponse = dropboxClient.Files.DownloadAsync($"/{this.repertoireJsonName}").Result;
                var content = downloadResponse.GetContentAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<GlassPiece>>(content);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            return null;
        }

        public bool SaveDatabaseFile()
        {
            try
            {
                var fileList = Directory.EnumerateFiles(this.folderPath);
                var enumerable = fileList as string[] ?? fileList.ToArray();
                var fileName = enumerable.First(x => x.Contains(this.databaseName));
                using var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                using var dropboxClient = GetClient();
                var success =
                    dropboxClient.Files.UploadAsync(
                        $"/{this.databaseName}", 
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


        public void SaveGlassPiecesFile(List<GlassPiece> practiceItems)
        {
            var payload = JsonConvert.SerializeObject(practiceItems, Formatting.None);
            try
            {
                using var fileStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(payload));
                using var dropboxClient = GetClient();
                var success = dropboxClient.Files.UploadAsync($"/{this.repertoireJsonName}", WriteMode.Overwrite.Instance, body: fileStream).Result;
                Debug.WriteLine(success.IsFile);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
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