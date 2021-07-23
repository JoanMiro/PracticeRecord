namespace PracticeRecord.Data
{
    using Dropbox.Api;
    using Dropbox.Api.Files;
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

        private readonly string folderPath;
        private readonly string databaseName;
        private readonly string repertoireJsonName;
        private readonly string databaseFullPath;


        public DropboxAccess(string folderPath, string databaseName = null, string repertoireJsonName = "PracticePieces.json")
        {
            this.folderPath = folderPath;
            databaseName ??= this.DefaultDatabaseName();
            this.databaseName = databaseName;
            this.repertoireJsonName = repertoireJsonName;
            this.databaseFullPath = Path.Combine(folderPath, databaseName);
            this.LocalUpdateTime = File.GetLastWriteTime(this.databaseFullPath).ToUniversalTime();
        }

        private string DefaultDatabaseName()
        {
            if (Application.Current is App currentApp)
            {
                return currentApp.DatabaseName;
            }

            throw new NullReferenceException("DatabaseName");
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

        public bool WriteLogEntry(string message)
        {
            var logTimeStamp = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm");
            var logFileName = $"{DateTime.Now:yyyy-MM-dd}_log.txt";
            try
            {
                var logFileEntries = this.GetLogFileEntries(logFileName);
                logFileEntries.Add($"[{logTimeStamp}] - {message}");
                return this.SaveLogFile(logFileName, logFileEntries).Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        private List<string> GetLogFileEntries(string logFileName)
        {
            try
            {
                using var dropboxClient = GetClient();
                var downloadResponse = dropboxClient.Files.DownloadAsync($"/{logFileName}").Result;
                var content = downloadResponse.GetContentAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<string>>(content);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<string> { $"LOG FILE {logFileName}" };
            }

        }

        private Task<bool> SaveLogFile(string logFileName, List<string> logFileLines)
        {
            var payload = JsonConvert.SerializeObject(logFileLines, Formatting.Indented);
            try
            {
                using var fileStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(payload));
                using var dropboxClient = GetClient();
                var success = dropboxClient.Files.UploadAsync($"/{logFileName}", WriteMode.Overwrite.Instance, body: fileStream).Result;
                Debug.WriteLine(success.IsFile);
                return Task.FromResult(true);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return Task.FromResult(false);
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