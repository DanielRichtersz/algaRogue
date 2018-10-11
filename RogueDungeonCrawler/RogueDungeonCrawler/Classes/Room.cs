using RogueDungeonCrawler.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueDungeonCrawler.Classes
{
    public class Room
    {
        bool isVisited { get; set; }

        //array 0=N 1=O 2=Z 3=W
        Hallway[] Hallways = new Hallway[4];

        public Room(bool isStart, bool isEnd, int x, int y)
        {
            this.isVisited = false;
        }

        public Hallway GetHallway(Direction direction)
        {
            return this.Hallways[(int)direction];
        }
    }
}
