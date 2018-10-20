using System;
using System.Collections.Generic;


namespace RogueDungeonCrawler.Classes
{
    public class Algorithms
    {
        private void PreVisit(Room vertex)
        {
            vertex.IsVisited = true;
        }

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

            NorthNeighbor = NorthNeighborHallway == null ? null : NorthNeighborHallway.GetConnectedRoom(vertex);
            EastNeighbor = EastNeighborHallway == null ? null : EastNeighborHallway.GetConnectedRoom(vertex);
            SouthNeighbor = SouthNeighborHallway == null ? null : SouthNeighborHallway.GetConnectedRoom(vertex);
            WestNeighbor = WestNeighborHallway == null ? null : WestNeighborHallway.GetConnectedRoom(vertex);

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

                PreVisit(vertex);
                level.DrawMap();

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
                    path.Add(current);
                    //and select next (or actually previous) room
                    current = previous[current];
                };

                //Add the last room (starting room)
                path.Add(startRoom);
                //Reverse the path so its in correct order from startroom to endroom
                path.Reverse();

                return path;
            };

            return shortestPath;
        }
    }
}