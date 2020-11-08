using TigerServer.Core.Infrastructor.DashBoards;

namespace TigerServer.Core.Infrastructor.Scenari.Triggers.Gateway
{
    public class TriggerGatewayBetweenStateInt : TriggerGateway
    {
        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        public override bool Triggered(GatewayPhysical gateway)
        {
            if (!int.TryParse(gateway.Value, out var intvalue))
                return false;

            return intvalue >= MinValue && intvalue <= MaxValue;
        }
    }
}
