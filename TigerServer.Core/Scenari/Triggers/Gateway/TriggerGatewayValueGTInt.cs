using TigerServer.Core.Infrastructor.DashBoards;

namespace TigerServer.Core.Infrastructor.Scenari.Triggers.Gateway
{
    public class TriggerGatewayValueGTInt : TriggerGateway
    {
        public int Value { get; set; }

        public override bool Triggered(GatewayPhysical gateway)
        {
            if (!int.TryParse(gateway.Value, out var intvalue))
                return false;

            return intvalue > Value;
        }
    }
}
