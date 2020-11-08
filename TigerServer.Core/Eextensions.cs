using Akka.Actor;
using TigerServer.Core.Infrastructor.Messages;
using TigerServer.Core.Infrastructor.Messages.Orchestrator;
using TigerServer.Core.Infrastructor.Messages.Physical;
using TigerServer.Core.Infrastructor.Messages.Scenari;

namespace TigerServer.Core.Infrastructor
{
    public static class extensions
    {
        public static void TellOrc(this IUntypedActorContext context, object msg)
        {
            context.ActorSelection("/user/orch").Tell(msg);
        }

        public static void Evalutate(this IUntypedActorContext context)
        {
            context.ActorSelection("/user/orch").Tell(new Evalutate());
        }
    }
}