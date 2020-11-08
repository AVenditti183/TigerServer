using TigerServer.Core.Infrastructor.Messages.Physical.Incoming;

namespace TigerServer.Core.Infrastructor.Messages.Orchestrator.Incoming
{
    public record DeviceStatus(DeviceInfo Source)
    {
        public DevicePhysicalStatus DevicePhysicalStatus => new DevicePhysicalStatus(this.Source);
    }
}