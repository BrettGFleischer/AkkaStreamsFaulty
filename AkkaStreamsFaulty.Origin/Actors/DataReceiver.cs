using Akka.Actor;
using Akka.Streams;
using Akka.Streams.Dsl;
using AkkaStreamsFaulty.Common.Helpers;
using AkkaStreamsFaulty.Common.Messages;
using AkkaStreamsFaulty.Origin.Messages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaStreamsFaulty.Origin.Actors
{
    public class DataReceiver : ReceiveActor
    {
        public static IActorRef DataSourceActorRef { get; set; }

        private static string serverURL = ConfigurationManager.AppSettings.Get("ServerUrl");

        private string serverPort = ConfigurationManager.AppSettings.Get("ServerPort");
        public DataReceiver()
        {
            TimeSpan throttle = TimeSpan.FromMilliseconds(100);

            string RegistryConnectionString = "akka.tcp://DataSourceActorSystem@" + serverURL + ":" + serverPort + "/user/DataSourceActor";

            ColourConsole.WriteLineMagenta("Sending ConnectMessage");
            ConnectMessage requestAccess = Context.ActorSelection(RegistryConnectionString).Ask<ConnectMessage>(new ConnectMessage(Self)).Result;//Changes

            DataSourceActorRef = requestAccess.ActorRef;
            
            Receive<PrepareUpload>(prepare =>
            {
                ColourConsole.WriteLineMagenta("Sending MeasurementsSinkReady");
                // obtain a sink you want to offer
                var sink = LogsSinksFor(prepare.Id);

                // materialize sink ref (remote is source data for us)
                StreamRefs.SinkRef<string>()
                    .Throttle(1, throttle, 1, ThrottleMode.Shaping)
                    .To(sink)
                    .Run(Context.System.Materializer())
                    //I suspect the issue is within the "PipeTo" method, The remote actor never receives the "MeasurementsSinkReady" message
                    .PipeTo(Sender, success: sinkRef => new MeasurementsSinkReady(prepare.Id, sinkRef));
            });

        }

        //This Sink may be flawed
        private Sink<string, Task> LogsSinksFor(int id) =>
            Sink.ForEach<string>(Console.WriteLine);
    }
}
