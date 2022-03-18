namespace PracticeRecord.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    using Models;

    using Services;

    using Xamarin.Forms;

    public class PracticeRecordViewModel : BaseViewModel
    {
        private const int PeriodLengthDays = 84;
        private DateTime currentDate;
        private DateTime periodStartDate;

        public PracticeRecordViewModel()
        {
            this.Title = "Practice Record";

            this.CurrentDate = DateTime.Today.Date >= this.PeriodStartDate.Date
            ? DateTime.Today.Date
            : this.PeriodStartDate.Date;

            _ = this.CheckForNewPeriod();
            this.PeriodStartDate = this.CurrentPeriodRecord.CycleStartDate;

            // this.RefreshDoneCollection();
            this.RefreshDoneFlagCollection();

            // this.DoneSwitchToggledCommand = new Command(this.DoneSwitchToggled);
            this.CheckState();

            MessagingCenter.Subscribe<App>(
                this,
                "SaveData",
                sender =>
                {
                    this.SaveData();
                });

            MessagingCenter.Subscribe<App>(
                this,
                "CheckState",
                sender =>
                {
                    this.CheckState();
                });
        }

        public PracticeItem CurrentPeriodRecord => this.PracticeDataViewModel.PracticeItems.OrderByDescending(i => i.CycleStartDate).First();

        public PracticeDataViewModel PracticeDataViewModel => Application.Current.MainPage.BindingContext as PracticeDataViewModel;
        
        public SettingsViewModel SettingsViewModel => (Application.Current as App)?.SettingsViewModel;

        public DateTime PeriodStartDate
        {
            get => this.periodStartDate;
            set => this.SetProperty(ref this.periodStartDate, value);
        }

        public DateTime PeriodEndDate => this.periodStartDate.Date.AddDays(PeriodLengthDays - 1);

        public DateTime CurrentDate
        {
            get => this.currentDate;
            set => this.SetProperty(ref this.currentDate, value);
        }

        public int DaysOffSet => (this.CurrentDate - this.PeriodStartDate).Days;

        public int WeekOffset => this.DaysOffSet / 7;

        //public ObservableCollection<Color> DoneCollection { get; } = new();

        public ObservableCollection<bool> DoneFlagCollection { get; } = new();

        // public bool DayIsDone => this.DoneCollection[this.DaysOffSet] == this.Done;
        public bool DayIsDone => this.DoneFlagCollection[this.DaysOffSet];

        public string WeeklyPracticePiece => this.CurrentPeriodRecord.SerializedPracticeSchedule.Split(',')[this.WeekOffset % 12];

        public ImageSource InfoImage => ImageSource.FromResource("PracticeRecord.Images.tab_about.png");

        //private void RefreshDoneCollection()
        //{
        //    this.DoneCollection.Clear();
        //    for (var colorIndex = 0; colorIndex < PeriodLengthDays; colorIndex++)
        //    {
        //        this.DoneCollection.Add(this.CurrentPeriodRecord.SerializedRecord[colorIndex] == '1'
        //        //? this.Done
        //        ? this.SettingsViewModel.DoneColour
        //        : this.NotDone);
        //    }
        //}

        private void RefreshDoneFlagCollection()
        {
            this.DoneFlagCollection.Clear();
            for (var colorIndex = 0; colorIndex < PeriodLengthDays; colorIndex++)
            {
                this.DoneFlagCollection.Add(this.CurrentPeriodRecord.SerializedRecord[colorIndex] == '1');
            }
        }

        private void CheckState()
        {
            this.CurrentDate = DateTime.Today.Date;
            this.PracticeDataViewModel.RefreshState();
            //this.RefreshDoneCollection();
            this.RefreshDoneFlagCollection();
            this.PracticeDataViewModel.OnRecordUpdated();
        }

        private void SaveData()
        {
            this.PracticeDataViewModel.SaveState();
        }

        private async Task CheckForNewPeriod()
        {
            var nextPeriodStartDate = this.CurrentPeriodRecord.CycleStartDate.AddDays(84);

            if (nextPeriodStartDate.Date <= this.CurrentDate.Date)
            {
                var pieceRandomiser = new PieceRandomiser();
                var practiceSchedule = pieceRandomiser.TakeRandom(12).Select(piece => piece.Title);
                var newCurrentPeriodRecord = new PracticeItem
                {
                    CycleStartDate = nextPeriodStartDate,
                    SerializedRecord = new string('0', 84),
                    SerializedPracticeSchedule = string.Join(",", practiceSchedule)
                };

                await this.PracticeDataViewModel.PracticeItemDataStore.AddItemAsync(newCurrentPeriodRecord).ConfigureAwait(false);
            }
        }

        private void UpdateDoneDatabaseRecord()
        {
            //this.CurrentPeriodRecord.SerializedRecord = new string(this.DoneCollection.Select(color => color == this.NotDone
            //? '0'
            //: '1')
            //.ToArray());
            
            this.CurrentPeriodRecord.SerializedRecord = new string(this.DoneFlagCollection.Select(flag => flag
            ? '1'
            : '0')
            .ToArray());

            var success = this.PracticeDataViewModel.PracticeItemDataStore.UpdateItemAsync(this.CurrentPeriodRecord).Result;
        }

        public void ToggleDay(int index)
        {
            //var toggled = this.DoneCollection[index] == this.NotDone;
            //this.DoneCollection[index] = toggled
            //? this.Done
            //: this.NotDone;

            var toggled = this.DoneFlagCollection[index] == false;
            this.DoneFlagCollection[index] = toggled;

            this.UpdateDoneDatabaseRecord();
            this.PracticeDataViewModel.OnRecordUpdated();
            this.PracticeDataViewModel.IsChangedLocally = true;
        }
    }
}