namespace PracticeRecord.Services
{
    using System.Threading.Tasks;
    using Models;

    public interface ISettingsRepository
    {
        Task AddSettings(Settings settings);

        Task UpdateSettings(Settings settings);

        Task<Settings> GetSettings();
    }
}