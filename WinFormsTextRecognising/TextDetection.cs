using System.Collections.Generic;

namespace WinFormsTextRecognising
{
    public class TextDetection
    {
        public List<Page> pages { get; set; }
        public TextDetection()
        {
            pages = new List<Page>();
        }
    }
}
