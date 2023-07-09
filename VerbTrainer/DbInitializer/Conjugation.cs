using System.Xml.Serialization;

namespace HebrewVerbs
{
    public class Conjugation
    {
        public string Text { get; set; }
        public string Transcription { get; set; }
        public string Meaning { get; set; }

        [XmlIgnore]
        public string OnlyVowels { get; set; }
    }

}
