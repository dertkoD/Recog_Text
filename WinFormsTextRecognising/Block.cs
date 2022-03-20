using System.Collections.Generic;

namespace WinFormsTextRecognising
{
    public class Block
    {
        public BoundingBox boundingBox { get; set; }
        public List<Line> lines { get; set; }
    }
}
