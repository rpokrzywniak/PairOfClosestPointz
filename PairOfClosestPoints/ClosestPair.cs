﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PairOfClosestPoints
{
    public class ClosestPair
    {
        public ClosestPair()
        {
            var S = FileReader.GetPoints().ToList();
            var Sx = S.OrderBy(point => point.X).ToList();
            var Sy = S.OrderBy(point => point.Y).ToList();

            var closestPair = GetClosestPair(Sx, Sy);
            Console.WriteLine("Closest Pair:");
            Console.WriteLine("P1:");
            Console.WriteLine("X = "+closestPair.P1.X);
            Console.WriteLine("Y = " + closestPair.P1.Y);
            Console.WriteLine("P2:");
            Console.WriteLine("X = " + closestPair.P2.X);
            Console.WriteLine("Y = " + closestPair.P2.Y);
            Console.WriteLine("Distance = "+ closestPair.Distance);
            Console.ReadLine();
        }
        public Result GetClosestPair(List<Point> Sx, List<Point> Sy)
        {
            if (Sx.Count <= 3)
            {
                return BruteForce(Sx);
            }

            var S1x = new List<Point>();
            var S2x = new List<Point>();
            int index = Sx.Count / 2;
            for (int i = 0; i < index; i++)
            {
                S1x.Add(Sx[i]);
            }
            for (int i = index; i < Sx.Count; i++)
            {
                S2x.Add(Sx[i]);
            }

            int line = S1x.Last().X;

            var S1y = new List<Point>();
            var S2y = new List<Point>();
            foreach (var point in Sy)
            {
                if (S1x.Contains(point))
                {
                    S1y.Add(point);
                }
                else
                {
                    S2y.Add(point);
                }
            }

            var resultS1 = GetClosestPair(S1x, S1y);
            var resultS2 = GetClosestPair(S2x, S2y);

            Result bestResult = (resultS1.Distance <= resultS2.Distance) ? resultS1 : resultS2;

            var splitResult = GetClosestSplitPair(Sx, Sy, bestResult, line);

            return (bestResult.Distance <= splitResult.Distance) ? bestResult : splitResult;
        }
        public Result BruteForce(List<Point> Sx)
        {
            var bruteForceResult = new Result
            {
                P1 = Sx[0],
                P2 = Sx[1],
                Distance = Sx[0].GetDistance(Sx[1])
            };
            var length = Sx.Count;
            if (length == 2)
            {
                return bruteForceResult;
            }

            for (int i = 0; i < length-1; i++)
            {
                for (int j = i + 1; j < length; j++)
                {
                    if(i != 0 && j != 1)
                    {
                        var d = Sx[i].GetDistance(Sx[j]);
                        if (d < bruteForceResult.Distance)
                        {
                            bruteForceResult.Distance = d;
                            bruteForceResult.P1 = Sx[i];
                            bruteForceResult.P2 = Sx[j];
                        }
                    }
                }
            }
            return bruteForceResult;
        }
        public Result GetClosestSplitPair(List<Point> Sx, List<Point> Sy, Result bestResult, int line)
        {
            var splitY = new List<Point>();
            foreach (var point in Sy)
            {
                if ((line - bestResult.Distance <= point.X) && (point.X <= line + bestResult.Distance))
                {
                    splitY.Add(point);
                }
            }

            var splitResult = new Result
            {
                Distance = bestResult.Distance
            };

            var length = splitY.Count;
            for (int i = 0; i < length-1; i++)
            {
                for (int j = i + 1; j < Math.Min(i + 7, length); j++)
                {
                    var p1 = splitY[i];
                    var p2 = splitY[j];
                    var distance = p1.GetDistance(p2);
                    if (distance < splitResult.Distance)
                    {
                        splitResult.P1 = p1;
                        splitResult.P2 = p2;
                        splitResult.Distance = distance;
                    }
                }
            }
            return splitResult;
        }
    }
}