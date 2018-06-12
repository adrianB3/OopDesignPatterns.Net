using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.dotMemoryUnit;
using NUnit.Framework;

namespace Repeating_Usernames
{
    public class User
    {
        private string fullName;

        public User(string fullName)
        {
            this.fullName = fullName;
        }
    }

    public class User2
    {
        static List<string> strings = new List<string>();
        private int[] names;
        public User2(string fullname)
        {
            int getOrAdd(string s)
            {
                int idx = strings.IndexOf(s);
                if (idx != -1) return idx;
                else
                {
                    strings.Add(s);
                    return strings.Count - 1;
                }
            }

            names = fullname.Split(' ').Select(getOrAdd).ToArray();
        }

        public string FullName => string.Join(" ", names.Select(i => strings[i]));
    }

    [TestFixture]
    public class Program
    {
        
        
        [Test]
        public void TestUser()  // 6748867 bytes for User
        {
            var firstNames = Enumerable.Range(0, 100).Select(_ => RandomString());
            var lastNames = Enumerable.Range(0, 100).Select(_ => RandomString());

            var users = new List<User>();
            foreach (var firstName in firstNames)
            {
                foreach (var lastName in lastNames)
                {
                    users.Add(new User($"{firstName} {lastName}"));
                }
            }

            ForceGC(); // force garbage collector

            dotMemory.Check(memory => { Console.WriteLine(memory.SizeInBytes); });
        }

        [Test]
        public void TestUser2()  // 6748867 bytes, 6412521 bytes for User2
        {
            // TODO: Weird behaviour - without the Ienumerable lastnames the size is actully bigger
            var firstNames = Enumerable.Range(0, 100).Select(_ => RandomString());
            var lastNames = Enumerable.Range(0, 100).Select(_ => RandomString());

            var users = new List<User2>();
            IEnumerable enumerable = lastNames as string[] ?? lastNames.ToArray();
            foreach (var firstName in firstNames)          
                foreach (var lastName in enumerable)
                {
                    users.Add(new User2($"{firstName} {lastName}"));
                }
            

            ForceGC(); // force garbage collector

            dotMemory.Check(memory => { Console.WriteLine(memory.SizeInBytes); });
        }

        private void ForceGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private string RandomString()
        {
            Random rand = new Random();
            return new string(
                Enumerable.Range(0, 10)
                    .Select(i => (char) ('a' + rand.Next(26))).ToArray());
        }
    }
}
