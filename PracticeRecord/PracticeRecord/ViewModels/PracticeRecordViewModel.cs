namespace PracticeRecord.ViewModels
{
    using Models;
    using Services;
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class PracticeRecordViewModel : BaseViewModel
    {
        private DateTime currentDate;
        private DateTime periodStartDate;
        public PracticeItem CurrentPeriodRecord => this.PracticeDataViewModel.PracticeItems.OrderByDescending(i => i.CycleStartDate).First();

        public PracticeRecordViewModel()
        {
            this.Title = "Practice Record";

            this.CurrentDate = DateTime.Today.Date >= this.PeriodStartDate.Date ? DateTime.Today.Date : this.PeriodStartDate.Date;

            _ = this.CheckForNewPeriod();
            this.PeriodStartDate = this.CurrentPeriodRecord.CycleStartDate;
            this.RefreshDoneCollection();

            // this.DoneSwitchToggledCommand = new Command(this.DoneSwitchToggled);
            this.CheckState();

            MessagingCenter.Subscribe<App>(
                this, "SaveData", sender =>
                {
                    this.SaveData();
                });

            MessagingCenter.Subscribe<App>(
                this, "CheckState", sender =>
                {
                    this.CheckState();
                });
        }

        private void RefreshDoneCollection()
        {
            this.DoneCollection.Clear();
            for (var colorIndex = 0; colorIndex < 84; colorIndex++)
            {
                this.DoneCollection.Add(this.CurrentPeriodRecord.SerializedRecord[colorIndex] == '1' ? this.Done : this.NotDone);
            }
        }

        private void CheckState()
        {
            this.CurrentDate = DateTime.Today.Date;
            this.PracticeDataViewModel.RefreshState();
            this.RefreshDoneCollection();
            this.PracticeDataViewModel.OnRecordUpdated();
        }

        private void SaveData()
        {
            this.PracticeDataViewModel.SaveState();
        }

        public PracticeDataViewModel PracticeDataViewModel => Application.Current.MainPage.BindingContext as PracticeDataViewModel;

        private async Task CheckForNewPeriod()
        {
            var nextPeriodStartDate = this.CurrentPeriodRecord.CycleStartDate.AddDays(84);
            if (nextPeriodStartDate.Date <= this.CurrentDate.Date)
            {
                // create new cycle...
                var newCurrentPeriodRecord = new PracticeItem { CycleStartDate = nextPeriodStartDate };
                await this.PracticeDataViewModel.PracticeItemDataStore.AddItemAsync(newCurrentPeriodRecord);
            }
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
        
        public int WeekOffset => this.DaysOffSet / 7;

        public ObservableCollection<Color> DoneCollection { get; } = new ObservableCollection<Color>();

        public bool DayIsDone => this.DoneCollection[this.DaysOffSet] == this.Done;

        private void UpdateDoneDatabaseRecord()
        {
            this.CurrentPeriodRecord.SerializedRecord = new string(this.DoneCollection.Select(color => color == this.NotDone ? '0' : '1').ToArray());
            var success = this.PracticeDataViewModel.PracticeItemDataStore.UpdateItemAsync(this.CurrentPeriodRecord).Result;
        }

        public void ToggleDay(int index)
        {
            var toggled = this.DoneCollection[index] == this.NotDone;
            this.DoneCollection[index] = toggled ? this.Done : this.NotDone;
            this.UpdateDoneDatabaseRecord();
            this.PracticeDataViewModel.OnRecordUpdated();
            this.PracticeDataViewModel.IsChangedLocally = true;
        }
    }
}