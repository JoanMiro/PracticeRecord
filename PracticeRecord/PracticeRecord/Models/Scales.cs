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

    public sealed class Scales : List<Scale>
    {
        static Scales()
        {
        }

        private Scales()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("PracticeRecord.Data.Scales.xml");
            var scalesDoc = new XmlDocument();
            scalesDoc.Load(stream);

            foreach (var childElement in scalesDoc.SelectNodes("descendant::Scale").Cast<XmlElement>())
            {
                var description = childElement.SelectSingleNode("descendant::Description").InnerText;
                var noteList = childElement.SelectNodes("descendant::NoteIndex").Cast<XmlNode>().Select(x => int.Parse(x.InnerText));
                var scale = Scale.Create(description, noteList.ToList());
                this.Add(scale);
            }
        }

        public static Scales Instance { get; } = new Scales();
    }
}