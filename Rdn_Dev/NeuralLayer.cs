using System;

namespace Rdn_Dev
{
    public class NeuralLayer
    {
        private Neuron[] neurons = null;
        private int neuronsCount = 0;

        public NeuralLayer(int neuronsCount)
        {
            initialize(neuronsCount);
        }

        public NeuralLayer(int neuronsCount, IFunctions f)
        {
            initialize(neuronsCount);
            setActivationFunction(f);
        }
        private void initialize(int neuronsCount)
        {
            if (neuronsCount <= 0)
            {
                throw new Exception();
            }

            this.neuronsCount = neuronsCount;
            neurons = new Neuron[neuronsCount];

            for (int i = 0; i < neuronsCount; i++)
            {
                neurons[i] = new Neuron();
            }
        }

        public void connectWith(NeuralLayer otherLayer)
        {
            foreach (Neuron otherNeurons in otherLayer.neurons)
            {
                foreach (Neuron n in neurons)
                {
                    otherNeurons.addSynapse(new Synapse(n));
                }
            }
        }

        public Neuron[] getNeurons()
        {
            return neurons;
        }

        public int getNeuronsCount()
        {
            return neuronsCount;
        }

        private void setActivationFunction(IFunctions f)
        {
            foreach (Neuron n in neurons)
            {
                n.setActivationFunction(f);
            }
        }
    }
}