using System;

namespace TigerServer.Core.Infrastructor.Scenari.Triggers.Time
{
    public class TriggerTimeEveryXSecond : TriggerTime
    {
        public int Seconds { get; set; }
        private DateTime LastExecutionDate;

        public TriggerTimeEveryXSecond()
        {
            LastExecutionDate = DateTime.Now;
        }

        public override bool Triggered()
        {
            if (LastExecutionDate.AddSeconds(Seconds) < DateTime.Now)
            {
                LastExecutionDate = DateTime.Now;
                return true;
            }
            return false;
        }
    }
}
