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

namespace AkkaStreamsFaulty.Origin
{
    class Program
    {
        static void Main(string[] args)
        {
            ColourConsole.WriteLineYellow("Creating ReceiverActorSystem");
            ActorSystem system = ActorSystem.Create("ReceiverActorSystem");

            ColourConsole.WriteLineYellow("Creating DataReceiverActor");
            IActorRef receiver = system.ActorOf(Props.Create<DataReceiver>(), "DataReceiverActor");

            ActorMaterializer materializer = system.Materializer();

            using (system)
            {
                using (materializer)
                {
                    MeasurementsSinkReady ready = receiver.Ask<MeasurementsSinkReady>(new PrepareUpload(121)).Result;//, timeout: TimeSpan.FromSeconds(30)).Result;
                }
            }

            Console.ReadLine();
        }
    }
}
