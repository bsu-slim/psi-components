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
    /// <summary>
    /// Controls data flow of Pipeline objects. Allows some external control of the pipeline flow by
    /// toggling this faucet on and off. When the faucet is off it ignores incoming data packages from the pipeline
    /// and throws them away. When it is on it passes the data packets down the pipeline.
    /// </summary>
    public class DataFaucet<T> : ConsumerProducer<T, T>
    {

        /// <summary>
        /// Used to toggle data faucet on or off
        /// </summary>
        public bool Open { get; set; } //is the faucet open

        public DataFaucet(Pipeline pipeline) : base(pipeline)
        {
            this.Open = false;
        }

        /// <summary>
        /// Pipeline package handler. This will either throw away incoming data packets or pass them down the pipeline.
        /// Depending on the state of the facuet (open or closed).
        /// </summary>
        /// <param name="obj">Data packet</param>
        /// <param name="e">Pipleine Envelope</param>
        protected override void Receive(T obj, Envelope e)
        {
            if (this.Open)
            {
                this.Out.Post(obj, e.OriginatingTime);
            }
        }
    }
}
