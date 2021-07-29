using System;

namespace GraphLibrary {
    /// <summary>
    /// An exception thrown to indicate a vertex doesn't exist in the specified graph.
    /// </summary>
    public class InvalidVertexException : ArgumentException {

        public InvalidVertexException() : base() { }

        public InvalidVertexException(String error) : base(error) {}

        public InvalidVertexException(String error, Exception inner) : base(error, inner) {}

    }

}
