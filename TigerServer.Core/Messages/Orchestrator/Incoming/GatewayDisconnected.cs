using TigerServer.Core.Infrastructor.Messages.DashBoard.Incoming;
using TigerServer.Core.Infrastructor.Messages.Physical.Incoming;
using TigerServer.Core.Infrastructor.Messages.Repository.Incoming;
//using TigerServer.Core.Infrastructor.Messages.Scenari;

namespace TigerServer.Core.Infrastructor.Messages.Orchestrator.Incoming
{
    public record GatewayDisconnected(string Source)
    {
        public GatewayPhysicalDisconnected GatewayPhysicalDisconnected => new GatewayPhysicalDisconnected(this.Source);
        public GatewayDashBoardDisconnected GatewayDashBoardDisconnected => new GatewayDashBoardDisconnected(this.Source);
        public GatewayRepositoryDisconnected GatewayRepositoryDisconnected => new GatewayRepositoryDisconnected(this.Source);
        //public GatewayScenarioDisconnected GatewayScenarioDisconnected => new GatewayScenarioDisconnected(this.Source);
    }
}