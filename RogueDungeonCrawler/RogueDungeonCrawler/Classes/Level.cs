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
            this.DrawMap();
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
                    if(j < Height- 1)
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
                        Hallway southHallway = Map[j - 1, i].GetHallway(Direction.North);
                        southHallway.SetSecondRoom(newRoom);
                        newRoom.SetHallway(Direction.South, southHallway);
                    }
                    if(i != 0)
                    {
                        Hallway westHallway = Map[j, i - 1].GetHallway(Direction.East);
                        westHallway.SetSecondRoom(newRoom);
                        newRoom.SetHallway(Direction.West, westHallway);
                    }
                    Map[j, i] = newRoom;
                }
            }
            SetStartRoom(1, Height);
            SetEndRoom(Width, 1);
            //Calculate Minimal Spending Tree 

            //Remove hallways according to MST
        }

        private void LinkRooms(Room roomOne, Room roomTwo)
        {

        }

        private bool RemoveHallway(Room room, Direction direction)
        {

            //Return false when the hallway direction doesn't exist
            return false;
        }

        public void SetStartRoom(int x, int y)
        {
            if (x < 1 || x > Width || y < 1 || y > Height)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The inserted position is not available in the current map");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
                HandleSetStart();
            }
            else
            {
                Room newStartRoom = Map[y - 1, x - 1];
                if (!newStartRoom.IsEnd)
                {
                    if (StartRoom != null)
                    {
                        StartRoom.IsStart = false;
                    }
                    newStartRoom.IsStart = true;
                    StartRoom = newStartRoom;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Startroom can not be the endroom");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();
                    HandleSetStart();
                }
            }
        }

        public void SetEndRoom(int x, int y)
        {
            if(x < 1 || x > Width || y < 1 || y > Height)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The inserted position is not available in the current map");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
                HandleSetEnd();
            }
            else
            {
                Room newEndRoom = Map[y - 1, x - 1];
                if (!newEndRoom.IsStart)
                {
                    if (EndRoom != null)
                    {
                        EndRoom.IsEnd = false;
                    }
                    newEndRoom.IsEnd = true;
                    EndRoom = newEndRoom;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Endroom can not be the startroom");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();
                    HandleSetEnd();
                }
            }
        }

        private void DrawMap()
        {
            System.Console.Clear();
            DrawLegenda();
            for (int j = Height - 1; j >= 0; j--)
            {
                Console.Write("  ");
                for (int l = 0; l < Width - 2; l++)
                {
                    Console.Write("|       ");
                }
                Console.WriteLine("|       |");
                for (int i = 0; i < Width; i++)
                {
                    Room currentRoom = Map[j, i];
                    if (i < Width - 1)
                    {
                        if (i == 0)
                        {
                            Console.Write("- ");
                        }
                        if (currentRoom.IsEnd)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        else if (currentRoom.IsStart)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        Console.Write(currentRoom.GetSymbol());
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(" - " + currentRoom.GetHallway(Direction.East).Enemy + " - ");
                    }
                    else
                    {
                        if (currentRoom.IsEnd)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        else if (currentRoom.IsStart)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        Console.Write(currentRoom.GetSymbol());
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(" -");
                        Console.Write("  ");
                        for (int p = 0; p < Width - 2; p++)
                        {
                            Console.Write("|       ");
                        }
                        Console.WriteLine("|       |");
                        if (j != 0)
                        {
                            Console.Write("  ");
                            for (int k = 0; k < Width; k++)
                            {
                                Console.Write(Map[j, k].GetHallway(Direction.South).Enemy + "       ");
                            }
                        }
                        Console.WriteLine();
                    }
                }
            }
        }
        
        public void DrawLegenda()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("S");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" = Room: Startpunt");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("E");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" = Room: Eindpunt");
            Console.WriteLine("X = Room: Niet bezocht");
            Console.WriteLine("* = Room: Bezocht");
            Console.WriteLine("~ = Hallway: Ingestort");
            Console.WriteLine("0 = Hallway: Level tegenstander (cost)");
            Console.WriteLine();
            Console.WriteLine();
        }

        public void HandleSetStart()
        {
            Console.WriteLine("give x and y coördinates of the startroom like so 1,5");
            CheckStartInput(Console.ReadLine(), true);
            DrawMap();
        }

        public void HandleSetEnd()
        {
            Console.WriteLine("give x and y coördinates of the endroom like so 1,5");
            CheckStartInput(Console.ReadLine(), false);
            DrawMap();
        }

        private void CheckStartInput(string RoomCoördinates, bool isStart)
        {
            if (RoomCoördinates.ToLower().Contains(',') && !RoomCoördinates.ToLower().Contains(' '))
            {
                string[] coördinates = RoomCoördinates.Split(',');
                //check if coördinates are integers
                bool isNumericX = int.TryParse(coördinates[0], out int xCoördinate);
                bool isNumericY = int.TryParse(coördinates[1], out int yCoördinate);
                if (isNumericX && isNumericY)
                {
                    if (isStart)
                    {
                        SetStartRoom(xCoördinate, yCoördinate);
                    }
                    else
                    {
                        SetEndRoom(xCoördinate, yCoördinate);
                    }
                }
                else if (!isNumericX)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(coördinates[0]);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" is not a valid number");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();
                    if(isStart)
                    {
                        HandleSetStart();
                    }
                    else
                    {
                        HandleSetEnd();
                    }
                }
                else if (!isNumericY)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(coördinates[1]);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" is not a valid number");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();
                    if (isStart)
                    {
                        HandleSetStart();
                    }
                    else
                    {
                        HandleSetEnd();
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("this is not a correct input value");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
                if (isStart)
                {
                    HandleSetStart();
                }
                else
                {
                    HandleSetEnd();
                }
            }
        }

        //public void HandleInput(direction) {
        
        //}
    }
}
