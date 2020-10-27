namespace PracticeRecord.Views
{
    using System;
    using Xamarin.Forms.Internals;

    public class PageOrientationEventArgs : EventArgs
    {
        public PageOrientationEventArgs(PageOrientation orientation)
        {
            this.Orientation = orientation;
        }

        public PageOrientation Orientation { get; }
    }
}