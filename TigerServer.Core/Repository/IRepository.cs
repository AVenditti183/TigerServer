using System.Collections.Generic;
using System.Threading.Tasks;

namespace TigerServer.Core.Infrastructor.Repository
{
    public interface IRepository
    {
        Task<string> NewGatewayId();
        Task<string> NewDeviceId();

        Task<IEnumerable<GatewayModel>> All();
        Task<GatewayModel> Get(string Id);
        Task<DeviceModel> GetDevice(string Id, string gatewayId);

        Task Add(GatewayModel gateway);
        Task AddDevice(DeviceModel device, string gatewayId);

        Task Update(GatewayModel gateway);
        Task UpdateDevice(DeviceModel device, string gatewayId);

        Task Remove(GatewayModel gateway);
        Task RemoveDevice(DeviceModel device, string gatewayId);

        Task SetValue(string Id, string value);
        Task SetValueDevice(string Id, string value);
    }


}
