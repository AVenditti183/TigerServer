using TigerServer.Core.Infrastructor.Messages.Physical.Incoming;

namespace TigerServer.Core.Infrastructor.Messages.Orchestrator.Incoming
{
    public record GatewayStatus(string Source)
    {
        public GatewayPhysicalStatus GatewayPhysicalStatus => new GatewayPhysicalStatus(this.Source);
    }
}