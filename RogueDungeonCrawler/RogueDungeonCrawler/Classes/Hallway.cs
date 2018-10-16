using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueDungeonCrawler.Classes
{
    public class Hallway
    {
        public int Enemy { get; set; }
        public bool IsCollapsable { get; set; }
        Room RoomOne;
        Room RoomTwo;

        public Hallway(int enemy, Room roomOne)
        {
            this.Enemy = enemy;
            this.RoomOne = roomOne;
        }

        public void SetSecondRoom(Room roomTwo)
        {
            this.RoomTwo = roomTwo;
        }

        public Room GetConnectedRoom(Room room)
        {
            if (room == this.RoomOne)
            {
                return this.RoomTwo;
            } else
            {
                return this.RoomOne;
            }
        }
    }
}
