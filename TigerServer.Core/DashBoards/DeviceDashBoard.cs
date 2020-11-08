namespace TigerServer.Core.Infrastructor.DashBoards
{
    public class DeviceDashBoard
    {
        public DeviceDashBoard(string id)
        {
            Id = id;
            IsActive = false;
        }

        public string Id { get; private set; }
        public bool IsActive { get; private set; }
        public string Value { get; private set; }
        public string Name { get; private set; }

        public void Disconnected() => IsActive = false;

        public void Start() => IsActive = true;

        public void Set(string value)
        {
            Value = value;
            IsActive = true;
        }

        public void Update(string Name) => this.Name = Name;
    }


}