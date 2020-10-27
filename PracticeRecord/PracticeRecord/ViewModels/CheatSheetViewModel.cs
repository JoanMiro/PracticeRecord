//------------------------------------------------------------------
//
// Copyright (c) 2012 - 2020 Openfeature Limited. All rights reserved.
//
//------------------------------------------------------------------

namespace PracticeRecord.ViewModels
{
    using System.Collections.ObjectModel;
    using Xamarin.Forms;

    public class CheatSheetViewModel : BaseViewModel
    {
        public CheatSheetViewModel()
        {
            this.Title = "Cheat Sheets";
            this.Images = new ObservableCollection<ImageSource>
            {
                ImageSource.FromResource("PracticeRecord.Images.CircleOfFifths_WithRelativeMinorKeys.png"),
                ImageSource.FromResource("PracticeRecord.Images.MajorScales.png"),
                ImageSource.FromResource("PracticeRecord.Images.MinorScales.png"),
                ImageSource.FromResource("PracticeRecord.Images.key_signatures_chart.png"),
                ImageSource.FromResource("PracticeRecord.Images.music-key-signatures.png")
            };
        }

        public ObservableCollection<ImageSource> Images { get; set; }
    }
}