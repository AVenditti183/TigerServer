using System;
using Akka.Actor;
using TigerServer.Core.Infrastructor.Messages;
using TigerServer.Core.Infrastructor.Messages.Orchestrator;
using TigerServer.Core.Infrastructor.Messages.Orchestrator.Incoming;
using TigerServer.Core.Infrastructor.Messages.Scenari.Incoming;

namespace TigerServer.Core.Infrastructor.Scenari
{
    public class ScenarioCoordinator : ReceiveActor
    {
        private readonly IScenarioEngine engine;

        public ScenarioCoordinator(IScenarioEngine engine)
        {
            this.engine = engine;

            engine.SetDevice += Engine_SetDevice;
            engine.SetGateway += Engine_SetGateway;

            Receive<EvalutateScenari>(msg => {
                Console.WriteLine("Evalutate Scenario");
                //Console.WriteLine(JsonSeriliaze.Serialize(msg));
                engine.Evalutate(msg.Status);
            });
        }

        private void Engine_SetGateway(object sender, GatewaySetEventArgs e)
        {
            Context.TellOrc(new GatewaySet(e.GatewayId, e.Value));
        }

        private void Engine_SetDevice(object sender, DeviceSetEventArgs e)
        {
            Context.TellOrc(new DeviceSet(new DeviceInfo(e.Device.Source, e.Device.Gateway), e.Value));
        }

        public static Props Props(IScenarioEngine engine)
        {
            return Akka.Actor.Props.Create(() => new ScenarioCoordinator(engine));
        }
    }
}
