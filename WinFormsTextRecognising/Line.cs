using System.Collections.Generic;

namespace WinFormsTextRecognising
{
    public class Line
    {
        public BoundingBox boundingBox { get; set; }
        public List<Word> words { get; set; }
        public decimal confidence { get; set; }
    }
}
