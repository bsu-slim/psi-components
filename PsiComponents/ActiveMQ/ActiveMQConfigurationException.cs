using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveMQComponent
{
    [Serializable]
    class ActiveMQConfigurationException : Exception
    {
        public ActiveMQConfigurationException()
        {
        }

        public ActiveMQConfigurationException(string message)
            : base(message)
        {
        }

        public ActiveMQConfigurationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}