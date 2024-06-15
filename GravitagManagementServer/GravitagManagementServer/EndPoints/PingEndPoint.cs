namespace GravitagManagementServer.EndPoints
{
    internal class PingEndpoint : EndpointBase
    {
        public PingEndpoint() : base("/ping/") { }

        public override string GetResponse()
        {
            return "pong";
        }
    }
}
