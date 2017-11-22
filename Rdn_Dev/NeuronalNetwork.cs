using System;
using Rdn_Dev.Events;

namespace Rdn_Dev
{
    public enum NetworkState
    {
        NEWLY_CREATED,

        LEARNING_IN_PROGRESS,

        LEARNING_TERMINATED,

        LEARNED
    }

    internal class NeuronalNetwork
    {
        private double mse = 0.0D;

        private NetworkState networkState = NetworkState.NEWLY_CREATED;
        private int inputs;
        private int outputs;
        private NeuralLayer inputLayer = null;
        private NeuralLayer outputLayer = null;
        private NeuralLayer[] hiddenLayers = null;

        private Boolean useBias = true;

        protected internal EventListenerList learningListeners = new EventListenerList();

        public NeuronalNetwork(int inputs, int outputs, NeuralLayer[] hiddenLayers, IFunctions f)
        {
            if ((inputs <= 0) || (outputs <= 0))
            {
                throw new IllegalArgumentException();
            }

            this.inputs = inputs;
            inputLayer = new NeuralLayer(inputs, f);
            this.outputs = outputs;
            outputLayer = new NeuralLayer(outputs, f);

            if ((hiddenLayers == null) || (hiddenLayers.Length == 0))
            {
                inputLayer.connectWith(outputLayer);
            }
            else
            {
                this.hiddenLayers = hiddenLayers;

                inputLayer.connectWith(hiddenLayers[0]);
                for (int i = 0; i < hiddenLayers.Length - 1; i++)
                {
                    hiddenLayers[i].connectWith(hiddenLayers[(i + 1)]);
                }
                hiddenLayers[(hiddenLayers.Length - 1)].connectWith(outputLayer);
            }
        }

        public void randomizeWeight(double min, double max)
        {
            Random rand = new Random();

            if (hiddenLayers != null)
            {
                for (int i = 0; i < hiddenLayers.Length; i++)
                {
                    foreach (Neuron n in hiddenLayers[i].getNeurons())
                    {
                        foreach (Synapse s in n.getInputSynapses())
                        {
                            s.setWeight(rand.NextDouble() * (max - min) + min);
                        }

                        if (useBias)
                        {
                            n.setBiasWeight(rand.NextDouble() * (max - min) + min);
                        }
                    }
                }

            }

            foreach (Neuron n in outputLayer.getNeurons())
            {
                foreach (Synapse s in n.getInputSynapses())
                {
                    s.setWeight(rand.NextDouble() * (max - min) + min);
                }

                if (useBias)
                {
                    n.setBiasWeight(rand.NextDouble() * (max - min) + min);
                }
            }
        }

        public double[] compute(double[] input)
        {
            double[] response = new double[outputs];

            Neuron[] inputNeurons = inputLayer.getNeurons();
            for (int i = 0; i < input.Length; i++)
            {
                inputNeurons[i].setOutput(input[i]);
            }

            if (hiddenLayers != null)
            {
                for (int i = 0; i < hiddenLayers.Length; i++)
                {
                    foreach (Neuron n in hiddenLayers[i].getNeurons())
                    {
                        n.compute();
                    }
                }
            }

            Neuron[] outputNeurons = outputLayer.getNeurons();

            for (int i = 0; i < response.Length; i++)
            {
                outputNeurons[i].compute();
                response[i] = outputNeurons[i].getOutput();
            }

            return response;
        }

        public void learn(double[][] inputs, double[][] desiredOutput, int epochs, double learningRate)
        {
            networkState = NetworkState.LEARNING_IN_PROGRESS;

            for (int i = 0; i < epochs; i++)
            {
                learn(inputs, desiredOutput, learningRate);

                if (i % 100 == 0)
                {
                    fireLearningEvent(new NeuralNetworkLearningEvent(this, i, getMse()));
                }

            }

            networkState = NetworkState.LEARNED;
        }

        public void learn(double[][] inputs, double[][] desiredOutput, double learningRate)
        {
            for (int j = 0; j < inputs.Length; j++)
            {
                double[] response = compute(inputs[j]);
                double globalError = 0.0D;
                Neuron outputNeuron;
                for (int k = 0; k < outputLayer.getNeuronsCount(); k++)
                {
                    outputNeuron = outputLayer.getNeurons()[k];
                    double error = desiredOutput[j][k] - response[k];
                    outputNeuron.setError(error);

                    globalError += error * error;

                    foreach (Synapse s in outputNeuron.getInputSynapses())
                    {
                        s.getNeuron().setError(s.getNeuron().getError() + s.getWeight() * outputNeuron.getError());
                    }

                }

                mse = Math.Sqrt(globalError / (desiredOutput.Length * outputs));

                if (hiddenLayers != null)
                {
                    for (int k = hiddenLayers.Length - 1; k > 0; k--)
                    {
                        //Neuron outputNeuron;
                        foreach (Neuron output_Neuron in hiddenLayers[k].getNeurons())
                        {
                            foreach (Synapse s in output_Neuron.getInputSynapses())
                            {
                                s.getNeuron().setError(s.getNeuron().getError() + s.getWeight() * output_Neuron.getError());
                            }
                        }
                    }
                }

                foreach (Neuron n in outputLayer.getNeurons())
                {
                    foreach (Synapse s in n.getInputSynapses())
                    {
                        s.setWeight(s.getWeight() + learningRate * n.getError() * n.getActivationFunction().computeDerivative(n.getOutput()) * s.getNeuron().getOutput());
                    }

                    if (useBias)
                    {
                        n.setBiasWeight(n.getBiasWeight() + learningRate * n.getError() * n.getActivationFunction().computeDerivative(n.getOutput()));
                    }

                    n.setError(0.0D);
                }

                if (hiddenLayers != null)
                    for (int k = 0; k < hiddenLayers.Length; k++)
                        foreach (Neuron n in hiddenLayers[k].getNeurons())
                        {
                            foreach (Synapse s in n.getInputSynapses())
                            {
                                s.setWeight(s.getWeight() + learningRate * n.getError() * n.getActivationFunction().computeDerivative(n.getOutput()) * s.getNeuron().getOutput());
                            }

                            if (useBias)
                            {
                                n.setBiasWeight(n.getBiasWeight() + learningRate * n.getError() * n.getActivationFunction().computeDerivative(n.getOutput()));
                            }

                            n.setError(0.0D);
                        }
            }
        }

        public Boolean isUseBias()
        {
            return useBias;
        }

        public void setUseBias(Boolean useBias)
        {
            this.useBias = useBias;
            foreach (NeuralLayer hiddenLayer in hiddenLayers)
            {
                foreach (Neuron n in hiddenLayer.getNeurons())
                {
                    n.setUseBias(useBias);
                }
            }
        }

        public void addLearningListener(NeuralNetworkEventListener l)
        {
            learningListeners.Add(l);
        }

        public void removeLearningListener(NeuralNetworkEventListener l)
        {
            learningListeners.Remove(l);
        }

        private void fireLearningEvent(NeuralNetworkLearningEvent e)
        {
            NeuralNetworkEventListener[] listeners = learningListeners.getListenerList();
            for (int i = 0; i < listeners.Length; i++)
            {
                if (listeners[i] is NeuralNetworkEventListener)
                {
                    listeners[i + 1].eventOccured(e);
                }
            }
        }

        public double getMse()
        {
            return mse;
        }

        public NetworkState getNetworkState()
        {
            return networkState;
        }

        public void setNetworkState(NetworkState networkState)
        {
            this.networkState = networkState;
        }
    }
}