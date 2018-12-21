using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Translation.V2;
using Microsoft.Psi;
using Microsoft.Psi.Components;

namespace GoogleTranslateComponent
{
    public class GoogleTranslate : ConsumerProducer<string, string>
    {
        public string OutputLanguageCode { get; set; }
        public string InputLanguageCode { get; set; }
        private TranslationClient client;

        public GoogleTranslate(Pipeline pipeline, string inputLanguage, string outputLanguage) : base(pipeline)
        {
            this.client = TranslationClient.Create();
            this.InputLanguageCode = inputLanguage;
            this.OutputLanguageCode = outputLanguage;
        }

        protected override void Receive(string text, Envelope e)
        {
            var response = client.TranslateText(text, this.OutputLanguageCode, this.InputLanguageCode);
            string translatedText = response.TranslatedText;
            this.Out.Post(translatedText, e.OriginatingTime);
        }
    }
}