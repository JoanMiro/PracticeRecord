namespace PracticeRecord.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using Data;
    using Models;
    using Services;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    public class PracticeDataViewModel : BaseViewModel
    {
        private readonly DropboxAccess dropboxAccess;

        public PracticeItemDataStore PracticeItemDataStore { get; }

        // private const string DatabaseName = "TestPracticeRecord.db3";

        private readonly ILogger logger;

        public PracticeDataViewModel()
        {
            var databaseName = this.DefaultDatabaseName();
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            this.DatabasePath = Path.Combine(folderPath, databaseName);
            this.dropboxAccess = new DropboxAccess(folderPath);

            this.PracticeItemDataStore = new PracticeItemDataStore(this.DatabasePath);
            this.RefreshPracticeItems();
            this.logger = new DropboxLoggerService();
        }

        public ObservableCollection<PracticeItem> PracticeItems { get; private set; }

        public bool IsChangedLocally { get; set; }

        public string DatabasePath { get; set; }

        public event EventHandler RecordUpdated;

        private void RefreshPracticeItems()
        {
            this.PracticeItems = new ObservableCollection<PracticeItem>(this.PracticeItemDataStore.GetItemsAsync().Result.ToList());
        }

        public void OnRecordUpdated()
        {
            this.PracticeItems = new ObservableCollection<PracticeItem>(this.PracticeItemDataStore.GetItemsAsync().Result.ToList());
            this.RecordUpdated?.Invoke(this, null);
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
                    this.logger.Error(e.Message);
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
                    this.logger.Error(e.Message);
                }
            }
        }

        private string DefaultDatabaseName()
        {
            if (Application.Current is App currentApp)
            {
                return currentApp.DatabaseName;
            }

            throw new NullReferenceException("DatabaseName");
        }
    }
}