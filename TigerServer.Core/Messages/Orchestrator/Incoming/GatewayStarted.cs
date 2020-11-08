using TigerServer.Core.Infrastructor.Messages.DashBoard.Incoming;
using TigerServer.Core.Infrastructor.Messages.Physical.Incoming;
using TigerServer.Core.Infrastructor.Messages.Repository.Incoming;
//using TigerServer.Core.Infrastructor.Messages.Scenari;

namespace TigerServer.Core.Infrastructor.Messages.Orchestrator.Incoming
{
    public record GatewayStarted(string Source, string Ip)
    {
        public GatewayPhysicalStarted GatewayPhysicalStarted => new GatewayPhysicalStarted(this.Source, this.Ip);
        public GatewayDashBoardStarted GatewayDashBoardStarted => new GatewayDashBoardStarted(this.Source, this.Ip);
        public GatewayRepositoryStarted GatewayRepositoryStarted => new GatewayRepositoryStarted(this.Source, this.Ip);
        //public GatewayScenarioStarted GatewayScenarioStarted => new GatewayScenarioStarted(this.Source, this.Ip);
    }
}