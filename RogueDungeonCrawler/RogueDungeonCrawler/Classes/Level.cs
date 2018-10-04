using RogueDungeonCrawler.Classes;
using RogueDungeonCrawler.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RogueDungeonCrawler
{
    public class Level
    {
        int Width { get; set; }
        int Height { get; set; }
        public Player Player { get; set; }

        //Start en endroom
        Room StartRoom;
        Room EndRoom;

        //Matrix
        Room[,] Map;
        Hallway[] Hallways;

        public Level(int width, int height)
        {
            Width = width;
            Height = height;
            //Generate map
            this.GenerateMap();
            this.Player = new Player(this.StartRoom);
        }

        private void GenerateMap()
        {
            //Generate rooms and hallways
            for(int i = 0; i < Height; i++)
            {
                for(int j = 0; j < Width; j++)
                {
                    Map[j, i] = new Room();
                }
            }
            //Calculate Minimal Spending Tree 

            //Remove hallways according to MST


            //Set this.StartRoom and this.EndRoom
            //this.Map = map;
            //this.StartRoom = this.Map[height,0]
            //this.EndRoom = this.Map[0, width]
            throw new NotImplementedException();
        }

        private void LinkRooms(Room roomOne, Room roomTwo)
        {

        }

        private bool RemoveHallway(Room room, Direction direction)
        {

            //Return false when the hallway direction doesn't exist
            return false;
        }

        public void HandleInput(direction) {
        
        }
    }
}
