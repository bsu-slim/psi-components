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
    class AudioOutput : IConsumer<AudioBuffer>
    {
        public Receiver<AudioBuffer> In { get; set; }

        public AudioOutput(Pipeline pipeline)
        {
            this.In = pipeline.CreateReceiver<AudioBuffer>(this, Receive, nameof(this.In));
        }

        protected void Receive(AudioBuffer audio, Envelope e)
        {
            IWaveProvider provider = new RawSourceWaveStream(new MemoryStream(audio.Data), new NAudio.Wave.WaveFormat(22000, 16, 1));
            var wo = new WaveOutEvent();
            wo.Init(provider);
            wo.Play();
        }
    }
}