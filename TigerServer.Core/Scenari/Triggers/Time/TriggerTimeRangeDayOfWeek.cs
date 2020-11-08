using System;

namespace TigerServer.Core.Infrastructor.Scenari.Triggers.Time
{
    public class TriggerTimeRangeDayOfWeek : TriggerTime
    {
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
        public DayOfWeek DayOfWeek { get; set; }

        public override bool Triggered()
        {
            return DateTime.Today.DayOfWeek == DayOfWeek && DateTime.Now.TimeOfDay >= From && DateTime.Now.TimeOfDay <= To;
        }
    }
}
