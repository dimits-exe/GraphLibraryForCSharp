using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace GraphLibrary {

    public abstract class Graph<VertexT, EdgeT> : IGraph<VertexT, EdgeT> {

        /// <summary>
        /// Creates a read-only wrapper for the graph. Users using this wrapper will have access to all the data within, but will 
        /// be unable to mutate them.<br></br>
        /// The graph can still be modified by using the underlying graph reference. To prevent this, create a copy of the graph 
        /// to pass as an argument to the method.
        /// </summary>
        public static ReadOnlyGraph<VertexT, EdgeT> AsReadOnly(Graph<VertexT, EdgeT> graph) {
            return new ReadOnlyGraph<VertexT, EdgeT>(graph);
        }

        //TODO: Implement static method for thread-safe instances

        private int size;
        private readonly bool isDirected;

        //==================== Concrete methods ====================

        public Graph(bool isDirected) {
            this.size = 0;
            this.isDirected = isDirected;
        }

        /// <summary>
        /// Constructs a graph identical to the one provided.
        /// </summary>
        /// <param name="g">A graph of any type.</param>
        public Graph(Graph<VertexT,EdgeT> g) {
            this.size = g.Size;
            this.isDirected = g.IsDirected;

            foreach (VertexT v in g.GetVertices())
                AddNode(v);

            foreach (Edge<VertexT, EdgeT> edge in g.GetEdges())
                AddConnection(edge.StartPoint, edge.StartPoint, edge.Value);
        }

        public int Size {
            get {
                return size;
            }
        }

        public bool IsDirected {
            get {
                return isDirected;
            }
        }

        public bool IsEmpty() {
            return size == 0;
        }

        //==================== Wrapper methods ====================

        public void AddVertex(VertexT key) {
            size++;
            AddNode(key);
        }

        public void RemoveVertex(VertexT key) {
            ThrowIfVertexNotExists(key);
            size--;
            RemoveNodeAndConnections(key);
        }

        public Edge<VertexT, EdgeT> Connect(VertexT obj1, VertexT obj2, EdgeT value) {
            ThrowIfVertexNotExists(obj1, obj2);

            Edge<VertexT, EdgeT> edge = AddConnection(obj1, obj2, value);
            if (!isDirected && (!obj1.Equals(obj2))) //if connection with itself, connect only once 
                AddConnection(obj2, obj1, value);

            return edge;
        }

        public EdgeT Disconnect(Edge<VertexT, EdgeT> edge) {
            VertexT obj1 = edge.StartPoint;
            VertexT obj2 = edge.EndPoint;
            ThrowIfEdgeNotExists(edge);

            EdgeT value = RemoveConnection(obj1, obj2);

            if (!isDirected && (!obj1.Equals(obj2))) //if connection with itself, disconnect only once
                RemoveConnection(obj2, obj1);

            return value;
        }

        public bool AreAdjacent(VertexT obj1, VertexT obj2) {
            ThrowIfVertexNotExists(obj1, obj2);
            return EdgeExists(obj1, obj2);
        }

        public void ReplaceVertex(VertexT oldValue, VertexT newValue) {
            ThrowIfVertexNotExists(oldValue);
            ReplaceVertexValue(oldValue, newValue);
        }

        public void ReplaceEdge(Edge<VertexT, EdgeT> edge, EdgeT newValue) {
            ThrowIfEdgeNotExists(edge);
            ReplaceEdgeValue(edge, newValue);
        }

        public ReadOnlyCollection<Edge<VertexT, EdgeT>> IncidentEdges(VertexT key) {
            ThrowIfVertexNotExists(key);
            return GetIncidentEdges(key).AsReadOnly();
        }

        public ReadOnlyCollection<VertexT> Vertices() {
            return GetVertices().AsReadOnly();
        }

        public ReadOnlyCollection<Edge<VertexT, EdgeT>> Edges() {
            return GetEdges().AsReadOnly();
        }

        //==================== Implementation methods ====================

        protected abstract bool EdgeExists(VertexT obj1, VertexT obj2);

        /// <summary>
        /// A method implementing a check on whether or not the specified node exists in the graph.<br></br>
        /// </summary>
        /// <param name="key">The node</param>
        /// <returns>True if the node exists, False otherwise.</returns>
        protected abstract bool NodeExists(VertexT key);

        /// <summary>
        /// A method implementing the <see cref="Graph{vertexT,EdgeT}.RemoveVertex(vertexT)"/> method by removing the node from the graph
        /// and deleting all edges pointing to it.
        /// </summary>
        /// <param name="key">The node to be removed.</param>
        protected abstract void RemoveNodeAndConnections(VertexT key);


        /// <summary>
        /// A method implementing the <see cref="Graph{VertexT,EdgeT}.Disconnect(Edge{VertexT, EdgeT})"/> method 
        /// by removing the edge between the 2 nodes from the graph.
        /// </summary>
        /// <returns>The value of the specified edge.</returns>
        protected abstract EdgeT RemoveConnection(VertexT obj1, VertexT obj2);

        /// <summary>
        /// A method implementing the <see cref="Graph{VertexT,EdgeT}.AddVertex(VertexT)"/> method by adding a node with the specified object in the graph.
        /// </summary>
        protected abstract void AddNode(VertexT key);

        /// <summary>
        /// A method implementing the <see cref="Graph{VertexT,EdgeT}.Connect(VertexT, VertexT, EdgeT)"/> method 
        /// by adding an edge between the specified nodes<br></br>
        /// and weight in the graph.
        /// </summary>
        protected abstract Edge<VertexT,EdgeT> AddConnection(VertexT obj1, VertexT obj2, EdgeT value);

        /// <summary>
        /// A method implementing the <see cref="Graph{VertexT,EdgeT}.ReplaceVertex(VertexT, VertexT)"/> method 
        /// by replacing the value of the given vertex.<br></br>
        /// and weight in the graph.
        /// </summary>
        protected abstract void ReplaceVertexValue(VertexT oldValue, VertexT newValue);

        /// <summary>
        /// A method implementing the <see cref="Graph{VertexT,EdgeT}.ReplaceEdge(Edge{VertexT, EdgeT}, EdgeT)"/> method 
        /// by replacing the value of the given edge.<br></br>
        /// and weight in the graph.
        /// </summary>
        protected abstract void ReplaceEdgeValue(Edge<VertexT, EdgeT> edge, EdgeT newValue);

        /// <summary>
        /// A method implementing the <see cref="Graph{VertexT,EdgeT}.IncidentEdges(VertexT)"/> method.
        /// </summary>
        /// <returns>A List of all edges incident on the vertex.</returns>
        /// <param name="vertex">The vertex whose neighbours were requested.</param>
        protected abstract List<Edge<VertexT, EdgeT>> GetIncidentEdges(VertexT vertex);

        /// <summary>
        /// A method implementing the <see cref="Graph{VertexT,EdgeT}.Edges()"/> method.
        /// </summary>
        /// <returns>A List of all edges in the graph.</returns>
        protected abstract List<Edge<VertexT, EdgeT>> GetEdges();

        /// <summary>
        /// A method implementing the <see cref="Graph{VertexT,EdgeT}.Vertices()"/> method.
        /// </summary>
        /// <returns>A List of all vertices in the graph.</returns>
        protected abstract List<VertexT> GetVertices();

        //==================== Private methods ====================

        /// <summary>
        /// Throws an exception if any of the nodes don't exist.
        /// </summary>
        private void ThrowIfVertexNotExists(params VertexT[] vertices) {
            foreach (VertexT v in vertices)
                if (!NodeExists(v))
                    throw new VertexNonExistentException<VertexT>(v);
        }

        /// <summary>
        /// Throws an exception if any of the nodes or the edge itself doesn't exist.
        /// </summary>
        private void ThrowIfEdgeNotExists(Edge<VertexT, EdgeT> edge) {
            try {
                ThrowIfVertexNotExists(edge.StartPoint, edge.EndPoint);
            } catch(VertexNonExistentException<VertexT> exc) { //rethrow as EdgeException
                throw new EdgeNonExistentException<VertexT, EdgeT>("The edge " + edge + " doesn't exist: ", exc);
            }

            if (!AreAdjacent(edge.StartPoint, edge.EndPoint))
                throw new EdgeNonExistentException<VertexT,EdgeT>(edge);
        }

    }

}
