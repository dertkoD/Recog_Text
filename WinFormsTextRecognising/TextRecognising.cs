using System.Collections.Generic;

namespace WinFormsTextRecognising
{
    public class TextRecognising
    {
        public string folderId { get; set; }
        public List<AnalyzeSpec> analyze_specs { get; set; }
        public TextRecognising()
        {
            analyze_specs = new List<AnalyzeSpec>();
        }    
    }
}
