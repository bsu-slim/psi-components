using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Psi;
using Microsoft.Psi.Components;
using Microsoft.Psi.Audio;
using System.IO;
using NAudio.Wave;

namespace AudioOutputComponent
{
    /// <summary>
    /// PSI component that will consume audio bytes and play them through the speaker.
    /// Uses NAudio nuget package to do this.
    /// </summary>
    class AudioOutput : IConsumer<AudioBuffer>
    {
        /// <summary>
        /// Pipeline Audio Receiver
        /// </summary>
        public Receiver<AudioBuffer> In { get; set; }

        public AudioOutput(Pipeline pipeline)
        {
            this.In = pipeline.CreateReceiver<AudioBuffer>(this, Receive, nameof(this.In));
        }

        /// <summary>
        /// Pipeline Package handler that will process audio bytes and play them using speaker.
        /// </summary>
        /// <param name="audio">Audio bytes to play on speaker</param>
        /// <param name="e">Pipeline Envelope</param>
        protected void Receive(AudioBuffer audio, Envelope e)
        {
            IWaveProvider provider = new RawSourceWaveStream(new MemoryStream(audio.Data), new NAudio.Wave.WaveFormat(22000, 16, 1));
            var wo = new WaveOutEvent();
            wo.Init(provider);
            wo.Play();
        }
    }
}