using TigerServer.Core.Infrastructor.Messages.DashBoard.Incoming;
using TigerServer.Core.Infrastructor.Messages.Physical.Incoming;
using TigerServer.Core.Infrastructor.Messages.Repository.Incoming;
//using TigerServer.Core.Infrastructor.Messages.Scenari;

namespace TigerServer.Core.Infrastructor.Messages.Orchestrator.Incoming
{
    public record DeviceDroped(DeviceInfo Source)
    {
        public DevicePhysicalDroped DevicePhysicalDroped => new DevicePhysicalDroped(this.Source);
        public DeviceDashBoardDroped DeviceDashBoardDroped => new DeviceDashBoardDroped(this.Source);
        public DeviceRepositoryDroped DeviceRepositoryDroped => new DeviceRepositoryDroped(this.Source);
        //public DeviceScenarioDroped DeviceScenarioDroped => new DeviceScenarioDroped(this.Source);
    }
}