//------------------------------------------------------------------
//
// Copyright (c) 2012 - 2020 Openfeature Limited. All rights reserved.
//
//------------------------------------------------------------------

namespace PracticeRecord.ViewModels
{
    using System.Threading.Tasks;
    using Extensions;
    using Services;
    using Xamarin.Forms;

    public class SettingsViewModel : BaseViewModel
    {
        private readonly ISettingsRepository settingsRepository;

        public SettingsViewModel(ISettingsRepository settingsRepository)
        {
            this.settingsRepository = settingsRepository;
            Task.Run(this.RetrieveColourProperties);
        }

        private async void RetrieveColourProperties()
        {
            var settings = await this.settingsRepository.GetSettings();
            this.blackKeySelectedChordColour = settings.BlackKeySelectedChordColour.ToColour();
            this.blackKeySelectedFinderColour = settings.BlackKeySelectedFinderColour.ToColour();
            this.blackKeySelectedScaleColour = settings.BlackKeySelectedScaleColour.ToColour();
            this.whiteKeySelectedChordColour = settings.WhiteKeySelectedChordColour.ToColour();
            this.whiteKeySelectedFinderColour = settings.WhiteKeySelectedFinderColour.ToColour();
            this.whiteKeySelectedScaleColour = settings.WhiteKeySelectedScaleColour.ToColour();
        }

        private bool arpeggiateIsEnabled;

        private bool audioIsEnabled;

        private Color blackKeyColour = Color.FromRgb(0, 0, 0);

        private Color blackKeySelectedChordColour = Color.DarkOrange;

        private Color blackKeySelectedFinderColour = Color.DarkGoldenrod;

        private Color blackKeySelectedScaleColour = Color.DarkGreen;

        private Color whiteKeyColour = Color.FromRgb(byte.MaxValue, byte.MaxValue, 240);

        private Color whiteKeySelectedChordColour = Color.LightSalmon;

        private Color whiteKeySelectedFinderColour = Color.PaleGoldenrod;

        private Color whiteKeySelectedScaleColour = Color.DarkSeaGreen;

        public Color BlackKeyColour
        {
            get => this.blackKeyColour;
            set
            {
                this.blackKeyColour = value;
                this.OnPropertyChanged();
            }
        }

        public Color BlackKeySelectedChordColour
        {
            get => this.blackKeySelectedChordColour;
            set
            {
                this.blackKeySelectedChordColour = value;
                this.OnPropertyChanged();
            }
        }

        public Color BlackKeySelectedScaleColour
        {
            get => this.blackKeySelectedScaleColour;
            set
            {
                this.blackKeySelectedScaleColour = value;
                this.OnPropertyChanged();
            }
        }

        public Color BlackKeySelectedFinderColour
        {
            get => this.blackKeySelectedFinderColour;
            set
            {
                this.blackKeySelectedFinderColour = value;
                this.OnPropertyChanged();
            }
        }

        public Color WhiteKeyColour
        {
            get => this.whiteKeyColour;
            set
            {
                this.whiteKeyColour = value;
                this.OnPropertyChanged();
            }
        }

        public Color WhiteKeySelectedChordColour
        {
            get => this.whiteKeySelectedChordColour;
            set
            {
                this.whiteKeySelectedChordColour = value;
                this.OnPropertyChanged();
            }
        }

        public Color WhiteKeySelectedScaleColour
        {
            get => this.whiteKeySelectedScaleColour;
            set
            {
                this.whiteKeySelectedScaleColour = value;
                this.OnPropertyChanged();
            }
        }

        public Color WhiteKeySelectedFinderColour
        {
            get => this.whiteKeySelectedFinderColour;
            set
            {
                this.whiteKeySelectedFinderColour = value;
                this.OnPropertyChanged();
            }
        }

        public bool AudioIsEnabled
        {
            get => this.audioIsEnabled;
            set
            {
                this.audioIsEnabled = value;
                this.OnPropertyChanged();
            }
        }

        public bool ArpeggiateIsEnabled
        {
            get => this.arpeggiateIsEnabled;
            set
            {
                this.arpeggiateIsEnabled = value;
                this.OnPropertyChanged();
            }
        }

        public void SetColourFromName(string propertyName, string colourName)
        {
            switch (propertyName)
            {
                case "BlackKeySelectedChordColour":
                    this.BlackKeySelectedChordColour = ColourUtilities.Colours[colourName];
                    break;
                case "WhiteKeySelectedChordColour":
                    this.WhiteKeySelectedChordColour = ColourUtilities.Colours[colourName];
                    break;
                case "BlackKeySelectedScaleColour":
                    this.BlackKeySelectedScaleColour = ColourUtilities.Colours[colourName];
                    break;
                case "WhiteKeySelectedScaleColour":
                    this.WhiteKeySelectedScaleColour = ColourUtilities.Colours[colourName];
                    break;
                case "BlackKeySelectedFinderColour":
                    this.BlackKeySelectedFinderColour = ColourUtilities.Colours[colourName];
                    break;
                case "WhiteKeySelectedFinderColour":
                    this.WhiteKeySelectedFinderColour = ColourUtilities.Colours[colourName];
                    break;
            }
        }
    }
}