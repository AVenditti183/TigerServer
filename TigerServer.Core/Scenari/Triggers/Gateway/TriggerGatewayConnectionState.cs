using TigerServer.Core.Infrastructor.DashBoards;

namespace TigerServer.Core.Infrastructor.Scenari.Triggers.Gateway
{
    public class TriggerGatewayConnectionState : TriggerGateway
    {
        public bool ConnectionState { get; set; }

        public override bool Triggered(GatewayPhysical gateway)
        {
            return gateway.IsActive == ConnectionState;
        }
    }
}
