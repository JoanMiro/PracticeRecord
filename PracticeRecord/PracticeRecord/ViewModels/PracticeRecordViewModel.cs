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

        public PracticeRecordViewModel()
        {
            this.Title = "Practice Record";

            this.currentPeriodRecord = this.PracticeDataViewModel.PracticeItems.OrderByDescending(i => i.CycleStartDate).First();
            this.CurrentDate = DateTime.Today.Date >= this.PeriodStartDate.Date ? DateTime.Today.Date : this.PeriodStartDate.Date;

            _ = this.CheckForNewPeriod();
            this.PeriodStartDate = this.currentPeriodRecord.CycleStartDate;
            for (var colorIndex = 0; colorIndex < 84; colorIndex++)
            {
                this.DoneCollection.Add(this.currentPeriodRecord.SerializedRecord[colorIndex] == '1' ? this.Done : this.NotDone);
            }

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

        private void CheckState()
        {
            this.CurrentDate = DateTime.Today.Date;
            this.PracticeDataViewModel.RefreshState();
        }

        private void SaveData()
        {
            this.PracticeDataViewModel.SaveState();
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
        
        public int WeekOffset => this.DaysOffSet / 7;

        public ObservableCollection<Color> DoneCollection { get; } = new ObservableCollection<Color>();

        public ICommand DoneSwitchToggledCommand { get; }

        public bool DayIsDone => this.DoneCollection[this.DaysOffSet] == this.Done;

        //private void DoneSwitchToggled(object toggledObject)
        //{
        //    var toggled = (bool)toggledObject;
        //    var daysOffset = this.CurrentDate.DayOfYear - this.PeriodStartDate.DayOfYear;
        //    this.DoneCollection[daysOffset] = toggled ? this.Done : this.NotDone;
        //    this.UpdateDoneDatabaseRecord();
        //}

        private void UpdateDoneDatabaseRecord()
        {
            this.currentPeriodRecord.SerializedRecord = new string(this.DoneCollection.Select(color => color == this.NotDone ? '0' : '1').ToArray());
            var success = this.PracticeDataViewModel.PracticeItemDataStore.UpdateItemAsync(this.currentPeriodRecord).Result;
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