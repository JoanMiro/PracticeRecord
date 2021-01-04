//------------------------------------------------------------------
//
// Copyright (c) 2012 - 2018 Openfeature Limited. All rights reserved.
//
//------------------------------------------------------------------

using PracticeRecord.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(AudioPlayer))]
namespace PracticeRecord.Droid
{
    using System;
    using System.Timers;
    using Android.Media;
    using Services;
    using Xamarin.Forms;
    using Application = Android.App.Application;
    using Exception = System.Exception;

    /// <summary>
    /// Class AudioPlayer.
    /// </summary>
    /// <seealso cref="IAudio" />
    /// <seealso cref="IAudio" />
    public class AudioPlayer : IAudio
    {
        /// <summary>
        /// The timer
        /// </summary>
        private readonly Timer timer;

        /// <summary>
        /// The wav resource ids
        /// </summary>
        private readonly int[] wavResourceIds;

        /// <summary>
        /// The media player
        /// </summary>
        private MediaPlayer mediaPlayer;

        /// <summary>
        /// The note index
        /// </summary>
        private int noteIndex;

        /// <summary>
        /// The arpeggiate length
        /// </summary>
        private const int ArpeggiateLength = 750;

        /// <summary>
        /// The simultaneous length
        /// </summary>
        private const int SimultaneousLength = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioPlayer" /> class.
        /// </summary>
        public AudioPlayer()
        {
            this.timer = new Timer();
            this.timer.Elapsed += this.TimerOnElapsed;

            if (!DesignMode.IsDesignModeEnabled)
            {
                this.mediaPlayer = new MediaPlayer();
            }

            this.wavResourceIds = new[]
            {
                Resource.Raw.PIANO_MED_C3,
                Resource.Raw.PIANO_MED_DB3,
                Resource.Raw.PIANO_MED_D3,
                Resource.Raw.PIANO_MED_EB3,
                Resource.Raw.PIANO_MED_E3,
                Resource.Raw.PIANO_MED_F3,
                Resource.Raw.PIANO_MED_GB3,
                Resource.Raw.PIANO_MED_G3,
                Resource.Raw.PIANO_MED_AB3,
                Resource.Raw.PIANO_MED_A3,
                Resource.Raw.PIANO_MED_BB3,
                Resource.Raw.PIANO_MED_B3,
                Resource.Raw.PIANO_MED_C4,
                Resource.Raw.PIANO_MED_DB4,
                Resource.Raw.PIANO_MED_D4,
                Resource.Raw.PIANO_MED_EB4,
                Resource.Raw.PIANO_MED_E4,
                Resource.Raw.PIANO_MED_F4,
                Resource.Raw.PIANO_MED_GB4,
                Resource.Raw.PIANO_MED_G4,
                Resource.Raw.PIANO_MED_AB4,
                Resource.Raw.PIANO_MED_A4,
                Resource.Raw.PIANO_MED_BB4,
                Resource.Raw.PIANO_MED_B4,
                Resource.Raw.PIANO_MED_C5,
                Resource.Raw.PIANO_MED_DB5,
                Resource.Raw.PIANO_MED_D5,
                Resource.Raw.PIANO_MED_EB5,
                Resource.Raw.PIANO_MED_E5,
                Resource.Raw.PIANO_MED_F5,
                Resource.Raw.PIANO_MED_GB5,
                Resource.Raw.PIANO_MED_G5,
                Resource.Raw.PIANO_MED_AB5,
                Resource.Raw.PIANO_MED_A5,
                Resource.Raw.PIANO_MED_BB5,
                Resource.Raw.PIANO_MED_B5
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

            this.timer.Interval = arpeggiate?AudioPlayer.ArpeggiateLength:SimultaneousLength;

            try
            {
                this.PlayNote();
                this.timer.Start();
                while (this.timer.Enabled)
                {
                }

                this.mediaPlayer.Reset();

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
        /// <param name="e">The <see cref="ElapsedEventArgs"/> instance containing the event data.</param>
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

            this.mediaPlayer.Release();
            this.mediaPlayer = MediaPlayer.Create(Application.Context, this.wavResourceIds[this.NoteQueue[this.noteIndex++]]);
            this.mediaPlayer.Start();
        }
    }
}