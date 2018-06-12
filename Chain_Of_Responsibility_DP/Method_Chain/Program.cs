using System;

namespace Method_Chain
{
    public class Creature
    {
        public string Name;
        public int Attack, Defense;

        public Creature(string name, int attack, int defense)
        {
            Name = name;
            Attack = attack;
            Defense = defense;
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Attack)}: {Attack}, {nameof(Defense)}: {Defense}";
        }
    }

    public class CreatureModifier
    {
        protected Creature creature;
        protected CreatureModifier next; // a linked list actually

        public CreatureModifier(Creature creature)
        {
            this.creature = creature ?? throw new ArgumentNullException(nameof(creature));
        }

        public void Add(CreatureModifier cm)
        {
            if (next != null)
            {
                next.Add(cm);
            }
            else
            {
                next = cm;
            }
        }

        public virtual void Handle() => next?.Handle();
    }

    public class DoubleAttackModifier : CreatureModifier
    {
        public DoubleAttackModifier(Creature creature) : base(creature)
        {
        }

        public override void Handle()
        {
            Console.WriteLine($"Doubling {creature.Name}'s attack.");
            creature.Attack *= 2;
            base.Handle();
        }
    }

    public class IncreaseDefenseModifier : CreatureModifier
    {
        public IncreaseDefenseModifier(Creature creature) : base(creature)
        {
        }

        public override void Handle()
        {
            Console.WriteLine($"Increasing {creature.Name}'s defense.");
            creature.Defense++;
            base.Handle();
        }
    }

    public class NoBonusesModifier : CreatureModifier
    {
        public NoBonusesModifier(Creature creature) : base(creature)
        {
        }

        public override void Handle()
        {
            // do nothing so that the next modifiers will not be called
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var goblin = new Creature("Goblin", 2,2);
            Console.WriteLine(goblin);

            var root = new CreatureModifier(goblin);

            //root.Add(new NoBonusesModifier(goblin));

            root.Add(new DoubleAttackModifier(goblin));
            root.Add(new IncreaseDefenseModifier(goblin));

            root.Handle(); // takes care of every modifier in the chain

            Console.WriteLine(goblin);
        }
    }
}
