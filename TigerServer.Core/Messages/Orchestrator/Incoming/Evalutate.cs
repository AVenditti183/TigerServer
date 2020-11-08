using TigerServer.Core.Infrastructor.Messages.Physical.Incoming;

namespace TigerServer.Core.Infrastructor.Messages.Orchestrator
{
    public record Evalutate()
    {
        public EvalutatePhysical EvalutatePhysical => new EvalutatePhysical();
    }
}