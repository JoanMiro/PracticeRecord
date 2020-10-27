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

    public partial class ChordKeyboardPage : KeyboardContentPage
    {
        private readonly IAudio audioPlayer;
        private readonly Color blackKeyColor = Color.Black;
        private readonly Color selectedBlackKeyColor = Color.SlateGray;
        private readonly Color selectedWhiteKeyColor = Color.LightGray;
        private readonly Color whiteKeyColor = Color.White;

        public ChordKeyboardPage()
        {
            this.InitializeComponent();
            this.audioPlayer = DependencyService.Get<IAudio>();
            this.BindingContext = ((App)Application.Current)?.ChordDataViewModel;

            this.ChordsPicker.SelectedIndexChanged += this.ChordsPickerSelectedIndexChanged;
            this.InversionPicker.SelectedIndexChanged += (s, e) => { this.ShowChord(); };
            this.ChordDataViewModel.PropertyChanged += this.ChordDataPropertyChanged;
            this.ChordDataViewModel.Settings.PropertyChanged += this.SettingsPropertyChanged;
            this.KeyboardGrid.PropertyChanged += this.KeyboardGrid_PropertyChanged;
            this.ChordDataViewModel.ChordPage = this;
            this.ChordDataViewModel.SettingsPage = new SettingsPage();
            this.GetKeys();
        }

        public App CurrentApp => Application.Current as App;

        public List<Button> AllChordKeys { get; private set; }

        public List<int> ChordBlackKeys { get; private set; }

        public List<int> ChordWhiteKeys { get; private set; }

        public ChordDataViewModel ChordDataViewModel => this.BindingContext as ChordDataViewModel;

        private void SettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.ShowChord();
        }

        private void ChordDataPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ChordRootNoteOffset")
            {
                this.ShowChord();
                this.PlayChord();
            }
        }

        private void GetKeys()
        {
            this.ChordBlackKeys = new List<int>();
            this.ChordWhiteKeys = new List<int>();

            this.AllChordKeys = new List<Button>
            {
                this.ChordKey0,
                this.ChordKey1,
                this.ChordKey2,
                this.ChordKey3,
                this.ChordKey4,
                this.ChordKey5,
                this.ChordKey6,
                this.ChordKey7,
                this.ChordKey8,
                this.ChordKey9,
                this.ChordKey10,
                this.ChordKey11,
                this.ChordKey12,
                this.ChordKey13,
                this.ChordKey14,
                this.ChordKey15,
                this.ChordKey16,
                this.ChordKey17,
                this.ChordKey18,
                this.ChordKey19,
                this.ChordKey20,
                this.ChordKey21,
                this.ChordKey22,
                this.ChordKey23,
                this.ChordKey24
            };

            this.ChordBlackKeys.Add(1);
            this.ChordBlackKeys.Add(3);
            this.ChordBlackKeys.Add(6);
            this.ChordBlackKeys.Add(8);
            this.ChordBlackKeys.Add(10);
            this.ChordBlackKeys.Add(13);
            this.ChordBlackKeys.Add(15);
            this.ChordBlackKeys.Add(18);
            this.ChordBlackKeys.Add(20);
            this.ChordBlackKeys.Add(22);
            this.ChordBlackKeys.Add(25);

            this.ChordWhiteKeys.Add(0);
            this.ChordWhiteKeys.Add(2);
            this.ChordWhiteKeys.Add(4);
            this.ChordWhiteKeys.Add(5);
            this.ChordWhiteKeys.Add(7);
            this.ChordWhiteKeys.Add(9);
            this.ChordWhiteKeys.Add(11);
            this.ChordWhiteKeys.Add(12);
            this.ChordWhiteKeys.Add(14);
            this.ChordWhiteKeys.Add(16);
            this.ChordWhiteKeys.Add(17);
            this.ChordWhiteKeys.Add(19);
            this.ChordWhiteKeys.Add(21);
            this.ChordWhiteKeys.Add(23);
            this.ChordWhiteKeys.Add(24);

            this.ChordsPicker.SelectedIndex = 0;
            this.InversionPicker.SelectedIndex = 0;
        }

        private int[] GetActualNotes(IEnumerable<int> notes, int offset)
        {
            return notes.Select(n => offset + n).ToArray();
        }

        private void ChordsPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            this.ShowChord();
        }

        private void ShowChord()
        {
            if (this.ChordsPicker.SelectedItem is Chord selectedChord)
            {
                var actualChordNotes = this.GetActualNotes(selectedChord.Notes, this.ChordDataViewModel.ChordRootNoteOffset);

                // Adjust for inversions
                for (var inversionNote = 0;
                    inversionNote < Math.Min(this.ChordDataViewModel.SelectedInversion.Value, selectedChord.Notes.Count);
                    inversionNote++)
                {
                    actualChordNotes[inversionNote] += 12;
                }

                // Cope with overflow
                for (var note = 0; note < actualChordNotes.Length; note++)
                {
                    actualChordNotes[note] = actualChordNotes[note] % this.AllChordKeys.Count;
                }

                for (var keyIndex = 0; keyIndex < this.AllChordKeys.Count; keyIndex++)
                {
                    if (actualChordNotes.Contains(keyIndex))
                    {
                        this.AllChordKeys[keyIndex].BackgroundColor =
                            this.ChordBlackKeys.Contains(keyIndex)
                                ? this.ChordDataViewModel.Settings.BlackKeySelectedChordColour
                                : this.ChordDataViewModel.Settings.WhiteKeySelectedChordColour;
                    }
                    else
                    {
                        this.AllChordKeys[keyIndex].BackgroundColor = this.ChordBlackKeys.Contains(keyIndex)
                            ? this.ChordDataViewModel.Settings.BlackKeyColour
                            : this.ChordDataViewModel.Settings.WhiteKeyColour;
                    }
                }
            }
        }

        private void PlayChord()
        {
            if (this.CurrentApp.SettingsViewModel.AudioIsEnabled)
            {
                if (this.ChordsPicker.SelectedItem is Chord selectedChord)
                {
                    var actualChordNotes = this.GetActualNotes(selectedChord.Notes, this.ChordDataViewModel.ChordRootNoteOffset);
                    Task.Run(() => { this.audioPlayer.PlayNotes(actualChordNotes, this.ChordDataViewModel.Settings.ArpeggiateIsEnabled); })
                        .ConfigureAwait(false);
                }
            }
        }

        private void ChordKey_OnClicked(object sender, EventArgs e)
        {
            if (sender is Button buttonSender)
            {
                this.ChordDataViewModel.ChordKeyboardTappedCommand.Execute($"Key{buttonSender.TabIndex}");
            }
        }
    }
}