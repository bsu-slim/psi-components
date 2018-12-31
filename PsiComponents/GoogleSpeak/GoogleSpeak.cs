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
    /// <summary>
    /// A Psi Component to translate text into audio bytes using Googe API.
    /// </summary>
    public class GoogleSpeak : ConsumerProducer<string, AudioBuffer>
    {
        /// <summary>
        /// Keeps track of the audio format that we want to produce to construct a PSI audio buffer
        /// </summary>
        private Microsoft.Psi.Audio.WaveFormat format { get; set; }
        /// <summary>
        /// Keeps track of the audio format that we want to produce to use with Google Speech API
        /// </summary>
        private AudioEncoding googleAudioFormat;
        /// <summary>
        /// Keeps track of the language of the text being passed in for tranlating to audio bytes
        /// </summary>
        public string TextLanguageCode { get; set; }
        /// <summary>
        /// Google API client object
        /// </summary>
        private TextToSpeechClient gClient;


        public GoogleSpeak(Pipeline pipeline, Microsoft.Psi.Audio.WaveFormat format, string languageCode) : base(pipeline)
        {
            this.gClient = TextToSpeechClient.Create();
            this.format = format;
            this.TextLanguageCode = languageCode;
            if (format.Equals(Microsoft.Psi.Audio.WaveFormat.Create16kHz1Channel16BitPcm())) {
                this.googleAudioFormat = AudioEncoding.Linear16;
            }
        }

        /// <summary>
        /// Handles pipeline packages of text. Translates text to audio and sends audio bytes
        /// down the pipeline
        /// </summary>
        /// <param name="text">Text to turn into audio</param>
        /// <param name="e">Pipeline Envelope</param>
        protected override void Receive(string text, Envelope e)
        {
            var response = gClient.SynthesizeSpeech(new SynthesizeSpeechRequest
            {
                Input = new SynthesisInput
                {
                    Text = text
                },
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
