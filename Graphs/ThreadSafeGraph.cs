using System.Collections.ObjectModel;
using System.Threading;

/*
 * This is a general imeplementation that is meant to support all graphs. Concurrent implementations for
 * each implementation class would certainly achieve much better performance, as we wouldn't have to lock the
 * entire graph for every operation. This however is out of the scope of this library for now.
 */

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

        int IGraph<VertexT, EdgeT>.Size => actualGraph.Size; //instant operation, doesn't need a lock

        bool IGraph<VertexT, EdgeT>.IsDirected => actualGraph.IsDirected; //instant operation, doesn't need a lock

        ReadOnlyCollection<VertexT> IGraph<VertexT, EdgeT>.Vertices {

            get {
                rwl.EnterReadLock();
                try {
                    return actualGraph.Vertices;
                }
                finally {
                    rwl.ExitReadLock();
                }
            }

        }

        ReadOnlyCollection<Edge<VertexT, EdgeT>> IGraph<VertexT, EdgeT>.Edges {

            get {
                rwl.EnterReadLock();
                try {
                    return actualGraph.Edges;
                }
                finally {
                    rwl.ExitReadLock();
                }
            }
            
         }

        void IGraph<VertexT, EdgeT>.AddVertex(VertexT key) {
            rwl.EnterWriteLock();
            try {
                actualGraph.AddVertex(key);
            }
            finally {
                rwl.ExitWriteLock();
            }
        }

        bool IGraph<VertexT, EdgeT>.AreAdjacent(VertexT obj1, VertexT obj2) {
            rwl.EnterReadLock();
            try {
                return actualGraph.AreAdjacent(obj1, obj2);
            }
            finally {
                rwl.ExitReadLock();
            }
        }

        Edge<VertexT, EdgeT> IGraph<VertexT, EdgeT>.Connect(VertexT obj1, VertexT obj2, EdgeT value) {
            rwl.EnterWriteLock();
            try {
                return actualGraph.Connect(obj1, obj2, value);
            }
            finally {
                rwl.ExitWriteLock();
            }
        }

        EdgeT IGraph<VertexT, EdgeT>.Disconnect(Edge<VertexT, EdgeT> edge) {
            rwl.EnterWriteLock();
            try {
                return actualGraph.Disconnect(edge);
            }
            finally {
                rwl.ExitWriteLock();
            }
        }

        Edge<VertexT, EdgeT> IGraph<VertexT, EdgeT>.GetEdge(VertexT obj1, VertexT obj2) {
            rwl.EnterReadLock();
            try {
                return actualGraph.GetEdge(obj1, obj2);
            }
            finally {
                rwl.ExitReadLock();
            }
        }

        ReadOnlyCollection<Edge<VertexT, EdgeT>> IGraph<VertexT, EdgeT>.IncidentEdges(VertexT key) {
            rwl.EnterReadLock();
            try {
                return actualGraph.IncidentEdges(key);
            }
            finally {
                rwl.ExitReadLock();
            }
        }

        bool IGraph<VertexT, EdgeT>.IsEmpty() {
            return actualGraph.IsEmpty();
        }

        void IGraph<VertexT, EdgeT>.RemoveVertex(VertexT key) {
            rwl.EnterWriteLock();
            try {
                actualGraph.RemoveVertex(key);
            }
            finally {
                rwl.ExitWriteLock();
            }
        }

        void IGraph<VertexT, EdgeT>.ReplaceEdge(Edge<VertexT, EdgeT> edge, EdgeT newValue) {
            rwl.EnterWriteLock();
            try {
                actualGraph.ReplaceEdge(edge, newValue);
            }
            finally {
                rwl.ExitWriteLock();
            }
        }

        void IGraph<VertexT, EdgeT>.ReplaceVertex(VertexT oldValue, VertexT newValue) {
            rwl.EnterWriteLock();
            try {
                actualGraph.ReplaceVertex(oldValue, newValue);
            }
            finally {
                rwl.ExitWriteLock();
            }
        }
    }

}
