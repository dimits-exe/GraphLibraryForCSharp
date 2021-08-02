using System.Collections.Generic;

namespace GraphLibrary {
    /// <summary>
    /// A graph implemented with adjacency lists. Uses less memory for sparse graphs.<br></br>
    /// Supports faster insertion and removal of vertices as well as faster incident edge lookup.
    /// </summary>
    /// 
    /// <remarks>
    /// The graph is implemented with a hash map (in the form of a <see cref="Dictionary{TKey, TValue}"/>) that holds a <see cref="LinkedList{T}"/> for every vertex.
    /// 
    /// This graph implementation guarantees the following time complexities for each operation:
    /// <list type="bullet">
    /// <item><see cref="Graph{VertexT, EdgeT}.AddVertex(VertexT)"/> O(1)</item>
    /// <item><see cref="Graph{VertexT, EdgeT}.RemoveVertex(VertexT)"/> O(m)</item>
    /// <item><see cref="Graph{VertexT, EdgeT}.Connect(VertexT, VertexT, EdgeT)"/> O(deg(w))</item>
    /// <item><see cref="Graph{VertexT, EdgeT}.Disconnect(Edge{VertexT, EdgeT})"/> O(deg(w))</item>
    /// <item><see cref="Graph{VertexT, EdgeT}.AreAdjacent(VertexT, VertexT)"/> O(min(deg(v), deg(w))</item>
    /// <item><see cref="Graph{VertexT, EdgeT}.ReplaceVertex(VertexT, VertexT)"/> O(deg(v))</item>
    /// <item><see cref="Graph{VertexT, EdgeT}.ReplaceEdge(Edge{VertexT, EdgeT}, EdgeT)"/> O(deg(v))</item>
    /// <item><see cref="Graph{VertexT, EdgeT}.IncidentEdges(VertexT)"/> O(deg(v))</item>
    /// <item><see cref="Graph{VertexT, EdgeT}.Edges()"/> O(m)</item>
    /// <item><see cref="Graph{VertexT, EdgeT}.Vertices()"/> O(n)</item>
    /// </list>
    /// Where v, w = the arguments' vertices, n = the number of all vertices, m = the number of all edges
    /// and deg(o) the degree of vertex 'o'.
    /// 
    /// </remarks>
    /// <typeparam name="VertexT">The type of objects stored in the graph's vertices.</typeparam>
    /// <typeparam name="EdgeT">The type of objects stored in the graph's edges.</typeparam>
    public class SparseGraph<VertexT, EdgeT> : Graph<VertexT, EdgeT> {

        private Dictionary<VertexT, LinkedList<Edge<VertexT, EdgeT>>> vertices= new Dictionary<VertexT, LinkedList<Edge<VertexT, EdgeT>>>();

        public SparseGraph(bool isDirected): base(isDirected) {}

        public SparseGraph(GraphData<VertexT, EdgeT> gd): base(gd) {}

        public SparseGraph(IGraph<VertexT, EdgeT> g): base(g) {}

        protected override Edge<VertexT, EdgeT> AddConnection(VertexT obj1, VertexT obj2, EdgeT value) {
            Edge<VertexT, EdgeT> newEdge = new Edge<VertexT, EdgeT>(obj1, obj2, value);
            vertices[obj1].AddLast(newEdge);
            return newEdge;
        }

        protected override void AddNode(VertexT key) {
            vertices.Add(key, new LinkedList<Edge<VertexT, EdgeT>>());
        }

        protected override bool EdgeExists(VertexT obj1, VertexT obj2) {
             //pick the vertex with the least amount of edges to accelerate search
            VertexT leastVertices;
            VertexT mostVertices;

            if (vertices[obj1].Count < vertices[obj2].Count) {
                leastVertices = obj1;
                mostVertices = obj2;
            }
            else {
                leastVertices = obj2;
                mostVertices = obj1;
            }

            foreach (Edge<VertexT, EdgeT> edge in vertices[leastVertices])
                if (edge.EndPoint.Equals(mostVertices))
                    return true;

            return false;
        }

        protected override bool NodeExists(VertexT key) {
            return vertices.ContainsKey(key);
        }

        protected override EdgeT RemoveConnection(VertexT obj1, VertexT obj2) {
            EdgeT wantedEdgeValue = default;

            foreach (Edge<VertexT, EdgeT> edge in vertices[obj1])
                if (edge.EndPoint.Equals(obj2)) {
                    wantedEdgeValue = edge.Value;
                    vertices[obj1].Remove(edge);
                    found = true;
                }

            return wantedEdgeValue;
        }

        protected override void RemoveNodeAndConnections(VertexT key) {
            vertices.Remove(key);

            //remove all edges pointing to the vertex
            foreach (VertexT vertex in vertices.Keys) {
                LinkedList<Edge<VertexT, EdgeT>> incidentEdges = vertices[vertex];
                LinkedList<Edge<VertexT, EdgeT>> removedEdges = new LinkedList<Edge<VertexT, EdgeT>>(); 

                foreach (var edge in incidentEdges) // keep note of all edges to be removed
                    if (edge.EndPoint.Equals(key))
                        removedEdges.AddLast(edge);

                foreach (var edge in removedEdges) // remove them
                    incidentEdges.Remove(edge);
            }
        }

        protected override void ReplaceVertexValue(VertexT oldValue, VertexT newValue) {
            vertices.Add(newValue, vertices[oldValue]);
            vertices.Remove(oldValue);
        }

        protected override void ReplaceEdgeValue(Edge<VertexT, EdgeT> edge, EdgeT newValue) {
            //find edge
            Edge<VertexT, EdgeT> target = default;
            foreach (Edge<VertexT, EdgeT> edge2 in vertices[edge.StartPoint])
                if (edge2.EndPoint.Equals(edge.EndPoint))
                    target = edge2;
            //modify edge
            target.Value = newValue;
        }

        protected override List<Edge<VertexT, EdgeT>> GetIncidentEdges(VertexT vertex) { 
            return new List<Edge<VertexT, EdgeT>>(vertices[vertex]); //make a copy of the internal list
        }

        protected override List<Edge<VertexT, EdgeT>> GetEdges() {
            List<Edge<VertexT, EdgeT>> allEdges = new List<Edge<VertexT, EdgeT>>(); //probably faster than linkedlist + cast

            foreach (VertexT key in vertices.Keys)
                foreach (Edge<VertexT, EdgeT> edge in vertices[key])
                    allEdges.Add(edge);

            return allEdges; 
        }

        protected override List<VertexT> GetVertices() {
            return new List<VertexT>(vertices.Keys);
        }
    }

}
