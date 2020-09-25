using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PracticeRecord.Models;
using SQLite;

namespace PracticeRecord.Services
{
    public class PracticeItemDataStore : IDataStore<PracticeItem>
    {
        private readonly SQLiteAsyncConnection databaseConnection;

        public PracticeItemDataStore(string databaseFilePath)
        {
            this.databaseConnection = new SQLiteAsyncConnection(databaseFilePath);
            var result = this.databaseConnection.CreateTableAsync<PracticeItem>().Result;
            Debug.WriteLine(result);
            if (this.GetItemsAsync().Result.ToList().Count == 0)
            {
                var startDate = new DateTime(2020, 9, 28);
                Task.Run(() => this.AddItemAsync(new PracticeItem
                {
                    CycleStartDate = startDate,
                    SerializedRecord = new string('0',84)
                }));
            }
        }

        public async Task<int> AddItemAsync(PracticeItem practiceItem)
        {
            if (practiceItem.Id != 0)
            {
                return this.databaseConnection.UpdateAsync(practiceItem).Result;
                //return await this.databaseConnection.UpdateAsync(practiceItem);
            }

            return await this.databaseConnection.InsertAsync(practiceItem);
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
            return this.databaseConnection.Table<PracticeItem>().ToListAsync().Result;
            //return await this.databaseConnection.Table<PracticeItem>().ToListAsync();
        }
    }
}