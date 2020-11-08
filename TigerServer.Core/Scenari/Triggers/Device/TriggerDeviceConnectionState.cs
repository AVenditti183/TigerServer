using TigerServer.Core.Infrastructor.DashBoards;

namespace TigerServer.Core.Infrastructor.Scenari.Triggers.Device
{
    public class TriggerDeviceConnectionState : TriggerDevice
    {
        public bool ConnectionState { get; set; }

        public override bool Triggered(DevicePhysical device)
        {
            return device.IsActive == ConnectionState;
        }
    }
}
