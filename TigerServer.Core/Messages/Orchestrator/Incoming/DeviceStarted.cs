using TigerServer.Core.Infrastructor.Messages.DashBoard.Incoming;
using TigerServer.Core.Infrastructor.Messages.Physical.Incoming;
using TigerServer.Core.Infrastructor.Messages.Repository.Incoming;
//using TigerServer.Core.Infrastructor.Messages.Scenari;

namespace TigerServer.Core.Infrastructor.Messages.Orchestrator.Incoming
{
    public record DeviceStarted(DeviceInfo Source, string Ip)
    {
        public DevicePhysicalStarted DevicePhysicalStarted => new DevicePhysicalStarted(this.Source, this.Ip);
        public DeviceDashBoardStarted DeviceDashBoardStarted => new DeviceDashBoardStarted(this.Source, this.Ip);
        public DeviceRepositoryStarted DeviceRepositoryStarted => new DeviceRepositoryStarted(this.Source, this.Ip);
        //public DeviceScenarioStarted DeviceScenarioStarted => new DeviceScenarioStarted(this.Source, this.Ip);
    }
}