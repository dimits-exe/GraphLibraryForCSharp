using System.Collections.ObjectModel;
using System.Threading;

namespace GraphLibrary {

    /// <summary>
    /// A thread-safe wrapper for the graph. Guarantees thread safety through the use of a read-write lock.<br></br>
    /// </summary>
    /// <typeparam name="VertexT">The type of objects stored in the graph's vertices.</typeparam>
    /// <typeparam name="EdgeT">The type of objects stored in the graph's edges.</typeparam>
    internal class ThreadSafeGraph<VertexT, EdgeT> : IGraph<VertexT, EdgeT> {

        private readonly IGraph<VertexT, EdgeT> actualGraph;
        private readonly ReaderWriterLockSlim rwl;

        internal ThreadSafeGraph(IGraph<VertexT, EdgeT> graph) {
            actualGraph = graph;
            rwl = new ReaderWriterLockSlim();
        }

        public int Size { //instant operation, doesn't need a lock
            get {
                return actualGraph.Size;
            }
        }

        public bool IsDirected { //instant operation, doesn't need a lock
            get {
                return actualGraph.IsDirected;
            }
        }

        public void AddVertex(VertexT key) {
            rwl.EnterWriteLock();
            try {
                actualGraph.AddVertex(key);
            } 
            finally {
                rwl.ExitWriteLock();
            }
        }

        public bool AreAdjacent(VertexT obj1, VertexT obj2) {
            rwl.EnterReadLock();
            try {
                return actualGraph.AreAdjacent(obj1, obj2);
            } 
            finally {
                rwl.ExitReadLock();
            }
        }

        public Edge<VertexT, EdgeT> Connect(VertexT obj1, VertexT obj2, EdgeT value) {
            rwl.EnterWriteLock();
            try {
                return actualGraph.Connect(obj1, obj2, value);
            } 
            finally {
                rwl.ExitWriteLock();
            }
        }

        public EdgeT Disconnect(Edge<VertexT, EdgeT> edge) {
            rwl.EnterWriteLock();
            try {
                return actualGraph.Disconnect(edge);
            }
            finally {
                rwl.ExitWriteLock();
            }
        }

        public ReadOnlyCollection<Edge<VertexT, EdgeT>> Edges() {
            rwl.EnterReadLock();
            try {
                return actualGraph.Edges();
            }
            finally {
                rwl.ExitReadLock();
            }
        }

        public ReadOnlyCollection<Edge<VertexT, EdgeT>> IncidentEdges(VertexT key) {
            rwl.EnterReadLock();
            try {
                return actualGraph.IncidentEdges(key);
            }
            finally {
                rwl.ExitReadLock();
            }
        }

        public bool IsEmpty() { //instant operation, doesn't need a lock
            return actualGraph.IsEmpty();
        }

        public void RemoveVertex(VertexT key) {
            rwl.EnterWriteLock();
            try {
                actualGraph.RemoveVertex(key);
            } 
            finally {
                rwl.ExitWriteLock();
            }
        }

        public void ReplaceEdge(Edge<VertexT, EdgeT> edge, EdgeT newValue) {
            rwl.EnterWriteLock();
            try {
                actualGraph.ReplaceEdge(edge, newValue);
            } 
            finally {
                rwl.ExitWriteLock();
            }
        }

        public void ReplaceVertex(VertexT oldValue, VertexT newValue) {
            rwl.EnterWriteLock();
            try {
                actualGraph.ReplaceVertex(oldValue, newValue);
            }
            finally {
                rwl.ExitWriteLock();
            }
        }

        public ReadOnlyCollection<VertexT> Vertices() {
            rwl.EnterReadLock();
            try {
                return actualGraph.Vertices();
            }
            finally {
                rwl.ExitReadLock();
            }
        }
    }

}
