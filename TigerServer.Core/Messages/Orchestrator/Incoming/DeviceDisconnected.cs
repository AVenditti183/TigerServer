using TigerServer.Core.Infrastructor.Messages.DashBoard.Incoming;
using TigerServer.Core.Infrastructor.Messages.Physical.Incoming;
using TigerServer.Core.Infrastructor.Messages.Repository.Incoming;
//using TigerServer.Core.Infrastructor.Messages.Scenari;

namespace TigerServer.Core.Infrastructor.Messages.Orchestrator.Incoming
{
    public record DeviceDisconnected(DeviceInfo Source)
    {
        public DevicePhysicalDisconnected DevicePhysicalDisconnected => new DevicePhysicalDisconnected(this.Source);
        public DeviceDashBoardDisconnected DeviceDashBoardDisconnected => new DeviceDashBoardDisconnected(this.Source);
        public DeviceRepositoryDisconnected DeviceRepositoryDisconnected => new DeviceRepositoryDisconnected(this.Source);
        //public DeviceScenarioDisconnected DeviceScenarioDisconnected => new DeviceScenarioDisconnected(this.Source);
    }
}