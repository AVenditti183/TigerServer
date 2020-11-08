namespace TigerServer.Core.Infrastructor.Messages.Physical.Incoming
{
    public record GatewayPhysicalSet(string Source, string Value)
    {
        public GatewayPhysicalSetMsg GatewayPhysicalSetMsg => new GatewayPhysicalSetMsg(this.Source, this.Value);
    }
}