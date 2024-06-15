namespace GravitagManagementServer.EndPoints
{
    internal abstract class EndpointBase
    {
        public string Path { get; set; }

        public EndpointBase(string path)
        {
            Path = path;
        }

        public abstract string GetResponse();
    }
}
