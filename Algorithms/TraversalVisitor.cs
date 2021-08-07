using System.Collections.Generic;
namespace GraphLibrary {

    /// <summary>
    /// A superclass defining visit operations and default methods for events occuring 
    /// during the graph's traversal.<br></br>
    /// Subclass the class to define a custom algorithm to be used during the traversal.<br></br>
    /// By default this class only keeps track of visited / unvisited vertices.
    /// </summary>
    /// <typeparam name="VertexT">The type of the graph's vertices.</typeparam>
    /// <typeparam name="EdgeT">The type of the graph's edge values.</typeparam>
    /// <typeparam name="ReturnV">The return type of the traversal.</typeparam>
    public class TraversalVisitor<VertexT, EdgeT, ReturnV> {
        private Dictionary<VertexT, Status> visitedVertices;
        private Dictionary<Edge<VertexT, EdgeT>, Status> visitedEdges;
        protected IGraph<VertexT, EdgeT> graph;

        internal VertexT defaultStart;

        private enum Status {VISITED, UNVISITED}

        /// <summary>
        /// Constructs a new visitor object.
        /// </summary>
        /// <param name="graph">The graph which will be visited.</param>
        /// <exception cref="InvalidGraphException">If the graph is empty.</exception>
        public TraversalVisitor(IGraph<VertexT, EdgeT> graph) {
            this.graph = Graph<VertexT, EdgeT>.AsReadOnly(graph);

            if (graph.IsEmpty())
                throw new InvalidGraphException("Can't traverse an empty graph");

            visitedVertices = new Dictionary<VertexT, Status>();
            visitedEdges = new Dictionary<Edge<VertexT, EdgeT>, Status>();

            var vertices = graph.Vertices;

            defaultStart = vertices[0];

            foreach (VertexT v in vertices)
                visitedVertices[v] = Status.UNVISITED;

            foreach (var edge in graph.Edges)
                visitedEdges[edge] = Status.UNVISITED;
        }

        /// <summary>
        /// Marks the vertex as visited.
        /// </summary>
        public void Visit(VertexT vertex) {
            ThrowIfNotExists(vertex);
            visitedVertices[vertex] = Status.VISITED;
        }

        /// <summary>
        /// Marks the edge as visited.
        /// </summary>
        public void Visit(Edge<VertexT, EdgeT> edge) {
            ThrowIfNotExists(edge);
            visitedEdges[edge] = Status.VISITED;
        }

        /// <summary>
        /// Marks the vertex as unvisited.
        /// </summary>
        public void UnVisit(VertexT vertex) {
            ThrowIfNotExists(vertex);
            visitedVertices[vertex] = Status.UNVISITED;
        }

        /// <summary>
        /// Marks the edge as unvisited.
        /// </summary>
        public void UnVisit(Edge<VertexT, EdgeT> edge) {
            ThrowIfNotExists(edge);
            visitedEdges[edge] = Status.UNVISITED;
        }

        /// <summary>
        /// Returns true if the vertex has been visited during a traversal.
        /// </summary>
        public bool IsVisited(VertexT vertex) {
            ThrowIfNotExists(vertex);
            return visitedVertices[vertex] == Status.VISITED;
        }

        /// <summary>
        /// Returns true if the vertex has been visited during a traversal.
        /// </summary>
        public bool IsVisited(Edge<VertexT, EdgeT> edge) {
            ThrowIfNotExists(edge);
            return visitedEdges[edge] == Status.VISITED;
        }

       /// <summary>
       /// Called at the start of the visit on a vertex.
       /// </summary>
       /// <param name="v">The vertex visited during the traversal.</param>
        public virtual void StartVisit(VertexT v) {}

        /// <summary>
        /// Called at the end of the exploration of a vertex.
        /// </summary>
        /// <param name="v">The vertex visited during the traversal.</param>
        public virtual void FinishVisit(VertexT v) {}

        /// <summary>
        /// Called when a previously undiscovered vertex is found.
        /// </summary>
        /// <param name="edge">The edge whose start is the previous vertex and
        /// its end the newly discovered vertex.</param>
        public virtual void TraverseDiscovery(Edge<VertexT, EdgeT> edge) {}

        /// <summary>
        /// Called when a back edge is traversed.
        /// </summary>
        /// <param name="edge">The edge whose start is the previous vertex and
        /// its end the newly discovered vertex.</param>
        public virtual void TraverseBack(Edge<VertexT, EdgeT> edge) {}

        /// <summary>
        /// Called to determine whether to end the traversal prematurely.
        /// </summary>
        public virtual bool IsDone() { return false; }

        /// <summary>
        /// Returns the value of the traversal method.
        /// </summary>
        public virtual ReturnV Result() { return default(ReturnV); }

        private void ThrowIfNotExists(VertexT v) {
            if (!visitedVertices.ContainsKey(v))
                throw new InvalidVertexException("The vertex " + v + " doesn't exist in the graph.");
        }

        private void ThrowIfNotExists(Edge<VertexT, EdgeT> edge) {
            if (!visitedEdges.ContainsKey(edge))
                throw new InvalidEdgeException("The edge " + edge + " doesn't exist in the graph.");
        }

    }
}
