using System;

namespace TigerServer.Core.Infrastructor.Scenari.Triggers.Time
{
    public class TriggerTimeOneShort : TriggerTime
    {
        public DateTime Date { get; set; }

        public override bool Triggered()
        {
            return Date.Date == DateTime.Today && Date.Hour == DateTime.Now.Hour && Date.Minute == DateTime.Now.Minute;
        }
    }
}
