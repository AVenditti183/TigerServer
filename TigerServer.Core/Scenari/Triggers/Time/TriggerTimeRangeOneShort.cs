using System;

namespace TigerServer.Core.Infrastructor.Scenari.Triggers.Time
{
    public class TriggerTimeRangeOneShort : TriggerTime
    {
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
        public DateTime Date { get; set; }

        public override bool Triggered()
        {
            return DateTime.Today == Date.Date && DateTime.Now.TimeOfDay >= From && DateTime.Now.TimeOfDay <= To;
        }
    }
}
