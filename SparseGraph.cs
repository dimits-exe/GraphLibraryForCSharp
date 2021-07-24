using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GraphLibrary {
    /// <summary>
    /// A graph implemented with adjacency lists. Uses less memory for sparse graphs.<br></br>
    /// Supports faster insertion and removal of vertices as well as faster incident edge lookup.
    /// </summary>
    /// 
    /// <remarks>
    /// The graph is implemented with a hash map (in the form of a Dictionary) that holds an array list for every vertex.
    /// The hash map gives O(1) access to every vertex and the List supports fast traversal through the graph's edges 
    /// at the expense of more costly on-average insertions and removals. Given that graph traversals are much more common 
    /// than insertions, the <see cref="List{T}"/> implementation is preferred over <see cref="LinkedList{T}"/>.
    /// 
    /// This graph implementation guarantees the following time complexities for each operation:
    /// <list type="bullet">
    /// <item><see cref="Graph{VertexT, EdgeT}.AddVertex(VertexT)"/> O(1)</item>
    /// <item><see cref="Graph{VertexT, EdgeT}.RemoveVertex(VertexT)"/> O(1)</item>
    /// <item><see cref="Graph{VertexT, EdgeT}.Connect(VertexT, VertexT, EdgeT)"/> O(1)</item>
    /// <item><see cref="Graph{VertexT, EdgeT}.Disconnect(Edge{VertexT, EdgeT})"/> O(1)</item>
    /// <item><see cref="Graph{VertexT, EdgeT}.AreAdjacent(VertexT, VertexT)"/> O(min(deg(v), deg(w))</item>
    /// <item><see cref="Graph{VertexT, EdgeT}.ReplaceVertex(VertexT, VertexT)"/> O(deg(v))</item>
    /// <item><see cref="Graph{VertexT, EdgeT}.ReplaceEdge(Edge{VertexT, EdgeT}, EdgeT)"/> O(deg(v))</item>
    /// <item><see cref="Graph{VertexT, EdgeT}.IncidentEdges(VertexT)"/> O(deg(v))</item>
    /// <item><see cref="Graph{VertexT, EdgeT}.Edges()"/> O(m)</item>
    /// <item><see cref="Graph{VertexT, EdgeT}.Vertices()"/> O(n)</item>
    /// </list>
    /// Where v, w = the arguments' vertices, n = the number of all vertices and m = the number of all edges.
    /// 
    /// </remarks>
    /// <typeparam name="VertexT">The type of objects stored in the graph's vertices.</typeparam>
    /// <typeparam name="EdgeT">The type of objects stored in the graph's edges.</typeparam>
    public class SparseGraph<VertexT, EdgeT> : Graph<VertexT, EdgeT> {

        private Dictionary<VertexT, List<Edge<VertexT, EdgeT>>> vertices;

        public SparseGraph(bool isDirected): base(isDirected) {
            vertices = new Dictionary<VertexT, List<Edge<VertexT, EdgeT>>>();
        }

        protected override Edge<VertexT, EdgeT> AddConnection(VertexT obj1, VertexT obj2, EdgeT value) {
            Edge<VertexT, EdgeT> newEdge = new Edge<VertexT, EdgeT>(obj1, obj2, value);
            vertices[obj1].Add(newEdge);
            return newEdge;
        }

        protected override void AddNode(VertexT key) {
            vertices.Add(key, new List<Edge<VertexT, EdgeT>>());
        }

        protected override bool EdgeExists(VertexT obj1, VertexT obj2) {
            //pick the vertex with the least amount of edges to accelerate search
            VertexT leastLargeVertex = vertices[obj1].Count < vertices[obj2].Count ? obj1 : obj2; 

            foreach (Edge<VertexT, EdgeT> edge in vertices[obj1])
                if (edge.EndPoint.Equals(obj2))
                    return true;

            return false;
        }

        protected override bool NodeExists(VertexT key) {
            return vertices.ContainsKey(key);
        }

        protected override EdgeT RemoveConnection(VertexT obj1, VertexT obj2) {
            EdgeT wantedEdgeValue = default;
            bool found = false; //TODO: REMOVE DEBUG

            foreach (Edge<VertexT, EdgeT> edge in vertices[obj1])
                if (edge.EndPoint.Equals(obj2)) {
                    wantedEdgeValue = edge.Value;
                    vertices[obj1].Remove(edge);
                    found = true;
                }
            Debug.Assert(found);

            return wantedEdgeValue;
        }

        protected override void RemoveNode(VertexT key) {
            vertices.Remove(key);
        }

        protected override void ReplaceVertexValue(VertexT oldValue, VertexT newValue) {
            vertices.Add(newValue, vertices[oldValue]);
            vertices.Remove(oldValue);
        }

        protected override void ReplaceEdgeValue(Edge<VertexT, EdgeT> edge, EdgeT newValue) {
            Edge<VertexT, EdgeT> target = vertices[edge.StartPoint].Find(x => x.EndPoint.Equals(edge.EndPoint));
            target.Value = newValue;
        }

        protected override List<Edge<VertexT, EdgeT>> GetIncidentEdges(VertexT vertex) { 
            return vertices[vertex]; 
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
