using PracticeRecord.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeRecord.Services
{
    using System.IO;

    public class PracticeItemDataStore : IDataStore<PracticeItem>
    {
        private readonly string databaseFilePath;
        private SQLiteAsyncConnection databaseConnection;

        public PracticeItemDataStore(string databaseFilePath)
        {
            this.databaseFilePath = databaseFilePath;
            this.OpenConnection();
            var result = this.databaseConnection.CreateTableAsync<PracticeItem>().Result;
            Debug.WriteLine(result);
            if (this.GetItemsAsync().Result.ToList().Count == 0)
            {
                this.SeedHistory();
            }

            // this.FileDump();
        }

        public async Task<int> AddItemAsync(PracticeItem practiceItem)
        {
            if (practiceItem.Id != 0)
            {
                return this.databaseConnection.UpdateAsync(practiceItem).Result;
                //return await this.databaseConnection.UpdateAsync(practiceItem);
            }

            return this.databaseConnection.InsertAsync(practiceItem).Result;
        }

        public async Task<int> UpdateItemAsync(PracticeItem practiceItem)
        {
            return this.databaseConnection.UpdateAsync(practiceItem).Result;
            //return await this.databaseConnection.UpdateAsync(practiceItem);
        }

        public async Task<int> DeleteItemAsync(int id)
        {
            return this.databaseConnection.DeleteAsync<PracticeItem>(id).Result;
            //return await this.databaseConnection.DeleteAsync<PracticeItem>(id);
        }

        public async Task<PracticeItem> GetItemAsync(int id)
        {
            return this.databaseConnection.Table<PracticeItem>().Where(item => item.Id == id).FirstOrDefaultAsync().Result;
            //return await this.databaseConnection.Table<PracticeItem>().Where(item => item.Id == id)
            //    .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PracticeItem>> GetItemsAsync(bool forceRefresh = false)
        {
            return this.databaseConnection.Table<PracticeItem>().OrderByDescending(rec => rec.CycleStartDate).ToListAsync().Result;
            //return await this.databaseConnection.Table<PracticeItem>().ToListAsync();
        }

        private void SeedHistory()
        {
            for (var historyPeriodIndex = 0; historyPeriodIndex < 3; historyPeriodIndex++)
            {
                var historyStartDate = new DateTime(2020, 1, 20).AddDays(84 * historyPeriodIndex);
                _ = this.AddItemAsync(
                    new PracticeItem
                    {
                        CycleStartDate = historyStartDate,
                        SerializedRecord = new string('1', 84)
                    }).Result;
            }

            var startDate = new DateTime(2020, 9, 28);
            _ = this.AddItemAsync(
                new PracticeItem
                {
                    CycleStartDate = startDate,
                    SerializedRecord = new string('0', 84)
                }).Result;


        }

        private async void FileDump()
        {
            try
            {
                var localFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
                var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var fileList = Directory.EnumerateFiles(folderPath);
                var enumerable = fileList as string[] ?? fileList.ToArray();
                Debug.WriteLine(enumerable.Length);
                var fileName = enumerable.First(x => x.Contains("PracticeRecord.db3"));
                using var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                using var writer = File.Create(localFilesPath);
                var bytes = new byte[fileStream.Length];
                _ = fileStream.Read(bytes, 0, (int)fileStream.Length);
                await writer.WriteAsync(bytes, 0, (int)fileStream.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void CloseConnection()
        {
            this.databaseConnection.CloseAsync();
        }

        public void OpenConnection()
        {
            this.databaseConnection = new SQLiteAsyncConnection(this.databaseFilePath);
        }
    }
}