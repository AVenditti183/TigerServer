namespace TigerServer.Core.Infrastructor
{
    public class DeviceSetValueAction : Action
    {
        public string GatewayId { get; set; }
        public string DeviceId { get; set; }
        public string Value { get; set; }
    }
}
