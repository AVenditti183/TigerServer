using System;
using System.Threading.Tasks;
using Akka.Actor;
using TigerServer.Core.Infrastructor.Messages;
using TigerServer.Core.Infrastructor.Messages.Repository.Incoming;

namespace TigerServer.Core.Infrastructor.Repository
{
    public class RepositoryCoordinator : ReceiveActor
    {
        private readonly IRepository repository;
        public RepositoryCoordinator(IRepository repository)
        {
            this.repository = repository;

            Receive<GatewayRepositoryCreated>(async msg =>
            {
                await repository.Add(new GatewayModel
                {
                    Id = msg.Source,
                    IsActive = false,
                    Value = ""
                });
            });

            Receive<DeviceRepositoryCreated>(async msg =>
            {
                await repository.AddDevice(new DeviceModel
                {
                    Id = msg.Source.Source,
                    IsActive = false,
                    Value = ""
                }, msg.Source.Gateway);
            });

            Receive<GatewayRepositoryDroped>(async msg =>
            {
                var gateway = await repository.Get(msg.Source);
                await repository.Remove(gateway);
            });

            Receive<DeviceRepositoryDroped>(async msg =>
            {
                var device = await repository.GetDevice(msg.Source.Source, msg.Source.Gateway);
                if (device != null)
                    await repository.RemoveDevice(device, msg.Source.Gateway);
            });

            Receive<GatewayRepositorySended>(async msg =>
            {
                await repository.SetValue(msg.Source, msg.Value);
            });

            Receive<DeviceRepositorySended>(async msg =>
            {
                await repository.SetValueDevice(msg.Source.Source, msg.Value);
            });

            Receive<GatewayRepositoryStarted>(async msg =>
            {
                await UpdateGateway(msg.Source, gateway =>
                {
                    gateway.IsActive = true;
                    gateway.Ip = msg.Ip;
                    gateway.LastConnectionDate = DateTime.Now;
                });
            });

            Receive<DeviceRepositoryStarted>(async msg =>
            {
                await UpdateDevice(msg.Source.Source, msg.Source.Gateway, device =>
                 {
                     device.IsActive = true;
                     device.Ip = msg.Ip;
                     device.LastConnectionDate = DateTime.Now;
                 });
            });

            Receive<GatewayRepositoryDisconnected>(async msg =>
            {
                await UpdateGateway(msg.Source, gateway =>
                {
                    gateway.IsActive = false;
                });
            });

            Receive<DeviceRepositoryDisconnected>(async msg =>
            {
                await UpdateDevice(msg.Source.Source, msg.Source.Gateway, device =>
                {
                    device.IsActive = false;
                });
            });

            Receive<GatewayRepositorySet>(async msg =>
            {
                await repository.SetValue(msg.Source, msg.Value);
            });

            Receive<DeviceRepositorySet>(async msg =>
            {
                await repository.SetValueDevice(msg.Source.Source, msg.Value);
            });

            Receive<GatewayRepositoryUpdate>(async msg =>
            {
                await UpdateGateway(msg.Source, gateway =>
                {
                    gateway.Name = msg.Name;
                });
            });

            Receive<DeviceRepositoryUpdate>(async msg =>
            {
                await UpdateDevice(msg.Source.Source, msg.Source.Gateway, device =>
                {
                    device.Name = msg.Name;
                });
            });

            Receive<GetAll>(async msg => Sender.Tell(await repository.All()));
        }

        async Task UpdateGateway(string Id, Action<GatewayModel> action)
        {
            var gateway = await repository.Get(Id);
            if (gateway != null)
            {
                action(gateway);
                await repository.Update(gateway);
            }
        }

        async Task UpdateDevice(string Id, string gatewayId, Action<DeviceModel> action)
        {
            var device = await repository.GetDevice(Id, gatewayId);
            if (device != null)
            {
                action(device);
                await repository.UpdateDevice(device, gatewayId);
            }
        }

        public static Props Props(IRepository repository)
        {
            return Akka.Actor.Props.Create(() => new RepositoryCoordinator(repository));
        }
    }


}
