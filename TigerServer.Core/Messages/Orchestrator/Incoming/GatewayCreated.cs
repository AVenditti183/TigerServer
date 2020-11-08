using TigerServer.Core.Infrastructor.Messages.DashBoard.Incoming;
using TigerServer.Core.Infrastructor.Messages.Physical.Incoming;
using TigerServer.Core.Infrastructor.Messages.Repository.Incoming;
//using TigerServer.Core.Infrastructor.Messages.Scenari;

namespace TigerServer.Core.Infrastructor.Messages.Orchestrator.Incoming
{
    public record GatewayCreated(string Source)
    {
        public GatewayPhysicalCreated GatewayPhysicalCreated => new GatewayPhysicalCreated(this.Source);
        public GatewayDashBoardCreated GatewayDashBoardCreated => new GatewayDashBoardCreated(this.Source);
        public GatewayRepositoryCreated GatewayRepositoryCreated => new GatewayRepositoryCreated(this.Source);
        //public GatewayScenarioCreated GatewayScenarioCreated => new GatewayScenarioCreated(this.Source);
    }
}