using RogueDungeonCrawler.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RogueDungeonCrawler
{
    class Level
    {
        private Dictionary<Room, Hallway> Map { get; set; }
        int Width { get; set; }
        int Height { get; set; }
        public Player Player { get; set; }


        public Level(int width, int height, int playerX, int playerY)
        {
            this.Width = width;
            this.Height = height;
            this.Player = new Player(playerX, playerY);

            //Generate map

        }

        public handleInput(enum direction)
        {
            Room room = player.move(direction);

        }


    }
}
