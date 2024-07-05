using CODESYS.OpcUaDataModel.Domain.OpcUaInformationModel;
using Opc.Ua;
using Opc.Ua.Client;
using OpcUaHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.ServiceModel.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace OPCUA_codesysTest
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();

        }

        Client client = new Client();
        Session Session;




        private void button1_Click(object sender, EventArgs e)
        {
            string serverUrl = UrlCB.Text;

            if (UrlCB.SelectedIndex >= 0)
            {
                serverUrl = (string)UrlCB.SelectedItem;
            }

            bool useSecurity = false;

            try
            {
                Session = client.Connect(serverUrl, useSecurity).GetAwaiter().GetResult();


                Task task = Task.Run(() =>
                {
                    addroottree();
                });


            }
            catch (ServiceResultException sre)
            {
            }

        }



        #region 增加变量树节点
        // 声明一个委托，用于执行在TreeView上添加节点的操作
        delegate void AddNodeDelegate(TreeNode node);
        BrowseDescriptionCollection nodesToBrowse = new BrowseDescriptionCollection();
        private NodeId[] m_referenceTypeIds = new NodeId[] { Opc.Ua.ReferenceTypeIds.HierarchicalReferences };
        ReferenceDescription reference = new ReferenceDescription();
        private void addroottree()
        {

            SortedDictionary<ExpandedNodeId, TreeNode> dictionary = new SortedDictionary<ExpandedNodeId, TreeNode>();


            for (int ii = 0; ii < m_referenceTypeIds.Length; ii++)
            {
                BrowseDescription nodeToBrowse = new BrowseDescription();

                nodeToBrowse.NodeId = ObjectIds.ObjectsFolder;
                nodeToBrowse.BrowseDirection = BrowseDirection.Forward;
                nodeToBrowse.ReferenceTypeId = m_referenceTypeIds[ii];
                nodeToBrowse.IncludeSubtypes = true;
                nodeToBrowse.NodeClassMask = 0;
                nodeToBrowse.ResultMask = (uint)BrowseResultMask.All;
                nodesToBrowse.Add(nodeToBrowse);
            }
            var references = client.BrowseNodeId(nodesToBrowse);

            if (references == null)
            {
                return;
            }
            foreach (var reference in references)
            {
                //去除相对Nodeid
                if (!reference.NodeId.IsAbsolute)
                {
                    Task task = Task.Run(() =>
                    {
                        TreeNode child = new TreeNode(reference.DisplayName.Text);
                        BrowseTV.Invoke((MethodInvoker)(() => BrowseTV.Nodes.Add(child)));
                        addchiletree(child, reference);
                    });
                    Thread.Sleep(500);
                }
            }
            BrowseTV.Invoke((MethodInvoker)(() => BrowseTV.Show()));


        }


        private void addchiletree(TreeNode treeNode, ReferenceDescription referenceDescription)
        {
            nodesToBrowse = new BrowseDescriptionCollection();
            SortedDictionary<ExpandedNodeId, TreeNode> dictionary = new SortedDictionary<ExpandedNodeId, TreeNode>();
            for (int ii = 0; ii < m_referenceTypeIds.Length; ii++)
            {
                BrowseDescription nodeToBrowse = new BrowseDescription();

                nodeToBrowse.NodeId = ObjectIds.ObjectsFolder;
                nodeToBrowse.BrowseDirection = BrowseDirection.Forward;
                nodeToBrowse.ReferenceTypeId = m_referenceTypeIds[ii];
                nodeToBrowse.IncludeSubtypes = true;
                nodeToBrowse.NodeClassMask = 0;
                nodeToBrowse.ResultMask = (uint)BrowseResultMask.DisplayName;

                if (referenceDescription != null)
                {
                    nodeToBrowse.NodeId = (NodeId)referenceDescription.NodeId;
                    label1.Invoke((MethodInvoker)(() => label1.Text = nodeToBrowse.NodeId.ToString()));
                }

                nodesToBrowse.Add(nodeToBrowse);
            }
            var references = client.BrowseNodeId(nodesToBrowse);

            foreach (var reference in references)
            {


                // ignore out of server references.
                if (reference.NodeId.IsAbsolute)
                {
                    continue;
                }

                if (dictionary.ContainsKey(reference.NodeId))
                {
                    continue;
                }

                if (!reference.NodeId.IsAbsolute)
                {

                    TreeNode child = new TreeNode(reference.ToString());
                    child.Tag = reference;
                    treeNode.TreeView.Invoke((MethodInvoker)(() => treeNode.Nodes.Add(child)));
                    addchiletree(child, reference);
                }
            }


        }

        #endregion
        internal static List<NodeModel> nodeModels = new List<NodeModel>();
        NodeModel nodeModel = new NodeModel();
        List<ServerErrorNodeDescription> serverErrorNodeDescriptions = new List<ServerErrorNodeDescription>();
        OpcUaClient opcUaClient;
        private void button3_Click(object sender, EventArgs e)
        {
            TreeNodeCollection treeNodeCollection = BrowseTV.Nodes;
            TraverseNodes(treeNodeCollection);

            var sss = nodeModels;

        }


        /// <summary>
        /// 遍历所有选择的节点，将节点信息加到节点模型中
        /// </summary>
        /// <param name="nodes"></param>
        private void TraverseNodes(TreeNodeCollection nodes)
        {

            foreach (TreeNode node in nodes)
            {
                if (node.Checked == true )
                {
                    reference = (ReferenceDescription)node.Tag;
                    if (reference != null)
                    {
                        try
                        {
                            Node singelnode = client.m_session.ReadNode(ExpandedNodeId.ToNodeId(reference.NodeId, client.m_session.NamespaceUris));

                            string baseName = singelnode.DisplayName.Text;
                            string text = baseName;
                            BrowseAccess accessLevel = GetAccessLevel(singelnode);

                            //获取类型
                            string @string = client.m_session.NamespaceUris.GetString(singelnode.NodeId.NamespaceIndex);
                            ExpandedNodeId nodeId = new ExpandedNodeId(singelnode.NodeId, @string);
                            if (!IsSupportedNodeIdType(nodeId.IdType))
                            {
                                serverErrorNodeDescriptions.Add(new ServerErrorNodeDescription()
                                {
                                    Description = "Unsupported node id type '{nodeId.IdType}'"
                                });
                            }
                            if (GetAccessLevel(singelnode) == BrowseAccess.Write)
                            {
                                serverErrorNodeDescriptions.Add(new ServerErrorNodeDescription()
                                {
                                    Description = "Cannot handle write only variable " + singelnode.DisplayName + "."
                                });
                            }
                            if (IsVariable(singelnode, out var valueRank, out var dimensions, out var dataType, out var typeName))
                            {
                                 CreateInstanceForVarNode(singelnode, valueRank, dimensions, dataType, typeName, text, accessLevel);
                            }


                        }
                        catch (ServiceResultException)
                        {
                        }

                    }
                }
                if (node.Nodes.Count > 0)
                {
                    TraverseNodes(node.Nodes);
                }

            }

       
        }


        private void CreateInstanceForVarNode(Node varNode, int valueRank, IEnumerable<uint> dimensions, IEnumerable<NodeId> dataType, string typeName, string varName, BrowseAccess browseAccess)
        {

            //if (valueRank >= 0)
            //{
            //    ExpandedNodeId nodeId = Browser.GetNodeId(varNode);
            //    if (opcUaType == null)
            //    {
            //        ErrorOr<IOpcUaType> errorOr = CreateEnumerationType(typeName, varNode);
            //        if (errorOr.HasError)
            //        {
            //            return errorOr.Error;
            //        }
            //        opcUaType = errorOr.Value;
            //    }
            //    return CreateArrayInstance(nodeId, dimensions, typeName, varName, browseAccess, opcUaType);
            //}
            if (valueRank == -1)
            {
                nodeModels.Add(new NodeModel()
                {
                    NodeIDName = varNode.NodeId.Identifier.ToString(),
                    NodeType = typeName,
                    NodeName = varNode.DisplayName.Text,
                });;;
            }
            //return new MappingException($"Cannot handle ValueRank={valueRank}");
        }

        private static bool IsSupportedNodeIdType(IdType idType)
        {
            if ((uint)idType <= 3u)
            {
                return true;
            }
            return false;
        }

        public enum BrowseAccess
        {
            None,
            Read,
            Write,
            ReadWrite,
            Void
        }

        public bool IsVariable(Node node, out int valueRank, out IEnumerable<uint> dimensions, out IEnumerable<NodeId> dataType, out string typeName)
        {
            if (node is VariableNode variableNode)
            {
                valueRank = variableNode.ValueRank;
                dimensions = variableNode.ArrayDimensions;
                try
                {
                    typeName = client.m_session .ReadNode(ExpandedNodeId.ToNodeId(variableNode.DataType, client.m_session.NamespaceUris)).DisplayName.Text;
                    dataType = GetDataTypes(variableNode);
                }
                catch (ServiceResultException ex)
                {
                    if (ex.StatusCode != 2150825984u && ex.StatusCode != 2150891520u)
                    {
                        throw;
                    }
                    typeName = "__MissingType__";
                    dataType = Enumerable.Empty<NodeId>();
                }
                return true;
            }
            valueRank = 0;
            dimensions = null;
            dataType = null;
            typeName = null;
            return false;
        }

        private readonly Func<NodeId, NodeId> GetBaseType_Memoized;
        private IEnumerable<NodeId> GetDataTypes(VariableNode varNode)
        {
            NodeId typeId = varNode.DataType;
            yield return typeId;
            while (true)
            {
                typeId = GetBaseType_Memoized(typeId);
                if (typeId != null)
                {
                    yield return typeId;
                    continue;
                }
                break;
            }
        }

        public BrowseAccess GetAccessLevel(Node node)
        {
            if (node is VariableNode variableNode)
            {
                BrowseAccess browseAccess = BrowseAccess.None;
                if (((uint)variableNode.AccessLevel & (true ? 1u : 0u)) != 0)
                {
                    browseAccess |= BrowseAccess.Read;
                }
                if ((variableNode.AccessLevel & 2u) != 0)
                {
                    browseAccess |= BrowseAccess.Write;
                }
                return browseAccess;
            }
            return BrowseAccess.ReadWrite;
        }

        public bool IsVolatileNode(IAddressSpaceNode node)
        {
            //if (ObjectIds.Server_ServerDiagnostics_SessionsDiagnosticsSummary.Equals(node.NodeId()))
            //{
            //    return true;
            //}
            //if (node is VariableNode variableNode)
            //{
            //    return DataTypeIds.SubscriptionDiagnosticsDataType.Equals(variableNode.DataType);
            //}
            return false;
        }










        public void Recursive(NodeId nodeID)
        {
            ReferenceDescriptionCollection references = GetReferenceDescriptionCollection(nodeID);
            foreach (var reference in references)
            {
                Console.WriteLine(reference.NodeId);
                Recursive((NodeId)reference.NodeId);
            }
        }
        ReferenceDescriptionCollection GetReferenceDescriptionCollection(NodeId sourceId)
        {
            TaskCompletionSource<ReferenceDescriptionCollection> task = new TaskCompletionSource<ReferenceDescriptionCollection>();

            // find all of the components of the node.
            BrowseDescription nodeToBrowse1 = new BrowseDescription();

            nodeToBrowse1.NodeId = sourceId;
            nodeToBrowse1.BrowseDirection = BrowseDirection.Forward;
            nodeToBrowse1.ReferenceTypeId = ReferenceTypeIds.Aggregates;
            nodeToBrowse1.IncludeSubtypes = true;
            nodeToBrowse1.NodeClassMask = (uint)(NodeClass.Object | NodeClass.Variable | NodeClass.Method | NodeClass.ReferenceType | NodeClass.ObjectType | NodeClass.View | NodeClass.VariableType | NodeClass.DataType);
            nodeToBrowse1.ResultMask = (uint)BrowseResultMask.All;

            // find all nodes organized by the node.
            BrowseDescription nodeToBrowse2 = new BrowseDescription();

            nodeToBrowse2.NodeId = sourceId;
            nodeToBrowse2.BrowseDirection = BrowseDirection.Forward;
            nodeToBrowse2.ReferenceTypeId = ReferenceTypeIds.Organizes;
            nodeToBrowse2.IncludeSubtypes = true;
            nodeToBrowse2.NodeClassMask = (uint)(NodeClass.Object | NodeClass.Variable | NodeClass.Method | NodeClass.View | NodeClass.ReferenceType | NodeClass.ObjectType | NodeClass.VariableType | NodeClass.DataType);
            nodeToBrowse2.ResultMask = (uint)BrowseResultMask.All;

            BrowseDescriptionCollection nodesToBrowse = new BrowseDescriptionCollection();
            nodesToBrowse.Add(nodeToBrowse1);
            nodesToBrowse.Add(nodeToBrowse2);

            // fetch references from the server.
            ReferenceDescriptionCollection references = FormUtils.Browse(opcUaClient.Session, nodesToBrowse, false);
            return references;
        }

        private void BrowseTV_AfterSelect(object sender, TreeViewEventArgs e)
        {
           
        }

        private void BrowseTV_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // 当节点被选中时，选中或取消选中其所有子节点
            /*foreach (TreeNode node in e.Node.Nodes)
            {
                node.Checked = e.Node.Checked;
            }*/
        }

        int a = 0;
        private void button4_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(100);
                    if (Session.KeepAliveStopped)
                    {
                        a++;
                    }
                }
            });
           





            //try
            //{
            //    // Thread.Sleep(5000);
            //    if (Session.KeepAliveStopped)
            //    {
            //        return;
            //    }
            //    else
            //    {
            //        Node singelnode = Session.ReadNode("ns=4;s=|var|Sinsegye-x86_64-Linux-SM-CNC.Application.GVL.date1");
            //        label1.Text = singelnode.ToString();
            //    }
            //}
            //catch(Exception ex)
            //{

            //}


        }

        private void button5_Click(object sender, EventArgs e)
        {
            label1.Text = a.ToString();

            label2.Text = Session.KeepAliveStopped.ToString();
        }
    }
}
