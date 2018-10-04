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

        public Level(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            //Generate map
            this.GenerateMap();
            this.Player = new Player(this.StartRoom);


        }

        private void GenerateMap()
        {
            //Generate rooms and hallways

            //Calculate Minimal Spending Tree 

            //Remove hallways according to MST


            //Set this.StartRoom and this.EndRoom
            //this.Map = map;
            //this.StartRoom = this.Map[height,0]
            //this.EndRoom = this.Map[0, width]
            throw new NotImplementedException();
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
