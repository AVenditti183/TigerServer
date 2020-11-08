using System;
using System.Collections.Generic;
using Akka.Actor;
using TigerServer.Core.Infrastructor.Messages;
using TigerServer.Core.Infrastructor.Messages.Orchestrator;
using TigerServer.Core.Infrastructor.Messages.Orchestrator.Incoming;
using TigerServer.Core.Infrastructor.Messages.Physical;
using TigerServer.Core.Infrastructor.Messages.Physical.Incoming;
using TigerServer.Core.Infrastructor.Messages.Physical.Outcoming;

namespace TigerServer.Core.Infrastructor.Physical
{
    public class DeviceManager : ReceiveActor
    {
        private readonly string _id;
        private bool _isActive;
        private string _value;
        private string _Ip;

        private Dictionary<string, IActorRef> devices;

        public DeviceManager(string id)
        {
            _id = id;
            _isActive = true;
            devices = new Dictionary<string, IActorRef>();

            #region Forward

            Receive<DevicePhysicalStarted>(msg =>
                Forward(msg.Source.Source, msg));

            Receive<DevicePhysicalDisconnected>(msg =>
                Forward(msg.Source.Source, msg));

            Receive<DevicePhysicalSended>(msg =>
                Forward(msg.Source.Source, msg));

            Receive<DevicePhysicalSet>(msg =>
                Forward(msg.Source.Source, msg));

            Receive<DevicePhysicalSetMsg>(msg => Context.Parent.Tell(msg));

            #endregion

            #region Gateway

            Receive<DevicePhysicalCreated>(msg =>
            {
                devices.TryGetValue(msg.Source.Source, out var device);
                if (device == null)
                {
                    device = Context.ActorOf(Device.Props(msg.Source.Source));
                    devices.Add(msg.Source.Source, device);
                    Context.TellOrc(new DeviceStatus(msg.Source));
                    Context.TellOrc(new PhysicalSetEnd());
                }
            });

            Receive<DevicePhysicalDroped>(msg =>
            {
                devices.Remove(msg.Source.Source);
                Context.TellOrc(new PhysicalSetEnd());

            });

            Receive<GatewayPhysicalDisconnected>(msg =>
            {
                _isActive = false;
                TellOrcForAllDevice(id => new DeviceDisconnected(new DeviceInfo(id, _id)));
                Context.TellOrc(new PhysicalSetEnd());
                
            });

            Receive<GatewayPhysicalStarted>(msg =>
            {
                _isActive = true;
                _Ip = msg.Ip;
                TellOrcForAllDevice(id => new DeviceStarted(new DeviceInfo(id, _id), msg.Ip));
                Context.Parent.Tell(new GatewayPhysicalSetMsg(msg.Source,_value));
                Context.TellOrc(new PhysicalSetEnd());
            });

            Receive<GatewayPhysicalSended>(msg =>
            {
                _value = msg.Value;
                _isActive = true;
                Context.TellOrc(new PhysicalSetEnd());
            });

            Receive<GatewayPhysicalSet>(msg =>
            {
                Context.Parent.Tell(msg.GatewayPhysicalSetMsg);
                Context.TellOrc(new PhysicalSetEnd());
            });

            Receive<RequiredId>(msg => Sender.Tell(_id));

            #endregion

            Receive<GatewayGetPhysical>(msg => {
            var devicesPhysical = new List<DevicePhysical>();

                foreach (var device in devices)
                {
                    devicesPhysical.Add(device.Value.Ask<DevicePhysical>(new DeviceGetPhysical()).Result);
                }

                Sender.Tell(new GatewayPhysical(_id, _value, _isActive, devicesPhysical.ToArray()));
            });
        }

        private void Forward(string id, object msg)
        {
            devices.TryGetValue(id, out var device);
            device?.Tell(msg);
        }

        private void TellOrcForAllDevice(Func<string, object> msg)
        {
            foreach (var device in devices)
            {
                Context.TellOrc(msg(device.Key));
            }
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(
                maxNrOfRetries: 10,
                withinTimeMilliseconds: 100,
                localOnlyDecider: ex => Directive.Restart);
        }

        public static Props Props(string id)
        {
            return Akka.Actor.Props.Create(() => new DeviceManager(id));
        }
    }


}