using PracticeRecord.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace PracticeRecord.Views
{
    using System;

    public partial class PracticeHistoryPage : ContentPage
    {
        public PracticeHistoryPage()
        {
            this.InitializeComponent();
            //this.InitializeGridBindings();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //this.CurrentDatePicker.MaximumDate = this.ViewModel.PeriodStartDate.AddDays(83);
            //this.CurrentDatePicker.MinimumDate = this.ViewModel.PeriodStartDate;
            //this.CurrentDatePicker.Date = SystemTime.Today.Date;
            this.ViewModel.PracticeDataViewModel.RecordUpdated += this.ViewModel_RecordUpdated;
            //if (this.PeriodPicker.ItemsSource.Count > 0)
            //{
            //    this.PeriodPicker.SelectedIndex = this.ViewModel.PracticeDataViewModel.PracticeItems.IndexOf(this.ViewModel.CurrentPeriodRecord);
            //}

        }

        private void ViewModel_RecordUpdated(object sender, EventArgs e)
        {
           this.RefreshRecordDataState();
        }

        private PracticeHistoryViewModel ViewModel => (PracticeHistoryViewModel)this.BindingContext;

        private void RefreshRecordDataState()
        {
            this.DataListView.ItemsSource = this.ViewModel.PracticeDataViewModel.PracticeItems;
        }
    }
}