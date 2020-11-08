using System.Collections.Generic;
using Akka.Actor;
using TigerServer.Core.Infrastructor.DashBoards;
using TigerServer.Core.Infrastructor.Messages;
using TigerServer.Core.Infrastructor.Messages.Orchestrator;
using TigerServer.Core.Infrastructor.Messages.Orchestrator.Incoming;
using TigerServer.Core.Infrastructor.Messages.Physical.Outcoming;
using TigerServer.Core.Infrastructor.Messages.Repository.Incoming;
using TigerServer.Core.Infrastructor.Physical;
using TigerServer.Core.Infrastructor.Repository;
using TigerServer.Core.Infrastructor.Scenari;

namespace TigerServer.Core.Infrastructor
{
    class Orchestration : ReceiveActor
    {
        private IActorRef deviceManager;
        private IActorRef dashboardManager;
        private IActorRef repositoryManager;

        public Orchestration(DashBoard dashBoard,string MQTTClientId,IRepository repository,IScenarioEngine engine)
        {
            deviceManager = Context.ActorOf(DeviceManagerCoordinator.Props("patano", MQTTClientId,engine), "physicaldevice");
            dashboardManager = Context.ActorOf(DashdoardCoordinator.Props("dashboard", dashBoard), "dashboard");
            repositoryManager = Context.ActorOf(RepositoryCoordinator.Props(repository), "repo");

            Receive<GatewayCreated>(msg =>
            {
                deviceManager.Tell(msg.GatewayPhysicalCreated);
                dashboardManager.Tell(msg.GatewayDashBoardCreated);
                repositoryManager.Tell(msg.GatewayRepositoryCreated);
            });

            Receive<DeviceCreated>(msg =>
            {
                deviceManager.Tell(msg.DevicePhysicalCreated);
                dashboardManager.Tell(msg.DeviceDashBoardCreated);
                repositoryManager.Tell(msg.DeviceRepositoryCreated);
            });

            Receive<GatewayDroped>(msg =>
            {
                deviceManager.Tell(msg.GatewayPhysicalDroped);
                dashboardManager.Tell(msg.GatewayDashBoardDroped);
                repositoryManager.Tell(msg.GatewayRepositoryDroped);
            });

            Receive<DeviceDroped>(msg =>
            {
                deviceManager.Tell(msg.DevicePhysicalDroped);
                dashboardManager.Tell(msg.DeviceDashBoardDroped);
                repositoryManager.Tell(msg.DeviceRepositoryDroped);
            });

            Receive<GatewaySended>(msg =>
            {
                deviceManager.Tell(msg.GatewayPhysicalSended);
                dashboardManager.Tell(msg.GatewayDashBoardSended);
                repositoryManager.Tell(msg.GatewayRepositorySended);
            });

            Receive<DeviceSended>(msg =>
            {
                deviceManager.Tell(msg.DevicePhysicalSended);
                dashboardManager.Tell(msg.DeviceDashBoardSended);
                repositoryManager.Tell(msg.DeviceRepositorySended);
            });

            Receive<GatewayStarted>(msg =>
            {
                deviceManager.Tell(msg.GatewayPhysicalStarted);
                dashboardManager.Tell(msg.GatewayDashBoardStarted);
                repositoryManager.Tell(msg.GatewayRepositoryStarted);
            });

            Receive<DeviceStarted>(msg =>
            {
                deviceManager.Tell(msg.DevicePhysicalStarted);
                dashboardManager.Tell(msg.DeviceDashBoardStarted);
                repositoryManager.Tell(msg.DeviceRepositoryStarted);
            });

            Receive<GatewayDisconnected>(msg =>
            {
                deviceManager.Tell(msg.GatewayPhysicalDisconnected);
                dashboardManager.Tell(msg.GatewayDashBoardDisconnected);
                repositoryManager.Tell(msg.GatewayRepositoryDisconnected);
            });

            Receive<DeviceDisconnected>(msg =>
            {
                deviceManager.Tell(msg.DeviceDashBoardDisconnected);
                dashboardManager.Tell(msg.DeviceDashBoardDisconnected);
                repositoryManager.Tell(msg.DeviceRepositoryDisconnected);
            });

            Receive<GatewaySet>(msg =>
            {
                deviceManager.Tell(msg.GatewayPhysicalSet);
                dashboardManager.Tell(msg.GatewayDashBoardSet);
                repositoryManager.Tell(msg.GatewayRepositorySet);
            });

            Receive<DeviceSet>(msg =>
            {
                deviceManager.Tell(msg.DevicePhysicalSet);
                dashboardManager.Tell(msg.DeviceDashBoardSet);
                repositoryManager.Tell(msg.DeviceRepositorySet);
            });

            Receive<GatewayUpdate>(msg =>
            {
                dashboardManager.Tell(msg.GatewayDashBoardUpdate);
                repositoryManager.Tell(msg.GatewayRepositoryUpdate);
            });

            Receive<DeviceUpdate>(msg =>
            {
                dashboardManager.Tell(msg.DeviceDashBoardUpdate);
                repositoryManager.Tell(msg.DeviceRepositoryUpdate);
            });

            Receive<GatewayStatus>(msg =>
            {
                deviceManager.Tell(msg.GatewayPhysicalStatus);
            });

            Receive<DeviceStatus>(msg =>
            {
                deviceManager.Tell(msg.DevicePhysicalStatus);
            });

            Receive<GetGatewaysBoard>(msg =>
            {
                var result = dashboardManager.Ask<List<GatewayDashBoard>>(msg).Result;
                Sender.Tell(result);
            });

            Receive<StatusChanged>(msg => { dashBoard.HasChange(); });

            Receive<SystemStart>(msg => InitSystem());

            Receive<Evalutate>(msg => deviceManager.Tell(msg.EvalutatePhysical));

            Receive<PhysicalSetEnd>(msg
                =>  Self.Tell(new Evalutate()));
             
           
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(
                maxNrOfRetries: 10,
                withinTimeMilliseconds: 100,
                localOnlyDecider: ex => Directive.Restart);
        }

