using System;
using System.Runtime.Serialization;

namespace GraphLibrary {
    
    /// <summary>
    /// A simple struct encapsulating information about any edge in a <see cref="Graph{T}"/> instance.<br></br>
    /// Specifically holds the names of the nodes being connected by the edge and the edge's value.
    /// </summary>
    /// <typeparam name="VertexT">The type of the objects each of the two <see cref="Graph{VertexT,EdgeT}"/> vertices hold.</typeparam>
    /// <typeparam name="EdgeT">The type of the objects held in each edge.</typeparam>
    [DataContract]
    public struct Edge<VertexT, EdgeT> {
        private VertexT startPoint;
        private VertexT endPoint;
        private EdgeT edgeValue;

        [DataMember]
        public VertexT StartPoint {
            get {
                return startPoint;
            }
            
            internal set { //used by the serializer
                startPoint = value;
            }
        }
    
        [DataMember]
        public VertexT EndPoint {
            get {
                return endPoint;
            }
            
            internal set { //used by the serializer
                endPoint = value;
            }
        }
        
        [DataMember]
        public EdgeT Value {
            get {
                return value;
            }
            
            internal set {
                edgeValue = value;
            }
        }

        internal Edge(VertexT startPoint, VertexT endPoint, EdgeT value) {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            this.edgeValue = value;
        }

        public override string ToString() {
             return String.Format("Edge from {0} to {1}", startPoint, endPoint);
        }
    }

}
