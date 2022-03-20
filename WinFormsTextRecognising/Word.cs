using System.Collections.Generic;

namespace WinFormsTextRecognising
{
    public class Word
    {
        public BoundingBox boundingBox { get; set; }
        public string text { get; set; }
        public decimal confidence { get; set; }
        public List<Language> languages { get; set; }
        public string entityIndex { get; set; }
    }
}
