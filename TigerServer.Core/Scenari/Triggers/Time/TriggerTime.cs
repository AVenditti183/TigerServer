using System;

namespace TigerServer.Core.Infrastructor.Scenari.Triggers.Time
{
    public abstract class TriggerTime : Trigger
    {
        public override bool Triggered(params object[] obj)
            => Triggered();

        public abstract bool Triggered();
    }
}
