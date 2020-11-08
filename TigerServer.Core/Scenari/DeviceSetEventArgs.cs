using TigerServer.Core.Infrastructor.Messages;

namespace TigerServer.Core.Infrastructor.Scenari
{
    public class DeviceSetEventArgs
    {
        public DeviceSetEventArgs(DeviceInfo device, string value)
        {
            Device = device;
            Value = value;
        }

        public DeviceInfo Device { get; set; }
        public string Value { get; set; }
    }
}
