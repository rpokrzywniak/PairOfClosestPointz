using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PairOfClosestPoints
{
    public class ClosestPair
    {
        public ClosestPair()
        {
            //var S = FileReader.GetPoints().ToList();
            var S = RandomGenerator.GetPoints().ToList();
            var Sx = S.OrderBy(point => point.X).ToList();
            var Sy = S.OrderBy(point => point.Y).ToList();
            var start = DateTime.Now;
            var closestPair = GetClosestPair(Sx, Sy);
            var end = (DateTime.Now - start).TotalMilliseconds;
            PrintResult(closestPair, end);
            start = DateTime.Now;
            var naiveClosestPair = NaiveResult(S);
            end = (DateTime.Now - start).TotalMilliseconds;
            PrintResult(naiveClosestPair, end);

        }
        private Result GetClosestPair(List<Point> Sx, List<Point> Sy)
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

            var splitResult = GetClosestSplitPair(Sx, Sy, bestResult, line, S1x);

            return (bestResult.Distance <= splitResult.Distance) ? bestResult : splitResult;
        }
        private Result BruteForce(List<Point> Sx)
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
        private Result GetClosestSplitPair(List<Point> Sx, List<Point> Sy, Result bestResult, int line, List<Point> S1x)
        {
            var R = new List<Point>();
            var B = new List<Point>();
            foreach (var point in Sy)
            {
                if ((line - bestResult.Distance <= point.X) && (point.X <= line) && S1x.Contains(point))
                {
                    R.Add(point);                 
                }
                else if ((line + bestResult.Distance >= point.X) && (point.X >= line) && !S1x.Contains(point))
                {
                    B.Add(point);
                }
            }

            var splitResult = new Result
            {
                Distance = bestResult.Distance
            };

            R.ForEach(pointR =>
            {
                B.Where(pointB => pointB.Y >= pointR.Y).Take(4).ToList().ForEach(pointB =>
                  {
                      var distance = pointR.GetDistance(pointB);
                      if (distance < splitResult.Distance)
                      {
                          splitResult.P1 = pointR;
                          splitResult.P2 = pointB;
                          splitResult.Distance = distance;
                      }
                  });
            });
            B.ForEach(pointB =>
            {
                R.Where(pointR => pointR.Y >= pointB.Y).Take(4).ToList().ForEach(pointR =>
                {
                    var distance = pointB.GetDistance(pointR);
                    if (distance < splitResult.Distance)
                    {
                        splitResult.P1 = pointB;
                        splitResult.P2 = pointR;
                        splitResult.Distance = distance;
                    }
                });
            });
            /*
            var lengthR = R.Count;
            var lengthB = B.Count;
            for (int i = 0; i < lengthR; i++)
            {
                for (int j = i + 1; j < Math.Min(i + 4, lengthB); j++)
                {
                    var p1 = R[i];
                    var p2 = B[j];
                    var distance = p1.GetDistance(p2);
                    if (distance < splitResult.Distance)
                    {
                        splitResult.P1 = p1;
                        splitResult.P2 = p2;
                        splitResult.Distance = distance;
                    }
                }
            }
            for (int i = 0; i < lengthB - 1; i++)
            {
                for (int j = i + 1; j < Math.Min(i + 4, lengthR); j++)
                {
                    var p1 = B[i];
                    var p2 = R[j];
                    var distance = p1.GetDistance(p2);
                    if (distance < splitResult.Distance)
                    {
                        splitResult.P1 = p1;
                        splitResult.P2 = p2;
                        splitResult.Distance = distance;
                    }
                }
            }*/
            return splitResult;
        }
        private Result NaiveResult(List<Point> S)
        {
            var result = new Result()
            {
                Distance = double.MaxValue
            };

            S.ForEach(a =>
            {
                S.ForEach(b =>
                {
                    var distance = a.GetDistance(b);
                    if(distance>0 && distance < result.Distance)
                    {
                        result.Distance = distance;
                        result.P1 = a;
                        result.P2 = b;
                    }
                });
            });
            return result;
        }
        private void PrintResult(Result result, double time)
        {
            Console.WriteLine("Closest Pair:");
            Console.WriteLine("P1:");
            Console.WriteLine("X = " + result.P1.X);
            Console.WriteLine("Y = " + result.P1.Y);
            Console.WriteLine("P2:");
            Console.WriteLine("X = " + result.P2.X);
            Console.WriteLine("Y = " + result.P2.Y);
            Console.WriteLine("Distance = " + result.Distance);
            Console.WriteLine("Time = " + time);
            Console.ReadLine();
        }
    }
}
