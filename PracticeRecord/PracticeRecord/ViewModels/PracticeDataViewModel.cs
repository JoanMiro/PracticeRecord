namespace PracticeRecord.ViewModels
{
    using System;
    using Models;
    using Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;

    public class PracticeDataViewModel : BaseViewModel
    {
        public PracticeItemDataStore PracticeItemDataStore { get; }

        public ObservableCollection<PracticeItem> PracticeItems { get; private set; }

        public event EventHandler RecordUpdated;

        public PracticeDataViewModel()
        {
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var databasePath = Path.Combine(folderPath, "PracticeRecord.db3");
            this.PracticeItemDataStore = new PracticeItemDataStore(databasePath);

            this.PracticeItems = new ObservableCollection<PracticeItem>(this.PracticeItemDataStore.GetItemsAsync().Result.ToList());
        }

        public void OnRecordUpdated()
        {
            this.PracticeItems = new ObservableCollection<PracticeItem>(this.PracticeItemDataStore.GetItemsAsync().Result.ToList());
            RecordUpdated?.Invoke(this, null);
        }
    }
}