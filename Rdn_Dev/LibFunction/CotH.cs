using System;

namespace Rdn_Dev.LibFunction
{
    internal class CotH : IFunctions
    {
        double IFunctions.Compute(double paramDouble)
        {
            return (1.0D / Math.Tanh(paramDouble));
        }

        double IFunctions.computeDerivative(double paramDouble)
        {
            return -Math.Pow(cosech(paramDouble), 2);
        }

        private double cosech(double paraDouble)
        {
            return (1.0D / Math.Sinh(paraDouble));
        }
    }
}