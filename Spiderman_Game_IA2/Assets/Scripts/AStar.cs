using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA_Proyecto_1
{
    public class AStar
    {
        public Dictionary<Point, Point> cameFrom = new Dictionary<Point, Point>();
        public Dictionary<Point, int> costSoFar = new Dictionary<Point, int>();

        public bool diagonals = false;

        public int Heuristic(Point origin, Point goal)
        {
            // D == 1 && D2 == 1 : Chebyschev distance
            // D == 1 && D2 == sqrt(2) : octile distance
            int D = 1;
            int D2 = 1;


            int dx = Math.Abs(origin.X - goal.X);
            int dy = Math.Abs(origin.Y - goal.Y);

            if (this.diagonals)
            {
                //Diagonal distance
                return D * (dx + dy) + (D2 - 2 * D) * Math.Min(dx, dy);
            }
            else
            {
                // Manhattan distance
                return D * (dx + dy);
            }
        }

        public AStar(City graph, Point origin, Point goal, Boolean diagonal)
        {
			this.diagonals = diagonal;
            var nodes = new MinHeap();
            nodes.Enqueue(origin, 0);

            cameFrom[origin] = origin;
            costSoFar[origin] = 0;

            while (nodes.Count > 0)
            {
                var current = nodes.Dequeue();
                if (current.Equals(goal))
                {
                    break;
                }

                ArrayList neighbors = graph.Neighbors(current.key, diagonals);
                foreach (Point neighbor in neighbors)
                {
                    int newCost = costSoFar[current.key] + 1;
                    if (!costSoFar.ContainsKey(neighbor)
                        || newCost < costSoFar[neighbor])
                    {
                        costSoFar[neighbor] = newCost;
                        int cost = newCost + Heuristic(neighbor, goal);
                        nodes.Enqueue(neighbor, cost);
                        cameFrom[neighbor] = current.key;
                    }
                }
            }
        }

    }
}
