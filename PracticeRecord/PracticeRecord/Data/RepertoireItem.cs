using Newtonsoft.Json;
using SQLite;

namespace PracticeRecord.Data
{
    public class RepertoireItem
    {
        [PrimaryKey, AutoIncrement]

        public int Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("learnt")]
        public bool Learnt { get; set; }
    }
}
