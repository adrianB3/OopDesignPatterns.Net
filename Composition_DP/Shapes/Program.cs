using System;
using System.Collections.Generic;
using System.Text;

namespace Shapes
{
    // Treating complex composite entities as simple ones
    // the same as grouping shapes and moving them as a single shape

    public class GraphicObject
    {
        public string Color;
        public virtual string Name { get; set; } = "Group";

        private Lazy<List<GraphicObject>> children = new Lazy<List<GraphicObject>>(); // the list is instantiated only if there is a need for it
        public List<GraphicObject> Children => children.Value;

        public void Print(StringBuilder sb, int depth)
        {
            sb.Append(new string('*', depth))
                .Append(string.IsNullOrWhiteSpace(Color) ? string.Empty : $"{Color} ")
                .AppendLine(Name);

            foreach (var child in Children)
            {
                child.Print(sb, depth+1);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            Print(sb, 0);
            return sb.ToString();
        }
    }

    public class Circle : GraphicObject
    {
        public override string Name => "Circle";
    }

    public class Square : GraphicObject
    {
        public override string Name => "Square";
    }
    class Program
    {
        static void Main(string[] args)
        {
            var drawing = new GraphicObject
                {Name = "My Drawing"};
            drawing.Children.Add(new Square {Color = "Red"});
            drawing.Children.Add(new Circle {Color = "Blue"});

            var group = new GraphicObject();
            group.Children.Add(new Circle {Color = "Yellow"});
            group.Children.Add(new Square {Color = "Yellow"});

            drawing.Children.Add(group);

            Console.WriteLine(drawing);
        }
    }
}
