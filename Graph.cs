using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary {

    public abstract class Graph<VertexT, EdgeT> : IGraph<VertexT, EdgeT> {

        //TODO: Implement static method for thread-safe instances
        //TODO: Implement static method for read-only instances

        private int size;
        private readonly bool isDirected;

        //==================== Concrete methods ====================

        public Graph(bool isDirected) {
            this.size = 0;
            this.isDirected = isDirected;
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
            RemoveNode(key);
        }

        public Edge<VertexT, EdgeT> Connect(VertexT obj1, VertexT obj2, EdgeT value) {
            ThrowIfVertexNotExists(obj1, obj2);

            Edge<VertexT, EdgeT> edge = AddConnection(obj1, obj2, value);
            if (!isDirected)
                AddConnection(obj2, obj1, value);

            return edge;
        }

        public EdgeT Disconnect(Edge<VertexT, EdgeT> edge) {
            VertexT obj1 = edge.StartPoint;
            VertexT obj2 = edge.EndPoint;

            ThrowIfEdgeNotExists(edge);

            if (AreAdjacent(obj1, obj2)) {
                EdgeT value = RemoveConnection(obj1, obj2);

                if (!isDirected)
                    RemoveConnection(obj2, obj1);

                return value;

            } else
                throw new Exception(String.Format("Couldn't disconnect nodes %s and %s", obj1, obj2));
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
        /// A method implementing the <see cref="Graph{vertexT,EdgeT}.RemoveVertex(vertexT)"/> method by removing the node from the graph.
        /// </summary>
        /// <param name="key">The node to be removed.</param>
        protected abstract void RemoveNode(VertexT key);


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
                    throw new Exception("The object " + v + " doens't exist in the graph");
        }

        private void ThrowIfEdgeNotExists(Edge<VertexT, EdgeT> edge) {
            ThrowIfVertexNotExists(edge.StartPoint, edge.EndPoint);

            if (!AreAdjacent(edge.StartPoint, edge.EndPoint))
                throw new Exception(String.Format("The nodes %s, %s aren't connected.", edge.StartPoint, edge.EndPoint));

        }

    }

}
