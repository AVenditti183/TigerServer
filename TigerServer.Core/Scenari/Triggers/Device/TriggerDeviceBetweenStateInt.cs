using TigerServer.Core.Infrastructor.DashBoards;

namespace TigerServer.Core.Infrastructor.Scenari.Triggers.Device
{
    public class TriggerDeviceBetweenStateInt : TriggerDevice
    {
        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        public override bool Triggered(DevicePhysical device)
        {
            if (!int.TryParse(device.Value, out var intvalue))
                return false;

            return intvalue >= MinValue && intvalue <= MaxValue;
        }
    }
}
