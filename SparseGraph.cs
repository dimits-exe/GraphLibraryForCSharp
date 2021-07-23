using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GraphLibrary {
    public class SparseGraph<VertexT, EdgeT> : Graph<VertexT, EdgeT> {

        private Dictionary<VertexT, LinkedList<Edge<VertexT, EdgeT>>> vertices;

        public SparseGraph(bool isDirected): base(isDirected) {
            vertices = new Dictionary<VertexT, LinkedList<Edge<VertexT, EdgeT>>>();
        }

        protected override Edge<VertexT, EdgeT> AddConnection(VertexT obj1, VertexT obj2, EdgeT value) {
            Edge<VertexT, EdgeT> newEdge = new Edge<VertexT, EdgeT>(obj1, obj2, value);
            vertices[obj1].AddLast(newEdge);
            return newEdge;
        }

        protected override void AddNode(VertexT key) {
            vertices.Add(key, new LinkedList<Edge<VertexT, EdgeT>>());
        }

        protected override bool EdgeExists(VertexT obj1, VertexT obj2) {
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
            throw new NotImplementedException();
        }

        protected override VertexT[] GetIncidentEdges(VertexT vertex) {
            throw new NotImplementedException();
        }

        protected override Edge<VertexT, EdgeT>[] GetEdges() {
            throw new NotImplementedException();
        }

        protected override VertexT[] GetVertices() {
            throw new NotImplementedException();
        }
    }

}
