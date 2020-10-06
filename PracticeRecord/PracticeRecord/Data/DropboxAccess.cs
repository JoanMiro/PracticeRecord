namespace PracticeRecord.Data
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Threading.Tasks;
    using Dropbox.Api;
    using Dropbox.Api.Auth;
    using Dropbox.Api.Files;

    public class DropboxAccess
    {
        private const string AccessToken =
            "5l6JIvpKWb8AAAAAAAAAAVo6D12GTKS2wMKA0RrQxmonx29R43fKL_KzrEHmWfIs"; 
        //private readonly DropboxClient dropboxClient;

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
                using var dropboxClient = new DropboxClient(AccessToken);
                var downloadResponse = dropboxClient.Files.DownloadAsync($"/{DatabaseName}").Result; // /PracticeRecord/
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
                using var dropboxClient = new DropboxClient(AccessToken);
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

        public async Task TestSaveAndRetrieve()
        {
            try
            {
                var dropboxClient = new DropboxClient(AccessToken);
                var someBytes = new byte[] { (byte)'1', (byte)'5', (byte)'4' };
                var memoryStream = new MemoryStream(someBytes);
                var writeSuccess = 
                    dropboxClient.Files.UploadAsync(
                        $"/PracticeRecord/{DatabaseName}", // /PracticeRecord/
                        WriteMode.Overwrite.Instance,
                        body: memoryStream).Result;

                var downloadResponse = await dropboxClient.Files.DownloadAsync($"/PracticeRecord/{DatabaseName}"); // /PracticeRecord/
                File.WriteAllBytes(this.databasePath, downloadResponse.GetContentAsByteArrayAsync().Result);
                var readSuccess =  downloadResponse.GetContentAsStreamAsync().Result;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}