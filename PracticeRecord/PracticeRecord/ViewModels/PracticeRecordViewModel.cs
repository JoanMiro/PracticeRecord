using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PracticeRecord.ViewModels
{
    public class PracticeRecordViewModel : BaseViewModel
    {
        private DateTime currentDate;
        private DateTime periodStartDate;

        public PracticeRecordViewModel()
        {
            this.Title = "Practice Record";
            this.OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamain-quickstart"));
            this.CurrentDate = DateTime.Today;
            this.PeriodStartDate = DateTime.Today.AddDays(-30);
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

        public ICommand OpenWebCommand { get; }
    }
}