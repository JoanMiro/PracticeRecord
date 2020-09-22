using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PracticeRecord.Services;
using PracticeRecord.Views;

namespace PracticeRecord
{
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
        }

        protected override void OnResume()
        {
        }
    }
}
