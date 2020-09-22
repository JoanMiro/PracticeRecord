using System;
using System.Diagnostics;
using System.Threading.Tasks;
using PracticeRecord.Models;
using Xamarin.Forms;

namespace PracticeRecord.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private string itemId;
        private string text;
        private string description;
        public string Id { get; set; }

        public string Text
        {
            get => this.text;
            set => this.SetProperty(ref this.text, value);
        }

        public string Description
        {
            get => this.description;
            set => this.SetProperty(ref this.description, value);
        }

        public string ItemId
        {
            get
            {
                return this.itemId;
            }
            set
            {
                this.itemId = value;
                this.LoadItemId(value);
            }
        }

        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await this.DataStore.GetItemAsync(itemId);
                this.Id = item.Id;
                this.Text = item.Text;
                this.Description = item.Description;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
