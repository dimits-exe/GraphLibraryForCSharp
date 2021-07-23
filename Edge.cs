using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary {
    /// <summary>
    /// A simple struct encapsulating information about any edge in a <see cref="Graph{T}"/> instance.<br></br>
    /// Specifically holds the names of the nodes being connected by the edge and the edge's weight.
    /// </summary>
    /// <typeparam name="VertexT">The type of the objects each of the two <see cref="Graph{VertexT,EdgeT}"/> vertices hold.</typeparam>
    /// <typeparam name="EdgeT">The type of the objects held in each edge.</typeparam>
    public readonly struct Edge<VertexT, EdgeT> {
        private readonly VertexT startPoint;
        private readonly VertexT endPoint;
        private readonly EdgeT value;

        public VertexT StartPoint {
            get {
                return startPoint;
            }
        }

        public VertexT EndPoint {
            get {
                return endPoint;
            }
        }

        public EdgeT Value {
            get {
                return value;
            }
        }

        internal Edge(VertexT startPoint, VertexT endPoint, EdgeT value) {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            this.value = value;
        }

        public override string ToString() {
            return String.Format("Edge from %s to %s", startPoint, endPoint);
        }
    }

}
