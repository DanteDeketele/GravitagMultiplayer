namespace GravitagManagementServer.EndPoints
{
    internal class HelloEndpoint : EndpointBase
    {
        public HelloEndpoint() : base("/hello/") { }

        public override string GetResponse()
        {
            return "Hello World!";
        }
    }
}
