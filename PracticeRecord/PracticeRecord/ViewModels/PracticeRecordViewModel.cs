namespace PracticeRecord.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Windows.Input;
    using Models;
    using Services;
    using Xamarin.Forms;

    public class PracticeRecordViewModel : BaseViewModel
    {
        private DateTime currentDate;
        private DateTime periodStartDate;
        private readonly PracticeItemDataStore practiceItemDataStore;
        private readonly List<PracticeItem> practiceItems;
        private PracticeItem currentPeriodRecord;

        public PracticeRecordViewModel()
        {
            this.Title = "Practice Record";
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var databasePath = Path.Combine(folderPath, "PracticeRecord.db3");
            this.practiceItemDataStore = new PracticeItemDataStore(databasePath);

            this.DoneSwitchToggledCommand = new Command(this.DoneSwitchToggled);
            this.practiceItems = this.practiceItemDataStore.GetItemsAsync().Result.ToList();
            this.currentPeriodRecord = this.practiceItems.OrderByDescending(i => i.CycleStartDate).Last();
            this.PeriodStartDate = this.currentPeriodRecord.CycleStartDate;
            for (var colorIndex = 0; colorIndex < 84; colorIndex++)
            {
                this.DoneCollection.Add(this.currentPeriodRecord.SerializedRecord[colorIndex] == '1' ? Color.Blue : Color.WhiteSmoke);
            }

            this.CurrentDate = DateTime.Today.Date >= this.PeriodStartDate.Date ? DateTime.Today.Date : this.PeriodStartDate.Date;
        }

        public DateTime PeriodStartDate
        {
            get => this.periodStartDate;
            set => this.SetProperty(ref this.periodStartDate, value);
        }

        public DateTime CurrentDate
        {
            get => this.currentDate;
            set => this.SetProperty(ref this.currentDate, value);
        }

        public int DaysOffSet => this.CurrentDate.DayOfYear - this.PeriodStartDate.DayOfYear;

        public ObservableCollection<Color> DoneCollection { get; } = new ObservableCollection<Color>();

        public ICommand DoneSwitchToggledCommand { get; }

        public bool DayIsDone => this.DoneCollection[this.DaysOffSet] == Color.Blue;

        private void DoneSwitchToggled(object toggledObject)
        {
            var toggled = (bool)toggledObject;
            var daysOffset = this.CurrentDate.DayOfYear - this.PeriodStartDate.DayOfYear;
            this.DoneCollection[daysOffset] = toggled ? Color.Blue : Color.WhiteSmoke;
            this.UpdateDoneDatabaseRecord();
        }

        private void UpdateDoneDatabaseRecord()
        {
            this.currentPeriodRecord.SerializedRecord = new string(this.DoneCollection.Select(color => color == Color.WhiteSmoke ? '0' : '1').ToArray());
            var success = this.practiceItemDataStore.UpdateItemAsync(this.currentPeriodRecord).Result;
        }
    }
}