using System;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Server;
using TigerServer.Core.Infrastructor.Messages;
using System.Linq;
using MQTTnet.Server.Status;
using TigerServer.Core.Infrastructor.Messages.Orchestrator;
using TigerServer.Core.Infrastructor.Messages.Orchestrator.Incoming;

namespace TigerServer.Core.Infrastructor.MQTT
{
    public class ServerMQTT
    {
        private IMqttServer _server;
        private readonly string servername;
        
        public event EventHandler<object> ReceiveMsg;

        public ServerMQTT(IMqttServer server,string internalClientId)
        {
            _server = server;
            servername = internalClientId;

            _server.UseClientConnectedHandler( async e =>
            {
                MqttClientStatus clientStatus =(MqttClientStatus)(await _server.GetClientStatusAsync()).FirstOrDefault(o => o.ClientId==e.ClientId);

                Console.WriteLine($"Connectd {e.ClientId}");
                Console.WriteLine($"{clientStatus.Endpoint}");

                await clientStatus.Session.ClearPendingApplicationMessagesAsync();

                ReceiveMsg?.Invoke(this, new GatewayStarted(e.ClientId, clientStatus.Endpoint));
            });

            _server.UseClientDisconnectedHandler(e =>
           {
               ReceiveMsg?.Invoke(this, new GatewayDisconnected(e.ClientId));
           });

            _server.UseApplicationMessageReceivedHandler(msg =>
           {
               Console.WriteLine($"Topic {msg.ClientId} {msg.ApplicationMessage.Topic} payload {msg.ApplicationMessage.ConvertPayloadToString()}");

               if (msg.ClientId == _server.Options.ClientId || msg.ClientId== servername)
                   return;

               var msgContext = ToMessage(msg.ApplicationMessage.Topic, msg.ApplicationMessage.ConvertPayloadToString());

               if (msgContext != null)
                   ReceiveMsg?.Invoke(this, msgContext);

           });
        }

        private object ToMessage(string topic,string value)
        {
            if (topic.Contains("devices"))
                return ToDeviceMessage(topic, value);
            return ToGetawayMessage(topic, value);
             
        }

        private object ToDeviceMessage(string topic,string value)
        {
            var gatewayId = topic.Split("/")[0];
            var deviceId = topic.Split("/")[2];
            var deviceInfo = new DeviceInfo(deviceId, gatewayId);
            return topic.Split("/")[3].ToLower() switch
            {
                "started" => new DeviceStarted(deviceInfo, value),
                "disconnectd" => new DeviceDisconnected(deviceInfo),
                "send" => new DeviceSended(deviceInfo,value),
                "set" => new DeviceSet(deviceInfo ,value),
                _ => null
            };
            
        }

        private object ToGetawayMessage(string topic,string value)
        {
            var gatewayId = topic.Split("/")[0];
           
            return topic.Split("/")[1].ToLower() switch
            {
                "started" => new GatewayStarted(gatewayId, value),
                "disconnectd" => new GatewayDisconnected(gatewayId),
                "send" => new GatewaySended(gatewayId, value),
                "set" => new GatewaySet(gatewayId, value),
                _ => null
            };
        }
    }
}
