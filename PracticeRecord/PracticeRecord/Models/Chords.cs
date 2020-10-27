//------------------------------------------------------------------
//
// Copyright (c) 2012 - 2020 Openfeature Limited. All rights reserved.
//
//------------------------------------------------------------------

namespace PracticeRecord.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Xml;

    public class Chords : List<Chord>
    {
        static Chords()
        {
        }

        private Chords()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("PracticeRecord.Data.Chords.xml");
            var chordsDoc = new XmlDocument();
            chordsDoc.Load(stream);

            foreach (var childElement in chordsDoc.SelectNodes("descendant::Chord").Cast<XmlElement>())
            {
                var description = childElement.SelectSingleNode("descendant::Description").InnerText;
                var noteList = childElement.SelectNodes("descendant::NoteIndex").Cast<XmlNode>().Select(x => int.Parse(x.InnerText));
                var chord = Chord.Create(description, noteList.ToList());
                this.Add(chord);
            }
        }

        public static Chords Instance { get; } = new Chords();
    }
}