using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace HebrewVerbs
{
    public class Tense
    {
        public TenseType Type { get; set; }
        public List<Conjugation> Conjugations { get; set; }
    }

}
