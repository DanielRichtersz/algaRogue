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

        //Algorithms
        public Dictionary<Room, HashSet<Room>> AdjacencyList { get; } = new Dictionary<Room, HashSet<Room>>();
        public void AddVertex(Room vertex)
        {
            AdjacencyList[vertex] = new HashSet<Room>();
        }

        public void AddEdge(Hallway edge)
        {
            if (AdjacencyList.ContainsKey(edge.RoomOne) && AdjacencyList.ContainsKey(edge.RoomTwo))
            {
                AdjacencyList[edge.RoomOne].Add(edge.RoomTwo);
                AdjacencyList[edge.RoomTwo].Add(edge.RoomOne);
            }
        }

        //End Algorithms part   

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
            //this.Player = new Player(this.StartRoom);
        }

        private void GenerateMap()
        {
            Random random = new Random();
            Map = new Room[Height, Width];
            //Generate rooms and hallways
            for(int i = 0; i < Width; i++)
            {
                for(int j = 0; j < Height; j++)
                {
                    Room newRoom = new Room();
                    if(j < Height - 1)
                    {
                        Hallway northHallway = new Hallway(random.Next(10), newRoom);
                        newRoom.SetHallway(Direction.North, northHallway);
                    }
                    if(i < Width - 1)
                    {
                        Hallway eastHallway = new Hallway(random.Next(10), newRoom);
                        newRoom.SetHallway(Direction.East, eastHallway);
                    }
                    if(j != 0)
                    {
                        // not sure what order to do this
                        Hallway southHallway = Map[j - 1, i].GetHallway(Direction.North);
                        southHallway.SetSecondRoom(newRoom);
                        newRoom.SetHallway(Direction.South, southHallway);
                    }
                    if(i != 0)
                    {
                        // same for this one
                        Hallway westHallway = Map[j, i - 1].GetHallway(Direction.East);
                        westHallway.SetSecondRoom(newRoom);
                        newRoom.SetHallway(Direction.West, westHallway);
                    }
                    Map[j, i] = newRoom;
                }
            }
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Console.WriteLine("The position = " + i + " " + j);
                    if (Map[j, i].GetHallway(Direction.East) != null)
                    {
                        Console.Write(" + " + Map[j, i].GetHallway(Direction.East).Enemy + " ");
                    } else
                    {
                        Console.WriteLine(" +");
                    }
                }
            }
                    //Calculate Minimal Spending Tree 

                    //Remove hallways according to MST


                    //Set this.StartRoom and this.EndRoom
                    //this.Map = map;
                    //this.StartRoom = this.Map[height,0]
                    //this.EndRoom = this.Map[0, width]
                    //throw new NotImplementedException();
        }

        private void LinkRooms(Room roomOne, Room roomTwo)
        {

        }

        private bool RemoveHallway(Room room, Direction direction)
        {
            //Return false when the hallway direction doesn't exist
            return false;
        }

        //public void HandleInput(direction) {
        
        //}
    }
}
