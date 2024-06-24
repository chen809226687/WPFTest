using System.Diagnostics;
using Opc.Ua;

namespace OPCUA_codesysTest
{
	[DebuggerDisplay("ServerNodeDescription {Variable}: {NodeId}")]
	public  class ServerErrorNodeDescription
	{
		public ExpandedNodeId NodeId { get; set; }

        public string Description { get; set; }
    }
}
