using System;
using System.Collections.Generic;
using System.Text;

namespace PairOfClosestPoints
{
    public class Point
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public double GetDistance(Point point)
        {
            return Math.Sqrt((point.X-X)*(point.X-X) + (point.Y-Y)*(point.Y-Y));
        }
    }
}
