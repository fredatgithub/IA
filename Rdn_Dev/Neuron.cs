using System;
using System.Collections.Generic;
using Rdn_Dev.LibFunction;
//using Rdn_Dev.LibFunction.Linear;

namespace Rdn_Dev
{
    public class Neuron
    {
        private double output;
        private double error = 0.0D;

        private Boolean useBias = true;
        private double biasWeight = 0.0D;

        private List<Synapse> inputSynapses = new List<Synapse>();
        private IFunctions activationFunction;

        public Neuron()
        {
            setActivationFunction(new Linear());
        }

        public Neuron(IFunctions f)
        {
            setActivationFunction(f);
        }

        public void addSynapse(Synapse s)
        {
            if (s != null)
                inputSynapses.Add(s);
        }

        public void setActivationFunction(IFunctions activationFunction)
        {
            this.activationFunction = activationFunction;
        }

        public List<Synapse> getInputSynapses()
        {
            return inputSynapses;
        }

        public double getOutput()
        {
            return output;
        }

        public void setOutput(double output)
        {
            this.output = output;
        }

        public void compute()
        {
            output = 0.0D;
            foreach (Synapse s in inputSynapses)
            {
                output += s.getNeuron().getOutput() * s.getWeight();
            }

            if (useBias)
            {
                output += biasWeight * 1.0D;
            }

            output = activationFunction.Compute(output);
        }

        public double getError()
        {
            return error;
        }

        public void setError(double error)
        {
            this.error = error;
        }

        public IFunctions getActivationFunction()
        {
            return activationFunction;
        }

        public double getBiasWeight()
        {
            return biasWeight;
        }

        public void setBiasWeight(double biasWeight)
        {
            this.biasWeight = biasWeight;
        }

        public Boolean isUseBias()
        {
            return useBias;
        }

        public void setUseBias(Boolean useBias)
        {
            this.useBias = useBias;
        }
    }
}