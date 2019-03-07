namespace MaxStream_Lab3
{
    public class Edge
    {
        public int? Number { get; set; }

        public int Capacity { get; set; }       // max flow
        public int Flow                         // currently used flow
        {
            get
            {
                return flow;
            }
            set
            {
                flow = value;
                if (Flow == Capacity)
                    Full = true;
            }
        }

        private int flow;

        public bool Full { get; set; }
        public bool Closed { get; set; }

        public Node StartNode { get; set; }
        public Node EndNode { get; set; }
    }
}