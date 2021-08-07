using System;

namespace GraphLibrary {
    /// <summary>
    /// An exception thrown to indicate an edge between two vertices doesn't exist in the specified graph.
    /// </summary>
    public class InvalidEdgeException : ArgumentException {

        public InvalidEdgeException() : base() {}

        public InvalidEdgeException(String error) : base(error) {}

        public InvalidEdgeException(String error, Exception inner) : base(error, inner) {}

    }
}
