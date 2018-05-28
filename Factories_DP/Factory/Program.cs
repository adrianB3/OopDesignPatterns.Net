using System;

namespace Factory
{
    public class Point
    {
        private double x, y;

        private  Point(double x, double y)  // Constructor is now private
        {
            this.x = x;
            this.y = y;
        }

        // what if we want a constructor for polar coordinates?
        // the constructor cannot be overloaded as the signature would be the same 
        // public Point(double rho, double theta)
        // A solution to this is using Factories

        public override string ToString()
        {
            return $"{nameof(x)}: {x}, {nameof(y)}: {y}";
        }

        // Inner Class
        public static class Factory
        {
            // Points are initialized through Factory Methods
            public static Point NewCartesianPoint(double x, double y)
            {
                return new Point(x, y);
            }

            public static Point NewPolarPoint(double rho, double theta)
            {
                return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var point = Point.Factory.NewPolarPoint(1.0, Math.PI / 2);
            Console.WriteLine(point);
        }
    }
}
