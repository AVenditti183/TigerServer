using System;
using System.Collections.Generic;
using System.Linq;
using TigerServer.Core.Infrastructor.DashBoards;
using TigerServer.Core.Infrastructor.Messages;
using TigerServer.Core.Infrastructor.Scenari.Triggers;
using TigerServer.Core.Infrastructor.Scenari.Triggers.Device;
using TigerServer.Core.Infrastructor.Scenari.Triggers.Gateway;
using TigerServer.Core.Infrastructor.Scenari.Triggers.Time;

namespace TigerServer.Core.Infrastructor.Scenari
{
    public class ScenarioEngine : IScenarioEngine
    {
        private IEnumerable<Scenario> scenari;

        public event EventHandler<DeviceSetEventArgs> SetDevice;
        public event EventHandler<GatewaySetEventArgs> SetGateway;

        public ScenarioEngine(IEnumerable<Scenario> scenari)
        {
            this.scenari = scenari;
        }

        public void Evalutate(PhysicalStatus status)
        {
            foreach (var scenario in scenari)
            {
                if (Triggered(scenario.Triggers, status))
                    foreach (var action in scenario.Actions)
                    {
                        ExecuteAction(action, status);
                    }
            }
        }

        private bool Triggered(IEnumerable<Trigger> triggers, PhysicalStatus status)
        {
            var result = true;

            foreach (var trigger in triggers)
            {
                var triggerResult = trigger switch
                {
                    TriggerTime => trigger.Triggered(),
                    TriggerDevice => EvaluateTriggerDevice((TriggerDevice)trigger, status),
                    TriggerGateway => ((TriggerGateway)trigger).Triggered(status.Gateways.FirstOrDefault(g => g.Id == ((TriggerGateway)trigger).GatewayId)),
                    _ => false
                };

                if (!triggerResult)
                    result = triggerResult;
            }

            return result;
        }

        private bool EvaluateTriggerDevice(TriggerDevice trigger, PhysicalStatus status)
        {
            var device = getDevice(status,trigger.Device.Source, trigger.Device.Gateway);
            if (device != null)
            {
                return ((TriggerDevice)trigger).Triggered(device);
            }
            return false;
        }

        private void ExecuteAction(Action action, PhysicalStatus status)
        {
            if (action is DeviceSetValueAction)
            {
                var actionDevice = (DeviceSetValueAction)action;
                var device = getDevice(status,actionDevice.DeviceId, actionDevice.GatewayId);
                if (device != null && device.Value != actionDevice.Value)
                {
                    SetDevice?.Invoke(this, new DeviceSetEventArgs(new DeviceInfo(actionDevice.DeviceId, actionDevice.GatewayId), actionDevice.Value));
                }
            }

            if (action is GatewaySetValueAction)
            {
                var actionGateway = (GatewaySetValueAction)action;
                var gateway = status.Gateways.FirstOrDefault(g => g.Id == actionGateway.GatewayId);
                if (gateway != null && gateway.Value != actionGateway.Value)
                {
                    SetGateway?.Invoke(this, new GatewaySetEventArgs(actionGateway.GatewayId, actionGateway.Value));
                }
            }
        }

        private DevicePhysical getDevice(PhysicalStatus status,string deviceId, string gatewayId)
        {
            return status.Gateways.FirstOrDefault(o => o.Id == gatewayId)?.Devices.FirstOrDefault(d => d.Id == deviceId);
        }
    }
}
