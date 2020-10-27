//------------------------------------------------------------------
//
// Copyright (c) 1995 - 2020 Openfeature Limited. All rights reserved.
//
//------------------------------------------------------------------
namespace PracticeRecord.Views
{
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Models;
    using Xamarin.Forms;

    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            this.InitializeComponent();

            this.BindingContext ??= this.CurrentApp.SettingsViewModel;

            this.BlackChordColourPicker.Color = this.CurrentApp.SettingsViewModel.BlackKeySelectedChordColour;
            this.WhiteChordColourPicker.Color = this.CurrentApp.SettingsViewModel.WhiteKeySelectedChordColour;
            this.BlackScaleColourPicker.Color = this.CurrentApp.SettingsViewModel.BlackKeySelectedScaleColour;
            this.WhiteScaleColourPicker.Color = this.CurrentApp.SettingsViewModel.WhiteKeySelectedScaleColour;
            this.BlackFinderColourPicker.Color = this.CurrentApp.SettingsViewModel.BlackKeySelectedFinderColour;
            this.WhiteFinderColourPicker.Color = this.CurrentApp.SettingsViewModel.WhiteKeySelectedFinderColour;

            this.BlackChordColourPicker.PropertyChanged += this.BlackChordColourPickerPropertyChanged;
            this.WhiteChordColourPicker.PropertyChanged += this.WhiteChordColourPickerPropertyChanged;
            this.BlackScaleColourPicker.PropertyChanged += this.BlackScaleColourPickerPropertyChanged;
            this.WhiteScaleColourPicker.PropertyChanged += this.WhiteScaleColourPickerPropertyChanged;
            this.BlackFinderColourPicker.PropertyChanged += this.BlackFinderColourPickerPropertyChanged;
            this.WhiteFinderColourPicker.PropertyChanged += this.WhiteFinderColourPickerPropertyChanged;
        }

        private void WhiteFinderColourPickerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.CurrentApp.SettingsViewModel.WhiteKeySelectedFinderColour = this.WhiteFinderColourPicker.Color;
        }

        private void BlackFinderColourPickerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.CurrentApp.SettingsViewModel.BlackKeySelectedFinderColour = this.BlackFinderColourPicker.Color;
        }

        private void WhiteScaleColourPickerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.CurrentApp.SettingsViewModel.WhiteKeySelectedScaleColour = this.WhiteScaleColourPicker.Color;
        }

        private void BlackScaleColourPickerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.CurrentApp.SettingsViewModel.BlackKeySelectedScaleColour = this.BlackScaleColourPicker.Color;
        }

        private void WhiteChordColourPickerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.CurrentApp.SettingsViewModel.WhiteKeySelectedChordColour = this.WhiteChordColourPicker.Color;
        }

        private void BlackChordColourPickerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.CurrentApp.SettingsViewModel.BlackKeySelectedChordColour = this.BlackChordColourPicker.Color;
        }

        public App CurrentApp => Application.Current as App;

        protected override void OnDisappearing()
        {
            Task.Run(this.SaveSettings);
            base.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
            Task.Run(this.SaveSettings);
            return base.OnBackButtonPressed();
        }

        private async Task SaveSettings()
        {
            var update = true;
            var settingsToSave = await App.SettingsRepository.GetSettings();
            if (settingsToSave == null)
            {
                settingsToSave = new Settings();
                update = false;
            }

            settingsToSave.ArpeggiateChord = this.CurrentApp.SettingsViewModel.ArpeggiateIsEnabled;
            settingsToSave.PlaySelection = this.CurrentApp.SettingsViewModel.AudioIsEnabled;

            //settingsToSave.BlackKeySelectedChordColour = ColourUtilities.Colours.First(c => c.Value == this.CurrentApp.SettingsViewModel.BlackKeySelectedChordColour).Key;
            //settingsToSave.WhiteKeySelectedChordColour = ColourUtilities.Colours.First(c => c.Value == this.CurrentApp.SettingsViewModel.WhiteKeySelectedChordColour).Key;
            //settingsToSave.BlackKeySelectedScaleColour = ColourUtilities.Colours.First(c => c.Value == this.CurrentApp.SettingsViewModel.BlackKeySelectedScaleColour).Key;
            //settingsToSave.WhiteKeySelectedScaleColour = ColourUtilities.Colours.First(c => c.Value == this.CurrentApp.SettingsViewModel.WhiteKeySelectedScaleColour).Key;
            //settingsToSave.BlackKeySelectedFinderColour = ColourUtilities.Colours.First(c => c.Value == this.CurrentApp.SettingsViewModel.BlackKeySelectedFinderColour).Key;
            //settingsToSave.WhiteKeySelectedFinderColour = ColourUtilities.Colours.First(c => c.Value == this.CurrentApp.SettingsViewModel.WhiteKeySelectedFinderColour).Key;

            settingsToSave.BlackKeySelectedChordColour = this.CurrentApp.SettingsViewModel.BlackKeySelectedChordColour.ToString();
            settingsToSave.WhiteKeySelectedChordColour = this.CurrentApp.SettingsViewModel.WhiteKeySelectedChordColour.ToString();
            settingsToSave.BlackKeySelectedScaleColour = this.CurrentApp.SettingsViewModel.BlackKeySelectedScaleColour.ToString();
            settingsToSave.WhiteKeySelectedScaleColour = this.CurrentApp.SettingsViewModel.WhiteKeySelectedScaleColour.ToString();
            settingsToSave.BlackKeySelectedFinderColour = this.CurrentApp.SettingsViewModel.BlackKeySelectedFinderColour.ToString();
            settingsToSave.WhiteKeySelectedFinderColour = this.CurrentApp.SettingsViewModel.WhiteKeySelectedFinderColour.ToString();

            if (update)
            {
                await App.SettingsRepository.UpdateSettings(settingsToSave);
            }
            else
            {
                await App.SettingsRepository.AddSettings(settingsToSave);
            }
        }
    }
}