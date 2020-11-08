using TigerServer.Core.Infrastructor.DashBoards;

namespace TigerServer.Core.Infrastructor.Scenari.Triggers.Device
{
    public class TriggerDeviceValueGTInt : TriggerDevice
    {
        public int Value { get; set; }

        public override bool Triggered(DevicePhysical device)
        {
            if (!int.TryParse(device.Value, out var intvalue))
                return false;

            return intvalue > Value;
        }
    }
}
