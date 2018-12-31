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
    /// <summary>
    /// PSI component to Aggregate PSI AudioBuffers then dump Aggregated Audio down
    /// pipeline as one PSI AudioBuffer when dump() is called.
    /// </summary>
    public class AggregateDump : ConsumerProducer<AudioBuffer, AudioBuffer>
    {
        /// <summary>
        /// List to aggregate audio
        /// </summary>
        private List<Byte> AudioData;

        /// <summary>
        /// Keeps track of audio format. The audio format of the first audio buffer aquired when aggregating
        /// will be used when constructing the Audiobuffer to dump down stream.
        /// </summary>
        private WaveFormat format;

        /// <summary>
        /// Keeps track of whether the AudioBuffer is currently empty so that we can keep track of the AudioFormat by using the AudioFormat
        /// of the first AudioBuffer. Also used to make sure we do not dump empty AudioBuffers.
        /// </summary>
        private bool Empty;

        /// <summary>
        /// Keeps track of the originating time of the first AudioBuffer. This is used for the Originating time that is passed down the pipeline
        /// with the aggregated audio that is dumped.
        /// </summary>
        private DateTime OriginatingTime;

        public AggregateDump(Pipeline pipeline) : base(pipeline)
        {
            this.AudioData = new List<byte>();
        }

        /// <summary>
        /// Pipeline package handler. Aggregates AudioBuffer data and keeps track of Originating time
        /// and AudioFormat.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="envelope"></param>
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

        /// <summary>
        /// Dumps aggregated Audio down pipeline in a PSI AudioBuffer object.
        /// 
        /// Will not do anything if no Audio has been aggregated.
        /// </summary>
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
