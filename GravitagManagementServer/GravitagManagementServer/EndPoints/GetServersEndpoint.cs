using Newtonsoft.Json;

namespace GravitagManagementServer.EndPoints
{
    internal class GetServersEndpoint : EndpointBase
    {
        public GetServersEndpoint() : base("/servers/") { }

        public override string GetResponse()
        {
            GameServer[] servers = Program.GameServers.ToArray();
            string response = JsonConvert.SerializeObject(servers);
            return response;
        }
    }
}
