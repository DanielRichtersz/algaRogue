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
        bool IsVisited { get; set; }

        //array 0=N 1=O 2=Z 3=W
        Hallway[] Hallways = new Hallway[4];

        public Room()
        {
            this.IsVisited = false;
        }

        public Hallway GetHallway(Direction direction)
        {
            return this.Hallways[(int)direction];
        }

        public void SetHallway(Direction direction, Hallway hallway)
        {
            this.Hallways[(int)direction] = hallway;
        }
    }
}
