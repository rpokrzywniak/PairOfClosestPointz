using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PairOfClosestPoints
{
    public static class FileReader
    {
        public static IList<Point> GetPoints()
        {
            var lines = File.ReadLines("2points.txt");
            var points = new List<Point>();
            int i = 0;
            foreach (var line in lines)
            {

                var splitted = line.Split(',');
                var point = new Point
                {
                    Id = i,
                    X = int.Parse(splitted[0]),
                    Y = int.Parse(splitted[1])
                };
                i++;
                points.Add(point);
            }
            return points;
        }
    }
}
