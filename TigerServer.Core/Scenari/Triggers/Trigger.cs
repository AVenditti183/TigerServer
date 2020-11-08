namespace TigerServer.Core.Infrastructor.Scenari.Triggers
{
    public abstract class Trigger
    {
       public int Id { get; set; }

       public abstract bool Triggered(params object[] obj);
    }
}
