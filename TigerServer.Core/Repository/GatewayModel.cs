using System;
using System.Collections.Generic;

namespace TigerServer.Core.Infrastructor.Repository
{
    public class GatewayModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public string Value { get; set; }
        public DateTime? LastValueDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastConnectionDate { get; set; }
        public ICollection<DeviceModel> Devices { get; set; }
        public ICollection<GatewayHistoryModel> History { get; set; }
    }


}
