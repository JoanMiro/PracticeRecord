namespace PracticeRecord.Services
{
    using ViewModels;

    public interface IViewModelService
    {
        ChordDataViewModel GetChordDataViewModel(SettingsViewModel settingsViewModel);
        FinderViewModel GetFinderViewModel(SettingsViewModel settingsViewModel);
        SettingsViewModel GetSettingsViewModel(ISettingsRepository settingsRepository);
    }
}