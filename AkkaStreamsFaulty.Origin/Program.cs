using Akka.Actor;
using Akka.Streams;
using AkkaStreamsFaulty.Common.Helpers;
using AkkaStreamsFaulty.Common.Messages;
using AkkaStreamsFaulty.Origin.Actors;
using AkkaStreamsFaulty.Origin.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Streams.Dsl;

namespace AkkaStreamsFaulty.Origin
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ColourConsole.WriteLineYellow("Creating ReceiverActorSystem");
            ActorSystem system = ActorSystem.Create("ReceiverActorSystem");

            ColourConsole.WriteLineYellow("Creating DataReceiverActor");
            IActorRef receiver = system.ActorOf(Props.Create<DataReceiver>(), "DataReceiverActor");

            ActorMaterializer materializer = system.Materializer();

            var ready = await receiver.Ask<MeasurementsSinkReady>(new PrepareUpload(121), timeout: TimeSpan.FromSeconds(30));
            Source.From(Enumerable.Range(1, 100))
                .Select(i => i.ToString())
                .RunWith(ready.SinkRef.Sink, materializer);

            Console.ReadLine();
        }
    }
}
