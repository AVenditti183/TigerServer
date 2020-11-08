using TigerServer.Core.Infrastructor.DashBoards;

namespace TigerServer.Core.Infrastructor.Scenari.Triggers.Gateway
{
    public abstract class TriggerGateway : Trigger
    {
        public string GatewayId { get; set; }

        public override bool Triggered(params object[] obj)
            => Triggered((string)obj[0], (string)obj[1]);

        public abstract bool Triggered(GatewayPhysical gateway);
    }
}