        private void InitSystem()
        {
            Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(1000, 1000, Self, new Evalutate(), ActorRefs.NoSender);

            var gatewaystrepo = repositoryManager.Ask<List<GatewayModel>>(new GetAll()).Result;
            foreach (var gateway in gatewaystrepo)
            {
                var gatewayCreated = new GatewayCreated(gateway.Id);
                var gatewaySended = new GatewaySended(gateway.Id, gateway.Value);
                var gatewayStarted = new GatewayStarted(gateway.Id, gateway.Ip);
                var gatewayUpdate = new GatewayUpdate(gateway.Id, gateway.Name);
                var gatewayDisconnectd = new GatewayDisconnected(gateway.Id);
                var gatewayStatus = new GatewayStatus(gateway.Id);

                deviceManager.Tell(gatewayCreated.GatewayPhysicalCreated);
                dashboardManager.Tell(gatewayCreated.GatewayDashBoardCreated);

                deviceManager.Tell(gatewaySended.GatewayPhysicalSended);
                dashboardManager.Tell(gatewaySended.GatewayDashBoardSended);

                dashboardManager.Tell(gatewayStarted.GatewayDashBoardStarted);

                dashboardManager.Tell(gatewayUpdate.GatewayDashBoardUpdate);

                deviceManager.Tell(gatewayDisconnectd.GatewayPhysicalDisconnected);
                dashboardManager.Tell(gatewayDisconnectd.GatewayDashBoardDisconnected);

                deviceManager.Tell(gatewayStatus.GatewayPhysicalStatus);

                foreach (var device in gateway.Devices)
                {
                    var deviceInfo = new DeviceInfo(device.Id, gateway.Id);
                    var deviceCreated = new DeviceCreated(deviceInfo);
                    var deviceSended = new DeviceSended(deviceInfo, device.Value);
                    var deviceStarted = new DeviceStarted(deviceInfo, device.Ip);
                    var deviceUpdate = new DeviceUpdate(deviceInfo, device.Name);
                    var deviceDisconnectd = new DeviceDisconnected(deviceInfo);
                    var deviceStatus = new DeviceStatus(deviceInfo);

                    deviceManager.Tell(deviceCreated.DevicePhysicalCreated);
                    dashboardManager.Tell(deviceCreated.DeviceDashBoardCreated);

                    deviceManager.Tell(deviceSended.DevicePhysicalSended);
                    dashboardManager.Tell(deviceSended.DeviceDashBoardSended);

                    dashboardManager.Tell(deviceStarted.DeviceDashBoardStarted);

                    dashboardManager.Tell(deviceUpdate.DeviceDashBoardUpdate);

                    deviceManager.Tell(deviceDisconnectd.DevicePhysicalDisconnected);
                    dashboardManager.Tell(deviceDisconnectd.DeviceDashBoardDisconnected);

                    deviceManager.Tell(deviceStatus.DevicePhysicalStatus);
                }
            }
        }

        public static Props Props(DashBoard dashBoard, string MQTTClientId,IRepository repository,IScenarioEngine engine )
        {
            return Akka.Actor.Props.Create(() => new Orchestration(dashBoard, MQTTClientId,repository,engine));
        }
    }


}