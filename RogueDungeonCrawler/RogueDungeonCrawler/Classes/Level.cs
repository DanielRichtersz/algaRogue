using RogueDungeonCrawler.Classes;
using RogueDungeonCrawler.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;


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

        //Algorithms
        Algorithms Algorithms;

        public Level()
        {
            Start();
            //Generate map
            this.GenerateMap();
            this.DrawMap();
            //this.Player = new Player(this.StartRoom);

            this.Algorithms = new Algorithms();
        }

        public Room[,] GetMap()
        {
            return this.Map;
        }

        public Room GetRoom(Room room)
        {
            for (int i = 0; i < this.Width; i++)
            {
                for (int j = 0; j < this.Height; j++)
                {
                    if (Map[j, i] == room)
                    {
                        return Map[j, i];
                    }
                }
            }
            return null;
        }

        internal void Move(Direction moveDirection)
        {
            Hallway moveDirectionHallway = this.StartRoom.GetHallway(moveDirection) ?? null;
            if (moveDirectionHallway != null && moveDirectionHallway.IsCollapsed == false)
            {
                this.StartRoom.IsStart = false;
                this.StartRoom = this.StartRoom.GetHallway(moveDirection).GetConnectedRoom(this.StartRoom);
                this.StartRoom.IsStart = true;
                DrawMap();
            }
            else
            {
                Console.WriteLine("No room is accesible in this direction...");
            }
        }

        private void GenerateMap()
        {
            Random random = new Random();
            Map = new Room[Height, Width];

            //Generate rooms and hallways
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Room newRoom = new Room();
                    if (j < Height - 1)
                    {
                        Hallway northHallway = new Hallway(random.Next(10), newRoom);
                        newRoom.SetHallway(Direction.North, northHallway);
                    }
                    if (i < Width - 1)
                    {
                        Hallway eastHallway = new Hallway(random.Next(10), newRoom);
                        newRoom.SetHallway(Direction.East, eastHallway);
                    }
                    if (j != 0)
                    {
                        Hallway southHallway = Map[j - 1, i].GetHallway(Direction.North);
                        southHallway.SetSecondRoom(newRoom);
                        newRoom.SetHallway(Direction.South, southHallway);
                    }
                    if (i != 0)
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

        private bool RemoveHallway(Room room, Direction direction)
        {
            Hallway removableHallway = room.GetHallway(direction);
            if (removableHallway != null)
            {
                Room connectedRoom = removableHallway.GetConnectedRoom(room);
                room.SetHallway(direction, null);
                switch (direction)
                {
                    case Direction.North:
                        connectedRoom.SetHallway(Direction.South, null);
                        break;
                    case Direction.East:
                        connectedRoom.SetHallway(Direction.West, null);
                        break;
                    case Direction.South:
                        connectedRoom.SetHallway(Direction.North, null);
                        break;
                    case Direction.West:
                        connectedRoom.SetHallway(Direction.East, null);
                        break;
                }
                return true;
            }
            else
            {
                return false;
            }
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
            if (x < 1 || x > Width || y < 1 || y > Height)
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

        public void DrawMap()
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
                        else if (currentRoom.IsVisited == true)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                        }
                        Console.Write(currentRoom.GetSymbol());
                        Console.ForegroundColor = ConsoleColor.White;
                        Hallway hallway = currentRoom.GetHallway(Direction.East);
                        if (hallway.IsCollapsable == false)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        }
                        if (hallway.IsCollapsed)
                        {
                            Console.Write(" - ~ - ");
                        }
                        else
                        {
                            Console.Write(" - " + hallway.Enemy + " - ");
                        }
                        Console.ForegroundColor = ConsoleColor.White;
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
                        else if (currentRoom.IsVisited)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
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
                                Hallway hallway = Map[j, k].GetHallway(Direction.South);
                                if (hallway.IsCollapsable == false)
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                }
                                if (hallway.IsCollapsed)
                                {
                                    Console.Write("~" + "       ");
                                }
                                else
                                {
                                    Console.Write(hallway.Enemy + "       ");
                                }
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                        Console.WriteLine();
                    }
                }
            }
            Console.WriteLine("Available actions: Talisman (T), Handgrenade (G), Compass (C)");
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
            this.CleanPathAndCollapsable();
            DrawMap();
        }

        public void HandleSetEnd()
        {
            Console.WriteLine("give x and y coördinates of the endroom like so 1,5");
            CheckStartInput(Console.ReadLine(), false);
            this.CleanPathAndCollapsable();
            DrawMap();
        }


        private void Start()
        {
            System.Console.Clear();
            Console.WriteLine("Enter the width and height of the dungeon like 5,5 (the values can not be larger than 10):");
            CheckInput(Console.ReadLine());
        }

        public void HandleTalisman()
        {
            this.CleanPathAndCollapsable();
            DrawMap();
            Room startVertex = this.StartRoom;
            var shortestPath = this.Algorithms.ShortestPathFunction<Room>(this, this.StartRoom);

            List<Room> pathToEndRoom = (List<Room>)shortestPath(this.EndRoom);
            if (pathToEndRoom != null)
            {
                foreach (Room room in pathToEndRoom)
                {
                    room.IsVisited = true;
                    DrawMap();
                    Console.WriteLine("You activate the talisman...");
                    Thread.Sleep(400);
                }
                Console.WriteLine("The talisman whispers to you: 'The endroom is " + (pathToEndRoom.Count - 1) + " rooms away'");
            }
            else
            {
                Console.WriteLine("The talisman whispers to you: 'You are to die in this place, for there is no possible path to the endroom!");
            }
        }

        public void HandleGrenade()
        {
            CleanPathAndCollapsable();
            DrawMap();
            this.Algorithms.PrimsSafetyProtocol(this, this.StartRoom);
            for (int i = 0; i < 4; i++)
            {
                Hallway hallway = this.StartRoom.GetHallway((Direction)i);
                
                //Check if hallways is collapsable
                if (hallway != null && hallway.IsCollapsable == true)
                {    
                    //If yes: Collapse hallway:
                    hallway.IsCollapsed = true;
                }
            }
            Console.WriteLine("You throw the grenade in a random direction...");
            DrawMap();
            Console.WriteLine("You hear it explode, and wonder what the explosion has hit");
        }

        public void HandleCompass()
        {
            DrawMap();
            Console.WriteLine("You activate the magical compass...");
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
                    if (isStart)
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

        private void CheckInput(string size)
        {
            if (size.ToLower().Contains(',') && !size.ToLower().Contains(' '))
            {
                string[] coördinates = size.Split(',');
                //check if coördinates are integers
                bool isNumericWidth = int.TryParse(coördinates[0], out int width);
                bool isNumericHeigth = int.TryParse(coördinates[1], out int height);
                if (isNumericWidth && isNumericHeigth && (width <= 10) && (height <= 10))
                {
                    Width = width;
                    Height = height;
                }
                else if (!isNumericWidth)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(coördinates[0]);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" is not a valid number");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();
                    Start();
                }
                else if (!isNumericHeigth)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(coördinates[1]);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" is not a valid number");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();
                    Start();
                }
                else if (width > 10)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(coördinates[0]);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" can not be greater than 10");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();
                    Start();
                }
                else if (height > 10)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(coördinates[1]);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" can not be greater than 10");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();
                    Start();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("this is not a correct input value");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
                Start();
            }
        }

        //Sets the isVisited property of all rooms to false
        public void CleanPathAndCollapsable()
        {
            foreach (Room room in this.Map)
            {
                foreach (Hallway hallway in room.GetHallways())
                {
                    if (hallway != null)
                    {
                        hallway.IsCollapsable = true;
                    }
                }
                room.IsVisited = false;
            }
        }
    }
}
