using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PairOfClosestPoints
{
    public static class RandomGenerator
    {
        public static IList<Point> GetPoints()
        {
            var points = new List<Point>();
            for(int i = 0; i < 10000; i++)
            {
                Random rnd = new Random();
                var point = new Point
                {
                    Id = i,
                    X = rnd.Next(-10000,10000),
                    Y = rnd.Next(-10000,10000)
                };
                if(points.Exists(x=>x.Y == point.Y && x.X == point.X))
                {
                    continue;
                }

                points.Add(point);
            }
            return points;
        }
    }
}
