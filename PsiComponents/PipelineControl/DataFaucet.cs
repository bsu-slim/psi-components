using Microsoft.Psi;
using Microsoft.Psi.Audio;
using Microsoft.Psi.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineControl
{
    public class DataFaucet : ConsumerProducer<AudioBuffer, AudioBuffer>
    {
        public bool Open { get; set; } //is the faucet open

        public DataFaucet(Pipeline pipeline) : base(pipeline)
        {
            this.Open = false;
        }

        protected override void Receive(AudioBuffer audio, Envelope e)
        {
            if (this.Open)
            {
                this.Out.Post(audio, e.OriginatingTime);
            }
        }
    }
}
