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
        public bool IsVisited { get; set; }
        public bool IsStart { get; set; }
        public bool IsEnd { get; set; }

        //array 0=N 1=O 2=Z 3=W
        Hallway[] Hallways = new Hallway[4];

        public Room()
        {
        }

        public Hallway GetHallway(Direction direction)
        {
            return this.Hallways[(int)direction];
        }

        public void SetHallway(Direction direction, Hallway hallway)
        {
            this.Hallways[(int)direction] = hallway;
        }

        public char GetSymbol()
        {
            if (IsVisited)
            {
                return '*';
            }
            else if(IsStart)
            {
                return 'S';
            } 
            else if(IsEnd)
            {
                return 'E';
            }
            else
            {
                return 'X';
            }
        }
    }
}
