using TigerServer.Core.Infrastructor.Messages.DashBoard.Incoming;
using TigerServer.Core.Infrastructor.Messages.Physical.Incoming;
using TigerServer.Core.Infrastructor.Messages.Repository.Incoming;
//using TigerServer.Core.Infrastructor.Messages.Scenari;

namespace TigerServer.Core.Infrastructor.Messages.Orchestrator.Incoming
{
    public record DeviceCreated(DeviceInfo Source)
    {
        public DevicePhysicalCreated DevicePhysicalCreated => new DevicePhysicalCreated(this.Source);
        public DeviceDashBoardCreated DeviceDashBoardCreated => new DeviceDashBoardCreated(this.Source);
        public DeviceRepositoryCreated DeviceRepositoryCreated => new DeviceRepositoryCreated(this.Source);
        //public DeviceScenarioCreated DeviceScenarioCreated => new DeviceScenarioCreated(this.Source);
    }
}