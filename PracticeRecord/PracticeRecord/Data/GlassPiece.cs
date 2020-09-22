using Newtonsoft.Json;

namespace PracticeRecord.Data
{
    public class GlassPiece
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("learnt")]
        public bool Learnt { get; set; }
    }
}
