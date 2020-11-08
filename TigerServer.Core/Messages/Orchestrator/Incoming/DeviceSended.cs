using TigerServer.Core.Infrastructor.Messages.DashBoard.Incoming;
using TigerServer.Core.Infrastructor.Messages.Physical.Incoming;
using TigerServer.Core.Infrastructor.Messages.Repository.Incoming;
//using TigerServer.Core.Infrastructor.Messages.Scenari;

namespace TigerServer.Core.Infrastructor.Messages.Orchestrator
{
    public record DeviceSended(DeviceInfo Source, string Value)
    {
        public DevicePhysicalSended DevicePhysicalSended => new DevicePhysicalSended(this.Source, this.Value);
        public DeviceDashBoardSended DeviceDashBoardSended => new DeviceDashBoardSended(this.Source, this.Value);
        public DeviceRepositorySended DeviceRepositorySended => new DeviceRepositorySended(this.Source, this.Value);
        //public DeviceScenarioSended DeviceScenarioSended => new DeviceScenarioSended(this.Source, this.Value);
    }
}