using Opc.Ua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPCUA_codesysTest
{
    internal class NodeModel
    {

        public NodeModel() { }

        public int ParameterOfNode { get; set; } // Node对应的Parameter
        public string NodeIDName { get; set; } // NodeId

        public AccessLevelType AccessLevelType { get; set; } // NodeId

        public string NodeName { get; set; }  //Node

        public string NodeType { get; set; }  //Node的类型

        public string IECType { get; set; }  //映射的IEC的类型
        public string IECName { get; set; }  //映射的IEC的类型

    }
}
