using System;
using System.Text;

namespace WithoutBuilder
{
    class Program
    {
        // Little example to show a program that is supposed to
        // output html format content without using a builder
        // The program easily gets very clutered adding more functionality
        static void Main(string[] args)
        {
            var hello = "hello";
            var sb = new StringBuilder();
            sb.Append("<p>");
            sb.Append(hello);
            sb.Append("</p>");
            Console.WriteLine(sb);

            var words = new[] {"hello", "world"};
            sb.Clear();
            sb.Append("<ul>");
            foreach (var word in words)
            {
                sb.AppendFormat("<li>{0}</li>", word);
            }

            sb.Append("</ul>");
            Console.WriteLine(sb);
        }
    }
}
