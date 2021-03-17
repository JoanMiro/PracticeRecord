namespace PracticeRecord.Services
{
    using System.Threading.Tasks;

    public interface ILogger
    {
         bool Verbose(string message);
         bool Debug(string message);
         bool Information(string message);
         bool Warning(string message);
         bool Error(string message);
         bool Fatal(string message);
    }
}