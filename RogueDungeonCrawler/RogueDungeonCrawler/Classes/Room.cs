using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueDungeonCrawler.Classes
{
    class Room
    {
        int x;
        int y;
        bool isStart { get; set; }
        bool isEnd { get; set; }
        bool isVisited { get; set; }

        public Room(bool isStart, bool isEnd, int x, int y)
        {
            this.x = x;
            this.y = y;
            this.isStart = isStart;
            this.isEnd = isEnd;
            this.isVisited = false;
        }
    }
}
