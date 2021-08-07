using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary {

    /// <summary>
    /// Checks whether or not the graph is connected.
    /// </summary>
    internal class ConnectivityVisitor<VertexT, EdgeT> : TraversalVisitor<VertexT, EdgeT, bool> {
        private int reached = 0;

        internal ConnectivityVisitor(IGraph<VertexT,EdgeT> graph): base(graph) {}

        public override void StartVisit(VertexT v) {
            reached++;
        }

        public override bool Result() {
            return reached == graph.Size;
        }
    }

    /// <summary>
    /// Finds a path from the "start" node to the "end" node.
    /// Returns an empty path if there is no such path.
    /// </summary>
    internal class FindPathVisitor<VertexT, EdgeT> : TraversalVisitor<VertexT, EdgeT, ICollection<VertexT>> {
        private LinkedList<VertexT> path = new LinkedList<VertexT>();
        private readonly VertexT end;
        private bool done = false;

        internal FindPathVisitor(IGraph<VertexT, EdgeT> graph, VertexT end) : base(graph) {
            this.end = end;
        }

        public override void StartVisit(VertexT v) {
            path.AddLast(v);
            if (v.Equals(end))
                done = true;
        }

        public override void FinishVisit(VertexT v) {
            //Couldn't find the end node in this path,
            //delete the path until the end of DFS's backtracking.
            if (path.Count != 0) 
                path.RemoveLast();
        }

        public override bool IsDone() {
            return done;
        }

        public override ICollection<VertexT> Result() {
            return path;
        }

    }

    /// <summary>
    /// Finds a cycle in a <b>connected</b> graph and returns its path, if it exists.
    /// Returns an empty path otherwise.
    /// </summary>
    internal class FindCycleVisitor<VertexT, EdgeT> : TraversalVisitor<VertexT, EdgeT, ICollection<VertexT>> {
        private LinkedList<VertexT> cycle = new LinkedList<VertexT>();
        private VertexT cycleStart;
        private bool done = false;
        
        internal FindCycleVisitor(IGraph<VertexT, EdgeT> graph): base(graph) {}

        public override void StartVisit(VertexT v) {
            cycle.AddLast(v);
        }

        public override void FinishVisit(VertexT v) {
            cycle.RemoveLast();
        }

        public override void TraverseBack(Edge<VertexT, EdgeT> edge) {
            cycleStart = edge.EndPoint;
            cycle.AddLast(cycleStart);
            done = true;
        }

        public override bool IsDone() {
            return done;
        }

        public override ICollection<VertexT> Result() {
            return cycle;
        }
    }


}
