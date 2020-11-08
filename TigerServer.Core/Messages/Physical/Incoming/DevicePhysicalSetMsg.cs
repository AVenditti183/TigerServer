namespace TigerServer.Core.Infrastructor.Messages.Physical.Incoming
{
    public record DevicePhysicalSetMsg(DeviceInfo Source, string Value);
}