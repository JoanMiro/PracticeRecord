namespace PracticeRecord
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Newtonsoft.Json.Linq;

    using Services;

    using ViewModels;

    using Xamarin.Forms;

    public partial class App : Application
    {
        // private const string DatabaseName = "TestPracticeRecord.db3";
        private SettingsViewModel settingsViewModel;

        public App()
        {
            this.InitializeComponent();

            DependencyService.Register<MockDataStore>();
            this.ReadSettingsFile();

            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            this.DatabasePath = Path.Combine(folderPath, this.DatabaseName);
            // PeriodMaintenanceService.CheckPeriodEnd(this.DatabasePath);
            SettingsRepository = new SettingsRepository(this.DatabasePath);
            this.ChordDataViewModel = new ChordDataViewModel { Settings = this.SettingsViewModel };
            this.FinderViewModel = new FinderViewModel { Settings = this.SettingsViewModel };
            this.MainPage = new AppShell();
        }

        public int PeriodLengthDays { get; private set; }

        public string DatabasePath { get; set; }

        public ChordDataViewModel ChordDataViewModel { get; set; }

        public static ISettingsRepository SettingsRepository { get; private set; }

        public SettingsViewModel SettingsViewModel
        {
            get => this.settingsViewModel ??= new SettingsViewModel(SettingsRepository);
            set => this.settingsViewModel = value;
        }

        public FinderViewModel FinderViewModel { get; set; }

        public string DatabaseName { get; private set; }

        private void ReadSettingsFile()
        {
            var assembly = Assembly.GetExecutingAssembly();
#if DEBUG
            var settingsFileName = "appsettings.debug.json";
#elif RELEASE
            var settingsFileName = "appsettings.release.json";
#endif

            var resourceName = assembly.GetManifestResourceNames()
            .FirstOrDefault(manifestResourceName => manifestResourceName
            .EndsWith(settingsFileName, StringComparison.OrdinalIgnoreCase));

            using var fileStream = assembly.GetManifestResourceStream(resourceName);
            using var streamReader = new StreamReader(fileStream ?? throw new InvalidOperationException("appsettings.json file resource stream not found"));

            var jsonContent = streamReader.ReadToEnd();
            var jsonObject = JObject.Parse(jsonContent);

            var databaseName = jsonObject.Value<string>("databaseName");

            this.DatabaseName = databaseName;

            this.PeriodLengthDays = jsonObject.Value<int>("periodLengthDays");
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

        //private BaseViewModel GetViewModel()
        //{
        //    if (this.MainPage is AppShell { CurrentItem: { } } appShell && appShell.CurrentItem.Items.Count > 0)
        //    {
        //        var controller = appShell.CurrentItem.Items[0] as IShellSectionController;
        //        if (controller?.PresentedPage?.BindingContext is BaseViewModel viewModel)
        //        {
        //            return viewModel;
        //        }
        //    }

        //    return null;
        //}
    }
}