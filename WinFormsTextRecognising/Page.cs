using System.Collections.Generic;

namespace WinFormsTextRecognising
{
    public class Page
    {
        public string width { get; set; }
        public string height { get; set; }
        public List<Block> blocks { get; set; }
    }
}
