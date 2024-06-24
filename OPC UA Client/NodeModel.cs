using Opc.Ua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPC_UA_Client
{
    internal class NodeModel
    {


        public NodeModel() { }

        public int ParameterOfNode { get; set; } // Node对应的Parameter
        public string NodeIDName { get; set; } // NodeId

        public AccessLevelType AccessLevelType { get; set; } // NodeId

        public string NodeName { get; set; }  //Node

        public string IECName { get; set; }  
        public string NodeType { get; set; }  //Node的类型

    }
}
