namespace TigerServer.Core.Infrastructor.Scenari
{
    public class GatewaySetEventArgs
    {
        public GatewaySetEventArgs(string gatewayId, string value)
        {
            GatewayId = gatewayId;
            Value = value;
        }

        public string GatewayId { get; set; }
        public string Value { get; set; }
    }
}
