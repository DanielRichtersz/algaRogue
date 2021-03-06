﻿using RogueDungeonCrawler.Enum;
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

            Level level = new Level();
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
                    case ConsoleKey.T:
                        level.HandleTalisman();
                        break;
                    case ConsoleKey.G:
                        level.HandleGrenade();
                        break;
                    case ConsoleKey.C:
                        level.HandleCompass();
                        break;
                    case ConsoleKey.UpArrow:
                        level.Move(Direction.North);
                        break;
                    case ConsoleKey.RightArrow:
                        level.Move(Direction.East);
                        break;
                    case ConsoleKey.DownArrow:
                        level.Move(Direction.South);
                        break;
                    case ConsoleKey.LeftArrow:
                        level.Move(Direction.West);
                        break;
                }
            }
        }
    }
}
