using System;
using System.Collections.Generic;
using System.IO;

namespace SRP
{
    // Single Responsibility Principle

    // A class should have only one reason to change, a single responsibility
    public class Journal
    {
        private readonly List<string> _entries = new List<string>();
        private static int _count = 0;

        public int AddEntry(string text)
        {
            _entries.Add($"{ ++_count }: { text }");
            return _count; // memento
        }

        public void RemoveEntry(int index)
        {
            _entries.RemoveAt(index);
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, _entries);
        }

        /*  Unrecommended, because it adds to many responibilities to the class
            Solution is to add another class that will handle the extended functionalities --> Persistence class in this example

        public void Save(string filename)
        {
            File.WriteAllText(filename, ToString());
        }

        public Journal Load(string filename)
        {
            throw new NotImplementedException();
        }
        */
    }

    // Now, we have separation of concerns 
    // Journal class is concerned with adding and removing entries to the journal
    // Persistance class is concerned with saving any object to a file
    public class Persistence
    {
        public void SaveToFile(Journal j, string filename, bool overwrite = false)
        {
            if (overwrite || !File.Exists(filename))
            {
                File.WriteAllText(filename, j.ToString());
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var myJournal = new Journal();
            myJournal.AddEntry("1st journal entry, yey");
            myJournal.AddEntry("I ate a bug today");

            Console.WriteLine(myJournal);

            var filePersistence = new Persistence();
            var filename = @"test.txt"; // saved in the debug/release folder
            filePersistence.SaveToFile(myJournal, filename, true);
        }
    }
}

