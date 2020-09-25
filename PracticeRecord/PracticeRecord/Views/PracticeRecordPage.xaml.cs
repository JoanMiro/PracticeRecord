using System.Collections.Generic;
using System.Linq;
using PracticeRecord.ViewModels;
using Xamarin.Forms;

namespace PracticeRecord.Views
{
    using System;

    public partial class PracticeRecordPage : ContentPage
    {
        private List<BoxView> boxViews;

        public PracticeRecordPage()
        {
            this.InitializeComponent();
            this.InitializeGridBindings();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.CurrentDatePicker.MaximumDate = this.ViewModel.PeriodStartDate.AddDays(84);
            this.CurrentDatePicker.MinimumDate = this.ViewModel.PeriodStartDate;

        }

        private PracticeRecordViewModel ViewModel => (PracticeRecordViewModel) this.BindingContext;

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

        private void DatePicker_OnDateSelected(object sender, DateChangedEventArgs e)
        {
            this.DoneSwitch.IsToggled = this.ViewModel.DayIsDone;
        }
    }
}