using TigerServer.Core.Infrastructor.Messages.DashBoard.Incoming;
using TigerServer.Core.Infrastructor.Messages.Physical.Incoming;
using TigerServer.Core.Infrastructor.Messages.Repository.Incoming;
//using TigerServer.Core.Infrastructor.Messages.Scenari;

namespace TigerServer.Core.Infrastructor.Messages.Orchestrator.Incoming
{
    public record GatewayDroped(string Source)
    {
        public GatewayPhysicalDroped GatewayPhysicalDroped => new GatewayPhysicalDroped(this.Source);
        public GatewayDashBoardDroped GatewayDashBoardDroped => new GatewayDashBoardDroped(this.Source);
        public GatewayRepositoryDroped GatewayRepositoryDroped => new GatewayRepositoryDroped(this.Source);
        //public GatewayScenarioDroped GatewayScenarioDroped => new GatewayScenarioDroped(this.Source);
    }
}