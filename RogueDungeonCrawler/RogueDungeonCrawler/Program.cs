using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueDungeonCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            Level level = new Level(5, 5);
            //Gameloop
            while (true)
            {
                var input = Console.ReadKey(true);
                switch (input.Key)
                {
                    case ConsoleKey.S:
                        level.HandleSetStart();
                        break;
                    case ConsoleKey.E:
                        level.HandleSetEnd();
                        break;
                }
            }
        }

    }
}
