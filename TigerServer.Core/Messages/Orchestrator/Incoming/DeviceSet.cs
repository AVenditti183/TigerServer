using TigerServer.Core.Infrastructor.Messages.DashBoard.Incoming;
using TigerServer.Core.Infrastructor.Messages.Physical.Incoming;
using TigerServer.Core.Infrastructor.Messages.Repository.Incoming;
//using TigerServer.Core.Infrastructor.Messages.Scenari;

namespace TigerServer.Core.Infrastructor.Messages.Orchestrator.Incoming
{
    public record DeviceSet(DeviceInfo Source, string Value)
    {
        public DevicePhysicalSet DevicePhysicalSet => new DevicePhysicalSet(this.Source, this.Value);
        public DeviceDashBoardSet DeviceDashBoardSet => new DeviceDashBoardSet(this.Source, this.Value);
        public DeviceRepositorySet DeviceRepositorySet => new DeviceRepositorySet(this.Source, this.Value);
        //public DeviceScenarioSet DeviceScenarioSet => new DeviceScenarioSet(this.Source, this.Value);
    }
}