using TigerServer.Core.Infrastructor.DashBoards;

namespace TigerServer.Core.Infrastructor.Scenari.Triggers.Device
{
    public class TriggerDeviceEqualsState : TriggerDevice
    {
        public string Value { get; set; }

        public override bool Triggered(DevicePhysical device)
        {
            return Value == device.Value;
        }
    }
}
