using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA_Proyecto_1
{
    class Grid
    {
        public int width, height;

        public Point[] neighbors_coord =
        {
            new Point(1, 0),
            new Point(0, -1),
            new Point(-1, 0),
            new Point(0, 1)
        };

        public List<Tuple<Point, List<Point>>> diagonals = new List<Tuple<Point, List<Point>>>();


        public HashSet<Point> obstructed = new HashSet<Point>();

        public Grid(int width, int height)
        {
            this.width = width;
            this.height = height;
            diagonals.Add(new Tuple<Point, List<Point>>(new Point(1, 1), new List<Point> { new Point(1, 0), new Point(0, 1) }));
            diagonals.Add(new Tuple<Point, List<Point>>(new Point(1, -1), new List<Point> { new Point(1, 0), new Point(0, -1) }));
            diagonals.Add(new Tuple<Point, List<Point>>(new Point(-1, 1), new List<Point> { new Point(-1, 0), new Point(0, 1) }));
            diagonals.Add(new Tuple<Point, List<Point>>(new Point(-1, -1), new List<Point> { new Point(-1, 0), new Point(0, -1) }));

        }

        public bool inBounds(Point pxy)
        {
            return 0 <= pxy.X && pxy.X < width && 0 <= pxy.Y && pxy.Y <= height;
        }

        public bool unBlocked(Point pxy)
        {
            return !obstructed.Contains(pxy);
        }

        public ArrayList Neighbors(Point pxy, bool diagonal)
        {
            ArrayList neighbors = new ArrayList();
            foreach (var coordinate in neighbors_coord)
            {
                Point point = new Point(pxy.X + coordinate.X, pxy.Y + coordinate.Y);
                if (inBounds(point) && unBlocked(point)) neighbors.Add(point);
            }

			if (diagonal)
			{
				foreach (var coordinate in diagonals)
				{
					Point point = new Point(pxy.X + coordinate.Item1.X, pxy.Y + coordinate.Item1.Y);
					if (inBounds(point) && unBlocked(point))
					{
						if (unBlocked(new Point(pxy.X + coordinate.Item2[0].X, pxy.Y + coordinate.Item2[0].Y))
							|| unBlocked(new Point(pxy.X + coordinate.Item2[1].X, pxy.Y + coordinate.Item2[1].Y)))
							neighbors.Add(point);
					}
				}
			}
            
            return neighbors;
        }
    }
}
