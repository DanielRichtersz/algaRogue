using RogueDungeonCrawler.Enum;
using System;
using System.Collections.Generic;
using System.Threading;

namespace RogueDungeonCrawler.Classes
{
    public class Algorithms
    {
        public List<Room> GetNeighbors(Level level, Room vertex)
        {
            Room NorthNeighbor;
            Room EastNeighbor;
            Room SouthNeighbor;
            Room WestNeighbor;
            //Iterate through array of hallway and get neighbors
            //check if the neighbors are visited
            Hallway NorthNeighborHallway = level.GetRoom(vertex).GetHallway(Enum.Direction.North);
            Hallway EastNeighborHallway = level.GetRoom(vertex).GetHallway(Enum.Direction.East);
            Hallway SouthNeighborHallway = level.GetRoom(vertex).GetHallway(Enum.Direction.South);
            Hallway WestNeighborHallway = level.GetRoom(vertex).GetHallway(Enum.Direction.West);

            NorthNeighbor = NorthNeighborHallway == null || NorthNeighborHallway.IsCollapsed ? null : NorthNeighborHallway.GetConnectedRoom(vertex);
            EastNeighbor = EastNeighborHallway == null || EastNeighborHallway.IsCollapsed ? null : EastNeighborHallway.GetConnectedRoom(vertex);
            SouthNeighbor = SouthNeighborHallway == null || SouthNeighborHallway.IsCollapsed ? null : SouthNeighborHallway.GetConnectedRoom(vertex);
            WestNeighbor = WestNeighborHallway == null || WestNeighborHallway.IsCollapsed ? null : WestNeighborHallway.GetConnectedRoom(vertex);

            List<Room> neighbors = new List<Room>();
            neighbors.Add(NorthNeighbor);
            neighbors.Add(EastNeighbor);
            neighbors.Add(SouthNeighbor);
            neighbors.Add(WestNeighbor);
            return neighbors;
        }

        public Func<Room, IEnumerable<Room>> ShortestPathFunction<T>(Level level, Room startRoom)
        {
            //Contains previous node from destination node to starting node
            var previous = new Dictionary<Room, Room>();

            //Create Queue and add startRoom
            Queue<Room> queue = new Queue<Room>();
            queue.Enqueue(startRoom);

            //While queue is not empty
            while (queue.Count > 0)
            {
                //Take first Room from queue and remove from queue
                Room vertex = queue.Dequeue();

                //To show what rooms are visited with the algorithm
                //vertex.IsVisited = true;
                //level.DrawMap();
                //Thread.Sleep(100);

                //Check for all neighbors
                foreach (Room neighbor in GetNeighbors(level, vertex))
                {
                    //if neighbor is not null
                    if (neighbor != null)
                    {
                        if (previous.ContainsKey(neighbor))
                        {
                            continue;
                        }
                        //If the neighbor hasn't already been added to a path
                        //Put the current Room as previous on the place of the neighbor (For backwards pathing)
                        previous[neighbor] = vertex;
                        queue.Enqueue(neighbor);
                    }
                }
            }

            //The return function
            Func<Room, IEnumerable<Room>> shortestPath = v =>
            {
                List<Room> path = new List<Room> { };

                Room current = v;

                //While the current room doesn't equal the start room
                while (!current.Equals(startRoom))
                {
                    //Add the current room to path                     
                    //and select next (or actually previous) room
                    path.Add(current);

                    //If the previous room is null (which is not supposed to happen)
                    try
                    {
                        current = previous[current];
                    }
                    catch (KeyNotFoundException)
                    {
                        //And the only room in the path list isn't the same as the start room (which could happen if the start and endroom are the same)
                        if (startRoom != v)
                        {
                            //return null indicating there is no path to the target
                            return null;
                        }
                    }
                };

                //Add the last room (starting room)
                path.Add(startRoom);
                //Reverse the path so its in correct order from startroom to endroom
                path.Reverse();

                return path;
            };

            return shortestPath;
        }

