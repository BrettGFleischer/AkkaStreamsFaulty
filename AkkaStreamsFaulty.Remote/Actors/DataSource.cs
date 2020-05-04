using Akka;
using Akka.Actor;
using Akka.Streams;
using Akka.Streams.Dsl;
using AkkaStreamsFaulty.Common.Helpers;
using AkkaStreamsFaulty.Common.Messages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaStreamsFaulty.Remote.Actors
{
    public class DataSource : ReceiveActor
    {
        public static IActorRef DataReceiverActorRef { get; set; }
        public DataSource()
        {

            TimeSpan throttle = TimeSpan.FromMilliseconds(1);

            Receive<ConnectMessage>(request =>
            {
                ColourConsole.WriteLineMagenta("Received ConnectMessage");
                DataReceiverActorRef = Sender;
                request.ActorRef = Self;
                Sender.Tell(request);

            });

            Receive<MeasurementsSinkReady>(request =>
            {
                ColourConsole.WriteLineMagenta("Received MeasurementsSinkReady");
                // create a source
                StreamLogs(request.Id)
                    //Throttle outbound stream
                    .Throttle(1, throttle, 1, ThrottleMode.Shaping)
                    // materialize it using stream refs
                    .RunWith(request.SinkRef.Sink, Context.System.Materializer());
            });
        }

        private Source<string, NotUsed> StreamLogs(int streamId) =>
            Source.From(Enumerable.Range(1, 100)).Select(i => i.ToString());
    }
}
