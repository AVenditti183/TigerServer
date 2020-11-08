namespace TigerServer.Core.Infrastructor.Messages.Physical.Outcoming
{
    public record MsgMQTTSend(string topic, string payload);
}