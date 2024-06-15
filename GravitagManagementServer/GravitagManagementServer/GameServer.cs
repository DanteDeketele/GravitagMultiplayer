﻿using System;
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
        public List<Player> Players { get; set; }

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
                Arguments = $"-batchmode -nographics {Adress} {Port}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            _process = Process.Start(startInfo);
            _process.OutputDataReceived += (sender, args) => Console.WriteLine($"[{DateTime.Now}] {_process.Id} - Server: {Name}, Port: {Port} - {args.Data}");
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
    }
}