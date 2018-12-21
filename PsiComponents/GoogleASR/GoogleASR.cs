using Google.Cloud.Speech.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Psi.Audio;
using Microsoft.Psi;
using Microsoft.Psi.Components;

namespace GoogleASRComponent
{
    public class GoogleASR : ConsumerProducer<AudioBuffer, string>
    {
        public string AudioLanguage { get; set; }
        private SpeechClient speech;

        public GoogleASR(Pipeline pipeline) : base(pipeline)
        {
            speech = SpeechClient.Create();
        }


        public GoogleASR(Pipeline pipeline, string audioLanguage) : base(pipeline)
        {
            this.speech = SpeechClient.Create();
            this.AudioLanguage = audioLanguage;
        }

        protected override void Receive(AudioBuffer audio, Envelope e)
        {
            if (audio.Data.Length > 0)
            {
                var response = speech.Recognize(new RecognitionConfig()
                {
                    Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
                    SampleRateHertz = 16000,
                    LanguageCode = this.AudioLanguage,
                    EnableWordTimeOffsets = true,
                }, RecognitionAudio.FromBytes(audio.Data));

                if (response.Results.Count > 0)
                {
                    string transcribedAudio = response.Results.First().Alternatives.First().Transcript;
                    this.Out.Post(transcribedAudio, e.OriginatingTime);
                }
            }
        }
    }
}