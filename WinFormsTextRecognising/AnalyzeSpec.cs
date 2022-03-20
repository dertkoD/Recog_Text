using System.Collections.Generic;

namespace WinFormsTextRecognising
{
    public class AnalyzeSpec
    {
        public string content { get; set; }

        public string mime_type { get; set; }
        public List<Feature> features { get; set; }
        public AnalyzeSpec()
        {
            features = new List<Feature>();
        }
    }
}
