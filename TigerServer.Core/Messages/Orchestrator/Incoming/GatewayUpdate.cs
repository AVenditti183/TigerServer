using TigerServer.Core.Infrastructor.Messages.DashBoard.Incoming;
using TigerServer.Core.Infrastructor.Messages.Physical.Incoming;
using TigerServer.Core.Infrastructor.Messages.Repository.Incoming;
//using TigerServer.Core.Infrastructor.Messages.Scenari;

namespace TigerServer.Core.Infrastructor.Messages.Orchestrator.Incoming
{
    public record GatewayUpdate(string Source, string Name)
    {
        public GatewayDashBoardUpdate GatewayDashBoardUpdate => new GatewayDashBoardUpdate(this.Source, this.Name);
        public GatewayRepositoryUpdate GatewayRepositoryUpdate => new GatewayRepositoryUpdate(this.Source, this.Name);
    }
}