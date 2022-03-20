using System.Collections.Generic;

namespace WinFormsTextRecognising
{
    public class TextDetectionConfig
    {
        public List<string> languageCodes { get; set; }
        //public string model { get; set; }
        public TextDetectionConfig()
        {
            languageCodes = new List<string>();
        }
    }
}
