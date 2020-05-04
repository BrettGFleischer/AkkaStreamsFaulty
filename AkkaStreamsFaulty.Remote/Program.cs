using Akka.Actor;
using AkkaStreamsFaulty.Common.Helpers;
using AkkaStreamsFaulty.Remote.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaStreamsFaulty.Remote
{
    class Program
    {
        static void Main(string[] args)
        {
            ColourConsole.WriteLineYellow("Creating DataSourceActorSystem");
            ActorSystem system = ActorSystem.Create("DataSourceActorSystem");

            ColourConsole.WriteLineYellow("Creating DataSourceActor");
            IActorRef sourceActor = system.ActorOf(Props.Create<DataSource>(), "DataSourceActor");

            Console.ReadLine();
        }
    }
}
