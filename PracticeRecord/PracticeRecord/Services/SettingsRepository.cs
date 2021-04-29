namespace PracticeRecord.Services
{
    using Models;
    using SQLite;
    using System;
    using System.Threading.Tasks;

    public class SettingsRepository : ISettingsRepository
    {
        private readonly SQLiteAsyncConnection connection;
        private readonly ILogger logger;

        public SettingsRepository(string dbPath)
        {
            this.connection = new SQLiteAsyncConnection(dbPath);
            this.connection.CreateTableAsync<Settings>().Wait();
            this.logger = new DropboxLoggerService();
        }

        public string StatusMessage { get; set; }

        public async Task AddSettings(Settings settings)
        {
            try
            {
                if (!settings.IsValid)
                {
                    throw new ArgumentException("settings");
                }

                var result = await this.connection.InsertAsync(settings);
                this.StatusMessage = $"{result} record(s) added";
            }
            catch (Exception exception)
            {
                this.StatusMessage = $"Failed to add settings record. Error: {exception.Message}";
                this.logger.Error(this.StatusMessage);
            }
        }

        public async Task UpdateSettings(Settings settings)
        {
            try
            {
                if (!settings.IsValid)
                {
                    throw new ArgumentException("settings");
                }

                var result = await this.connection.UpdateAsync(settings);
                this.StatusMessage = $"{result} record(s) updated";
            }
            catch (Exception exception)
            {
                this.StatusMessage = $"Failed to update settings record. Error: {exception.Message}";
                this.logger.Error(this.StatusMessage);
            }
        }

        public async Task<Settings> GetSettings()
        {
            var settings = new Settings();
            try
            {
                settings = await this.connection.Table<Settings>().FirstOrDefaultAsync();
            }
            catch (Exception exception)
            {
                this.StatusMessage = $"Failed to retrieve data. {exception.Message}";
                this.logger.Error(this.StatusMessage);
            }

            return settings ?? new Settings();
        }
    }
}