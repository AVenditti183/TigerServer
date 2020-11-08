using System.Threading;
using Akka.Actor;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using TigerServer.Core.Infrastructor.Messages.Physical.Outcoming;

namespace TigerServer.Core.Infrastructor.MQTT
{
    public class ClientMQTT :ReceiveActor
    {
        private readonly IMqttClient mqttClient;
        public ClientMQTT(string ClientId)
        {
            var options = new MqttClientOptionsBuilder()
                            .WithClientId(ClientId)
                            .WithTls()
                            .WithWebSocketServer("localhost:5001/mqtt")
                            .WithCleanSession()
                            .Build();

            var factory = new MqttFactory();
            mqttClient = factory.CreateMqttClient();

            mqttClient.ConnectAsync(options, CancellationToken.None).Wait();

            Receive<MsgMQTTSend>(async msg =>
            {
                var message = new MqttApplicationMessageBuilder()
                        .WithTopic(msg.topic)
                        .WithPayload(msg.payload)
                        .Build();
                if(!mqttClient.IsConnected)
                    await mqttClient.ConnectAsync(options, CancellationToken.None);

                await mqttClient.PublishAsync(message, CancellationToken.None);
            });
        }

        public static Props Props(string ClientId)
        {
            return Akka.Actor.Props.Create(() => new ClientMQTT(ClientId));
        }
    }
}
