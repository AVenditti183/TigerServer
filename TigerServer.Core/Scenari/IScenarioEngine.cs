using System;

namespace TigerServer.Core.Infrastructor.Scenari
{
    public interface IScenarioEngine
    {
        event EventHandler<DeviceSetEventArgs> SetDevice;
        event EventHandler<GatewaySetEventArgs> SetGateway;
        void Evalutate(PhysicalStatus status);
    }
}
