namespace Rdn_Dev.LibFunction
{
    internal class Linear:IFunctions  
    {
        public double Alpha { get; set; }

        public Linear()
        {
            Alpha = 1.0D;
        }

        public Linear(double Value)
        {
            Alpha = Value;
        }
        
        double IFunctions.Compute(double paramDouble)
        {
            return Alpha * paramDouble;
        }

        double IFunctions.computeDerivative(double paramDouble)
        {
            return Alpha;
        }
    }
}
