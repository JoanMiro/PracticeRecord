using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PracticeRecord.Services;
using PracticeRecord.Views;

namespace PracticeRecord
{
    using ViewModels;

    public partial class App : Application
    {

        public App()
        {
            this.InitializeComponent();

            DependencyService.Register<MockDataStore>();
            this.MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            MessagingCenter.Send(this, "SaveData");

            //if (this.GetViewModel() is PracticeRecordViewModel practiceRecordViewModel)
            //{
            //    practiceRecordViewModel.PracticeDataViewModel.SaveState();
            //}
        }

        protected override void OnResume()
        {
            MessagingCenter.Send(this, "CheckState");

            //if (this.GetViewModel() is PracticeRecordViewModel practiceViewModel)
            //{
            //    practiceViewModel.CurrentDate = DateTime.Today.Date;
            //}
        }

        private BaseViewModel GetViewModel()
        {
            if (this.MainPage is AppShell appShell && appShell.CurrentItem != null && appShell.CurrentItem.Items.Count > 0)
            {
                var controller = appShell.CurrentItem.Items[0] as IShellSectionController;
                if (controller?.PresentedPage != null)
                {
                    if (controller.PresentedPage.BindingContext is BaseViewModel viewModel)
                    {
                        return viewModel;
                    }
                }
            }

            return null;
        }
    }
}
