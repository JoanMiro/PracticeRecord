﻿using System;
using PracticeRecord.Extensions;
using SQLite;

namespace PracticeRecord.Models
{
    public class PracticeItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime CycleStartDate { get; set; }

        public string SerializedRecord { get; set; }

        public string SerializedPracticeSchedule { get; set; }

        [Ignore]
        public string Stats => this.PercentageStats();
    }
}