﻿namespace PracticeRecord.ViewModels
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
        private PracticeItem currentPeriodRecord;

        public event EventHandler RecordUpdated;

        public PracticeRecordViewModel()
        {
            this.Title = "Practice Record";

            this.currentPeriodRecord = this.PracticeDataViewModel.PracticeItems.OrderByDescending(i => i.CycleStartDate).First();
            this.CurrentDate = DateTime.Today.Date >= this.PeriodStartDate.Date ? DateTime.Today.Date : this.PeriodStartDate.Date;

            _ = this.CheckForNewPeriod();
            this.PeriodStartDate = this.currentPeriodRecord.CycleStartDate;
            for (var colorIndex = 0; colorIndex < 84; colorIndex++)
            {
                this.DoneCollection.Add(this.currentPeriodRecord.SerializedRecord[colorIndex] == '1' ? Color.Blue : Color.WhiteSmoke);
            }

            this.DoneSwitchToggledCommand = new Command(this.DoneSwitchToggled);
        }

        public PracticeDataViewModel PracticeDataViewModel => Application.Current.MainPage.BindingContext as PracticeDataViewModel;

        private async Task CheckForNewPeriod()
        {
            var nextPeriodStartDate = this.currentPeriodRecord.CycleStartDate.AddDays(84);
            if (nextPeriodStartDate.Date <= this.CurrentDate.Date)
            {
                // create new cycle...
                this.currentPeriodRecord = new PracticeItem { CycleStartDate = nextPeriodStartDate };
                await this.PracticeDataViewModel.PracticeItemDataStore.AddItemAsync(this.currentPeriodRecord);
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
            var success = this.PracticeDataViewModel.PracticeItemDataStore.UpdateItemAsync(this.currentPeriodRecord).Result;
        }

        public void ToggleDay(int index)
        {
            var toggled = this.DoneCollection[index] == Color.WhiteSmoke;
            this.DoneCollection[index] = toggled ? Color.Blue : Color.WhiteSmoke;
            this.UpdateDoneDatabaseRecord();
            this.OnRecordUpdated();
        }

        private void OnRecordUpdated()
        {
            RecordUpdated?.Invoke(this, null);
        }
    }
}