using System.Collections.Generic;
using Akka.Actor;
using TigerServer.Core.Infrastructor.Messages;
using TigerServer.Core.Infrastructor.Messages.Orchestrator.Incoming;
using TigerServer.Core.Infrastructor.Messages.Physical.Incoming;
using TigerServer.Core.Infrastructor.Messages.Physical.Outcoming;
using TigerServer.Core.Infrastructor.Messages.Scenari.Incoming;
using TigerServer.Core.Infrastructor.MQTT;
using TigerServer.Core.Infrastructor.Scenari;

namespace TigerServer.Core.Infrastructor.Physical
{
    public class DeviceManagerCoordinator : ReceiveActor
    {
        private readonly string _id;
        private Dictionary<string, IActorRef> gateways;
        private IActorRef senderMQTT;
        private IActorRef scenariManager;
        public DeviceManagerCoordinator(string id,string ClientMQTTId,IScenarioEngine engine)
        {
            _id = id;
            senderMQTT = Context.ActorOf(ClientMQTT.Props(ClientMQTTId), "senderMQTT");
            scenariManager = Context.ActorOf(ScenarioCoordinator.Props(engine), "scenari");

            gateways = new Dictionary<string, IActorRef>();

            Receive<GatewayPhysicalCreated>(msg =>
            {
                gateways.TryGetValue(msg.Source, out var gateway);
                if (gateway == null)
                {
                    gateway = Context.ActorOf(DeviceManager.Props(msg.Source));
                    gateways.Add(msg.Source, gateway);
                    Context.TellOrc(new GatewayStatus(msg.Source));
                }
            });

            Receive<DevicePhysicalCreated>(msg =>
                Forward(msg.Source.Gateway, msg));

            Receive<GatewayPhysicalDroped>(msg =>
            {
                gateways.Remove(msg.Source);
            });

            Receive<DevicePhysicalDroped>(msg =>
                Forward(msg.Source.Gateway, msg));

            Receive<GatewayPhysicalSended>(msg =>
                Forward(msg.Source, msg));

            Receive<DevicePhysicalSended>(msg =>
                Forward(msg.Source.Gateway, msg));

            Receive<GatewayPhysicalStarted>(msg =>
                Forward(msg.Source, msg));

            Receive<DevicePhysicalStarted>(msg =>
                Forward(msg.Source.Gateway, msg));

            Receive<GatewayPhysicalDisconnected>(msg =>
                Forward(msg.Source, msg));

            Receive<DevicePhysicalDisconnected>(msg =>
                Forward(msg.Source.Gateway, msg));

            Receive<GatewayPhysicalSet>(msg =>
                Forward(msg.Source, msg));

            Receive<DevicePhysicalSet>(msg =>
                Forward(msg.Source.Gateway, msg));

            Receive<GatewayPhysicalStatus>(msg =>
               senderMQTT.Tell(new MsgMQTTSend($"{msg.Source}/status", "")));

            Receive<DevicePhysicalStatus>(msg =>
                senderMQTT.Tell(new MsgMQTTSend($"{msg.Source.Gateway}/devices/{msg.Source.Source}/status", "")));

            Receive<GatewayPhysicalSetMsg>(msg =>
                senderMQTT.Tell(new MsgMQTTSend($"{msg.Source}/set", msg.Value)));

            Receive<DevicePhysicalSetMsg>(msg =>
                senderMQTT.Tell(new MsgMQTTSend($"{msg.Source.Gateway}/devices/{msg.Source.Source}/set", msg.Value)));

            Receive<EvalutatePhysical>(msg => {

                var gatewaysStatus = new List<GatewayPhysical>();
                foreach(var gateway in gateways)
                {
                    gatewaysStatus.Add(gateway.Value.Ask<GatewayPhysical>(new GatewayGetPhysical()).Result);
                }
                scenariManager.Tell(new EvalutateScenari(new PhysicalStatus(gatewaysStatus.ToArray())));
            });

            
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(
                maxNrOfRetries: 10,
                withinTimeMilliseconds: 100,
                localOnlyDecider: ex => Directive.Restart);
        }

        private void Forward(string id, object msg)
        {
            gateways.TryGetValue(id, out var gateway);
            gateway?.Tell(msg);
        }

        public static Props Props(string id, string ClientMQTTId, IScenarioEngine engine)
        {
            return Akka.Actor.Props.Create(() => new DeviceManagerCoordinator(id, ClientMQTTId, engine));
        }

    }


}