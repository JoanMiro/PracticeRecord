using System.Collections.Generic;
using System.Threading.Tasks;
using PracticeRecord.Data;
using SQLite;

namespace PracticeRecord.Services
{
    public class RepertoireItemDataStore : IDataStore<RepertoireItem>
    {
        private readonly SQLiteAsyncConnection databaseConnection;

        public RepertoireItemDataStore(string databaseFilePath)
        {
            this.databaseConnection = new SQLiteAsyncConnection(databaseFilePath);
            this.databaseConnection.CreateTableAsync<RepertoireItem>().Wait();
        }

        public async Task<int> AddItemAsync(RepertoireItem repertoireItem)
        {
            if (repertoireItem.Id != 0)
            {
                return await this.databaseConnection.UpdateAsync(repertoireItem);
            }

            return await this.databaseConnection.InsertAsync(repertoireItem);
        }

        public async Task<int> UpdateItemAsync(RepertoireItem repertoireItem)
        {
            return await this.databaseConnection.UpdateAsync(repertoireItem);
        }

        public async Task<int> DeleteItemAsync(int id)
        {
            return await this.databaseConnection.DeleteAsync<RepertoireItem>(id);
        }

        public async Task<RepertoireItem> GetItemAsync(int id)
        {
            return await this.databaseConnection.Table<RepertoireItem>().Where(item => item.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<RepertoireItem>> GetItemsAsync(bool forceRefresh = false)
        {
            return await this.databaseConnection.Table<RepertoireItem>().ToListAsync();
        }
    }
}