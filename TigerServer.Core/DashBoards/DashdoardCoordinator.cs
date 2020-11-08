using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using TigerServer.Core.Infrastructor.Messages;
using TigerServer.Core.Infrastructor.Messages.DashBoard;
using TigerServer.Core.Infrastructor.Messages.DashBoard.Incoming;
using TigerServer.Core.Infrastructor.Messages.Orchestrator;
using TigerServer.Core.Infrastructor.Messages.Orchestrator.Incoming;

namespace TigerServer.Core.Infrastructor.DashBoards
{
    public class DashdoardCoordinator : ReceiveActor
    {
        private readonly string _id;
        private DashBoard DashBoard;

        public DashdoardCoordinator(string id, DashBoard dashBoard)
        {
            _id = id;
            this.DashBoard = dashBoard;

            Receive<GetGatewaysBoard>(msg =>
                Sender.Tell(dashBoard.Gateways));

            Receive<GatewayDashBoardCreated>(msg =>
            {
                if (DashBoard.Gateways.FirstOrDefault(o => o.Id == msg.Source) == null)
                    DashBoard.Gateways.Add(new GatewayDashBoard(msg.Source));

                Context.TellOrc(new StatusChanged());
            });

            Receive<DeviceDashBoardCreated>(msg =>
                GatewayExcecute(msg.Source.Gateway, g => g.Add(new DeviceDashBoard(msg.Source.Source))));

            Receive<GatewayDashBoardDroped>(msg =>
                DashBoard.Gateways = DashBoard.Gateways.Where(o => o.Id != msg.Source).ToList());

            Receive<DeviceDashBoardDroped>(msg =>
                GatewayExcecute(msg.Source.Gateway, g => g.Remove(new DeviceDashBoard(msg.Source.Source))));

            Receive<GatewayDashBoardDisconnected>(msg =>
                   GatewayExcecute(msg.Source, g => g.Disconnected()));

            Receive<GatewayDashBoardStarted>(msg =>
                   GatewayExcecute(msg.Source, g => g.Start(msg.Ip)));

            Receive<GatewayDashBoardSended>(msg =>
                   GatewayExcecute(msg.Source, g => g.Set(msg.Value)));

            Receive<GatewayDashBoardSet>(msg =>
                   GatewayExcecute(msg.Source, g => g.Set(msg.Value)));

            Receive<GatewayDashBoardUpdate>(msg =>
               GatewayExcecute(msg.Source, g => g.Update(msg.Name)));

            Receive<DeviceDashBoardDisconnected>(msg =>
                GatewayExcecute(msg.Source.Gateway, g => g.DeviceDisconnectd(msg.Source.Source)));

            Receive<DeviceDashBoardStarted>(msg =>
                GatewayExcecute(msg.Source.Gateway, g => g.DeviceStart(msg.Source.Source)));

            Receive<DeviceDashBoardSended>(msg =>
                GatewayExcecute(msg.Source.Gateway, g => g.DeviceSet(msg.Source.Source, msg.Value)));

            Receive<DeviceDashBoardSet>(msg =>
                GatewayExcecute(msg.Source.Gateway, g => g.DeviceSet(msg.Source.Source, msg.Value)));

            Receive<DeviceDashBoardUpdate>(msg =>
               GatewayExcecute(msg.Source.Gateway, g => g.DeviceUpdate(msg.Source.Source, msg.Name)));

        }

        private void GatewayExcecute(string id, Action<GatewayDashBoard> action)
        {
            var gateway = DashBoard.Gateways.FirstOrDefault(o => o.Id == id);
            if (gateway != null)
                action(gateway);
            Context.TellOrc(new StatusChanged());

        }
        public static Props Props(string id,DashBoard dashBoard)
        {
            return Akka.Actor.Props.Create(() => new DashdoardCoordinator(id, dashBoard));
        }
    }


}