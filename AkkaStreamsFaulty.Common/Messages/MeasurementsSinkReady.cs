using Akka.Streams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaStreamsFaulty.Common.Messages
{
    public sealed class MeasurementsSinkReady
    {
        public int Id { get; }
        public ISinkRef<string> SinkRef { get; }
        public MeasurementsSinkReady(int id, ISinkRef<string> sinkRef)
        {
            Id = id;
            SinkRef = sinkRef;
        }
    }
}
