using PracticeRecord.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace PracticeRecord.Views
{
    using System;

    public partial class PracticeHistoryPage : ContentPage
    {
        private List<BoxView> boxViews;

        public PracticeHistoryPage()
        {
            this.InitializeComponent();
            this.InitializeGridBindings();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //this.CurrentDatePicker.MaximumDate = this.ViewModel.PeriodStartDate.AddDays(83);
            //this.CurrentDatePicker.MinimumDate = this.ViewModel.PeriodStartDate;
            //this.CurrentDatePicker.Date = DateTime.Today.Date;
            this.ViewModel.RecordUpdated += this.ViewModel_RecordUpdated;
            if (this.PeriodPicker.ItemsSource.Count > 0)
            {
                this.PeriodPicker.SelectedIndex = this.ViewModel.PracticeDataViewModel.PracticeItems.IndexOf(this.ViewModel.CurrentPeriodRecord);
            }

        }

        private void ViewModel_RecordUpdated(object sender, EventArgs e)
        {
            this.RefreshBoxViewState();
        }

        private PracticeHistoryViewModel ViewModel => (PracticeHistoryViewModel)this.BindingContext;

        private void InitializeGridBindings()
        {
            this.boxViews = this.FindByName<Grid>("TableGrid").Children.Where(child => child is BoxView).Cast<BoxView>().ToList();
            this.RefreshBoxViewState();
        }

        private void RefreshBoxViewState()
        {
            if (this.boxViews != null)
            {
                for (var boxViewIndex = 0; boxViewIndex < this.boxViews.Count; boxViewIndex++)
                {
                    this.boxViews[boxViewIndex].Color = this.ViewModel.DoneCollection[boxViewIndex];
                }
            }
        }

        private void DoneSwitchToggled(object sender, ToggledEventArgs e)
        {
            this.ViewModel.DoneSwitchToggledCommand.Execute(e.Value);
            this.RefreshBoxViewState();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var index = this.boxViews.IndexOf(sender as BoxView);
            if (this.ViewModel.PeriodStartDate.AddDays(index) <= this.ViewModel.CurrentDate)
            {
                this.ViewModel.ToggleDay(index);
            }

        }

        private void PeriodPicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}