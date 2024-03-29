﻿//------------------------------------------------------------------
//
// Copyright (c) 2012 - 2020 Openfeature Limited. All rights reserved.
//
//------------------------------------------------------------------

namespace PracticeRecord.Models
{
    using SQLite;

    [Table("settings")]
    public class Settings
    {
        public Settings()
        {
            this.BlackKeySelectedChordColour = "Red";
            this.BlackKeySelectedFinderColour = "Green";
            this.BlackKeySelectedScaleColour = "Blue";
            this.WhiteKeySelectedChordColour = "Pink";
            this.WhiteKeySelectedFinderColour = "LightGreen";
            this.WhiteKeySelectedScaleColour = "LightBlue";
            this.DoneColour = "PaleGoldenrod";
        }

        [PrimaryKey]
        public int Id { get; set; }

        public bool PlaySelection { get; set; }

        public bool ArpeggiateChord { get; set; }

        public string BlackKeySelectedScaleColour { get; set; }

        public string WhiteKeySelectedScaleColour { get; set; }

        public string WhiteKeySelectedFinderColour { get; set; }

        public string BlackKeySelectedFinderColour { get; set; }

        public string BlackKeySelectedChordColour { get; set; }

        public string WhiteKeySelectedChordColour { get; set; }

        public string DoneColour { get; set; }
        
        public bool IsValid =>
            this.BlackKeySelectedChordColour != null
            && this.BlackKeySelectedScaleColour != null
            && this.WhiteKeySelectedChordColour != null
            && this.WhiteKeySelectedScaleColour != null
            && this.DoneColour != null;
    }
}