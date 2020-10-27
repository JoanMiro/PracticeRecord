//------------------------------------------------------------------
//
// Copyright (c) 1995 - 2020 Openfeature Limited. All rights reserved.
//
//------------------------------------------------------------------

namespace PracticeRecord.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;
    using Models;
    using Xamarin.Forms;

    public class ChordDataViewModel : BaseViewModel
    {
        private readonly string[] noteNames =
        {
            "C", "C#", "D", "Eb", "E", "F", "F#", "G", "G#", "A", "Bb", "B"
        };

        private int chordRootNoteOffset;

        private int finderRootNoteOffset;

        private Chord finderChord;

        private Chord identifiedChord;

        private int scaleRootNoteOffset;

        private Chord selectedChord;

        private Inversion selectedInversion;

        private Scale selectedScale;

        public ChordDataViewModel()
        {
            this.Title = "Chord Factory";
            this.ChordKeyboardTappedCommand = new Command(this.OnChordKeyboardTapped);
            this.FinderKeyboardTappedCommand = new Command(this.OnFinderKeyboardTapped);
            this.ScaleKeyboardTappedCommand = new Command(this.OnScaleKeyboardTapped);
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

        public string ChordRootNoteName => this.noteNames[this.ChordRootNoteOffset % 12];

        public string FinderRootNoteName => this.noteNames[this.FinderRootNoteOffset % 12];

        public string ScaleRootNoteName => this.noteNames[this.ScaleRootNoteOffset % 12];

        public Chords Chords => Chords.Instance;

        public Scales Scales => Scales.Instance;

        public List<Inversion> Inversions { get; } = new List<Inversion>
        {
            new Inversion { Value = 0, Description = "Root" },
            new Inversion { Value = 1, Description = "First inversion" },
            new Inversion { Value = 2, Description = "Second inversion" },
            new Inversion { Value = 3, Description = "Third inversion" }
        };

        public Chord SelectedChord
        {
            get => this.selectedChord;
            set
            {
                this.selectedChord = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.SelectedChordFullName));
            }
        }

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

        public string SelectedChordNoteNames
        {
            get
            {
                if (this.SelectedChord != null)
                {
                    var chordNoteNames = new StringBuilder();
                    foreach (var note in this.SelectedChord.Notes)
                    {
                        chordNoteNames.Append(this.noteNames[(note + this.ChordRootNoteOffset) % 12] + " ");
                    }

                    return $"[{chordNoteNames.ToString().Trim()}]";
                }

                return string.Empty;
            }
        }

        public string SelectedChordFullName
        {
            get
            {
                if (this.SelectedChord != null)
                {
                    var chordNoteNames = new StringBuilder();
                    foreach (var note in this.SelectedChord.Notes)
                    {
                        chordNoteNames.Append(this.noteNames[(note + this.ChordRootNoteOffset) % 12] + " ");
                    }

                    return $"{this.noteNames[this.ChordRootNoteOffset]} {this.SelectedChord.Description} [{chordNoteNames.ToString().TrimEnd('+')}]";
                }

                return string.Empty;
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

                    return $"{this.noteNames[this.FinderRootNoteOffset]} {this.IdentifiedChord.Description} [{chordNoteNames.ToString().TrimEnd('+')}]";
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

                    ////return $"{this.noteNames[this.ChordRootNoteOffset]} {this.FinderChord.Description} [{chordNoteNames.ToString().TrimEnd('+')}]";
                    return $"[{chordNoteNames.ToString().Trim()}]";
                }

                return string.Empty;
            }
        }

        public string SelectedScaleFullName
        {
            get
            {
                if (this.SelectedScale != null)
                {
                    var scaleNoteNames = new StringBuilder();
                    foreach (var note in this.SelectedScale.Notes)
                    {
                        scaleNoteNames.Append(this.noteNames[(note + this.ScaleRootNoteOffset) % 12] + " ");
                    }

                    ////return $"{this.noteNames[this.ScaleRootNoteOffset]} {this.SelectedScale.Description} [{scaleNoteNames.ToString().TrimEnd('+')}]";
                    return $"[{scaleNoteNames.ToString().Trim()}]";
                }

                return string.Empty;
            }
        }

        public Scale SelectedScale
        {
            get => this.selectedScale;
            set
            {
                this.selectedScale = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.SelectedScaleFullName));
            }
        }

        public ICommand ChordKeyboardTappedCommand { get; }

        public ICommand FinderKeyboardTappedCommand { get; }

        public ICommand ScaleKeyboardTappedCommand { get; }

        public int ChordRootNoteOffset
        {
            get => this.chordRootNoteOffset;
            set
            {
                this.chordRootNoteOffset = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.ChordRootNoteName));
                this.OnPropertyChanged(nameof(this.SelectedChordFullName));
            }
        }

        public int FinderRootNoteOffset
        {
            get => this.finderRootNoteOffset;
            set
            {
                this.finderRootNoteOffset = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.FinderRootNoteName));
                this.OnPropertyChanged(nameof(this.FinderChordFullName));
            }
        }
        public int ScaleRootNoteOffset
        {
            get => this.scaleRootNoteOffset;
            set
            {
                this.scaleRootNoteOffset = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.ScaleRootNoteName));
                this.OnPropertyChanged(nameof(this.SelectedScaleFullName));
            }
        }

        public ContentPage SettingsPage { get; set; }

        public ContentPage ChordPage { get; set; }

        public ContentPage FinderPage { get; set; }

        public ContentPage ScalePage { get; set; }

        public SettingsViewModel Settings { get; set; }

        private void OnChordKeyboardTapped(object key)
        {
            this.ChordRootNoteOffset = int.Parse(key.ToString().Substring(3)) % 12;
        }

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

        private void FindChord()
        {
            var chordFound = false;
            Chord foundChord = null;
            this.IdentifiedChord = null;

            var finderRootOffset = this.FinderChord.Notes.Count > 0 ? this.FinderChord.Notes[0] : 0;
   
            foreach (var chord in this.Chords.Where(c => c.Notes.Count == this.FinderChord.Notes.Count))
            {
                foreach (var chordNote in this.FinderChord.Notes)
                {
                    if (!chord.Notes.Contains(chordNote - finderRootOffset))
                    {
                        chordFound = false;
                        break;
                    }
                    else
                    {
                        chordFound = true;
                        foundChord = chord;
                    }
                }

                if (chordFound)
                {
                    this.FinderRootNoteOffset = finderRootOffset;
                    this.IdentifiedChord = foundChord;
                    break;
                }

                this.FinderRootNoteOffset = finderRootOffset;
            }
        }

        private void OnScaleKeyboardTapped(object key)
        {
            this.ScaleRootNoteOffset = int.Parse(key.ToString().Substring(3)) % 12;
        }
    }
}