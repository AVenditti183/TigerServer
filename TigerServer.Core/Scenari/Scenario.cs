using System;
using System.Collections.Generic;
using TigerServer.Core.Infrastructor.Scenari.Triggers;

namespace TigerServer.Core.Infrastructor.Scenari
{
    public class Scenario
    {
        public IEnumerable<Trigger> Triggers { get; set; }
        public IEnumerable<Action> Actions { get; set; }
        public DateTime LastExecution { get; set; }
    }
}
