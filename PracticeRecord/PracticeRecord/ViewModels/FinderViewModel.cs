//------------------------------------------------------------------
//
// Copyright (c) 1995 - 2019 Openfeature Limited. All rights reserved.
//
//------------------------------------------------------------------

namespace PracticeRecord.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;
    using Models;
    using Xamarin.Forms;

    public class FinderViewModel : BaseViewModel
    {
        private readonly string[] noteNames =
        {
            "C", "C#", "D", "Eb", "E", "F", "F#", "G", "G#", "A", "Bb", "B"
        };

        private Chord finderChord;

        private int finderRootNoteOffset;

        private Chord identifiedChord;

        private Inversion selectedInversion;

        public FinderViewModel()
        {
            this.Title = "Chord Finder";
            this.FinderKeyboardTappedCommand = new Command(this.OnFinderKeyboardTapped);
            this.selectedInversion = this.Inversions[0];
            this.FinderChord = Chord.Create("Mystery Chord", new List<int>());
        }

        public Inversion SelectedInversion
        {
            get => this.selectedInversion;
            set
            {
                this.selectedInversion = value;
                this.OnPropertyChanged();
            }
        }

        public string FinderRootNoteName => this.noteNames[this.FinderRootNoteOffset % 12];

        public Chords Chords => Chords.Instance;

        public List<Inversion> Inversions { get; } = new List<Inversion>
        {
            new Inversion { Value = 0, Description = "Root" },
            new Inversion { Value = 1, Description = "First inversion" },
            new Inversion { Value = 2, Description = "Second inversion" },
            new Inversion { Value = 3, Description = "Third inversion" }
        };

        public Chord IdentifiedChord
        {
            get => this.identifiedChord;
            set
            {
                this.identifiedChord = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.FoundChordFullName));
            }
        }

        public Chord FinderChord
        {
            get => this.finderChord;
            set
            {
                this.finderChord = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.FinderChordFullName));
            }
        }

        public string FoundChordFullName
        {
            get
            {
                if (this.IdentifiedChord != null)
                {
                    var chordNoteNames = new StringBuilder();
                    foreach (var note in this.IdentifiedChord.Notes)
                    {
                        chordNoteNames.Append(this.noteNames[(note + this.FinderRootNoteOffset) % 12] + " ");
                    }

                    var inversionInfo = this.SelectedInversion.Value > 0 ? this.SelectedInversion.Description : string.Empty;
                    return $"{this.noteNames[this.FinderRootNoteOffset]} {this.IdentifiedChord.Description} [{chordNoteNames.ToString().TrimEnd('+')}] {inversionInfo}";
                    ////return $"[{chordNoteNames.ToString().Trim()}] {inversionInfo}";
                }

                return string.Empty;
            }
        }

        public string FinderChordFullName
        {
            get
            {
                if (this.FinderChord != null)
                {
                    var chordNoteNames = new StringBuilder();
                    foreach (var note in this.FinderChord.Notes)
                    {
                        chordNoteNames.Append(this.noteNames[(note + this.FinderRootNoteOffset) % 12] + " ");
                    }

                    var inversionInfo = this.SelectedInversion.Value > 0  ? this.SelectedInversion.Description : string.Empty;
                    ////return $"{this.noteNames[this.ChordRootNoteOffset]} {this.FinderChord.Description} [{chordNoteNames.ToString().TrimEnd('+')}]";
                    return $"[{chordNoteNames.ToString().Trim()}] {inversionInfo}";
                }

                return string.Empty;
            }
        }

        public ICommand FinderKeyboardTappedCommand { get; }

        public int FinderRootNoteOffset
        {
            get => this.finderRootNoteOffset;
            set
            {
                this.finderRootNoteOffset = value % 12;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.FinderRootNoteName));
                this.OnPropertyChanged(nameof(this.FinderChordFullName));
            }
        }

        public ContentPage SettingsPage { get; set; }

        public ContentPage FinderPage { get; set; }

        public SettingsViewModel Settings { get; set; }

        private void OnFinderKeyboardTapped(object key)
        {
            ////var notePicked = int.Parse(key.ToString().Substring(3)) % 12;
            var notePicked = int.Parse(key.ToString().Substring(3));

            if (this.FinderChord.Notes.Contains(notePicked))
            {
                this.FinderChord.Notes.Remove(notePicked);
            }
            else
            {
                this.FinderChord.Notes.Add(notePicked);
            }

            this.FinderChord.Notes.Sort();
            this.FindChord();
        }

        private void FindChord(int inversion = 0)
        {
            var chordFound = false;
            Chord foundChord = null;
            this.IdentifiedChord = null;

            var inversionNotes = this.FinderChord.Notes.ToArray();
            this.SelectedInversion = this.Inversions[0];

            if (this.FinderChord.Notes.Count >= 3)
            {
                for (var inversionNote = 0; inversionNote < inversion; inversionNote++)
                {
                    if (inversionNotes.Length > inversionNote && inversionNotes[inversionNotes.Length - 1 - inversionNote] >= 12)
                    {
                        inversionNotes[inversionNotes.Length - 1 - inversionNote] -= 12;
                        this.SelectedInversion = this.Inversions.First(i => i.Value == inversion);
                    }
                }

                Array.Sort(inversionNotes);
            }

            var finderRootOffset = inversionNotes.Length > 0 ? inversionNotes[0] : 0;

            foreach (var chord in this.Chords.Where(c => c.Notes.Count == inversionNotes.Length))
            {
                foreach (var chordNote in inversionNotes)
                {
                    if (!chord.Notes.Contains((chordNote - finderRootOffset) % 12))
                    {
                        chordFound = false;
                        break;
                    }

                    chordFound = true;
                    foundChord = chord;
                }

                if (chordFound)
                {
                    this.FinderRootNoteOffset = finderRootOffset;
                    this.IdentifiedChord = foundChord;
                    break;
                }

                this.FinderRootNoteOffset = finderRootOffset;
            }

            // Not found - look for inversions...
            if (chordFound == false && inversion < 3)
            {
                this.FindChord(++inversion);
            }
        }
    }
}