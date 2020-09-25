using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PracticeRecord.Models;

namespace PracticeRecord.Services
{
    public class MockDataStore : IDataStore<PracticeItem>
    {
        readonly List<PracticeItem> items;

        public MockDataStore()
        {
            this.items = new List<PracticeItem>()
            {
                new PracticeItem { CycleStartDate = new DateTime(2020,9,22) },
                new PracticeItem { CycleStartDate = new DateTime(2020,9,22) },
                new PracticeItem { CycleStartDate = new DateTime(2020,9,22) },
                new PracticeItem { CycleStartDate = new DateTime(2020,9,22) },
                new PracticeItem { CycleStartDate = new DateTime(2020,9,22) },
                new PracticeItem { CycleStartDate = new DateTime(2020,9,22) }
            };
        }

        public async Task<int> AddItemAsync(PracticeItem practiceItem)
        {
            this.items.Add(practiceItem);

            return await Task.FromResult(0);
        }

        public async Task<int> UpdateItemAsync(PracticeItem practiceItem)
        {
            var oldItem = this.items.FirstOrDefault(arg => arg.CycleStartDate == practiceItem.CycleStartDate);
            this.items.Remove(oldItem);
            this.items.Add(practiceItem);

            return await Task.FromResult(0);
        }

        public async Task<int> DeleteItemAsync(int id)
        {
            var oldItem = this.items.FirstOrDefault(arg => arg.Id == id);
            this.items.Remove(oldItem);

            return await Task.FromResult(0);
        }

        public async Task<PracticeItem> GetItemAsync(int id)
        {
            return await Task.FromResult(this.items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<PracticeItem>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(this.items);
        }
    }
}