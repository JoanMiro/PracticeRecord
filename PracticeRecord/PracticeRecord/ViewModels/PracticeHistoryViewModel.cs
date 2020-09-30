namespace PracticeRecord.ViewModels
{
    using Models;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class PracticeHistoryViewModel : BaseViewModel
    {
        private DateTime currentDate;
        private DateTime periodStartDate;

        public PracticeItem CurrentPeriodRecord { get; private set; }

        public PracticeHistoryViewModel()
        {
            this.Title = "Practice History";

            this.CurrentPeriodRecord = this.PracticeDataViewModel.PracticeItems.OrderByDescending(i => i.CycleStartDate).First();
            this.CurrentDate = DateTime.Today.Date >= this.PeriodStartDate.Date ? DateTime.Today.Date : this.PeriodStartDate.Date;

            _ = this.CheckForNewPeriod();
            this.PeriodStartDate = this.CurrentPeriodRecord.CycleStartDate;
            for (var colorIndex = 0; colorIndex < 84; colorIndex++)
            {
                this.DoneCollection.Add(this.CurrentPeriodRecord.SerializedRecord[colorIndex] == '1' ? this.Done : this.NotDone);
            }

            this.DoneSwitchToggledCommand = new Command(this.DoneSwitchToggled);
        }

        public PracticeDataViewModel PracticeDataViewModel => Application.Current.MainPage.BindingContext as PracticeDataViewModel;

        private async Task CheckForNewPeriod()
        {
            var nextPeriodStartDate = this.CurrentPeriodRecord.CycleStartDate.AddDays(84);
            if (nextPeriodStartDate.Date <= this.CurrentDate.Date)
            {
                // create new cycle...
                this.CurrentPeriodRecord = new PracticeItem { CycleStartDate = nextPeriodStartDate };
                await this.PracticeDataViewModel.PracticeItemDataStore.AddItemAsync(this.CurrentPeriodRecord);
            }
        }

        public DateTime PeriodEndDate => this.periodStartDate.AddDays(83);

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

        public bool DayIsDone => this.DoneCollection[this.DaysOffSet] == this.Done;

        private void DoneSwitchToggled(object toggledObject)
        {
            var toggled = (bool)toggledObject;
            var daysOffset = this.CurrentDate.DayOfYear - this.PeriodStartDate.DayOfYear;
            this.DoneCollection[daysOffset] = toggled ? this.Done : this.NotDone;
            this.UpdateDoneDatabaseRecord();
        }

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
        }
    }
}