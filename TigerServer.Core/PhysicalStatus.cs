using System;
namespace TigerServer.Core.Infrastructor
{
    public record PhysicalStatus(
            GatewayPhysical[] Gateways
        );
    

    public record GatewayPhysical(string Id,string Value,bool IsActive, DevicePhysical[] Devices);

    public record DevicePhysical(string Id,string Value,bool IsActive);
}
