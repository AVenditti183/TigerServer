using System;
using TigerServer.Core.Infrastructor.DashBoards;

namespace TigerServer.Core.Infrastructor.Scenari.Triggers.Gateway
{
    public class TriggerGatewayValueLTDouble : TriggerGateway
    {
        public double Value { get; set; }

        public override bool Triggered(GatewayPhysical gateway)
        {
            if (!Single.TryParse(gateway.Value, out var floatvalue))
                return false;

            return floatvalue < Value;
        }
    }
}
