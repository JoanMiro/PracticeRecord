//------------------------------------------------------------------
//
// Copyright (c) 1995 - 2020 Openfeature Limited. All rights reserved.
//
//------------------------------------------------------------------
namespace PracticeRecord.Views
{
    using Services;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Markup;

    public partial class ChordFinderKeyboardPage : KeyboardContentPage
    {
        private readonly IAudio audioPlayer;

        private readonly Color blackKeyColor = Color.Black;

        private readonly Color selectedBlackKeyColor = Color.SlateGray;

        private readonly Color selectedWhiteKeyColor = Color.LightGray;

        private readonly Color whiteKeyColor = Color.White;

        public ChordFinderKeyboardPage()
        {
            this.InitializeComponent();
            this.GetKeys();
            this.audioPlayer = DependencyService.Get<IAudio>();
            this.BindingContext = ((App)Application.Current)?.FinderViewModel;
            this.FinderViewModel.PropertyChanged += this.ChordDataPropertyChanged;
            this.FinderViewModel.Settings.PropertyChanged += this.SettingsPropertyChanged;
            this.KeyboardGrid.PropertyChanged += this.KeyboardGrid_PropertyChanged;
            this.FinderViewModel.FinderPage = this;
            this.FinderViewModel.SettingsPage = new SettingsPage();
        }

        public App CurrentApp => Application.Current as App;

        public List<Button> AllChordKeys { get; private set; }

        public List<int> ChordBlackKeys { get; private set; }

        public List<int> ChordWhiteKeys { get; private set; }

        public FinderViewModel FinderViewModel => this.BindingContext as FinderViewModel;

        private void SettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.ShowChord();
        }

        private void ChordDataPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FinderRootNoteOffset")
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
        }

        private int[] GetActualNotes(IEnumerable<int> notes, int offset)
        {
            return notes.Select(n => offset + n).ToArray();
        }

        private void ShowChord()
        {
            if (this.FinderViewModel != null)
            {
                for (var keyIndex = 0; keyIndex < this.AllChordKeys.Count; keyIndex++)
                {
                    this.AllChordKeys[keyIndex].BackgroundColor = this.ChordBlackKeys.Contains(keyIndex)
                        ? this.FinderViewModel.Settings.BlackKeyColour
                        : this.FinderViewModel.Settings.WhiteKeyColour;
                }

                if (this.FinderViewModel.FinderChord.Notes.Count > 0)
                {
                    var actualChordNotes = this.FinderViewModel.FinderChord.Notes.ToArray();

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
                                    ? this.FinderViewModel.Settings.BlackKeySelectedFinderColour
                                    : this.FinderViewModel.Settings.WhiteKeySelectedFinderColour;
                        }
                    }
                }

                // this.AdjustZOrder();
            }
        }

        private void PlayChord()
        {
            if (this.CurrentApp.SettingsViewModel.AudioIsEnabled && this.FinderViewModel.IdentifiedChord != null)
            {
                var actualChordNotes = this.GetActualNotes(this.FinderViewModel.IdentifiedChord.Notes, this.FinderViewModel.FinderRootNoteOffset);
                Task.Run(() => { this.audioPlayer.PlayNotes(actualChordNotes, this.FinderViewModel.Settings.ArpeggiateIsEnabled); })
                    .ConfigureAwait(false);
            }
        }
        /*
        private void AdjustZOrder()
        {
            //foreach (var anyButton in this.AllChordKeys)
            //{
            //    anyButton.Order(1);
            //}

            for (var keyIndex = 0; keyIndex < this.AllChordKeys.Count; keyIndex++)
            {
                if (!this.ChordBlackKeys.Contains(keyIndex))
                {
                    var tempColour = this.AllChordKeys[keyIndex].BorderColor;
                    this.AllChordKeys[keyIndex].BorderColor = Color.PaleGoldenrod;
                    this.AllChordKeys[keyIndex].BorderColor = tempColour;
                }
            }

            for (var keyIndex = 0; keyIndex < this.AllChordKeys.Count; keyIndex++)
            {
                if (this.ChordBlackKeys.Contains(keyIndex))
                {
                    var tempColour = this.AllChordKeys[keyIndex].BorderColor;
                    this.AllChordKeys[keyIndex].BorderColor = Color.PaleGoldenrod;
                    this.AllChordKeys[keyIndex].BorderColor = tempColour;
                }
            }
        }
        */

        private void ChordKey_OnClicked(object sender, EventArgs e)
        {
            if (sender is Button buttonSender)
            {
                this.FinderViewModel.FinderKeyboardTappedCommand.Execute($"Key{buttonSender.TabIndex}");
                this.ShowChord();
            }
        }

        private void ClearButton_OnClicked(object sender, EventArgs e)
        {
            this.FinderViewModel.ResetFinderChord();
            this.ShowChord();
        }

        private void PlayButton_OnClicked(object sender, EventArgs e)
        {
            this.PlayChord();
        }
    }
}