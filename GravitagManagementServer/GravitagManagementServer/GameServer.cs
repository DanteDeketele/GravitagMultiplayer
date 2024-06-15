using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitagManagementServer
{
    internal class GameServer
    {
        public string Name { get; set; }
        public string Adress { get; set; }
        public int Port { get; set; }
        internal List<Player> Players { get; set; }
        public int PlayerCount => Players.Count;

        private Process _process;

        public GameServer()
        {
            Players = new List<Player>();

            Name = "Server";
            Adress = Program.URL;
            Port = Program.Port + Program.GameServers.Count + 1;

            Program.GameServers.Add(this);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[{DateTime.Now}] {Name} created on port {Port}");
            Console.ResetColor();

            string path = "GameServer/Build.exe";
            // lets make sure it works in the current build directory and in visual studio
            string actualPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), path);

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = actualPath,
                Arguments = $"-batchmode -nographics {Port}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            _process = Process.Start(startInfo);
            _process.OutputDataReceived += (sender, args) =>
            {
                // if the data is empty the server may have shut down
                if (args.Data == null ||args.Data.Count() == 0 || string.IsNullOrWhiteSpace(args.Data) || _process.HasExited)
                {
                    OnShutdown();
                }
                else if (args.Data != null && args.Data.Contains("Server started"))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[{DateTime.Now}] {_process.Id} - Server: {Name}, Port: {Port} - Server started");
                    Console.ResetColor();
                }
                else
                {
                    // filter out stupid messages
                    string[] stupid = { "Shader" };

                    if (stupid.Any(args.Data.Contains))
                    {
                        return;
                    }

                    // color type
                    string type;
                    if (args.Data != null && args.Data.Contains("Error"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        type = "Error";
                    }
                    else if (args.Data != null && args.Data.Contains("Warning"))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        type = "Warning";
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        type = "Log";
                    }

                    

                    Console.WriteLine($"[{DateTime.Now}] {_process.Id} - Server: {Name}, Port: {Port} - {type}: {args.Data}");
                    Console.ResetColor();
                }
            };
            _process.BeginOutputReadLine();
            _process.ErrorDataReceived += (sender, args) => Console.WriteLine($"[{DateTime.Now}] {_process.Id} - Server: {Name}, Port: {Port} - {args.Data}");
            _process.BeginErrorReadLine();
        }

        public void Stop()
        {
            if (!_process.HasExited)
            {
                _process.Kill();
            }
        }

        public void OnShutdown()
        {
            Stop();
            Console.ForegroundColor = ConsoleColor.Yellow;
            string message = $"[{DateTime.Now}] {_process.Id} - Server: {Name}, Port: {Port} - Server shut down";
            Console.WriteLine(message);
            Console.ResetColor();

            Program.GameServers.Remove(this);
        }
    }
}
