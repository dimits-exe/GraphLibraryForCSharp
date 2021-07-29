using System;
using System.Collections.Generic;

namespace GraphLibrary{
    
    /// <summary>
    /// A graph implemented with an adjacency matrix. Uses less memory for dense graphs.<br></br>
    /// Guarantees instant connection checks and connect/disconnect operations at the cost of slower vertex removals and additions.
    /// </summary>
    /// 
    /// <remarks>
    /// The graph is not actually implemented with a pure 2D array but with nested <see cref="List{T}"/>s. This guarantees improved memory management
    /// by avoiding unnecessary resizes, but might be inefficient for specialized uses like intensive matrix operations.
    /// 
    /// This graph implementation guarantees the following time complexities for each operation:
    /// <list type="bullet">
    /// <item><see cref="Graph{VertexT, EdgeT}.AddVertex(VertexT)"/> O(n)</item>
    /// <item><see cref="Graph{VertexT, EdgeT}.RemoveVertex(VertexT)"/> O(n^2)</item>
    /// <item><see cref="Graph{VertexT, EdgeT}.Connect(VertexT, VertexT, EdgeT)"/> O(1)</item>
    /// <item><see cref="Graph{VertexT, EdgeT}.Disconnect(Edge{VertexT, EdgeT})"/> O(1)</item>
    /// <item><see cref="Graph{VertexT, EdgeT}.AreAdjacent(VertexT, VertexT)"/> O(1)</item>
    /// <item><see cref="Graph{VertexT, EdgeT}.ReplaceVertex(VertexT, VertexT)"/> O(1)</item>
    /// <item><see cref="Graph{VertexT, EdgeT}.ReplaceEdge(Edge{VertexT, EdgeT}, EdgeT)"/> O(1)</item>
    /// <item><see cref="Graph{VertexT, EdgeT}.IncidentEdges(VertexT)"/> O(n)</item>
    /// <item><see cref="Graph{VertexT, EdgeT}.Edges()"/> O(n^2)</item>
    /// <item><see cref="Graph{VertexT, EdgeT}.Vertices()"/> O(n)</item>
    /// </list>
    /// Where v, w = the arguments' vertices, n = the number of all vertices and m = the number of all edges.
    /// 
    /// </remarks>
    /// <typeparam name="VertexT">The type of objects stored in the graph's vertices.</typeparam>
    /// <typeparam name="EdgeT">The type of objects stored in the graph's edges.</typeparam>
    public class DenseGraph<VertexT, EdgeT> : Graph<VertexT, EdgeT> {

        private List<List<EdgeT>> matrix;               //2D array
        private Dictionary<VertexT, int> vertexDict;    // convert objects to indices
        private List<VertexT> vertexList;               // convert indices to objects

        public DenseGraph(bool isDirected) : base(isDirected) {
            matrix = new List<List<EdgeT>>();
            vertexDict = new Dictionary<VertexT, int>();
            vertexList = new List<VertexT>();
        }

        protected override Edge<VertexT, EdgeT> AddConnection(VertexT obj1, VertexT obj2, EdgeT value) {
            if (!isValid(value))
                throw new ArgumentException("Can't use default parameters for values in this graph!");

            matrix[VertToIndex(obj1)][VertToIndex(obj2)] = value;
            return new Edge<VertexT, EdgeT>(obj1, obj2, value);
        }

        protected override void AddNode(VertexT key) {
            vertexDict[key] = Size-1;                 //update index list
            vertexList.Add(key);                        //update name list

            List<EdgeT> newList = new List<EdgeT>();    //build row
            for (int i = 0; i <= Size-1; i++) 
                newList.Add(default);

            foreach (var ls in matrix)
                ls.Add(default);

            matrix.Add(newList);                        //add row
        }

        protected override bool EdgeExists(VertexT obj1, VertexT obj2) {
            return isValid(matrix[VertToIndex(obj1)][VertToIndex(obj2)]);
        }

        protected override List<Edge<VertexT, EdgeT>> GetEdges() {
            List<Edge<VertexT, EdgeT>> edges = new List<Edge<VertexT, EdgeT>>();

            for(int i=0; i<=Size - 1; i++)
                for(int j=0; j<= Size - 1; j++) {
                    EdgeT value = matrix[i][j];
                    if (isValid(value))
                        edges.Add(new Edge<VertexT, EdgeT>(IndexToVert(i), IndexToVert(j), value));
                    else
                        throw new ArgumentException("Can't use default parameters for values in this graph!");
                }

            return edges;
        }

        protected override List<Edge<VertexT, EdgeT>> GetIncidentEdges(VertexT vertex) {
            List<Edge<VertexT, EdgeT>> edges = new List<Edge<VertexT, EdgeT>>();

            for(int j=0; j<=Size - 1; j++) {
                EdgeT value = matrix[VertToIndex(vertex)][j];
                if (isValid(value))
                    edges.Add(new Edge<VertexT, EdgeT>(vertex, IndexToVert(j), value));
            }

            return edges;
        }

        protected override List<VertexT> GetVertices() {
            return new List<VertexT>(vertexList);
        }

        protected override bool NodeExists(VertexT key) {
            return vertexDict.ContainsKey(key);
        }

        protected override EdgeT RemoveConnection(VertexT obj1, VertexT obj2) {
            int i = VertToIndex(obj1);
            int j = VertToIndex(obj2);

            EdgeT value = matrix[i][j];
            matrix[i][j] = default;

            return value;
        }

        protected override void RemoveNodeAndConnections(VertexT key) {
            int removedPos = VertToIndex(key);

            //update name list
            vertexList.RemoveAt(removedPos);

            //update index dictionary
            vertexDict.Remove(key);
            foreach (VertexT vertex in vertexDict.Keys)
                if (VertToIndex(vertex) > removedPos)
                    vertexDict[vertex]--;           //shift indexes to the left

            //update matrix
            matrix.RemoveAt(removedPos);
            foreach (List<EdgeT> ls in matrix)
                ls.RemoveAt(removedPos);
        }

        protected override void ReplaceEdgeValue(Edge<VertexT, EdgeT> edge, EdgeT newValue) {
            int i = VertToIndex(edge.StartPoint);
            int j = VertToIndex(edge.EndPoint);

            matrix[i][j] = newValue;
        }

        protected override void ReplaceVertexValue(VertexT oldValue, VertexT newValue) {
            vertexDict.Add(newValue, vertexDict[oldValue]);
            vertexDict.Remove(oldValue);
            vertexList[VertToIndex(oldValue)] = newValue;
        }

        private int VertToIndex(VertexT key) {
            return vertexDict[key];
        }

        private VertexT IndexToVert(int index) {
            return vertexList[index];
        }

        private bool isValid(EdgeT value) {
            return !value.Equals(default(EdgeT));
        }
    }
}
