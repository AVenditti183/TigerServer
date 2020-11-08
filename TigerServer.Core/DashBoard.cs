using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TigerServer.Core.Infrastructor.DashBoards;
using System.Linq;

namespace TigerServer.Core.Infrastructor
{
    public class DashBoard
    {
        public List<GatewayDashBoard> Gateways { get; set; }

        public event EventHandler Change;

        public DashBoard()
        {
            Gateways = new List<GatewayDashBoard>();
        }

        public void HasChange()
        {
            Change?.Invoke(this, new EventArgs());
        }
    }

    public class IoTWorld
    {
        private List<IDevice> devices;
        public ReadOnlyCollection<IDevice> Devices => devices.AsReadOnly();

        public IoTWorld()
        { 
            
        }

        public void add()
        {
           
        }
    }

    public interface IDevice
    {
        public string Id { get; init; }
        public IDeviceTransport Transport { get; init; }

    }

    public interface IDeviceTransport
    {

    }

}