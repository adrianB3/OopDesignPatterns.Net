using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NeuralNetworks
{
    public static class ExtentionMethods
    {
        public static void ConnectTo(this IEnumerable<Neuron> self, IEnumerable<Neuron> other)
        {
            if(ReferenceEquals(self, other)) return;

            foreach (var From in self)
            foreach (var To in other)
            {
                From.Out.Add(To);
                To.In.Add(From);
            }
        }
    }

    public class Neuron : IEnumerable<Neuron>
    {
        public float Value;
        public List<Neuron> In, Out;

        public IEnumerator<Neuron> GetEnumerator()
        {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class NeuronLayer : Collection<Neuron>
    {

    }

    class Program
    {
        static void Main(string[] args)
        {
            var neuron1 = new Neuron();
            var neuron2 = new Neuron();
            
            neuron1.ConnectTo(neuron2);

            var neuronLayer1 = new NeuronLayer();
            var neuronLayer2 = new NeuronLayer();

            neuron1.ConnectTo(neuronLayer1);
            neuronLayer2.ConnectTo(neuronLayer1);
        }
    }
}
