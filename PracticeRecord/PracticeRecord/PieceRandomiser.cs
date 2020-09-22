using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using PracticeRecord.Data;

namespace PracticeRecord
{
    public class PieceRandomiser : List<GlassPiece>
    {
        private readonly Random random;
        private readonly string fileName = "PracticeRecord.Data.GlassPieces.json";

        public PieceRandomiser()
        {
            this.random = new Random(DateTime.Now.Second);
            this.GetGlassPieces();
        }

        private void GetGlassPieces()
        {
            using var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream(this.fileName);
            using var streamReader =
                new StreamReader(stream ?? throw new InvalidOperationException("Couldn't get JSON data stream"));
            var jsonString = streamReader.ReadToEnd();
            var items = JsonConvert.DeserializeObject<List<GlassPiece>>(jsonString);

            this.AddRange(items);
        }

        public string RandomLearnSelection()
        {
            var index = this.random.Next(this.Count);
            return this[index].Title;
        }

        public string RandomPracticeSelection()
        {
            var practiceList = this.Where(x => x.Learnt).ToList();
            var index = this.random.Next(practiceList.Count);
            return practiceList[index].Title;
        }
    }
}