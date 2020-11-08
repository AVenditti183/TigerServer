using TigerServer.Core.Infrastructor.DashBoards;

namespace TigerServer.Core.Infrastructor.Scenari.Triggers.Gateway
{
    public class TriggerGatewayEqualsState : TriggerGateway
    {
        public string Value { get; set; }

        public override bool Triggered(GatewayPhysical gateway)
        {
            return Value == gateway.Value;
        }
    }
}
