namespace PracticeRecord.Models
{
    using System.Linq;

    public static class PracticeItemExtensions
    {
        public static string PercentageStats(this PracticeItem practiceItem)
        {
            return $"{practiceItem.SerializedRecord.Count(record => record == '1')}/{practiceItem.SerializedRecord.Length}";

        }
    }
}