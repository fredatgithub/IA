using System.Collections.Generic;

namespace Rdn_Dev.Events
{
    internal class EventListenerList : List<NeuralNetworkEventListener>
    {
        internal NeuralNetworkEventListener[] getListenerList()
        {
            return ToArray();
        }
    }
}