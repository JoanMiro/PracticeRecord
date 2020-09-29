namespace PracticeRecord.ViewModels
{
    using System;
    using Models;
    using Services;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class PracticeDataViewModel : BaseViewModel
    {
        public PracticeItemDataStore PracticeItemDataStore { get; }
        public List<PracticeItem> PracticeItems { get; }

        public PracticeDataViewModel()
        {
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var databasePath = Path.Combine(folderPath, "PracticeRecord.db3");
            this.PracticeItemDataStore = new PracticeItemDataStore(databasePath);

            this.PracticeItems = this.PracticeItemDataStore.GetItemsAsync().Result.ToList();
        }
    }
}