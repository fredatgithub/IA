namespace Rdn_Dev.Events
{
    public class NeuralNetworkLearningEvent
    {
        private int epochs;
        private double error;

        public NeuralNetworkLearningEvent(object source, int epochs, double error)
        //  : base(source)
        {
            this.epochs = epochs;
            this.error = error;
        }

        public virtual int Epochs => epochs;

        public virtual double Error => error;
    }
}