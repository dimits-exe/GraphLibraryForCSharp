using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GraphLibrary;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace GraphTest {

    [TestClass]
    public class BasicGraphTest {
        private IGraph<String, int> g;

        private void NewGraph() {
            g = new SparseGraph<String, int>(false); //change graph type here
        }

        [TestMethod]
        public void TestVertices() {
            NewGraph();

            g.AddVertex("Node 1");
            g.AddVertex("Node 2");
            g.AddVertex("Removed node");


            //check Vertices()
            System.IO.StringWriter sw = new System.IO.StringWriter();
            Console.SetOut(sw);
            foreach (String vertex in g.Vertices())
                Console.WriteLine(vertex);

            String results = sw.ToString();
            Assert.IsTrue(results.Contains("Node 1"));
            Assert.IsTrue(results.Contains("Node 2"));
            Assert.IsTrue(results.Contains("Removed node"));


            //check Connect() / AreAdjacent()
            Assert.IsFalse(g.AreAdjacent("Node 1", "Node 2"));

            Edge<String, int> e = g.Connect("Node 1", "Removed node", 1);
            Assert.IsTrue(g.AreAdjacent("Node 1", "Removed node"));
            Assert.IsTrue(e.StartPoint == "Node 1");
            Assert.IsTrue(e.EndPoint == "Removed node");


            //check ReplaceVertex()
            try {
                g.ReplaceVertex("Non-existent node", "New node");
                Assert.Fail("Replaced a non-existent node");
            }
            catch (InvalidVertexException) {}

            g.ReplaceVertex("Removed node", "New removed node");
            Assert.IsTrue(g.AreAdjacent("Node 1", "New removed node"));
            g.ReplaceVertex("New removed node", "Removed node");

            
            //check RemoveVertex()
            g.RemoveVertex("Removed node");
            Assert.IsTrue(g.Size == 2);

           try{
                g.Connect("Removed node", "Node 2", 3);
                Assert.Fail("Connected a non-existent node");
            } 
            catch (InvalidVertexException) { }

            try {
                g.Disconnect(e);
                Assert.Fail("Disconnected a non-existent edge");
            }
            catch (InvalidEdgeException) { }
        }

        [TestMethod]
        public void TestEdges() {
            NewGraph();
            System.IO.StringWriter sw = new System.IO.StringWriter();
            Console.SetOut(sw);

            String n1 = "Node 1";
            String n2 = "Node 2";
            String n3 = "Node 3";
            String n4 = "Node 4";
            String n5 = "Node 5";

            g.AddVertex(n1);
            g.AddVertex(n2);
            g.AddVertex(n3);
            g.AddVertex(n4);

            g.Connect(n1, n2, 1);
            g.Connect(n1, n3, 2);
            g.Connect(n1, n4, 3);
            g.Connect(n1, n1, 4);

            foreach (Edge<String, int> edge in g.IncidentEdges(n1)) 
                Console.WriteLine(edge.EndPoint);

            String results = sw.ToString();
            Assert.IsTrue(results.Contains(n1));
            Assert.IsTrue(results.Contains(n2));
            Assert.IsTrue(results.Contains(n3));
            Assert.IsTrue(results.Contains(n4));

            //check whether or not the edges are updated to remove n5
            g.AddVertex(n5);
            g.Connect(n5, n2, 5);
            g.Connect(n3, n5, 6);
            g.RemoveVertex(n5);

            foreach (Edge<String, int> edge in g.IncidentEdges(n3))
                Assert.IsFalse(edge.EndPoint.Equals(n5));


        }

        [TestMethod]
        public void TestSerialization() {
            NewGraph();
            //Same setup as previous test
            System.IO.StringWriter sw = new System.IO.StringWriter();
            Console.SetOut(sw);

            String n1 = "Node 1";
            String n2 = "Node 2";
            String n3 = "Node 3";
            String n4 = "Node 4";

            g.AddVertex(n1);
            g.AddVertex(n2);
            g.AddVertex(n3);
            g.AddVertex(n4);

            g.Connect(n1, n2, 1);
            g.Connect(n1, n3, 2);
            g.Connect(n1, n4, 3);
            g.Connect(n1, n1, 4);

            //Serialize the data
            FileStream writer = new FileStream("graph.xml", FileMode.Create);
            DataContractSerializer ser = new DataContractSerializer(typeof(GraphData<String, int>));
            ser.WriteObject(writer, g.GetGraphData());
            writer.Close();


            FileStream fs = new FileStream("graph.xml", FileMode.Open);
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());

            // Deserialize the data 
            GraphData<String, int> deserializedData = (GraphData<String, int>)ser.ReadObject(reader, true);
            reader.Close();
            fs.Close();

            //Convert and check data integrity
            SparseGraph<String, int> newGraph = new SparseGraph<string, int>(deserializedData);

            foreach (Edge<String, int> edge in g.IncidentEdges(n1))
                Console.WriteLine(edge.EndPoint);

            String results = sw.ToString();
            Assert.IsTrue(results.Contains(n1));
            Assert.IsTrue(results.Contains(n2));
            Assert.IsTrue(results.Contains(n3));
            Assert.IsTrue(results.Contains(n4));
        }

 
    }
}
