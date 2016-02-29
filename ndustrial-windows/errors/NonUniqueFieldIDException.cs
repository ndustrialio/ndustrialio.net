using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.ndustrialio.api.errors
{

    [Serializable()]
    public class NonUniqueFieldIDException : System.Exception
    {
        public NonUniqueFieldIDException() : base() { }
        public NonUniqueFieldIDException(string message) : base(message) { }
        public NonUniqueFieldIDException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an 
        // exception propagates from a remoting server to the client.  
        protected NonUniqueFieldIDException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }

    }
}
