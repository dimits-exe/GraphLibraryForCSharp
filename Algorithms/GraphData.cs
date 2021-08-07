using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace GraphLibrary {

    /// <summary>
    /// A data structure abstracting the graph into common components. Used during serialization and deserialization.<br></br>
    /// The instance represents a <b>snapshot</b> of the graph and won't reflect changes to it after its creation.
    /// </summary>
    /// <typeparam name="VertexT">The type of objects stored in the graph's vertices.</typeparam>
    /// <typeparam name="EdgeT">The type of objects stored in the graph's edges.</typeparam>
    [DataContract]
    public class GraphData<VertexT, EdgeT> {
        [DataMember]
        public readonly ReadOnlyCollection<VertexT> vertices;
        [DataMember]
        public readonly ReadOnlyCollection<Edge<VertexT, EdgeT>> edges;
        [DataMember]
        public readonly int size;
        [DataMember]
        public readonly bool isDirected;

        internal GraphData(IGraph<VertexT, EdgeT> graph) {
            vertices = graph.Vertices();
            edges = graph.Edges();
            size = graph.Size;
            isDirected = graph.IsDirected;
        }     
    }
}
