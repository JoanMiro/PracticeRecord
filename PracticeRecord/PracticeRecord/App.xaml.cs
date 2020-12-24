namespace PracticeRecord
{
    using Services;
    using System;
    using System.IO;
    using ViewModels;
    using Xamarin.Forms;

    public partial class App : Application
    {
        private const string DatabaseName = "PracticeRecord.db3";
        private SettingsViewModel settingsViewModel;

        public App()
        {
            this.InitializeComponent();

            DependencyService.Register<MockDataStore>();
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            this.DatabasePath = Path.Combine(folderPath, DatabaseName);
            SettingsRepository = new SettingsRepository(this.DatabasePath);
            this.ChordDataViewModel = new ChordDataViewModel { Settings = this.SettingsViewModel };
            this.FinderViewModel = new FinderViewModel { Settings = this.SettingsViewModel };
            this.MainPage = new AppShell();
        }

        public string DatabasePath { get; set; }

        public ChordDataViewModel ChordDataViewModel { get; set; }

        public static ISettingsRepository SettingsRepository { get; private set; }

        public SettingsViewModel SettingsViewModel
        {
            get => this.settingsViewModel ??= new SettingsViewModel(SettingsRepository);
            set => this.settingsViewModel = value;
        }

        public FinderViewModel FinderViewModel { get; set; }

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
                if (controller?.PresentedPage?.BindingContext is BaseViewModel viewModel)
                {
                    return viewModel;
                }
            }

            return null;
        }
    }
}