        public void PrimsSafetyProtocol(Level level, Room startRoom)
        {
            List<Room> visited = new List<Room>();
            List<Room> unvisited = new List<Room>();
            List<Hallway> notCollapsable = new List<Hallway>();

            //Add all nodes to the unvisited
            foreach (Room room in level.GetMap())
            {
                unvisited.Add(room);
            }

            //Haal de startkamer uit unvisited en doe deze bij visited (eerste kamer om vanuit te werken)
            unvisited.Remove(startRoom);
            visited.Add(startRoom);

            //While unvisited is not empty           
            while (unvisited.Count > 0)
            {
                Room lowestRoom = new Room();
                Hallway lowestHallway = new Hallway(999, null);

                //Check foreach room which hallway has the lowest cost
                foreach (Room n in visited)
                {
                    //To show what rooms are visited with the algorithm
                    //n.IsVisited = true;
                    //level.DrawMap();
                    //Thread.Sleep(20);

                    Hallway h = n.GetLowestLevelHallway(visited);
                    if (h != null && h.Enemy < lowestHallway.Enemy)
                    {
                        //Set lowestHallway and lowestRoom to the lowestcost hallway and the connected room
                        lowestHallway = h;
                        lowestRoom = h.GetConnectedRoom(n);
                    }
                }
                if (lowestRoom == null)
                {
                    break;
                }
                notCollapsable.Add(lowestHallway);
                visited.Add(lowestRoom);
                unvisited.Remove(lowestRoom);
            }

            //To remove the discovery animation
            level.CleanPathAndCollapsable();

            //This can be done in the above while loop, not using a list of notCollapsable but immediately setting the notCollapsable property
            //Also better this way to make the discovery animation
            foreach (Hallway h in notCollapsable)
            {
                h.IsCollapsable = false;
            }

            //Explode random 
            Random random = new Random();
            while (true)
            {
                Hallway hallway = startRoom.GetHallway((Direction)random.Next(1, 4));
                if (hallway != null)
                {
                    hallway.Enemy = 0;
                    break;
                }
            }
        }

        public List<Room> DijkstraShortestPath(Level level, Room startRoom, Room endRoom)
        {
            Dictionary<Room, KeyValuePair<Room, int>> cost = new Dictionary<Room, KeyValuePair<Room, int>>();
            List<Room> visited = new List<Room>();
            List<Room> unvisited = new List<Room>();
            List<Room> reversePath = new List<Room>();

            Room currentRoom = startRoom;

            //Add all rooms to unvisited list
            foreach (Room room in level.GetMap())
            {
                unvisited.Add(room);
            }

            //Remove startroom and add to visited (like in MST)
            unvisited.Remove(startRoom);
            visited.Add(startRoom);

            //Add startroom to cost, with a cost of 0
            cost.Add(startRoom, new KeyValuePair<Room, int>(null, 0));

            //While there are still unvisited rooms
            while (unvisited.Count > 0)
            {
                //To show what rooms are visited with the algorithm
                currentRoom.IsVisited = true;
                level.DrawMap();
                Thread.Sleep(25);

                //For each hallway for current room
                foreach (Hallway hallway in currentRoom.GetHallways())
                {
                    //To show what rooms are visited with the algorithm
                    //currentRoom.IsVisited = true;
                    //level.DrawMap();
                    //Thread.Sleep(50);

                    //Check null or collapsed
                    if (hallway != null && hallway.IsCollapsed == false)
                    {
                        //Set cost to infinite
                        int currentCost = int.MaxValue;

                        //Get the connected room
                        Room nextRoom = hallway.GetConnectedRoom(currentRoom);

                        //Calculate cost
                        currentCost = cost[currentRoom].Value + hallway.Enemy;

                        //Compare costs
                        if (cost.ContainsKey(nextRoom))
                        {
                            if (cost[nextRoom].Value > currentCost)
                            {
                                cost[nextRoom] = new KeyValuePair<Room, int>(currentRoom, currentCost);
                            }
                        }
                        else
                        {
                            cost.Add(nextRoom, new KeyValuePair<Room, int>(currentRoom, currentCost));
                        }
                    }
                }

                //Set lowestcost again to maxvalue to compare it with other values
                int lowestCost = int.MaxValue;
                foreach (var x in cost)
                {
                    //If the value is lower than lowestCost and the room isn't in visited anymore, set currentRoom to x.key and lowestcost to the cost of x.key
                    if (x.Value.Value < lowestCost && visited.Contains(x.Key) == false)
                    {
                        currentRoom = x.Key;
                        lowestCost = x.Value.Value;
                    }
                }
                //Remove currentRoom and add to visited
                unvisited.Remove(currentRoom);
                if (currentRoom == endRoom)
                {
                    break;
                }
                visited.Add(currentRoom);
            }

            //Clean discovery animations
            level.CleanPathAndCollapsable();

            //Get the path and reverse
            Room reverse = endRoom;
            while (reverse != null)
            {
                //Draw route animation
                reverse.IsVisited = true;
                level.DrawMap();
                Thread.Sleep(400);

                reversePath.Add(reverse);
                var reverseCost = cost[reverse];
                reverse = reverseCost.Key;
            }

            reversePath.Reverse();
            return reversePath;
        }
    }
}