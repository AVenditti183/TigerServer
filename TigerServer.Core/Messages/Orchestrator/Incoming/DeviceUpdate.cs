using TigerServer.Core.Infrastructor.Messages.DashBoard.Incoming;
using TigerServer.Core.Infrastructor.Messages.Physical.Incoming;
using TigerServer.Core.Infrastructor.Messages.Repository.Incoming;
//using TigerServer.Core.Infrastructor.Messages.Scenari;

namespace TigerServer.Core.Infrastructor.Messages.Orchestrator
{
    public record DeviceUpdate(DeviceInfo Source, string Name)
    {
        public DeviceDashBoardUpdate DeviceDashBoardUpdate => new DeviceDashBoardUpdate(this.Source, this.Name);
        public DeviceRepositoryUpdate DeviceRepositoryUpdate => new DeviceRepositoryUpdate(this.Source, this.Name);
    }
}