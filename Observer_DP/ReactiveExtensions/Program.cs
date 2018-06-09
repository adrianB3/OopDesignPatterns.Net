using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ReactiveExtensions.Annotations;

namespace ReactiveExtensions
{
    public class Market
    {
        public BindingList<float> Prices = new BindingList<float>();
        public void AddPrice(float price)
        {
            Prices.Add(price);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var market = new Market();
            market.Prices.ListChanged += (sender, eventArgs) =>
            {
                if (eventArgs.ListChangedType == ListChangedType.ItemAdded)
                {
                    float price = ((BindingList<float>) sender)[eventArgs.NewIndex];
                    Console.WriteLine($"Binding list got a price of {price}");
                }
            };
            market.AddPrice(123f);
        }
    }
}
