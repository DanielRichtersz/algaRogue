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
            return this.Hallways[(int)direction] ?? null;
        }

        public void SetHallway(Direction direction, Hallway hallway)
        {
            this.Hallways[(int)direction] = hallway;
        }

        public Hallway GetLowestLevelHallway(List<Room> visited)
        {
            Hallway lowest = new Hallway(999, new Room());
            for (int i = 0; i < 4; i++)
            {
                if ((this.Hallways[i] != null 
                    && this.Hallways[i].IsCollapsed == false 
                    && visited.Contains(this.Hallways[i].GetConnectedRoom(this)) == false) 
                    && this.Hallways[i].Enemy < lowest.Enemy)
                {
                    lowest = this.Hallways[i];
                }
            }
            return lowest;
        }

        public Hallway[] GetHallways()
        {
            return this.Hallways;
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
