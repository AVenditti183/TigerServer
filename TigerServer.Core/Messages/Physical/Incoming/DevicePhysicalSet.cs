namespace TigerServer.Core.Infrastructor.Messages.Physical.Incoming
{
    public record DevicePhysicalSet(DeviceInfo Source, string Value)
    {
        public DevicePhysicalSetMsg DevicePhysicalSetMsg => new DevicePhysicalSetMsg(this.Source, this.Value);
    }
}