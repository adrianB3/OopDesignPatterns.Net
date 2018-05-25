using System;
using System.Collections.Generic;
using System.Linq;

namespace DIP
{
    // Dependency Inversion Principle
    // High level parts of the system doesn't need to depend on
    // lower level parts
    // they should be linked through an abstraction

    public enum Relationship
    {
        Parent,
        Child,
        Sibling
    }

    public class Person
    {
        public string Name;
    }

    // Solution to dip problem --> now the dependency is for an abstraction
    // so the low level part can change the way it stores data
    public interface IRelationShipBrowser
    {
        IEnumerable<Person> FindAllChildrenOf(string name);
    }

    // low-level
    public class Relationships : IRelationShipBrowser
    {
        //This way the problem is that class Relationships cannot change the
        // way it stores the data
        // The solution is to make an abstraction, maybe an interface for being able
        // to access certain high level data 
        private List<(Person, Relationship, Person)> relations = new List<(Person, Relationship, Person)>();

        public void AddParentAndChild(Person parent, Person child)
        {
            relations.Add((parent, Relationship.Parent, child));
            relations.Add((child, Relationship.Child, parent));
        }

        //public List<(Person, Relationship, Person)> Relations => relations; // this is not nice
        public IEnumerable<Person> FindAllChildrenOf(string name)
        {
            return relations.Where(
                x => x.Item1.Name == name &&
                     x.Item2 == Relationship.Parent
            ).Select(r => r.Item3);
        }
    }

    public class Research
    {
        /*public Research(Relationships relationships)
        {
            var relations = relationships.Relations;
            foreach (var relation in relations.Where(
                x => x.Item1.Name == "John" &&
                     x.Item2 == Relationship.Parent
                     ))
            {
                Console.WriteLine($"John has a child called {relation.Item3.Name}");
            }
        }*/

        public Research(IRelationShipBrowser browser)
        {
            foreach (var person in browser.FindAllChildrenOf("John"))
            {
                Console.WriteLine($"John has a child called {person.Name}");
            }
        }

        static void Main(string[] args)
        {
            var parent = new Person() {Name = "John"};
            var child1 = new Person() {Name = "Chris"};
            var child2 = new Person() {Name = "Mary"};

            var relationships = new Relationships();
            relationships.AddParentAndChild(parent, child1);
            relationships.AddParentAndChild(parent, child2);

            new Research(relationships);
        }
    }
}
