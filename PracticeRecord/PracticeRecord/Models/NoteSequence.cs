//------------------------------------------------------------------
//
// Copyright (c) 2012 - 2020 Openfeature Limited. All rights reserved.
//
//------------------------------------------------------------------

namespace PracticeRecord.Models
{
    using System.Collections.Generic;

    public abstract class NoteSequence
    {
        public List<int> Notes { get; set; }

        public string Description { get; set; }
   }
}