using System.Collections.Generic;

namespace GraphLibrary {

    /// <summary>
    /// A class holding a selection of static methods used to traverse andd modify graphs.
    /// </summary>
    public static class Algorithms {

        /// <summary>
        /// A <see href="https://en.wikipedia.org/wiki/Depth-first_search">DFS algorithm </see> traversing the graph while gathering information in 
        /// a <see cref="TraversalVisitor{VertexT, EdgeT, ReturnV}"/> object. The method begins the traversal from a given node and notifies it 
        /// for every discovery made during its traversal. <br></br>
        /// Make sure the visitor instance was not previously used in a traversal.
        /// </summary>
        /// <typeparam name="VertexT">The type of the graph's vertices.</typeparam>
        /// <typeparam name="EdgeT">The type of the graph's edge values.</typeparam>
        /// <typeparam name="ReturnT">The return type of the traversal.</typeparam>
        /// <param name="v"> The visitor object that specifies what the algorithm should do.
        /// <param name="graph">The graph instance to be traversed.</param>
        /// <param name="start">The first node to be traversed. A random node is chosen if one isn't provided.</param>
        /// <returns>The return value of <see cref="TraversalVisitor{VertexT, EdgeT, ReturnV}.Result"/></returns>
        public static ReturnT DFS<VertexT, EdgeT, ReturnT>(TraversalVisitor<VertexT, EdgeT, ReturnT> v,
            IGraph<VertexT, EdgeT> graph, VertexT start = default(VertexT)){

            if (start.Equals(default(VertexT)))
                start = v.defaultStart;

            DFSImpl(v, graph, start);

            return v.Result();
        }

        /// <summary>
        /// Finds a cycle in any graph, if it exists, and returns its path.
        /// </summary>
        /// <typeparam name="VertexT">The type of the graph's vertices.</typeparam>
        /// <typeparam name="EdgeT">The type of the graph's edge values.</typeparam>
        /// <param name="graph">The graph to be checked.</param>
        /// <returns>A collection holding the vertices of the path, empty if there is no cycle.</returns>
        /// <exception cref="InvalidGraphException">If the graph is empty.</exception>
        public static ICollection<VertexT> FindCycle<VertexT, EdgeT>(IGraph<VertexT, EdgeT> graph) {
            TraversalVisitor<VertexT, EdgeT, ICollection<VertexT>> visitor = new FindCycleVisitor<VertexT, EdgeT>(graph);

            foreach (VertexT v in graph.Vertices)
                if (!visitor.IsVisited(v)) {
                    ICollection<VertexT> path = DFS(visitor, graph, v);
                    if (path.Count != 0)
                        return path;
                }

            return new LinkedList<VertexT>(); //return empty if no cycles
        }

        /// <summary>
        /// Checks to see if the graph has any cycles.
        /// </summary>
        /// <typeparam name="VertexT">The type of the graph's vertices.</typeparam>
        /// <typeparam name="EdgeT">The type of the graph's edge values.</typeparam>
        /// <param name="graph">The graph to be checked.</param>
        /// <returns>True if there is a cycle, False otherwise.</returns>
        /// <exception cref="InvalidGraphException">If the graph is empty.</exception>
        public static bool HasCycle<VertexT, EdgeT>(IGraph<VertexT, EdgeT> graph) {
            return FindCycle(graph).Count != 0;
        }

        /// <summary>
        /// Checks whether all vertices in the graph are connected with each other.
        /// </summary>
        /// <typeparam name="VertexT">The type of the graph's vertices.</typeparam>
        /// <typeparam name="EdgeT">The type of the graph's edge values.</typeparam>
        /// <param name="graph">The graph to be checked.</param>
        /// <returns>True if the graph is connected, False if not.</returns>
        /// <exception cref="InvalidGraphException">If the graph is empty.</exception>
        /// <seealso cref="GetGraphComponents{VertexT, EdgeT}(IGraph{VertexT, EdgeT})"/>
        public static bool IsConnected<VertexT, EdgeT>(IGraph<VertexT, EdgeT> graph) {
            return DFS(new ConnectivityVisitor<VertexT, EdgeT>(graph), graph);
        }

        /// <summary>
        /// Checks whether the graph is a Directed Acyclical Graph.
        /// </summary>
        /// <typeparam name="VertexT">The type of the graph's vertices.</typeparam>
        /// <typeparam name="EdgeT">The type of the graph's edge values.</typeparam>
        /// <param name="graph">The graph to be checked.</param>
        /// <returns>True if the graph is a DAG, False otherwise.</returns>
        /// <exception cref="InvalidGraphException">If the graph is empty.</exception>
        public static bool IsDAGd<VertexT, EdgeT>(IGraph<VertexT, EdgeT> graph) {
            return graph.IsDirected && !HasCycle(graph);
        }

        /// <summary>
        /// Takes note of all the <see href="https://en.wikipedia.org/wiki/Component_(graph_theory)"> components </see>
        /// in the graph and tags vertices in the same component with the same number.
        /// </summary>
        /// <typeparam name="VertexT">The type of the graph's vertices.</typeparam>
        /// <typeparam name="EdgeT">The type of the graph's edge values.</typeparam>
        /// <param name="graph"> The graph whose components will be returned. </param>
        /// <returns>A <see cref="Dictionary{VertexT, int}"/> where if 2 vertices have the same assigned int, exist in the same component.</returns>
        /// <exception cref="InvalidGraphException">If the graph is empty.</exception>
        public static Dictionary<VertexT, int> GetGraphComponents<VertexT, EdgeT>(IGraph<VertexT, EdgeT> graph) {
            Dictionary<VertexT, int> compMap = new Dictionary<VertexT, int>();
            int numOfComponents = 1;
            TraversalVisitor<VertexT, EdgeT, bool> visitor = new(graph);

            foreach(VertexT v in graph.Vertices) {
                if (!visitor.IsVisited(v)) {
                    numOfComponents++;
                    DFSImpl(visitor, graph, v);
                }
            }

            return compMap;
        }

        /// <summary>
        /// Recursive implementation of the DFS algorithm.
        /// </summary>
        private static void DFSImpl<VertexT, EdgeT, ReturnT>(TraversalVisitor<VertexT, EdgeT, ReturnT> v,
           IGraph<VertexT, EdgeT> graph, VertexT node) {

            if (!v.IsDone())
                v.StartVisit(node);
            if (!v.IsDone())
                v.Visit(node);

            foreach (var edge in graph.IncidentEdges(node)) {
                if (!v.IsVisited(edge)) {
                    //if unexplored edge
                    v.Visit(edge);
                    VertexT endPoint = edge.EndPoint;
                    if (!v.IsVisited(endPoint)) {
                        //the node is unexplored, this is a new / discovery edge
                        v.TraverseDiscovery(edge);
                        if (v.IsDone())
                            break;
                        DFSImpl(v, graph, endPoint); //recursive call
                        if (v.IsDone())
                            break;
                    }
                }
                else {
                    //already explored edge, this is a back edge
                    v.TraverseBack(edge);
                    if (v.IsDone())
                        break;
                }
            }//foreach

            //exploration has returned to this node
            if (!v.IsDone())
                v.FinishVisit(node);
        }


    }//class
}//namespace
