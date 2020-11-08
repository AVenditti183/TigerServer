using System;
using TigerServer.Core.Infrastructor.DashBoards;

namespace TigerServer.Core.Infrastructor.Scenari.Triggers.Gateway
{
    public class TriggerGatewayBetweenStateDouble : TriggerGateway
    {
        public double MinValue { get; set; }
        public double MaxValue { get; set; }

        public override bool Triggered(GatewayPhysical gateway)
        {
            if (!Single.TryParse(gateway.Value, out var floatvalue))
                return false;

            return floatvalue >= MinValue && floatvalue <= MaxValue;
        }
    }
}
