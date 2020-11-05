namespace PracticeRecord.ViewModels
{
    using Data;
    using Models;
    using Services;
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using Xamarin.Essentials;

    public class PracticeDataViewModel : BaseViewModel
    {
        private readonly DropboxAccess dropboxAccess;

        public PracticeItemDataStore PracticeItemDataStore { get; }

        private const string DatabaseName = "PracticeRecord.db3";

        public ObservableCollection<PracticeItem> PracticeItems { get; private set; }

        public event EventHandler RecordUpdated;

        public PracticeDataViewModel()
        {
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            this.DatabasePath = Path.Combine(folderPath, DatabaseName);
            this.dropboxAccess = new DropboxAccess(folderPath);

            this.PracticeItemDataStore = new PracticeItemDataStore(this.DatabasePath);
            this.RefreshPracticeItems();
        }
        
        public bool IsChangedLocally { get; set; }

        public string DatabasePath { get; set; }

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
                try
                {
                    if (this.dropboxAccess.FetchDatabaseFile())
                    {
                        this.RefreshPracticeItems();
                        this.IsChangedLocally = false;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void SaveState()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet && this.IsChangedLocally)
            {
                try
                {
                    this.dropboxAccess.SaveDatabaseFile();
                    this.IsChangedLocally = false;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}