using Google.Cloud.TextToSpeech.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Psi.Audio;
using Microsoft.Psi;
using Microsoft.Psi.Components;

namespace GoogleSpeakComponent
{
    public class GoogleSpeak : ConsumerProducer<string, AudioBuffer>
    {
        public Microsoft.Psi.Audio.WaveFormat format { get; set; }
        public string TextLanguageCode { get; set; }
        private TextToSpeechClient gClient;


        public GoogleSpeak(Pipeline pipeline, Microsoft.Psi.Audio.WaveFormat format, string languageCode) : base(pipeline)
        {
            this.gClient = TextToSpeechClient.Create();
            this.format = format;
            this.TextLanguageCode = languageCode;
        }

        protected override void Receive(string text, Envelope e)
        {
            var response = gClient.SynthesizeSpeech(new SynthesizeSpeechRequest
            {
                Input = new SynthesisInput
                {
                    Text = text
                },
                // Note: voices can also be specified by name
                Voice = new VoiceSelectionParams
                {
                    LanguageCode = this.TextLanguageCode,
                    SsmlGender = SsmlVoiceGender.Female
                },
                AudioConfig = new AudioConfig
                {
                    AudioEncoding = AudioEncoding.Linear16
                }
            });

            AudioBuffer textAudio = new AudioBuffer(response.AudioContent.ToByteArray(), this.format);
            this.Out.Post(textAudio, e.OriginatingTime);
        }
    }
}
