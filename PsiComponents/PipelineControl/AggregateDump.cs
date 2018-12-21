using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Psi.Components;
using Microsoft.Psi.Audio;
using Microsoft.Psi;

namespace PipelineControl
{
    public class AggregateDump : ConsumerProducer<AudioBuffer, AudioBuffer>
    {
        private List<Byte> AudioData;
        private WaveFormat format;
        private bool Empty;
        private DateTime OriginatingTime;

        public AggregateDump(Pipeline pipeline) : base(pipeline)
        {
            this.AudioData = new List<byte>();
        }

        protected override void Receive(AudioBuffer data, Envelope envelope)
        {
            if (this.Empty) //We need to track the originating time of the whole audio for when we dump downstream in the pipeline
            {
                this.OriginatingTime = envelope.OriginatingTime;
                this.format = data.Format;
                this.Empty = false;
            }
            if (data.Data != null)
            {
                this.AudioData.AddRange(data.Data);
            }
        }

        public void Dump()
        {
            if (!this.Empty)
            {
                this.Out.Post(new AudioBuffer(this.AudioData.ToArray(), this.format), this.OriginatingTime);
                this.AudioData = new List<byte>();
                this.Empty = true;
            }
        }
    }
}
