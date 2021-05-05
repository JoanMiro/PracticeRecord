namespace PracticeRecord.Services
{
    using System;
    using ViewModels;

    public class ViewModelService : IViewModelService
    {
        public ChordDataViewModel GetChordDataViewModel(SettingsViewModel settingsViewModel)
        {
            return new() { Settings = settingsViewModel };
        }

        public FinderViewModel GetFinderViewModel(SettingsViewModel settingsViewModel)
        {
            return new() { Settings = settingsViewModel };
        }

        public SettingsViewModel GetSettingsViewModel(ISettingsRepository settingsRepository)
        {
            return new(settingsRepository);
        }
    }
}