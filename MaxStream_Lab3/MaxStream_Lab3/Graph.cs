using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxStream_Lab3
{
    public class Graph
    {
        public List<Node> Nodes { get; set; }
        public List<Edge> Edges { get; set; }
        public List<Edge> UnusedEdges { get; set; }
        public List<Edge> UsedEdges { get; set; }
        public int[][] AdjacencyMatrix { get; set; }
        public int[][] RoutesMatrix { get; set; }
        public int EdgeNumber { get; set; }


        public Graph()
        {
            Nodes = new List<Node>();
            Edges = new List<Edge>();
            ReadFromFile("data.txt");
            PrintNodes();
            FindMaxFlow("a", "f");

        }

        public void FindMaxFlow(string StartNodeName, string EndNodeName)
        {
            List<Edge> path = new List<Edge>();
            Node currentNode;
            Edge edge;
            bool Finish = false;
            bool Stop = false;

            while(!Finish)
            {
                edge = null;
                currentNode = Nodes.FirstOrDefault(n => n.Name == StartNodeName);
                path.Clear();
                Stop = false;

                while (currentNode.Name != EndNodeName)
                {
                    edge = currentNode.OutEdges.FirstOrDefault(e => !e.Full && !e.Closed);
                    if (edge == null)
                    {
                        if (path.Count > 0)
                        {
                            path.Last().Closed = true;
                            if (currentNode.Name == Nodes.FirstOrDefault(n=>n.Name == StartNodeName).OutEdges.Last().EndNode.Name)
                            {
                                Finish = true;
                                Stop = true;
                                path.Clear();
                                break;
                            }
                                
                        }
                    }
                    else
                    {
                        path.Add(edge);
                        currentNode = edge.EndNode;

                    }
                }
                if (!Stop)
                {
                    int minCapacity = path.Min(s => s.Capacity - s.Flow);
                    path.ForEach(s => s.Flow += minCapacity);
                    PrintPath(path);
                }
            }
            
        }

        public void AddEdge(int edgeNumber, int edgeCapacity, string startNodeName, string endNodeName)
        {
            Edge newEdge = new Edge() { Capacity = edgeCapacity, Number = edgeNumber };
            Node startNode = Nodes.FirstOrDefault(n => n.Name == startNodeName);
            Node endNode = Nodes.FirstOrDefault(n => n.Name == endNodeName);
            if (startNode == null)
            {
                startNode = new Node()
                {
                    Name = startNodeName
                };
                Nodes.Add(startNode);
            }
            if (endNode == null)
            {
                endNode = new Node()
                {
                    Name = endNodeName
                };
                Nodes.Add(endNode);
            }
            endNode.InEdges.Add(newEdge);
            startNode.OutEdges.Add(newEdge);
            newEdge.StartNode = startNode;
            newEdge.EndNode = endNode;
            Edges.Add(newEdge);
        }

        public void ReadFromFile(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            while (sr.Peek() >= 0)
            {
                try
                {
                    var line = sr.ReadLine()?.Split(' ');
                    if (line == null)
                        throw new Exception();
                    if (line[0] == "#")
                        continue;
                    if (!int.TryParse(line[0], out var edgeCapacity))
                    {
                        throw new Exception();
                    }

                    string startNodeName = line[1];
                    string endNodeName = line[2];
                    AddEdge(++EdgeNumber, edgeCapacity, startNodeName, endNodeName);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed reading from file.");
                }
            }
            sr.Close();
            fs.Close();
        }

        public void PrintNodes()
        {
            Nodes.ForEach(n =>
            {
                Console.WriteLine($"{n.Name}:");
                if (n.InEdges.Count > 0)
                {
                    Console.WriteLine("\tInEdges:");
                    n.InEdges.ForEach(e => Console.WriteLine($"\t\t{e.Number} ({e.Capacity})"));
                }
                if (n.OutEdges.Count > 0)
                {
                    Console.WriteLine("\tOutEdges:");
                    n.OutEdges.ForEach(e => Console.WriteLine($"\t\t{e.Number} ({e.Capacity})"));
                }
            });
        }

        public void PrintPath(List<Edge> Path)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine();
            foreach(Edge e in Path)
            {
                Console.Write($"{e.StartNode.Name} - {e.EndNode.Name} {e.Capacity}({e.Flow})\t");
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void MakeAdjacencyMatrix()
        {
            AdjacencyMatrix = new int[Nodes.Count][];
            RoutesMatrix = new int[Nodes.Count][];
            for (int i = 0; i < Nodes.Count; i++)
            {
                RoutesMatrix[i] = new int[Nodes.Count];
                AdjacencyMatrix[i] = new int[Nodes.Count];
                for (int j = 0; j < Nodes.Count; j++)
                {
                    AdjacencyMatrix[i][j] = -1;
                    RoutesMatrix[i][j] = -1;
                }
            }
            foreach (var edge in Edges)
            {
                AdjacencyMatrix[Nodes.IndexOf(edge.StartNode)][Nodes.IndexOf(edge.EndNode)] = edge.Capacity;
                AdjacencyMatrix[Nodes.IndexOf(edge.EndNode)][Nodes.IndexOf(edge.StartNode)] = edge.Capacity;
            }
        }
    }
}