using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Adapter_DP
{
    public class Point
    {
        public int X, Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            return obj is Point point &&
                   X == point.X &&
                   Y == point.Y;
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }
    }

    public class Line
    {
        public Point Start, End;

        public Line(Point start, Point end)
        {
            Start = start ?? throw new ArgumentNullException(nameof(start));
            End = end ?? throw new ArgumentNullException(nameof(end));
        }

        public override bool Equals(object obj)
        {
            return obj is Line line &&
                   EqualityComparer<Point>.Default.Equals(Start, line.Start) &&
                   EqualityComparer<Point>.Default.Equals(End, line.End);
        }

        public override int GetHashCode()
        {
            var hashCode = -1676728671;
            hashCode = hashCode * -1521134295 + EqualityComparer<Point>.Default.GetHashCode(Start);
            hashCode = hashCode * -1521134295 + EqualityComparer<Point>.Default.GetHashCode(End);
            return hashCode;
        }
    }

    public class VectorObject : Collection<Line>
    {

    }

    public class VectorRectangle : VectorObject
    {
        public VectorRectangle(int x, int y, int width, int height)
        {
            Add(new Line(new Point(x, y), new Point(x + width, y)));
            Add(new Line(new Point(x + width, y), new Point(x + width, y + height)));
            Add(new Line(new Point(x, y), new Point(x, y + height)));
            Add(new Line(new Point(x, y + height), new Point(x + width, y + height)));
        }
    }

    public class LineToPointAdapter : IEnumerable<Point>
    {
        private static int count;
        static Dictionary<int, List<Point>> cache // HashCode as key
            = new Dictionary<int, List<Point>>();
        public LineToPointAdapter(Line line)
        {
            var hash = line.GetHashCode();
            if (cache.ContainsKey(hash)) return; // nothing happens if line already exists

            Console.WriteLine($"{++count}: Generating points for line [{line.Start.X}, {line.Start.Y}]-[{line.End.X}, {line.End.Y}]");

            var points = new List<Point>();

            int left = Math.Min(line.Start.X, line.End.X);
            int rigth = Math.Max(line.Start.X, line.End.X);
            int top = Math.Min(line.Start.Y, line.End.Y);
            int bottom = Math.Max(line.Start.X, line.End.Y);
            int dx = rigth - left;
            int dy = line.End.Y - line.Start.Y;

            if (dx == 0)
            {
                for (int y = top; y <= bottom; ++y)
                {
                    points.Add(new Point(left, y));
                }
            }

            else if (dy == 0)
            {
                for (int x = left; x <= rigth; ++x)
                {
                    points.Add(new Point(x, top));
                }
            }

            cache.Add(hash, points);
        }

        public IEnumerator<Point> GetEnumerator()
        {
            return cache.Values.SelectMany(x => x).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    class Program
    {
        // We have two vector Rectangles, but we know only how to draw
        // points
        // So, we need to make an adapter to convert the set of lines of the rectangle to 
        // a set of points
        private static readonly List<VectorObject> vectorObjects
            = new List<VectorObject>()
            {
                new VectorRectangle(1,1,10,10),
                new VectorRectangle(3,3,6,6)
            };


        public static void DrawPoint(Point p)
        {
            Console.Write(".");
        }
        static void Main(string[] args)
        {
            Draw();
            Draw(); // the points are generated only 8 times because we used caching
        }

        // A side effect of the adapter is that is generating a lot of temporary information
        // this can be resolved using a cached adapter
        private static void Draw()
        {
            foreach (var vectorObject in vectorObjects)
            {
                foreach (var line in vectorObject)
                {
                    var adapter = new LineToPointAdapter(line);
                    foreach (var point in adapter)
                    {
                        DrawPoint(point);
                    }
                }
            }
        }
    }
}
