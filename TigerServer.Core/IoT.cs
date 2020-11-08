using System.Collections.Generic;
using Akka.Actor;
using MQTTnet.Server;
using TigerServer.Core.Infrastructor.DashBoards;
using TigerServer.Core.Infrastructor.Messages;
using TigerServer.Core.Infrastructor.Messages.Orchestrator.Incoming;
using TigerServer.Core.Infrastructor.MQTT;
using TigerServer.Core.Infrastructor.Repository;
using TigerServer.Core.Infrastructor.Scenari;

namespace TigerServer.Core.Infrastructor
{
    public class IoT
    {
        private ActorSystem system;
        private IActorRef orchastraion;
        private DashBoard dashBoard;
        private ServerMQTT mqtt;
        public IoT(DashBoard dashBoard, ServerMQTT mqtt,string mqttClientId,IRepository repository,IScenarioEngine engine)
        {
            this.dashBoard = dashBoard;
            system = ActorSystem.Create("iot");
            orchastraion = system.ActorOf(Orchestration.Props(this.dashBoard, mqttClientId,repository,engine), "orch");

            //AddGateway(new GatewayDashBoard("1"));
            //AddDevice(new GatewayDashBoard("1"), new DeviceDashBoard("2"));

            orchastraion.Tell(new SystemStart());

            this.mqtt = mqtt;
            mqtt.ReceiveMsg += Mqtt_ReceiveMsg;
        }

        private void Mqtt_ReceiveMsg(object sender, object msg)
        {
            orchastraion.Tell(msg);
        }

        public List<GatewayDashBoard> DashBoard => orchastraion.Ask<List<GatewayDashBoard>>(new GetGatewaysBoard()).Result;

        public void AddGateway(GatewayDashBoard gateway)
        {
            orchastraion.Tell(new GatewayCreated(gateway.Id));
            foreach (var device in gateway.Devices)
            {
                orchastraion.Tell(new DeviceCreated(new DeviceInfo(device.Id, gateway.Id)));
            }
        }

        public void AddDevice(GatewayDashBoard gateway, DeviceDashBoard device)
        {
            orchastraion.Tell(new DeviceCreated(new DeviceInfo(device.Id, gateway.Id)));
        }

        public void SendMsg(object msg)
        {
            orchastraion.Tell(msg);
        }
    }


}