using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueDungeonCrawler.Classes
{
    public class Player
    {
        Room currentRoom;
        public Player(Room startingRoom)
        {
            this.currentRoom = startingRoom;
        }
    }
}
