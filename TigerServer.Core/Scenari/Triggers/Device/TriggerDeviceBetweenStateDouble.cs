using System;
using TigerServer.Core.Infrastructor.DashBoards;

namespace TigerServer.Core.Infrastructor.Scenari.Triggers.Device
{

    public class TriggerDeviceBetweenStateDouble : TriggerDevice
    {
        public double MinValue { get; set; }
        public double MaxValue { get; set; }

        public override bool Triggered(DevicePhysical device)
        {
            if (!Single.TryParse(device.Value, out var floatvalue))
                return false;

            return floatvalue >= MinValue && floatvalue <= MaxValue;
        }
    }
}
