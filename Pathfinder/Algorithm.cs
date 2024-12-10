using System;
using System.Collections.Generic;
using System.Linq;

namespace Pathfinder
{
    internal static class Algorithm
    {
        public static IEnumerable<Node> ShortestPath(Graph graph, Node from, Node to)
        {
            var distance = new Dictionary<Node, double>();
            var previous = new Dictionary<Node, Node>();
            const double tolerance = 0.0;
            var priorityQueue = new SortedSet<(double distance, Node node)>(Comparer<(double, Node)>.Create((x, y) => Math.Abs(x.Item1 - y.Item1) > tolerance ? x.Item1.CompareTo(y.Item1) : x.Item2.GetHashCode().CompareTo(y.Item2.GetHashCode())));
            
            foreach (var node in graph.Nodes)
            {
                distance[node] = double.PositiveInfinity;
                previous[node] = null;
            }

            distance[from] = 0;
            priorityQueue.Add((0, from));

            while (priorityQueue.Any())
            {
                var current = priorityQueue.Min.node;
                priorityQueue.Remove(priorityQueue.Min);

                if (current.Equals(to))
                {
                    var path = new List<Node>();
                    while (previous[current] != null)
                    {
                        path.Add(current);
                        current = previous[current];
                    }
                    path.Add(from);
                    path.Reverse();
                    return path;
                }

                foreach (var (weight, neighbor) in graph.Neighbors(current))
                {
                    var currentToNeighbor = distance[current] + weight;
                    if (!(currentToNeighbor < distance[neighbor])) continue;
                    priorityQueue.Remove((distance[neighbor], neighbor));
                    distance[neighbor] = currentToNeighbor;
                    previous[neighbor] = current;
                    priorityQueue.Add((currentToNeighbor, neighbor));
                }
            }

            return new Node[] { };
        }
    }
}
