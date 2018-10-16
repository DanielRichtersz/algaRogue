using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueDungeonCrawler.Classes
{
    public class Algorithms
    {
        private void PreVisit(Room vertex)
        {
            Console.Write("Visiting vertext: ");
            Console.Write(vertex);
            vertex.IsVisited = true;
        }

        public HashSet<Room> BreadthFirstSearch(Level level, Room startRoom)
        {
            var visited = new HashSet<Room>();

            if (!level.AdjacencyList.ContainsKey(startRoom))
            {
                return visited;
            }

            var queue = new Queue<Room>();
            queue.Enqueue(startRoom);

            while (queue.Count > 0)
            {
                var vertex = queue.Dequeue();

                if (visited.Contains(vertex))
                {
                    continue;
                }

                PreVisit(vertex);

                visited.Add(vertex);

                foreach (var neighbor in level.AdjacencyList[vertex])
                {
                    if (!visited.Contains(neighbor))
                    {
                        queue.Enqueue(neighbor);
                    }
                }
            }
            return visited;
        }
    }
}