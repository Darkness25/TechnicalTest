namespace ClientPerson
{
    public interface IServiceSettings
    {
        public HostType TechnicalTest { get; set; }
    }

    public class ServiceSettings : IServiceSettings
    {
        public HostType TechnicalTest { get; set; }
    }

    public class HostType
    {
        public ConnectionStrings ConnectionStrings { get; set; }

    }

    public class ConnectionStrings
    {
        public string ConnectionString { get; set; }

    }
}