using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PracticeRecord.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            this.Title = "Cheat Sheets";
            this.Images = new ObservableCollection<ImageSource>
            {
                ImageSource.FromResource("PracticeRecord.Images.CircleOfFifths_WithRelativeMinorKeys.png"),
                ImageSource.FromResource("PracticeRecord.Images.MajorScales.png"),
                ImageSource.FromResource("PracticeRecord.Images.MinorScales.png")
            };
        }

        public ObservableCollection<ImageSource> Images { get; set; }
    }
}