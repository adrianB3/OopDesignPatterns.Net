using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatRoom
{
    public class Person
    {
        public string Name;
        public Room Room;
        private List<string> chatLog = new List<string>();

        public Person(string name)
        {
            Name = name;
        }

        public void Say(string message)
        {
            Room.Broadcast(Name, message);
        }

        public void PrivateMessage(string who, string message)
        {
            Room.Message(Name, who, message);
        }

        public void Receive(string sender, string message)
        {
            string s = $"{sender} : {message}";
            chatLog.Add(s);
            Console.WriteLine($"[{Name}'s chat session] {s}");
        }
    }

    public class Room
    {
        private List<Person> people = new List<Person>();

        public void Join(Person p)
        {
            string joinMsg = $"{p.Name} joins the chat";
            Broadcast("room", joinMsg);

            p.Room = this;
            people.Add(p);
        }

        public void Broadcast(string source, string message)
        {
            foreach (var person in people)
            {
                if(person.Name != source)
                    person.Receive(source, message);
            }
        }

        public void Message(string source, string destination, string message)
        {
            people.FirstOrDefault(p => p.Name == destination)?.Receive(source, message);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var room = new Room();

            var John = new Person("John");
            var Jane = new Person("Jane");

            room.Join(John);
            room.Join(Jane);

            John.Say("hello");
            John.Say("I am John");
            Jane.Say("hello to you too");

            var Simon = new Person("Simon");
            room.Join(Simon);
            Simon.Say("Hello I am Simon!");

            Jane.PrivateMessage("Simon", "hello simon");
        }
    }
}
