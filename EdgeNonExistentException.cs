using System;

namespace GraphLibrary {
    /// <summary>
    /// An exception thrown to indicate an edge between two vertices doesn't exist in the specified graph.
    /// </summary>
    /// <typeparam name="VertexT">The parameter of the vetrices in the graph.</typeparam>
    /// <typeparam name="EdgeT">The parameter of the edge's values in the graph.</typeparam>
    public class EdgeNonExistentException<VertexT, EdgeT> : ArgumentException {

        public EdgeNonExistentException(Edge<VertexT,EdgeT> edge) : base(String.Format("The nodes %s, %s aren't connected.", edge.StartPoint, edge.EndPoint)) {}

        public EdgeNonExistentException() : base() {}

        public EdgeNonExistentException(String error) : base(error) {}

        public EdgeNonExistentException(String error, Exception inner) : base(error, inner) {}

    }
}
