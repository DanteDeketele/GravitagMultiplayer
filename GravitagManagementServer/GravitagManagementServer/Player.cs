using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitagManagementServer
{
    internal class Player
    {
        public string Name { get; set; }
        public string Adress { get; set; }
        public DateTime LastInteractionDate { get; set; }

        public Player(string name, string adress)
        {
            Name = name;
            Adress = adress;
            LastInteractionDate = DateTime.Now;

            Program.Players.Add(this);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[{DateTime.Now}] {Name} joined");
            Console.ResetColor();
        }

        public void Update()
        {
            if (DateTime.Now - LastInteractionDate > TimeSpan.FromSeconds(3))
            {
                Program.Players.Remove(this);

                // LeaveMessage
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now}] {Name} left");
                Console.ResetColor();
            }
        }
    }
}
