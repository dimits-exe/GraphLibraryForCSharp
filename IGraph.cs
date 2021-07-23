﻿using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary {
    /// <summary>
    /// An interface defining all standard operations in any generalized graph.
    /// The graph holds a reference to any object for each internal vertex.<br></br>
    /// This way clients can store and get references to their objects
    /// while traversing the graph.
    /// </summary>
    /// <typeparam name="VertexT">The type of objects stored in the graph's vertices.</typeparam>
    /// <typeparam name="EdgeT">The type of objects stored in the graph's edges.</typeparam>
    interface IGraph<VertexT, EdgeT> {

        int Size {get;}

        bool IsDirected {get;}

        bool IsEmpty();

        /// <summary>
        /// Adds a new vertex in the graph, storing the object reference within.
        /// </summary>
        /// <param name="key">A reference to the object to be stored in the graph</param>
        void AddVertex(VertexT key);

        /// <summary>
        /// Removes a vertex from the graph.
        /// </summary>
        /// <param name="key">The object whose vertex needs to be removed.</param>
        /// <exception cref="Exception">If there is no vertex with the object requested.</exception>
        void RemoveVertex(VertexT key);

        /// <summary>
        /// Connects the vertices holding the objects.<br></br> 
        /// If the graph is directed, connects obj1 to obj2, if not connects obj2 to obj1 as well.
        /// </summary>
        /// <param name="obj1">The start of the newly created edge between the vertices</param>
        /// <param name="obj2">The end of the newly created edge between the vertices</param>
        /// <param name="weight">The weight of the edge. The default weight is 1.</param>
        Edge<VertexT, EdgeT> Connect(VertexT obj1, VertexT obj2, EdgeT value);

        /// <summary>
        /// Deletes the edge and returns its stored contents.
        /// </summary>
        /// <param name="edge">The edge to be removedd</param>
        /// <returns>The content of the edge.</returns>
        /// <exception cref="Exception">If thereis no such edge in the graph.</exception>
        EdgeT Disconnect(Edge<VertexT, EdgeT> edge);

        /// <summary>
        /// Checks whether or not the vertices containing the two objects are connected. 
        /// </summary>
        /// <returns>True if connected, False otherwise</returns>
        /// <exception cref="Exception">If any of the objects don't exist in the graph.</exception>
        bool AreAdjacent(VertexT obj1, VertexT obj2);

        /// <summary>
        /// Get all edges starting from the given vertex.
        /// </summary>
        /// <param name="key">The vertex whose neighbours were requested.</param>
        /// <returns>A read-only collection of <see cref="VertexT"/>s starting from the given vertex and ending on all the neighbouring vertices.</returns>
        ReadOnlyCollection<VertexT> IncidentEdges(VertexT key);

        /// <summary>
        /// Get all objects stored in the graph.
        /// </summary>
        /// <returns>A read-only collection containing all objects in no particular order.</returns>
        ReadOnlyCollection<VertexT> Vertices();

        /// <summary>
        /// Get all edges existing in the graph.
        /// </summary>
        /// <returns>A read-only collection containing all objects in no particular order.</returns>
        ReadOnlyCollection<Edge<VertexT, EdgeT>> Edges();

        /// <summary>
        /// Replace the object stored in vertex oldValue with a new value.
        /// </summary>
        /// <param name="oldValue">The object to be replaced</param>
        /// <param name="newValue">The new object</param>
        /// <exception cref="Exception">If "oldValue" doesn't exist.</exception>
        void ReplaceVertex(VertexT oldValue, VertexT newValue);

        /// <summary>
        /// Replace the object stored in the edge starting from "startPoint" and ending on
        /// "endPoint" with a new value
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <param name="newValue">The value to replace the old one.</param>
        /// <exception cref="Exception">If the edge doesn't exist.</exception>
        void ReplaceEdge(Edge<VertexT, EdgeT> edge, EdgeT newValue);
    }
}
