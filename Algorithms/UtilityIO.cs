using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace GraphLibrary {

    /// <summary>
    /// A set of utility methods for the purposes of saving, loading and transferring
    /// a graph's data in an implementation independent way.
    /// </summary>
    public class UtilityIO {

        /// <summary>
        /// Saves the graph to a portable XML file.
        /// </summary>
        /// <typeparam name="VertexT">The type of the graph's vertices.</typeparam>
        /// <typeparam name="EdgeT">The type of the graph's edge values.</typeparam>
        /// <param name="filePath">The directory in which the file will be saved.</param>
        /// <param name="fileName">The file's name.</param>
        /// <param name="graph">The graph to be saved.</param>
        public static void SaveToFile<VertexT, EdgeT>(String filePath, String fileName, IGraph<VertexT, EdgeT> graph) {
            if (!fileName.ToLower().EndsWith(".xml"))
                fileName += ".xml";

            FileStream writer = new FileStream(filePath + fileName, FileMode.Create);
            DataContractSerializer serializer = new DataContractSerializer(typeof(GraphData<String, int>));
            serializer.WriteObject(writer, GetData<VertexT, EdgeT>(graph));
            writer.Close();
        }

        /// <summary>
        /// Loads a graph's file and returns its data. The data can then be used to construct any 
        /// specific graph implementation.
        /// </summary>
        /// <typeparam name="VertexT">The type of the graph's vertices.</typeparam>
        /// <typeparam name="EdgeT">The type of the graph's edge values.</typeparam>
        /// <param name="filePath">The path to the file.</param>
        /// <returns>A <see cref="GraphData{VertexT, EdgeT}"/> instance encapsulating the graph's data.</returns>
        public static GraphData<VertexT, EdgeT> LoadFromFile<VertexT, EdgeT>(String filePath) {
            //Open and read the file
            FileStream fs = new FileStream(filePath, FileMode.Open);
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
            DataContractSerializer serializer = new DataContractSerializer(typeof(GraphData<VertexT, EdgeT>));

            // Deserialize the data 
            GraphData<VertexT, EdgeT> deserializedData = (GraphData<VertexT, EdgeT>)serializer.ReadObject(reader, true);
            reader.Close();
            fs.Close();
            return deserializedData;
        }

        /// <summary>
        /// Returns a <see cref="GraphData{VertexT, EdgeT}"/> data structure holding all the graph's data in a uniform way. 
        /// Used in serialization and de-serialization.
        /// </summary>
        /// <returns></returns>
        public static GraphData<VertexT, EdgeT> GetData<VertexT, EdgeT>(IGraph<VertexT, EdgeT> graph) {
            return new GraphData<VertexT, EdgeT>(graph);
        }
    }

}
