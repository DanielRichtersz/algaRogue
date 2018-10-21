using System;
using System.Collections.Generic;


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
            WestNeighbor = WestNeighborHallway == null || WestNeighborHallway.IsCollapsed? null : WestNeighborHallway.GetConnectedRoom(vertex);

            List<Room> neighbors = new List<Room>();
            neighbors.Add(NorthNeighbor);
            neighbors.Add(EastNeighbor);
            neighbors.Add(SouthNeighbor);
            neighbors.Add(WestNeighbor);
            return neighbors;
        }

        public HashSet<Room> BreadthFirstSearch(Level level, Room startVertex)
        {
            var visited = new HashSet<Room>();

            //If the starting room doesn't exist, return empty
            if (level.GetRoom(startVertex) != null)
            {
                return visited;
            }

            //Create queue and add starting room to the end of the queue
            var queue = new Queue<Room>();
            queue.Enqueue(startVertex);

            //While the queue is not empty
            while (queue.Count > 0)
            {
                //Remove the room at beginning of queue
                Room vertex = queue.Dequeue();

                //If the list of visited rooms contains the last gotten room, continue to next room
                if (visited.Contains(vertex))
                {
                    continue;
                }

                //PreVisit possibility

                visited.Add(vertex);

                //Foreach neighbor, check if it has been visited, if not add to queue
                foreach (Room room in GetNeighbors(level, vertex))
                {
                    if (room != null)
                    {
                        if (visited.Contains(room))
                        {
                            queue.Enqueue(room);
                        }
                    }
                }
            }
            return visited;
        }

        public List<Room> DijkstraShortestPath(Level level, Room startRoom, Room endRoom)
        {
            var previous = new Dictionary<Room, Room>();
            var distances = new Dictionary<Room, int>();
            var nodes = new List<Room>();

            List<Room> path = null;

            foreach (var vertex in level.GetMap())
            {
                if (vertex.IsStart)
                {
                    distances[vertex] = 0;
                }
                else
                {
                    distances[vertex] = int.MaxValue;
                }

                nodes.Add(vertex);
            }

            while (nodes.Count != 0)
            {
                
            }
            //Added return statement, else a ''not all paths return a value '' error was given
            return path;
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
            Func<Room, IEnumerable<Room>> shortestPath = v => {
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

            unvisited.Remove(startRoom);
            visited.Add(startRoom);



            while (unvisited.Count > 0)
            {
                Room lowestRoom = new Room();
                Hallway lowestHallway = new Hallway(999, null);

                foreach (Room n in visited)
                {
                    Hallway h = n.GetLowestLevelHallway(visited);
                    if (h != null && h.Enemy < lowestHallway.Enemy)
                    {
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

            foreach (Hallway h in notCollapsable)
            {
                h.IsCollapsable = false;
            }

            //Explode random room
            

        }
    }
}