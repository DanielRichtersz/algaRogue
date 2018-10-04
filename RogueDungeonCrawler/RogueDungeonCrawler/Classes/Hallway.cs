using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueDungeonCrawler.Classes
{
    public class Hallway
    {
        public int enemy { get; set; }
        public bool isCollapsable { get; set; }
        Room RoomOne;
        Room RoomTwo;

        public Hallway(int enemy, Room roomOne, Room roomTwo)
        {
            this.enemy = enemy;
            this.RoomOne = roomOne;
            this.RoomTwo = roomTwo;
        }
    }
}
