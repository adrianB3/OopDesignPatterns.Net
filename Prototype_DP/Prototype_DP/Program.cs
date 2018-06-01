using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace Prototype_DP
{
    public static class ExtensionMethods
    {
        // Deep copy using serialization that can work on any type of object
        // It is used when the object that we want to copy
        // depends on other classes, so the whole tree of classes is serialized and deep copied
        public static T DeepCopy<T>(this T self)
        {
            var stream = new MemoryStream();
            var formatter = new BinaryFormatter(); // using the binary formatter it's fast, but
            formatter.Serialize(stream, self);      // every class should be marked with [Serializable]
            stream.Seek(0, SeekOrigin.Begin);
            object copy = formatter.Deserialize(stream);
            stream.Close();
            return (T) copy;
        }

        // Another way is to use an XML serializer

        public static T DeepCopyXML<T>(this T self)
        {
            using (var ms = new MemoryStream())
            {
                var s = new XmlSerializer(typeof(T));
                s.Serialize(ms, self);
                ms.Position = 0;
                return (T) s.Deserialize(ms);
            }
        }
    }

    // [Serializable]
    public class Person
    {
        public string[] Names;
        public AddressC Address;

        public Person()
        {
            // Needed for XML Serializer
        }

        public Person(string[] names, AddressC address)
        {
            Names = names ?? throw new ArgumentNullException(nameof(names));
            this.Address = address ?? throw new ArgumentNullException(nameof(address));
        }

        // Copy Constructor
        public Person(Person other)
        {
            Names = other.Names;
            Address = new AddressC(other.Address);
        }

        public override string ToString()
        {
            return $"{nameof(Names)}: {string.Join(" ",Names)}, {nameof(Address)}: {Address}";
        }
    }

    // [Serializable]
    public class AddressC
    {
        public string StreetName;
        public int HouseNumber;

        public AddressC()
        {
            // Needed for XML Serializer
        }
        public AddressC(string streetName, int houseNumber)
        {
            StreetName = streetName ?? throw new ArgumentNullException(nameof(streetName));
            HouseNumber = houseNumber;
        }

        // Copy constructor
        public AddressC(AddressC other)
        {
            StreetName = other.StreetName;
            HouseNumber = other.HouseNumber;
        }

        public override string ToString()
        {
            return $"{nameof(StreetName)}: {StreetName}, {nameof(HouseNumber)}: {HouseNumber}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var john = new Person(new string[]{ "John", "Smith" }, new AddressC("London Road", 123));
            /*
            var jane = john; // only the reference is copied, shallow copy, changes made from jane affect john
            jane.Names[0] = "Jane";
            */

           /*
            var mary = new Person(john); // deep copy using copy constructor
            mary.Address.HouseNumber = 321;
            mary.Names[0] = "Mary"; // TODO: Names are still changing
            */

            var mary = john.DeepCopyXML();
            mary.Names[0] = "Mary";
            mary.Address.HouseNumber = 321;

            Console.WriteLine(john);
            Console.WriteLine(mary);
        }
    }
}
