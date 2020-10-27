namespace PracticeRecord.Views
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using Xamarin.Forms;

    public class KeyboardContentPage : ContentPage
    {
        protected Thickness GridLandscapeMargin;
        protected Thickness GridPortraitMargin;
        private double pageHeight;
        private double pageWidth;

        public KeyboardContentPage()
        {
            this.SizeChanged += this.KeyboardPage_SizeChanged;
            this.GridLandscapeMargin = new Thickness(30, 20);
            this.GridPortraitMargin = new Thickness(10, 20);
            this.StoreDimensions();
        }

        private void StoreDimensions()
        {
            this.pageWidth = this.Width;
            this.pageHeight = this.Height;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            var oldWidth = this.pageWidth;
            const double sizeNotAllocated = -1;

            base.OnSizeAllocated(width, height);
            if (Equals(this.pageWidth, width) && Equals(this.pageHeight, height))
            {
                return;
            }

            this.pageWidth = width;
            this.pageHeight = height;

            // ignore if the previous height was size unallocated
            if (Equals(oldWidth, sizeNotAllocated))
            {
                return;
            }

            // Has the device been rotated ?
            if (!Equals(width, oldWidth))
            {
                this.OnOrientationChanged.Invoke(
                    this, new PageOrientationEventArgs(width < height ? PageOrientation.Vertical : PageOrientation.Horizontal));
            }
        }

        public event EventHandler<PageOrientationEventArgs> OnOrientationChanged = (e, a) => { };

        /// <summary>
        /// Handles the PropertyChanged event of the KeyboardGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected void KeyboardGrid_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.ToLowerInvariant() == "height")
            {
                this.SizeKeyboardToAspectRatio(sender as Grid);
            }
        }

        /// <summary>
        /// Sizes the keyboard to aspect ratio.
        /// </summary>
        private void SizeKeyboardToAspectRatio(VisualElement keyboardGrid)
        {
            if (keyboardGrid.Height > 0)
            {
                keyboardGrid.WidthRequest = keyboardGrid.Height * 2.5;
            }
        }

        /// <summary>
        /// Handles the SizeChanged event of the KeyboardPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void KeyboardPage_SizeChanged(object sender, EventArgs e)
        {
            var keyboardContentPage = (KeyboardContentPage)sender;
            var keyboardGrid = keyboardContentPage.FindByName<Grid>("KeyboardGrid");

            if (this.Height > 0 && this.Width > 0)
            {
                keyboardGrid.Margin = this.Height > this.Width ? this.GridPortraitMargin : this.GridLandscapeMargin;
            }
            else
            {
                Debug.WriteLine("Hey!");
            }
        }
    }
}