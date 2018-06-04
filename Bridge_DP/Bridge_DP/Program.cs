using System;
using Autofac;

namespace Bridge_DP
{
    // Used for avoiding cartesian requirments scenario

    public interface IRenderer
    {
        void RenderCircle(float radius);
    }

    public class VectorRenderer : IRenderer
    {
        public void RenderCircle(float radius)
        {
            Console.WriteLine($"Drawing a circle of radius {radius} using VectorRenderer");
        }
    }

    public class RasterRenderer : IRenderer
    {
        public void RenderCircle(float radius)
        {
            Console.WriteLine($"Drawing a circle of radius {radius} using RasterRenderer");
        }
    }

    public abstract class Shape
    {
        protected IRenderer renderer;

        protected Shape(IRenderer renderer)
        {
            this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        }

        public abstract void Draw();
        public abstract void Resize(float factor);
    }


    public class Circle : Shape
    {
        private float radius;

        public Circle(IRenderer renderer, float radius) : base(renderer)
        {
            this.radius = radius;
        }

        public override void Draw()
        {
            renderer.RenderCircle(radius);
        }

        public override void Resize(float factor)
        {
            radius *= factor;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            IRenderer renderer = new RasterRenderer();
            IRenderer renderer2 = new VectorRenderer();
            var circle = new Circle(renderer, 5);
            var circle2 = new Circle(renderer2, 5);

            circle.Draw();
            circle.Resize(2);
            circle.Draw();

            circle2.Draw();
            circle2.Resize(3);
            circle2.Draw();

            // using Dependency Injection with Autofac
            var cb = new ContainerBuilder();
            cb.RegisterType<VectorRenderer>().As<IRenderer>();
            cb.Register((c, p) => new Circle(c.Resolve<IRenderer>(), p.Positional<float>(0)));
            using (var c = cb.Build())
            {
                var circle3 = c.Resolve<Circle>(new PositionalParameter(0, 5.0f));
                circle3.Draw();
                circle3.Resize(2);
                circle3.Draw();
            }
        }
    }
}
