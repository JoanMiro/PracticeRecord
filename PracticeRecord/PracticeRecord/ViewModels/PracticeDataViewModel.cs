namespace PracticeRecord.ViewModels
{
    using System;
    using Models;
    using Services;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Xamarin.Essentials;

    public class PracticeDataViewModel : BaseViewModel
    {
        private readonly DropboxAccess dropboxAccess;
        public PracticeItemDataStore PracticeItemDataStore { get; }

        public ObservableCollection<PracticeItem> PracticeItems { get; private set; }

        public event EventHandler RecordUpdated;

        public PracticeDataViewModel()
        {
            this.dropboxAccess = new DropboxAccess();
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var databasePath = Path.Combine(folderPath, "PracticeRecord.db3");
            this.PracticeItemDataStore = new PracticeItemDataStore(databasePath);
            this.RefreshPracticeItems();
        }

        private void RefreshPracticeItems()
        {
            this.PracticeItems = new ObservableCollection<PracticeItem>(this.PracticeItemDataStore.GetItemsAsync().Result.ToList());
        }

        public void OnRecordUpdated()
        {
            this.PracticeItems = new ObservableCollection<PracticeItem>(this.PracticeItemDataStore.GetItemsAsync().Result.ToList());
            RecordUpdated?.Invoke(this, null);
        }

        public void RefreshState()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                //this.PracticeItems = new ObservableCollection<PracticeItem>(this.dropboxAccess.DownloadSerializedFile());
                this.dropboxAccess.FetchDatabaseFile();
                this.RefreshPracticeItems();
            }
        }
        public void SaveState()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                //this.dropboxAccess.UploadSerializedFile(this.PracticeItems.ToList());
                this.dropboxAccess.SaveDatabaseFile();
            }
        }
    }
}