using System;

namespace GraphLibrary {
    /// <summary>
    /// An exception thrown to indicate a vertex doesn't exist in the specified graph.
    /// </summary>
    /// <typeparam name="VertexT">The type of the vertex</typeparam>
    public class VertexNonExistentException<VertexT> : ArgumentException {
        public VertexNonExistentException(VertexT vertex) : base("The object " + vertex + " doesn't exist in the graph") {}

        public VertexNonExistentException() : base() { }

        public VertexNonExistentException(String error) : base(error) {}

        public VertexNonExistentException(String error, Exception inner) : base(error, inner) {}

    }

}
