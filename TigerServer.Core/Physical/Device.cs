using System.Net.Mail;
using System;
using Akka.Actor;
using TigerServer.Core.Infrastructor.Messages.Orchestrator;
using TigerServer.Core.Infrastructor.Messages.Orchestrator.Incoming;
using TigerServer.Core.Infrastructor.Messages.Physical;
using TigerServer.Core.Infrastructor.Messages.Physical.Incoming;
using TigerServer.Core.Infrastructor.Messages.Physical.Outcoming;

namespace TigerServer.Core.Infrastructor.Physical
{
    public class Device : UntypedActor
    {
        private string _id;
        private string _value;
        private bool _isActive;

        public Device(string id)
        {
            _id = id;
            _isActive = true;

        }
        //public Device()
        //{
        //    _isActive = true;
        //}

        //public override string PersistenceId => _id;

        
        protected override void OnReceive(object message)
        {
            switch (message) {
                case DevicePhysicalCreated msg:
                    _id = msg.Source.Source;
                    break;

                case DevicePhysicalStarted msg:
                    _isActive = true;
                    var gatewayDevicePhysicalStartedId = Context.Parent.Ask<string>(new RequiredId()).Result;
                    Context.Parent.Tell(new DevicePhysicalSetMsg(msg.Source, _value));
                    Context.TellOrc(new PhysicalSetEnd());
                    break;

                case DevicePhysicalDisconnected:
                    _isActive = false;
                    Context.TellOrc(new PhysicalSetEnd());
                    break;

                case DevicePhysicalSended msg:
                    _value = msg.Value;
                    _isActive = true;
                    Context.TellOrc(new PhysicalSetEnd());
                    break;

                case DeviceGetPhysical msg:
                    Sender.Tell(new DevicePhysical(_id,_value,_isActive));
                    break;

                case DevicePhysicalSet msg:
                    //throw new Exception();
                    _value = msg.Value;
                    var gatewayDevicePhysicalSetId = Context.Parent.Ask<string>(new RequiredId()).Result;
                    Context.Parent.Tell(msg.DevicePhysicalSetMsg);
                    Context.TellOrc(new PhysicalSetEnd());
                    break;

                default:
                    break;
                    //return false; 
            }
            //SaveSnapshot(this);

            //return true;
        }

        
        public static Props Props(string id)
        {
            return Akka.Actor.Props.Create(() => new Device(id));
        }
    }


}