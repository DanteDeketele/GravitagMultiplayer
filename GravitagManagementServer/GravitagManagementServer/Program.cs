using GravitagManagementServer.EndPoints;
using System.Net;
using System.Text;

namespace GravitagManagementServer
{
    class Program
    {
        static List<EndpointBase> _endPoints = new List<EndpointBase>();
        static public int Port = 7000;
        static public string URL = "http://localhost";
        static string _url = URL + ":" + Port;
        static public List<Player> Players = new List<Player>();
        static public List<GameServer> GameServers = new List<GameServer>();
        static Timer? _playerUpdateTimer;

        static void Main(string[] args)
        {
            // Add endpoints
            _endPoints.Add(new HelloEndpoint());
            _endPoints.Add(new PingEndpoint());
            _endPoints.Add(new GetServersEndpoint());

            // Create an instance of HttpListener
            HttpListener listener = new HttpListener();

            // Add prefixes (endpoints)
            foreach (var endPoint in _endPoints)
            {
                listener.Prefixes.Add(_url + endPoint.Path);
            }

            // Start the listener
            listener.Start();

            // Display startup message
            Console.Title = "Gravitag Management Server";
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(StartupMessage(listener));
            Console.ResetColor();

            // Set up a timer to call the UpdatePlayers method every 6 seconds
            _playerUpdateTimer = new Timer(UpdatePlayers, null, 0, 500);
            
            // Create a new GameServer
            new GameServer();
            new GameServer();

            // Run the listener loop
            while (true)
            {
                // Wait for an incoming request
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                // Find the endpoint
                string responseString = "404 Not Found";
                foreach (var endPoint in _endPoints)
                {
                    if (request.Url?.AbsolutePath == endPoint.Path)
                    {
                        responseString = endPoint.GetResponse();
                        
                        if (responseString != "pong")
                        {
                            Console.WriteLine($"[{DateTime.Now}] {request.HttpMethod} {request.Url?.AbsolutePath} => {responseString}");
                        }else
                        {
                            // Player management
                            string playerName = $"Player {Players.Count + 1}";
                            string playerAdress = request.RemoteEndPoint.Address.ToString();

                            var player = Players.Find(p => p.Adress == playerAdress);
                            if (player == null)
                            {
                                player = new Player(playerName, playerAdress);
                            }
                            else
                            {
                                player.LastInteractionDate = DateTime.Now;
                            }
                        }
                        break;
                    }
                }

                // Prepare the response
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;

                // Send the response
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }
        }

        static string StartupMessage(HttpListener listener)
        {
            string message = "";

            message += $"---[ Gravitag Management Server ]---\n";
            message += $"author: Dante Deketele\n";
            message += $"Listening on {_url}\n";
            message += $"Listening for the following endpoints:\n";
            foreach (var prefix in listener.Prefixes)
            {
                message += $"- {prefix}\n";
            }
            message += $"------------------------------------\n";

            return message;
        }

        // Update method for players
        static void UpdatePlayers(object? state)
        {
            if (Players.Count == 0)
            {
                return;
            }
            for (int i = Players.Count - 1; i >= 0; i--)
            {
                Players[i].Update();
            }
            // Console.ForegroundColor = ConsoleColor.Yellow;
            // Console.WriteLine($"[{DateTime.Now}] Updated players, count: {Players.Count}");
            // Console.ResetColor();
        }
    }
}
