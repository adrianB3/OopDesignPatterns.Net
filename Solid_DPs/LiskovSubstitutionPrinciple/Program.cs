using System;

namespace LiskovSubstitutionPrinciple
{
    public class Rectangle
    {
        // Liskov Substitution Principle
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }

        public Rectangle()
        {
            
        }

        public Rectangle(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"{nameof(Width)}: {Width}, {nameof(Height)}: {Height}";
        }
    }

    public class Square : Rectangle
    {
        public override int Width
        {
            set { base.Width = base.Height = value; }
        }

        public override int Height
        {
            set { base.Height = base.Height = value; }
        }
    }

    class Program
    {
        static public int Area(Rectangle r) => r.Height * r.Width;
        static void Main(string[] args)
        {
            Rectangle rc = new Rectangle(2,3);
            Console.WriteLine($"{rc} has area {Area(rc)}");
            Rectangle sq = new Square();
            sq.Width = 4;
            Console.WriteLine($"{sq} has area {Area(sq)}");
        }
    }
}
