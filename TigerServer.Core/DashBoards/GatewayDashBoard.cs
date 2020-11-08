using System;
using System.Collections.Generic;
using System.Linq;
namespace TigerServer.Core.Infrastructor.DashBoards
{
    public class GatewayDashBoard
    {
        public string Id { get; private set; }
        public bool IsActive { get; private set; }
        public string Ip { get; private set; }
        public string Value { get; private set; }
        public string Name { get; private set; }

        public List<DeviceDashBoard> Devices { get; private set; }

        public GatewayDashBoard(string id)
        {
            Id = id;
            IsActive = false;
            Devices = new List<DeviceDashBoard>();
        }

        public void Add(DeviceDashBoard device)
        {
            if (Devices.FirstOrDefault(o => o.Id == device.Id) == null)
                Devices.Add(device);
        }

        public void Remove(DeviceDashBoard device)
        {
            Devices = Devices.Where(o => o.Id != device.Id).ToList();
        }

        public void Disconnected() => IsActive = false;

        public void Start(string ip)
        {
            IsActive = true;
            this.Ip = ip;
        }

        public void Set(string value)
        {
            Value = value;
            IsActive = true;
        }

        public void Update(string Name) => this.Name = Name;

        public void DeviceDisconnectd(string id) => DeviceExcecute(id, d => d.Disconnected());

        public void DeviceStart(string id) => DeviceExcecute(id, d => d.Start());

        public void DeviceSet(string id, string value) => DeviceExcecute(id, d => d.Set(value));

        public void DeviceUpdate(string id, string Name) => DeviceExcecute(id, d => d.Update(Name));

        private void DeviceExcecute(string id, Action<DeviceDashBoard> action)
        {
            var device = Devices.FirstOrDefault(o => o.Id == id);
            if (device != null)
                action(device);
        }
    }


}