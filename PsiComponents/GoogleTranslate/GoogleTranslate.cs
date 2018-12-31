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
    /// <summary>
    /// PSI component to translate text from one language to another using Google API.
    /// </summary>
    public class GoogleTranslate : ConsumerProducer<string, string>
    {
        /// <summary>
        /// Language for output text
        /// </summary>
        public string OutputLanguageCode { get; set; }

        /// <summary>
        /// Expected input text language
        /// </summary>
        public string InputLanguageCode { get; set; }

        /// <summary>
        /// Google Translate API client object
        /// </summary>
        private TranslationClient client;

        public GoogleTranslate(Pipeline pipeline, string inputLanguage, string outputLanguage) : base(pipeline)
        {
            this.client = TranslationClient.Create();
            this.InputLanguageCode = inputLanguage;
            this.OutputLanguageCode = outputLanguage;
        }

        /// <summary>
        /// Pipeline package handler to handle incoming text. Translates text from the expected input language
        /// to the specified output langauge and sends resulting translated string down pipeline.
        /// 
        /// Translates text using Google Translate API
        /// </summary>
        /// <param name="text"></param>
        /// <param name="e"></param>
        protected override void Receive(string text, Envelope e)
        {
            var response = client.TranslateText(text, this.OutputLanguageCode, this.InputLanguageCode);
            string translatedText = response.TranslatedText;
            this.Out.Post(translatedText, e.OriginatingTime);
        }
    }
}