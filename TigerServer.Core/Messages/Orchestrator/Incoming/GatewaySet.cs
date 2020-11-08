using TigerServer.Core.Infrastructor.Messages.DashBoard.Incoming;
using TigerServer.Core.Infrastructor.Messages.Physical.Incoming;
using TigerServer.Core.Infrastructor.Messages.Repository.Incoming;
//using TigerServer.Core.Infrastructor.Messages.Scenari;

namespace TigerServer.Core.Infrastructor.Messages.Orchestrator.Incoming
{
    public record GatewaySet(string Source, string Value)
    {
        public GatewayPhysicalSet GatewayPhysicalSet => new GatewayPhysicalSet(this.Source, this.Value);
        public GatewayDashBoardSet GatewayDashBoardSet => new GatewayDashBoardSet(this.Source, this.Value);
        public GatewayRepositorySet GatewayRepositorySet => new GatewayRepositorySet(this.Source, this.Value);
        //public GatewayScenarioSet GatewayScenarioSet => new GatewayScenarioSet(this.Source, this.Value);
    }
}