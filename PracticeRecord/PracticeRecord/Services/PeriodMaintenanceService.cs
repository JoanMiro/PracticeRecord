namespace PracticeRecord.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using Models;
    using SQLite;
    using Xamarin.Forms;

    public static class PeriodMaintenanceService
    {
        private static SQLiteAsyncConnection databaseConnection;

        public static int PeriodLengthDays => 84;

        public static Task CheckPeriodEnd(string databaseFilePath = null)
        {
            databaseFilePath ??= DefaultDBFilePath();

            databaseConnection = new SQLiteAsyncConnection(databaseFilePath);
            var result = databaseConnection.CreateTableAsync<PracticeItem>().Result;
            if (result != CreateTableResult.Created && result != CreateTableResult.Migrated)
            {
                return Task.FromException(new FileNotFoundException(databaseFilePath));
            }

            var latestPeriod = databaseConnection.Table<PracticeItem>()
            .OrderByDescending(rec => rec.CycleStartDate)
            .ToListAsync()
            .Result.FirstOrDefault();

            PracticeItem newPracticeItem;
            if (latestPeriod == null)
            {
                newPracticeItem = CreateNewItem(DateTime.Today.Date);
            }
            else
            {
                var nextPeriodStartDate = latestPeriod.CycleStartDate.Date.AddDays(PeriodLengthDays);
                var currentPeriodEndDate = latestPeriod.CycleStartDate.Date.AddDays(PeriodLengthDays);
                if (currentPeriodEndDate > nextPeriodStartDate)
                {
                    return Task.CompletedTask;
                }

                newPracticeItem = CreateNewItem(nextPeriodStartDate);
            }

            _ = databaseConnection.InsertAsync(newPracticeItem).Result;

            return Task.CompletedTask;
        }

        private static string DefaultDBFilePath()
        {
            if (Application.Current is App currentApp)
            {
                return currentApp.DatabasePath;
            }

            throw new NullReferenceException("DatabaseFilePath");
        }

        private static PracticeItem CreateNewItem(DateTime periodStartDate)
        {
            var pieceRandomiser = new PieceRandomiser();
            var practiceSchedule = pieceRandomiser.TakeRandom(12).Select(piece => piece.Title);
            return new PracticeItem
            {
                CycleStartDate = periodStartDate,
                SerializedRecord = new string('0', PeriodLengthDays),
                SerializedPracticeSchedule = string.Join(",", practiceSchedule)
            };
        }
    }
}