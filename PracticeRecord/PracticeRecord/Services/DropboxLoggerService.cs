namespace PracticeRecord.Services
{
    using Data;
    using System.Threading.Tasks;

    public class DropboxLoggerService : ILogger
    {
        private readonly DropboxAccess dropboxAccess;

        public DropboxLoggerService()
        {
            this.dropboxAccess = new DropboxAccess();
        }
        public enum LogLevel
        {
            Verbose,
            Debug,
            Information,
            Warning,
            Error,
            Fatal
        }

        private bool Write(LogLevel logLevel, string message)
        {
            return this.dropboxAccess.WriteLogEntry($"[{logLevel}] {message}");
        }

        public bool Verbose(string message)
        {
            return Write(LogLevel.Verbose, message);
        }

        public bool Debug(string message)
        {
            return Write(LogLevel.Debug, message);
        }

        public bool Information(string message)
        {
            return Write(LogLevel.Information, message);
        }

        public bool Warning(string message)
        {
            return Write(LogLevel.Warning, message);
        }

        public bool Error(string message)
        {
            return Write(LogLevel.Error, message);
        }

        public bool Fatal(string message)
        {
            return Write(LogLevel.Fatal, message);
        }
    }
}