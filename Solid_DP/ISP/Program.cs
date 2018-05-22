using System;

namespace ISP
{
    // Interface Segregation Principle

    public class Document
    {

    }

    // To many responsibilities attached to interface
    // The entities that should be implemented through the interface should be separable
    // Smaller interfaces --> atomicity
    public interface IMachine
    {
        void Print(Document d);
        void Scan(Document d);
        void Fax(Document d);
    }

    public class MultifunctionPrinter : IMachine
    {
        public void Print(Document d)
        {
            //
        }

        public void Scan(Document d)
        {
            //
        }

        public void Fax(Document d)
        {
            //
        }
    }

    public class OldFashionedPrinter : IMachine
    {
        public void Print(Document d)
        {
            // Only this one gets implemented
            // so the others would need documentation to show that
            // the whole interface cannot be implemented
        }

        public void Scan(Document d)
        {
            throw new NotImplementedException();
        }

        public void Fax(Document d)
        {
            throw new NotImplementedException();
        }
    }

    // Solution is to implement smaller interfaces that describe lighter functionality

    public interface IPrinter
    {
        void Print(Document d);
    }

    public interface IScanner
    {
        void Scan(Document d);
    }

    public class Photocopier : IPrinter, IScanner
    {
        public void Print(Document d)
        {
            //
        }

        public void Scan(Document d)
        {
            //
        }
    }

    // Interfaces can be inherited so we can also have an interface for a multifunctional entity

    public interface IMultifunctionDevice : IScanner, IPrinter //...
    {

    }

    public class MultiFunctionDevice : IMultifunctionDevice
    {
        private IPrinter printer;
        private IScanner scanner;

        public MultiFunctionDevice(IPrinter printer, IScanner scanner)
        {
            this.printer = printer ?? throw new ArgumentNullException(paramName: nameof(printer));
            this.scanner = scanner ?? throw new ArgumentNullException(paramName: nameof(scanner));
        }

        public void Print(Document d)
        {
            printer.Print(d);
        }

        public void Scan(Document d)
        {
            scanner.Scan(d);
        }
        // Decorator pattern
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
