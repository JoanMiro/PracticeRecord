using PracticeRecord.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace PracticeRecord.Views
{
    using System;

    public partial class PracticeRecordPage : ContentPage
    {
        private List<BoxView> boxViews;
        private List<Label> labels;

        public PracticeRecordPage()
        {
            this.InitializeComponent();
            this.InitializeGridBindings();
            this.ViewModel.PracticeDataViewModel.RecordUpdated += this.ViewModel_RecordUpdated;
        }

        private Style CurrentWeekStyle => this.Resources.First(res => res.Key == "CurrentWeekLabel").Value as Style;

        private Style WeekStyle => this.Resources.First(res => res.Key == "WeekLabel").Value as Style;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.CurrentDatePicker.MaximumDate = this.ViewModel.PeriodStartDate.AddDays(83);
            this.CurrentDatePicker.MinimumDate = this.ViewModel.PeriodStartDate;
            this.CurrentDatePicker.Date = DateTime.Today.Date;
            this.HighlightWeekLabels();
        }

        private void ViewModel_RecordUpdated(object sender, EventArgs e)
        {
            this.RefreshBoxViewState();
        }

        private PracticeRecordViewModel ViewModel => (PracticeRecordViewModel)this.BindingContext;

        private void InitializeGridBindings()
        {
            this.boxViews = this.FindByName<Grid>("TableGrid").Children.Where(child => child is BoxView).Cast<BoxView>().ToList();
            this.labels = this.FindByName<Grid>("TableGrid").Children.Where(child => child is Label).Cast<Label>().ToList();
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

        private void HighlightWeekLabels()
        {
            if (this.labels != null)
            {
                for (var labelIndex = 0; labelIndex <= 12; labelIndex++)
                {
                    this.labels[labelIndex].Style = labelIndex == this.ViewModel.WeekOffset ? this.CurrentWeekStyle : this.WeekStyle;
                }

                for (var labelIndex = 12; labelIndex <= 23; labelIndex++)
                {
                    this.labels[labelIndex].Style = labelIndex - 12 == this.ViewModel.WeekOffset ? this.CurrentWeekStyle : this.WeekStyle;
                }
            }
        }

        private void PracticeCompleteBox_Tapped(object sender, EventArgs e)
        {
            var index = this.boxViews.IndexOf(sender as BoxView);
            if (this.ViewModel.PeriodStartDate.AddDays(index) <= this.ViewModel.CurrentDate)
            {
                this.ViewModel.ToggleDay(index);
            }

        }
    }
}