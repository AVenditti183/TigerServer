using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace TigerServer.Core.Infrastructor.Repository
{
    public class RepositoryInMemory : IRepository
    {
        private List<GatewayModel> gateways;
        public RepositoryInMemory()
        {
            gateways = new List<GatewayModel>();
            gateways.Add(new GatewayModel()
            {
                Id = "1",
                Name="Gabbiotto",
                Ip="Ip gateway",
                History = new List<GatewayHistoryModel>(),
                Devices = new List<DeviceModel> {
                    new DeviceModel
                    {
                        Id="2",
                        Name ="Sensore 1",
                        Ip ="Ip sensore",
                        History = new List<DeviceHistoryModel>()
                    }
                }
            });
        }

        public Task Add(GatewayModel gateway)
        {
            gateway.Devices = gateway.Devices ?? new List<DeviceModel>();
            gateway.History = gateway.History ?? new List<GatewayHistoryModel>();

            gateways.Add(gateway);
            return Task.CompletedTask;
        }

        public Task AddDevice(DeviceModel device, string gatewayId)
        {
            device.History = device.History ?? new List<DeviceHistoryModel>();

            gateways.FirstOrDefault(o => o.Id == gatewayId)?.Devices.Add(device);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<GatewayModel>> All()
        {
            return Task.FromResult<IEnumerable<GatewayModel>>(gateways);

        }

        public Task<GatewayModel> Get(string Id)
        {
            return Task.FromResult<GatewayModel>(gateways.FirstOrDefault(o => o.Id == Id));
        }

        public Task<DeviceModel> GetDevice(string Id, string gatewayId)
        {
            return Task.FromResult<DeviceModel>(gateways.FirstOrDefault(o => o.Id == gatewayId)?.Devices.FirstOrDefault(s => s.Id == Id));
        }

        public Task<string> NewDeviceId()
        {
            throw new NotImplementedException();
        }

        public Task<string> NewGatewayId()
        {
            throw new NotImplementedException();
        }

        public Task Remove(GatewayModel gateway)
        {
            gateways.Remove(gateway);
            return Task.CompletedTask;
        }

        public Task RemoveDevice(DeviceModel device, string gatewayId)
        {
            gateways.FirstOrDefault(o => o.Id == gatewayId)?.Devices.Remove(device);
            return Task.CompletedTask;
        }

        public Task SetValue(string Id, string value)
        {
            var gateway = gateways.FirstOrDefault(o => o.Id == Id);
            if (gateway != null)
            {
                gateway.Value = value;
                gateway.LastValueDate = DateTime.Now;
                gateway.History.Add(new GatewayHistoryModel { Date = DateTime.Now, Value = value });
            }

            Console.WriteLine(JsonSerializer.Serialize(gateways));
            return Task.CompletedTask;
        }

        public Task SetValueDevice(string Id, string value)
        {
            var device = gateways.FirstOrDefault(o => o.Devices.Any(p => p.Id == Id))?.Devices.FirstOrDefault(s => s.Id == Id);
            if (device != null)
            {
                device.Value = value;
                device.LastValueDate = DateTime.Now;
                device.History.Add(new DeviceHistoryModel { Date = DateTime.Now, Value = value });
            }

            Console.WriteLine(JsonSerializer.Serialize(gateways));
            return Task.CompletedTask;
        }

        public Task Update(GatewayModel gateway)
        {
            var gatewayrepo = gateways.FirstOrDefault(o => o.Id == gateway.Id);
            if (gatewayrepo != null)
            {
                gatewayrepo.Ip = gateway.Ip;
                gatewayrepo.IsActive = gateway.IsActive;
                gatewayrepo.LastConnectionDate = gateway.LastConnectionDate;
                gatewayrepo.Value = gateway.Value;
                gatewayrepo.Name = gateway.Name;
                gatewayrepo.LastValueDate = gateway.LastValueDate;
            }

            return Task.CompletedTask;
        }

        public Task UpdateDevice(DeviceModel device, string gatewayId)
        {
            var devicerepo = gateways.FirstOrDefault(o => o.Id == gatewayId)?.Devices.FirstOrDefault(s => s.Id == device.Id);
            if (devicerepo != null)
            {
                devicerepo.Ip = device.Ip;
                devicerepo.IsActive = device.IsActive;
                devicerepo.LastConnectionDate = device.LastConnectionDate;
                devicerepo.Value = device.Value;
                devicerepo.Name = device.Name;
                devicerepo.LastValueDate = device.LastValueDate;
            }

            return Task.CompletedTask;
        }
    }


}
