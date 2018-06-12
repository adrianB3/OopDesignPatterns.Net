using System;

namespace Broker_Chain
{
    class Program
    {
        public class Game // the mediator
        {
            public event EventHandler<Query> Queries;

            public void PerformQuery(object sender, Query q)
            {
                Queries?.Invoke(sender, q);
            }
        }

        public class Query
        {
            public string CreatureName;
            public enum Argument
            {
                Attack, Defense
            }

            public Argument WhatToQuery;
            public int Value;

            public Query(string creatureName, Argument whatToQuery, int value)
            {
                CreatureName = creatureName ?? throw new ArgumentNullException(nameof(creatureName));
                WhatToQuery = whatToQuery;
                Value = value;
            }
        }

        public class Creature
        {
            public Game game;
            public string Name;
            private int attack, defense;

            public Creature(Game game, string name, int attack, int defense)
            {
                this.game = game ?? throw new ArgumentNullException(nameof(game));
                Name = name ?? throw new ArgumentNullException(nameof(name));
                this.attack = attack;
                this.defense = defense;
            }

            public int Attack
            {
                get
                {
                    // collect Bonuses
                    var q = new Query(Name, Query.Argument.Attack, attack);
                    game.PerformQuery(this, q); // q.Value -> actual attack value
                    return q.Value;
                }
            }

            public int Defense
            {
                get
                {
                    // collect Bonuses
                    var q = new Query(Name, Query.Argument.Defense, attack);
                    game.PerformQuery(this, q); // q.Value -> actual defense value
                    return q.Value;
                }
            }

            public override string ToString()
            {
                return $"{nameof(Name)}: {Name}, {nameof(Attack)}: {Attack}, {nameof(Defense)}: {Defense}";
            }
        }

        public abstract class CreatureModifier : IDisposable
        {
            protected Game game;
            protected Creature creature;

            protected CreatureModifier(Game game, Creature creature)
            {
                this.game = game ?? throw new ArgumentNullException(nameof(game));
                this.creature = creature ?? throw new ArgumentNullException(nameof(creature));
                game.Queries += Handle; // subscribe to smth abstract so that the derived classes should be called
            }

            protected abstract void Handle(object sender, Query q);

            public void Dispose()
            {
                game.Queries -= Handle; // unsubscribe
            }
        }

        public class DoubleAttackModifier : CreatureModifier
        {
            public DoubleAttackModifier(Game game, Creature creature) : base(game, creature)
            {
            }

            protected override void Handle(object sender, Query q)
            {
                if (q.CreatureName == creature.Name && q.WhatToQuery == Query.Argument.Attack)
                {
                    q.Value *= 2;
                }
            }
        }

        public class IncreaseDefenseModifier : CreatureModifier
        {
            public IncreaseDefenseModifier(Game game, Creature creature) : base(game, creature)
            {
            }

            protected override void Handle(object sender, Query q)
            {
                if (q.CreatureName == creature.Name && q.WhatToQuery == Query.Argument.Defense)
                {
                    q.Value++;
                }
            }
        }

        static void Main(string[] args)
        {
            var game = new Game();
            var goblin = new Creature(game,"Gobin", 2,2);
            Console.WriteLine(goblin);

            using (new DoubleAttackModifier(game, goblin))
            {
                Console.WriteLine(goblin);
                using (new IncreaseDefenseModifier(game, goblin))
                {
                    Console.WriteLine(goblin);
                }
            }

            Console.WriteLine(goblin); // we can now go back to previous state
        }
    }

    
}
