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
    /// <summary>
    /// PSI component to convert audio bytes to text using Google API.
    /// </summary>
    public class GoogleASR : ConsumerProducer<AudioBuffer, string>
    {
        /// <summary>
        /// The language that incoming audio bytes will be interpretted as.
        /// </summary>
        public string AudioLanguage { get; set; }
        /// <summary>
        /// Google API client object
        /// </summary>
        private SpeechClient speech;

        /// <summary>
        /// Default constructor will set expected audio language to english.
        /// </summary>
        /// <param name="pipeline"></param>
        public GoogleASR(Pipeline pipeline) : base(pipeline)
        {
            speech = SpeechClient.Create();
            this.AudioLanguage = "en";
        }


        public GoogleASR(Pipeline pipeline, string audioLanguage) : base(pipeline)
        {
            this.speech = SpeechClient.Create();
            this.AudioLanguage = audioLanguage;
        }

        /// <summary>
        /// Pipeline function that will handle incoming pipeline packages of audio bytes.
        /// Will translate audio bytes to text and send text down the pipeline.
        /// </summary>
        /// <param name="audio"></param>
        /// <param name="e"></param>
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