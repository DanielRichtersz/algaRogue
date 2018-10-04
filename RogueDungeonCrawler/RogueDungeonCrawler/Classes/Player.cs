using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueDungeonCrawler.Classes
{
    public class Player
    {
        Room CurrentRoom;
        public Player(Room startingRoom)
        {
            this.CurrentRoom = startingRoom;
        }
    }
}
