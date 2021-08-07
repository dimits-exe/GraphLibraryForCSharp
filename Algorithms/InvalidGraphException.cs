using System;

namespace GraphLibrary {

    /// <summary>
    /// Thrown when the graph's state is invalid for the execution of a method.
    /// </summary>
    public class InvalidGraphException : InvalidOperationException {
        public InvalidGraphException() : base() { }

        public InvalidGraphException(String error) : base(error) { }

        public InvalidGraphException(String error, Exception inner) : base(error, inner) { }
    }
}
