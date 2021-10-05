using System;
using System.Collections.ObjectModel;

namespace GraphLibrary {

    /// <summary>
    /// A read-only wrapper for the graph. Users using this wrapper will have access to all the data within, but will 
    /// be unable to mutate them.<br></br>
    /// The graph can still be modified by using the underlying graph reference. To prevent this, create a copy of the graph 
    /// to pass as an argument to the constructor.
    /// </summary>
    /// <typeparam name="VertexT">The type of objects stored in the graph's vertices.</typeparam>
    /// <typeparam name="EdgeT">The type of objects stored in the graph's edges.</typeparam>
    internal class ReadOnlyGraph<VertexT, EdgeT> : IGraph<VertexT, EdgeT> {

        private readonly IGraph<VertexT, EdgeT> actualGraph;

        private static readonly String errorMessage = "This operation violates the READ-ONLY policy of this object.";

        /// <summary>
        /// Creates a new read-only wrapper for the graph. O(1) operation.
        /// </summary>
        /// <param name="graph">The graph to be used in the wrapper.</param>
        internal ReadOnlyGraph(IGraph<VertexT,EdgeT> graph) {
            actualGraph = graph;
        }

        public int Size {
            get {
                return actualGraph.Size;
            }
        }

        public bool IsDirected {
            get {
                return actualGraph.IsDirected;
            }
        }

        ReadOnlyCollection<VertexT> IGraph<VertexT, EdgeT>.Vertices => actualGraph.Vertices;

        ReadOnlyCollection<Edge<VertexT, EdgeT>> IGraph<VertexT, EdgeT>.Edges => actualGraph.Edges;

        public void AddVertex(VertexT key) {
            throw new InvalidOperationException(errorMessage);
        }

        public Edge<VertexT, EdgeT> Connect(VertexT obj1, VertexT obj2, EdgeT value) {
            throw new InvalidOperationException(errorMessage);
        }

        public EdgeT Disconnect(Edge<VertexT, EdgeT> edge) {
            throw new InvalidOperationException(errorMessage);
        }

        public void RemoveVertex(VertexT key) {
            throw new InvalidOperationException(errorMessage);
        }

        public void ReplaceEdge(Edge<VertexT, EdgeT> edge, EdgeT newValue) {
            throw new InvalidOperationException(errorMessage);
        }

        public void ReplaceVertex(VertexT oldValue, VertexT newValue) {
            throw new InvalidOperationException(errorMessage);
        }

        public ReadOnlyCollection<Edge<VertexT, EdgeT>> IncidentEdges(VertexT key) {
            return actualGraph.IncidentEdges(key);
        }

        public bool IsEmpty() {
            return actualGraph.IsEmpty();
        }

        public bool AreAdjacent(VertexT obj1, VertexT obj2) {
            return actualGraph.AreAdjacent(obj1, obj2);
        }

        public Edge<VertexT, EdgeT> GetEdge(VertexT obj1, VertexT obj2) {
            return actualGraph.GetEdge(obj1, obj2);
        }
    }
}
