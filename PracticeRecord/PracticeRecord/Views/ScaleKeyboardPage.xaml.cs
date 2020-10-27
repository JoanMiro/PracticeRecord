namespace PracticeRecord.Views
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Models;
    using Services;
    using ViewModels;
    using Xamarin.Forms;

    public partial class ScaleKeyboardPage : KeyboardContentPage
    {
        private readonly IAudio audioPlayer;
        private readonly Color blackKeyColor = Color.Black;
        private readonly Color selectedBlackKeyColor = Color.SlateGray;
        private readonly Color selectedWhiteKeyColor = Color.LightGray;
        private readonly Color whiteKeyColor = Color.White;

        public ScaleKeyboardPage()
        {
            this.InitializeComponent();
            this.audioPlayer = DependencyService.Get<IAudio>();
            this.BindingContext = ((App)Application.Current)?.ChordDataViewModel;

            this.ScalesPicker.SelectedIndexChanged += this.ScalesPickerSelectedIndexChanged;
            this.ChordDataViewModel.PropertyChanged += this.ChordDataPropertyChanged;
            this.ChordDataViewModel.Settings.PropertyChanged += this.SettingsPropertyChanged;
            this.KeyboardGrid.PropertyChanged += this.KeyboardGrid_PropertyChanged;
            this.ChordDataViewModel.ScalePage = this;
            this.ChordDataViewModel.SettingsPage = new SettingsPage();
            this.GetKeys();
        }

        public App CurrentApp => Application.Current as App;

        public List<Button> AllScaleKeys { get; private set; }

        public List<int> ScaleBlackKeys { get; private set; }

        public List<int> ScaleWhiteKeys { get; private set; }

        public ChordDataViewModel ChordDataViewModel => this.BindingContext as ChordDataViewModel;

        private void SettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.ShowScale();
        }

        private void ChordDataPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ScaleRootNoteOffset")
            {
                this.ShowScale();
                this.PlayScale();
            }
        }

        private void GetKeys()
        {
            this.ScaleBlackKeys = new List<int>();
            this.ScaleWhiteKeys = new List<int>();

            this.AllScaleKeys = new List<Button>
            {
                this.ScaleKey0,
                this.ScaleKey1,
                this.ScaleKey2,
                this.ScaleKey3,
                this.ScaleKey4,
                this.ScaleKey5,
                this.ScaleKey6,
                this.ScaleKey7,
                this.ScaleKey8,
                this.ScaleKey9,
                this.ScaleKey10,
                this.ScaleKey11,
                this.ScaleKey12,
                this.ScaleKey13,
                this.ScaleKey14,
                this.ScaleKey15,
                this.ScaleKey16,
                this.ScaleKey17,
                this.ScaleKey18,
                this.ScaleKey19,
                this.ScaleKey20,
                this.ScaleKey21,
                this.ScaleKey22,
                this.ScaleKey23,
                this.ScaleKey24
            };

            this.ScaleBlackKeys.Add(1);
            this.ScaleBlackKeys.Add(3);
            this.ScaleBlackKeys.Add(6);
            this.ScaleBlackKeys.Add(8);
            this.ScaleBlackKeys.Add(10);
            this.ScaleBlackKeys.Add(13);
            this.ScaleBlackKeys.Add(15);
            this.ScaleBlackKeys.Add(18);
            this.ScaleBlackKeys.Add(20);
            this.ScaleBlackKeys.Add(22);
            this.ScaleBlackKeys.Add(25);

            this.ScaleWhiteKeys.Add(0);
            this.ScaleWhiteKeys.Add(2);
            this.ScaleWhiteKeys.Add(4);
            this.ScaleWhiteKeys.Add(5);
            this.ScaleWhiteKeys.Add(7);
            this.ScaleWhiteKeys.Add(9);
            this.ScaleWhiteKeys.Add(11);
            this.ScaleWhiteKeys.Add(12);
            this.ScaleWhiteKeys.Add(14);
            this.ScaleWhiteKeys.Add(16);
            this.ScaleWhiteKeys.Add(17);
            this.ScaleWhiteKeys.Add(19);
            this.ScaleWhiteKeys.Add(21);
            this.ScaleWhiteKeys.Add(23);
            this.ScaleWhiteKeys.Add(24);

            this.ScalesPicker.SelectedIndex = 0;
        }

        private void ScalesPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            this.ShowScale();
        }

        private void ShowScale()
        {
            if (this.ScalesPicker.SelectedItem is Scale selectedScale)
            {
                var actualScaleNotes = this.GetActualNotes(selectedScale.Notes, this.ChordDataViewModel.ScaleRootNoteOffset);
                for (var keyIndex = 0; keyIndex < this.AllScaleKeys.Count; keyIndex++)
                {
                    if (actualScaleNotes.Contains(keyIndex))
                    {
                        this.AllScaleKeys[keyIndex].BackgroundColor =
                            this.ScaleBlackKeys.Contains(keyIndex)
                                ? this.ChordDataViewModel.Settings.BlackKeySelectedScaleColour
                                : this.ChordDataViewModel.Settings.WhiteKeySelectedScaleColour;
                    }
                    else
                    {
                        this.AllScaleKeys[keyIndex].BackgroundColor = this.ScaleBlackKeys.Contains(keyIndex)
                            ? this.ChordDataViewModel.Settings.BlackKeyColour
                            : this.ChordDataViewModel.Settings.WhiteKeyColour;
                    }
                }
            }
        }

        private int[] GetActualNotes(IEnumerable<int> notes, int offset)
        {
            return notes.Select(n => offset + n).ToArray();
        }

        private void PlayScale()
        {
            if (this.CurrentApp.SettingsViewModel.AudioIsEnabled)
            {
                if (this.ScalesPicker.SelectedItem is Scale selectedScale)
                {
                    var actualScaleNotes = this.GetActualNotes(selectedScale.Notes, this.ChordDataViewModel.ScaleRootNoteOffset);
                    Task.Run(() => { this.audioPlayer.PlayNotes(actualScaleNotes, true); })
                        .ConfigureAwait(false);
                }
            }
        }

        private void ScaleKey_OnClicked(object sender, EventArgs e)
        {
            if (sender is Button buttonSender)
            {
                this.ChordDataViewModel.ScaleKeyboardTappedCommand.Execute($"Key{buttonSender.TabIndex}");
            }
        }
    }
}