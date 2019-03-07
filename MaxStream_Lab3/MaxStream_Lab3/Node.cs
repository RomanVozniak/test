using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxStream_Lab3
{
    public class Node
    {
        public string Name { get; set; }
        public List<Edge> InEdges { get; set; }
        public List<Edge> OutEdges { get; set; }


        public Node()
        {
            InEdges = new List<Edge>();
            OutEdges = new List<Edge>();
        }
    }
}
