namespace PracticeRecord.Services
{
    using Data;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Xamarin.Essentials;

    public class PieceRandomiser : List<GlassPiece>
    {
        private readonly Random random;
        private readonly string fileName = "PracticeRecord.Data.GlassPieces.json";
        private readonly DropboxAccess dropboxAccess;

        public PieceRandomiser()
        {
            this.random = new Random(DateTime.Now.Second);
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            this.dropboxAccess = new DropboxAccess(folderPath);
            this.GetGlassPieces();
        }

        private void GetGlassPieces()
        {
            List<GlassPiece> glassPieces = null;
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                glassPieces = this.dropboxAccess.FetchGlassPiecesFile();
            }

            if (glassPieces == null)
            {
                using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(this.fileName);
                using var streamReader = new StreamReader(stream ?? throw new InvalidOperationException("Couldn't get JSON data stream"));
                var jsonString = streamReader.ReadToEnd();
                glassPieces = JsonConvert.DeserializeObject<List<GlassPiece>>(jsonString);
                this.dropboxAccess.SaveGlassPiecesFile(glassPieces);
            }

            this.AddRange(glassPieces);
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

        public IEnumerable<GlassPiece> TakeRandom(int take)
        {
            //var random = new Random();
            var available = this.Count();
            var needed = take;
            foreach (var item in this)
            {
                if (this.random.Next(available) < needed)
                {
                    needed--;
                    yield return item;
                    if (needed == 0)
                    {
                        break;
                    }
                }
                available--;
            }
        }
    }
}