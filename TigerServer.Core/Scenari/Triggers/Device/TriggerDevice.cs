using TigerServer.Core.Infrastructor.DashBoards;
using TigerServer.Core.Infrastructor.Messages;

namespace TigerServer.Core.Infrastructor.Scenari.Triggers.Device
{
    public abstract class TriggerDevice : Trigger
    {
        public DeviceInfo Device { get; set; }

        public override bool Triggered(params object[] obj)
            => Triggered((DeviceInfo) obj[0],(string) obj[1]);

        public abstract bool Triggered(DevicePhysical device);
    }
}
