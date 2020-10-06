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

    public class PracticeDataViewModel : BaseViewModel
    {
        private readonly DropboxAccess dropboxAccess;
        public PracticeItemDataStore PracticeItemDataStore { get; }

        public ObservableCollection<PracticeItem> PracticeItems { get; private set; }

        public event EventHandler RecordUpdated;

        public PracticeDataViewModel()
        {
            this.dropboxAccess = new DropboxAccess();
            this.dropboxAccess.SaveDatabaseFile();
            this.dropboxAccess.FetchDatabaseFile();
            //this.dropboxAccess.TestSaveAndRetrieve();
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var databasePath = Path.Combine(folderPath, "PracticeRecord.db3");
            this.PracticeItemDataStore = new PracticeItemDataStore(databasePath);

            this.PracticeItems = new ObservableCollection<PracticeItem>(this.PracticeItemDataStore.GetItemsAsync().Result.ToList());
        }

        public void OnRecordUpdated()
        {
            this.PracticeItems = new ObservableCollection<PracticeItem>(this.PracticeItemDataStore.GetItemsAsync().Result.ToList());
            RecordUpdated?.Invoke(this, null);
            this.dropboxAccess.SaveDatabaseFile();
        }
    }
}