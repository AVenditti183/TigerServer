using System;

namespace TigerServer.Core.Infrastructor.Scenari.Triggers.Time
{
    public class TriggerTimeOneShortDayOfWeek : TriggerTime
    {
        public TimeSpan Time { get; set; }
        public DayOfWeek DayOfWeek { get; set; }

        public override bool Triggered()
        {
            return DayOfWeek == DateTime.Today.DayOfWeek && Time.Hours == DateTime.Now.Hour && Time.Minutes == DateTime.Now.Minute;
        }
    }
}
