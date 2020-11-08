using TigerServer.Core.Infrastructor.Messages.DashBoard.Incoming;
using TigerServer.Core.Infrastructor.Messages.Physical.Incoming;
using TigerServer.Core.Infrastructor.Messages.Repository.Incoming;
//using TigerServer.Core.Infrastructor.Messages.Scenari;

namespace TigerServer.Core.Infrastructor.Messages.Orchestrator.Incoming
{
    public record GatewaySended(string Source, string Value)
    {
        public GatewayPhysicalSended GatewayPhysicalSended => new GatewayPhysicalSended(this.Source, this.Value);
        public GatewayDashBoardSended GatewayDashBoardSended => new GatewayDashBoardSended(this.Source, this.Value);
        public GatewayRepositorySended GatewayRepositorySended => new GatewayRepositorySended(this.Source, this.Value);
        //public GatewayScenarioSended GatewayScenarioSended => new GatewayScenarioSended(this.Source, this.Value);
    }
}