//------------------------------------------------------------------
//
// Copyright (c) 2012 - 2018 Openfeature Limited. All rights reserved.
//
//------------------------------------------------------------------

using PracticeRecord.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(AudioPlayer))]

namespace PracticeRecord.iOS
{
    using System;
    using System.IO;
    using System.Timers;
    using AVFoundation;
    using Foundation;
    using Services;

    /// <summary>
    /// Class AudioPlayer.
    /// </summary>
    /// <seealso cref="IAudio" />
    public class AudioPlayer : IAudio
    {
        /// <summary>
        /// The arpeggiate length
        /// </summary>
        private const int ArpeggiateLength = 750;

        /// <summary>
        /// The simultaneous length
        /// </summary>
        private const int SimultaneousLength = 1;

        /// <summary>
        /// The timer
        /// </summary>
        private readonly Timer timer;

        /// <summary>
        /// The wav file names
        /// </summary>
        private readonly string[] wavFileNames;

        /// <summary>
        /// The media player
        /// </summary>
        private AVAudioPlayer mediaPlayer;

        /// <summary>
        /// The note index
        /// </summary>
        private int noteIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioPlayer" /> class.
        /// </summary>
        public AudioPlayer()
        {
            this.timer = new Timer();
            this.timer.Elapsed += this.TimerOnElapsed;
            this.wavFileNames = new[]
            {
                "PIANO_MED_C3.wav",
                "PIANO_MED_DB3.wav",
                "PIANO_MED_D3.wav",
                "PIANO_MED_EB3.wav",
                "PIANO_MED_E3.wav",
                "PIANO_MED_F3.wav",
                "PIANO_MED_GB3.wav",
                "PIANO_MED_G3.wav",
                "PIANO_MED_AB3.wav",
                "PIANO_MED_A3.wav",
                "PIANO_MED_BB3.wav",
                "PIANO_MED_B3.wav",
                "PIANO_MED_C4.wav",
                "PIANO_MED_DB4.wav",
                "PIANO_MED_D4.wav",
                "PIANO_MED_EB4.wav",
                "PIANO_MED_E4.wav",
                "PIANO_MED_F4.wav",
                "PIANO_MED_GB4.wav",
                "PIANO_MED_G4.wav",
                "PIANO_MED_AB4.wav",
                "PIANO_MED_A4.wav",
                "PIANO_MED_BB4.wav",
                "PIANO_MED_B4.wav",
                "PIANO_MED_C5.wav",
                "PIANO_MED_DB5.wav",
                "PIANO_MED_D5.wav",
                "PIANO_MED_EB5.wav",
                "PIANO_MED_E5.wav",
                "PIANO_MED_F5.wav",
                "PIANO_MED_GB5.wav",
                "PIANO_MED_G5.wav",
                "PIANO_MED_AB5.wav",
                "PIANO_MED_A5.wav",
                "PIANO_MED_BB5.wav",
                "PIANO_MED_B5.wav"
            };
        }

        /// <summary>
        /// Gets or sets the note queue.
        /// </summary>
        /// <value>The note queue.</value>
        public int[] NoteQueue { get; set; }

        /// <summary>
        /// Plays the notes.
        /// </summary>
        /// <param name="noteArray">The notes.</param>
        /// <param name="arpeggiate">if set to <c>true</c> [arpeggiate].</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool PlayNotes(int[] noteArray, bool arpeggiate)
        {
            this.noteIndex = 0;
            this.NoteQueue = noteArray;
            Array.Sort(this.NoteQueue);

            this.timer.Interval = arpeggiate?AudioPlayer.ArpeggiateLength:AudioPlayer.SimultaneousLength;
            try
            {
                this.PlayNote();
                this.timer.Start();
                while (this.timer.Enabled)
                {
                }

                this.mediaPlayer.FinishedPlaying += (sender, e) => { this.mediaPlayer = null; };
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Timers the on elapsed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ElapsedEventArgs" /> instance containing the event data.</param>
        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            this.PlayNote();
        }

        /// <summary>
        /// Plays the note.
        /// </summary>
        private void PlayNote()
        {
            if (this.noteIndex >= this.NoteQueue.Length && this.timer.Enabled)
            {
                this.timer.Stop();
                return;
            }

            var fileName = this.wavFileNames[this.NoteQueue[this.noteIndex++]];
            var filePath = NSBundle.MainBundle.PathForResource(Path.GetFileNameWithoutExtension(fileName), Path.GetExtension(fileName));
            var url = NSUrl.FromString(filePath);
            this.mediaPlayer = AVAudioPlayer.FromUrl(url);
            this.mediaPlayer.Play();
        }
    }
}