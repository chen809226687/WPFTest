using MathNet.Numerics.Interpolation;
using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Export;
using OpcUaHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Accord.Math;
using System.Drawing.Drawing2D;


namespace OPC_UA_Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


            UrlCB.SelectedIndex = 0;
        }

        #region Private Fields
        private ApplicationConfiguration m_configuration = new ApplicationConfiguration()
        {
            ClientConfiguration = new ClientConfiguration(),
        };
        private Session m_session;
        private SessionReconnectHandler m_reconnectHandler;
        private CertificateValidationEventHandler m_CertificateValidation;
        private EventHandler m_ReconnectComplete;
        private EventHandler m_ReconnectStarting;
        private EventHandler m_KeepAliveComplete;
        private EventHandler m_ConnectComplete;
        private StatusStrip m_StatusStrip;
        private ToolStripItem m_ServerStatusLB;
        private ToolStripItem m_StatusUpateTimeLB;
        private Dictionary<Uri, EndpointDescription> m_endpoints;
        #endregion

        #region Private Fields

        private ViewDescription m_view;
        private uint m_maxReferencesReturned;
        private BrowseDirection m_browseDirection;
        private NodeId m_referenceTypeId;
        private bool m_includeSubtypes;
        private uint m_nodeClassMask;
        private uint m_resultMask;
        private event BrowserEventHandler m_MoreReferences;
        private bool m_continueUntilDone;
        private bool m_browseInProgress;





        Browser browse;
        BrowseResultCollection results;
        DiagnosticInfoCollection diagnosticInfos;
        private NodeId[] m_referenceTypeIds = new NodeId[] { Opc.Ua.ReferenceTypeIds.HierarchicalReferences };
        private NodeId m_rootId;

        private Dictionary<NodeId, int> m_typeImageMapping = new Dictionary<NodeId, int>();
        #endregion        
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
                Connect(serverUrl, useSecurity).GetAwaiter().GetResult();


   

                addroottree();
            }
            catch (ServiceResultException sre)
            {
            }


        }

        public bool DisableDomainCheck { get; set; }
        public string SessionName { get; set; } = "sinsegye";
        /// <summary>
        /// Default session values.
        /// </summary>
        public static readonly uint DefaultSessionTimeout = 60000;
        public static readonly int DefaultDiscoverTimeout = 1500000;
        public static readonly int DefaultReconnectPeriod = 1;
        public static readonly int DefaultReconnectPeriodExponentialBackOff = 10;

        /// <summary>
        /// The currently active session. 
        /// </summary>
        public Session Session => m_session;

        /// <summary>
        /// The number of seconds between reconnect attempts (0 means reconnect is disabled).
        /// </summary>
        public int ReconnectPeriod { get; set; } = DefaultReconnectPeriod;

        /// <summary>
        /// The discover timeout in ms.
        /// </summary>
        public int DiscoverTimeout { get; set; } = DefaultDiscoverTimeout;

        /// <summary>
        /// The session timeout in ms.
        /// </summary>
        public uint SessionTimeout { get; set; } = DefaultSessionTimeout;

        /// <summary>
        /// The locales to use when creating the session.
        /// </summary>
        public string[] PreferredLocales { get; set; }
        public IUserIdentity UserIdentity { get; set; }
        /// <summary>
        /// Creates a new session.
        /// </summary>
        /// <returns>The new session object.</returns>
        private async Task<Session> Connect(
            string serverUrl,
            bool useSecurity,
            uint sessionTimeout = 0)
        {
            // disconnect from existing session.
            //InternalDisconnect();

            // select the best endpoint.
            var endpointDescription = CoreClientUtils.SelectEndpoint(m_configuration, serverUrl, useSecurity, DiscoverTimeout);
            var endpointConfiguration = EndpointConfiguration.Create(m_configuration);
            var endpoint = new ConfiguredEndpoint(null, endpointDescription, endpointConfiguration);

            m_session = await Session.Create(
                m_configuration,
                endpoint,
                false,
                false,
                "ApplicationName",
                (uint)DiscoverTimeout,
                UserIdentity,
                PreferredLocales);


            // return the new session.
            return m_session;
  
        }

        private void DoConnectComplete(object state)
        {
            if (m_ConnectComplete != null)
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new System.Threading.WaitCallback(DoConnectComplete), state);
                    return;
                }

                m_ConnectComplete(this, null);
            }
        }



        public static ReferenceDescriptionCollection Browsesss(Session session, ViewDescription view, BrowseDescriptionCollection nodesToBrowse, bool throwOnError)
        {
            try
            {
                ReferenceDescriptionCollection references = new ReferenceDescriptionCollection();

                while (nodesToBrowse.Count > 0)
                {
                    // start the browse operation.
                    BrowseResultCollection results = null;
                    DiagnosticInfoCollection diagnosticInfos = null;

                    ResponseHeader responseHeader = session.Browse(
                        null,
                        view,
                        0,
                        nodesToBrowse,
                        out results,
                        out diagnosticInfos);

                    ClientBase.ValidateResponse(results, nodesToBrowse);
                    ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToBrowse);

                    ByteStringCollection continuationPoints = new ByteStringCollection();
                    BrowseDescriptionCollection unprocessedOperations = new BrowseDescriptionCollection();

                    for (int ii = 0; ii < nodesToBrowse.Count; ii++)
                    {
                        // check for error.
                        if (StatusCode.IsBad(results[ii].StatusCode))
                        {
                            // this error indicates that the server does not have enough simultaneously active 
                            // continuation points. This request will need to be resent after the other operations
                            // have been completed and their continuation points released.
                            if (results[ii].StatusCode == StatusCodes.BadNoContinuationPoints)
                            {
                                unprocessedOperations.Add(nodesToBrowse[ii]);
                            }

                            continue;
                        }

                        // check if all references have been fetched.
                        if (results[ii].References.Count == 0)
                        {
                            continue;
                        }

                        // save results.
                        references.AddRange(results[ii].References);

                        // check for continuation point.
                        if (results[ii].ContinuationPoint != null)
                        {
                            continuationPoints.Add(results[ii].ContinuationPoint);
                        }
                    }

                    // process continuation points.
                    while (continuationPoints.Count > 0)
                    {
                        // continue browse operation.
                        session.BrowseNext(
                            null,
                            false,
                            continuationPoints,
                            out results,
                            out diagnosticInfos);

                        ClientBase.ValidateResponse(results, continuationPoints);
                        ClientBase.ValidateDiagnosticInfos(diagnosticInfos, continuationPoints);

                        ByteStringCollection revisedContinuationPoints = new ByteStringCollection();
                        for (int ii = 0; ii < continuationPoints.Count; ii++)
                        {
                            // check for error.
                            if (StatusCode.IsBad(results[ii].StatusCode))
                            {
                                continue;
                            }

                            // check if all references have been fetched.
                            if (results[ii].References.Count == 0)
                            {
                                continue;
                            }

                            // save results.
                            references.AddRange(results[ii].References);

                            // check for continuation point.
                            if (results[ii].ContinuationPoint != null)
                            {
                                revisedContinuationPoints.Add(results[ii].ContinuationPoint);
                            }
                            if (references.Count == 10000)
                            {

                            }
                        }

                        // check if browsing must continue;
                        continuationPoints = revisedContinuationPoints;
                    }

                    // check if unprocessed results exist.
                    nodesToBrowse = unprocessedOperations;
                }

                // return complete list.
                return references;
            }
            catch (Exception exception)
            {
                if (throwOnError)
                {
                    throw new ServiceResultException(exception, StatusCodes.BadUnexpectedError);
                }

                return null;
            }
        }

        public static List<NodeId> TranslateBrowsePaths(
           Session session,
           NodeId startNodeId,
           NamespaceTable namespacesUris,
           params string[] relativePaths)
        {
            // build the list of browse paths to follow by parsing the relative paths.
            BrowsePathCollection browsePaths = new BrowsePathCollection();

            if (relativePaths != null)
            {
                for (int ii = 0; ii < relativePaths.Length; ii++)
                {
                    BrowsePath browsePath = new BrowsePath();

                    // The relative paths used indexes in the namespacesUris table. These must be 
                    // converted to indexes used by the server. An error occurs if the relative path
                    // refers to a namespaceUri that the server does not recognize.

                    // The relative paths may refer to ReferenceType by their BrowseName. The TypeTree object
                    // allows the parser to look up the server's NodeId for the ReferenceType.

                    browsePath.RelativePath = RelativePath.Parse(
                        relativePaths[ii],
                        session.TypeTree,
                        namespacesUris,
                        session.NamespaceUris);

                    browsePath.StartingNode = startNodeId;

                    browsePaths.Add(browsePath);
                }
            }

            // make the call to the server.
            BrowsePathResultCollection results;
            DiagnosticInfoCollection diagnosticInfos;

            ResponseHeader responseHeader = session.TranslateBrowsePathsToNodeIds(
                null,
                browsePaths,
                out results,
                out diagnosticInfos);

            // ensure that the server returned valid results.
            Session.ValidateResponse(results, browsePaths);
            Session.ValidateDiagnosticInfos(diagnosticInfos, browsePaths);

            // collect the list of node ids found.
            List<NodeId> nodes = new List<NodeId>();

            for (int ii = 0; ii < results.Count; ii++)
            {
                // check if the start node actually exists.
                if (StatusCode.IsBad(results[ii].StatusCode))
                {
                    nodes.Add(null);
                    continue;
                }

                // an empty list is returned if no node was found.
                if (results[ii].Targets.Count == 0)
                {
                    nodes.Add(null);
                    continue;
                }

                // Multiple matches are possible, however, the node that matches the type model is the
                // one we are interested in here. The rest can be ignored.
                BrowsePathTarget target = results[ii].Targets[0];

                if (target.RemainingPathIndex != UInt32.MaxValue)
                {
                    nodes.Add(null);
                    continue;
                }

                // The targetId is an ExpandedNodeId because it could be node in another server. 
                // The ToNodeId function is used to convert a local NodeId stored in a ExpandedNodeId to a NodeId.
                nodes.Add(ExpandedNodeId.ToNodeId(target.TargetId, session.NamespaceUris));
            }

            // return whatever was found.
            return nodes;
        }





        private const int Attribute = 0;
        private const int Property = 1;
        private const int Variable = 2;
        private const int Method = 3;
        private const int Object = 4;
        private const int OpenFolder = 5;
        private const int ClosedFolder = 6;
        private const int ObjectType = 7;
        private const int View = 8;
        private const int Reference = 9;
        private const int NumberValue = 10;
        private const int StringValue = 11;
        private const int ByteStringValue = 12;
        private const int StructureValue = 13;
        private const int ArrayValue = 14;
        private const int InputArgument = 15;
        private const int OutputArgument = 16;
        public static int GetImageIndex(Session session, NodeClass nodeClass, ExpandedNodeId typeDefinitionId, bool selected)
        {
            if (nodeClass == NodeClass.Variable)
            {
                if (session.NodeCache.IsTypeOf(typeDefinitionId, Opc.Ua.VariableTypeIds.PropertyType))
                {
                    return Property;
                }

                return Variable;
            }

            if (nodeClass == NodeClass.Object)
            {
                if (session.NodeCache.IsTypeOf(typeDefinitionId, Opc.Ua.ObjectTypeIds.FolderType))
                {
                    if (selected)
                    {
                        return OpenFolder;
                    }
                    else
                    {
                        return ClosedFolder;
                    }
                }

                return Object;
            }

            if (nodeClass == NodeClass.Method)
            {
                return Method;
            }

            if (nodeClass == NodeClass.View)
            {
                return View;
            }

            return ObjectType;
        }



        private void setChildNodeCheckedState(TreeNode currNode, bool state)
        {
            TreeNodeCollection nodes = currNode.Nodes;
            if (nodes.Count > 0)
            {
                foreach (TreeNode tn in nodes)
                {
                    tn.Checked = state;
                    setChildNodeCheckedState(tn, state);
                }
            }
        }

        private void setParentsNodeCheckedState(TreeNode currNode, bool state)
        {
            TreeNode nodes = currNode.LastNode;

            if (nodes != null)
            {

                nodes.Checked = state;
                setParentsNodeCheckedState(nodes, state);

            }


        }

        private void BrowseTV_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {

        }


        List<NodeModel> nodeModels = new List<NodeModel>();

        private void BrowseTV_AfterCheck(object sender, TreeViewEventArgs e)
        {
            //ReferenceDescription reference = (ReferenceDescription)e.Node.Tag;
            //if (reference != null)
            //{
            //    if ( reference.NodeClass == NodeClass.Variable)
            //    {
            //        VariableNode NodeDate = (VariableNode)m_session.ReadNode(reference.NodeId.ToString());

            //        if (NodeDate.AccessLevel == 3)
            //        {

            //            nodeModel.NodeIDName = reference.NodeId.ToString();

            //            if (e.Node.Checked)
            //            {
            //                nodeModels.Add(nodeModel);
            //            }
            //            else if(nodeModels.Contains(nodeModel))
            //            {
            //                nodeModels.Remove(nodeModel);
            //            }

            //        }
            //    }

            //}

        }


        //取消节点选中状态之后，取消所有父节点的选中状态
        private void setParentNodeCheckedState(TreeNode currNode, bool state)
        {
            TreeNode parentNode = currNode.Parent;
            parentNode.Checked = state;
            if (currNode.Parent.Parent != null)
            {
                setParentNodeCheckedState(currNode.Parent, state);
            }
        }



        private void addroottree()
        {
            try
            {
                TreeNode child = null;
                BrowseDescriptionCollection nodesToBrowse = new BrowseDescriptionCollection();
                ReferenceDescription reference = new ReferenceDescription();
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
                ReferenceDescriptionCollection references = Browsesss(m_session, null, nodesToBrowse, false);

                for (int ii = 0; references != null && ii < references.Count; ii++)
                {
                    reference = references[ii];

                    // ignore out of server references.
                    if (reference.NodeId.IsAbsolute)
                    {
                        continue;
                    }

                    if (dictionary.ContainsKey(reference.NodeId))
                    {
                        continue;
                    }

                    child = new TreeNode(reference.ToString());
                    BrowseTV.Nodes.Add(child);

                    Task.Run(() => addchiletree(child, reference));


                }



                BrowseTV.Show();

            }

            catch (Exception exception)
            {

            }
        }



        private void addchiletree(TreeNode treeNode, ReferenceDescription referenceDescription)
        {
            try
            {
                BrowseDescriptionCollection nodesToBrowse = new BrowseDescriptionCollection();
                ReferenceDescription reference = new ReferenceDescription();
                reference = referenceDescription;
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

                    if (reference != null)
                    {
                        nodeToBrowse.NodeId = (NodeId)reference.NodeId;
                    }

                    nodesToBrowse.Add(nodeToBrowse);
                }
                ReferenceDescriptionCollection references = Browsesss(m_session, null, nodesToBrowse, false);

                for (int ii = 0; references != null && ii < references.Count; ii++)
                {
                    reference = references[ii];

                    // ignore out of server references.
                    if (reference.NodeId.IsAbsolute)
                    {
                        continue;
                    }

                    if (dictionary.ContainsKey(reference.NodeId))
                    {
                        continue;
                    }

                    TreeNode child = new TreeNode(reference.ToString());

                    treeNode.Nodes.Add(child);

                    child.Tag = reference;
                    addchiletree(child, reference);
                }
            }

            catch (Exception exception)
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BrowseTV.Nodes.Clear();
        }

        private void BrowseTV_AfterSelect(object sender, TreeViewEventArgs e)
        {







        }

        ReferenceDescription reference = new ReferenceDescription();
        NodeModel nodeModel;
        private void TraverseNodes(TreeNodeCollection nodes)
        {


            try
            {
                foreach (TreeNode node in nodes)
                {
                    if (node.Checked == true)
                    {
                        reference = (ReferenceDescription)node.Tag;
                        if (reference != null)
                        {
                            if (reference.NodeClass == NodeClass.Variable)
                            {
                                VariableNode NodeDate = (VariableNode)m_session.ReadNode(reference.NodeId.ToString());
                                if (true)
                                {
                                    nodeModel = new NodeModel();

                                    var AAAAA = TypeInfo.GetBuiltInType(NodeDate.DataType.ToString());


                                    if (TypeInfo.GetBuiltInType(NodeDate.DataType.ToString()) < (BuiltInType)29 && TypeInfo.GetBuiltInType(NodeDate.DataType.ToString()) > (BuiltInType)0)
                                    {
                                        nodeModel.NodeType = TypeInfo.GetBuiltInType(NodeDate.DataType).ToString();

                                    }
                                    else
                                    {
                                        DataValue value = m_session.ReadValue(reference.NodeId.ToString());
                                        if (value.WrappedValue.TypeInfo != null)
                                        {
                                            nodeModel.NodeType = value.WrappedValue.TypeInfo.BuiltInType.ToString();
                                        }
                                        else
                                        {
                                            continue;
                                        }

                                    }

                                    if (nodeModel.NodeType == "ExtensionObject")
                                    {
                                        continue;
                                    }

                                    nodeModel.NodeName = NodeDate.DisplayName.ToString();
                                    nodeModel.IECName = node.FullPath.Replace("\\", "_");
                                    nodeModel.NodeIDName = reference.NodeId.ToString();
                                    nodeModel.AccessLevelType = (AccessLevelType)NodeDate.AccessLevel;

                                    nodeModels.Add(nodeModel);


                                }

                            }
                        }
                    }
                    if (node.Nodes.Count > 0)
                    {
                        TraverseNodes(node.Nodes);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("节点信息有误，添加失败！");
            }



        }




        public static VariableNode GetVariableNode()
        {
            // 你的实现代码
            return null;
        }

        private void button3_Click(object sender, EventArgs e)
        {


            TraverseNodes(BrowseTV.Nodes);



        }

        private void BrowseTV_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {

            if (e.Action == TreeViewAction.ByMouse)
            {

                if (e.Node.Checked == true)
                {
                    //选中节点之后，选中该节点所有的子节点
                    setChildNodeCheckedState(e.Node, true);
                    setParentsNodeCheckedState(e.Node, true);
                }
                else if (e.Node.Checked == false)
                {
                    //取消节点选中状态之后，取消该节点所有子节点选中状态
                    setChildNodeCheckedState(e.Node, false);
                    //如果节点存在父节点，取消父节点的选中状态
                    if (e.Node.Parent != null)
                    {
                        setParentNodeCheckedState(e.Node, false);
                    }
                }

            }

        }



        List<aaaa> aaaas = new List<aaaa>();

        aaaa aaaa1 = new aaaa();
        aaaa aaaa2 = new aaaa();


        private void button4_Click(object sender, EventArgs e)
        {
            var aaaa = nodeModels;


        }

        private void button5_Click(object sender, EventArgs e)
        {
            // stop any reconnect operation.
            if (m_reconnectHandler != null)
            {
                m_reconnectHandler.Dispose();
                m_reconnectHandler = null;
            }

            // disconnect any existing session.
            if (m_session != null)
            {
                m_session.Close(10000);
                m_session = null;
            }

        }



        private void BrowseTV_AfterCheck_1(object sender, TreeViewEventArgs e)
        {

        }

        private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;

                // 递归调用以检查所有子节点
                CheckAllChildNodes(node, nodeChecked);
            }
        }

        private void CheckAllParentNodes(TreeNode treeNode, bool nodeChecked)
        {

        }


        private void BrowseTV_BeforeCheck_1(object sender, TreeViewCancelEventArgs e)
        {
            CheckAllParentNodes(e.Node, e.Node.Checked);
        }
        private OpcUaClient opcUaClient = new OpcUaClient();
        private void button6_Click(object sender, EventArgs e)
        {
            opcUaClient = new OpcUaClient();
            opcUaClient.UserIdentity = new UserIdentity(new AnonymousIdentityToken());
            opcUaClient.ConnectServer(@"opc.tcp://192.168.110.8");

            if (opcUaClient.Connected)

            {
                //打印Objects节点下所有NodeId
                Recursive(ObjectIds.ObjectsFolder);

            }
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

        class aaaa
        {

            public string Name { get; set; }

            public int Number { get; set; }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpcUaHelper.Forms.FormBrowseServer formBrowseServer = new OpcUaHelper.Forms.FormBrowseServer();
            formBrowseServer.ShowDialog();
        }

        public double Interpolate(double xi)
        {
            int index = Array.BinarySearch(x, xi);
            if (index < 0)
            {
                index = ~index - 1;
                if (index < 0)
                {
                    index = 0;
                }
                if (index >= x.Length - 1)
                {
                    index = x.Length - 2;
                }
            }

            double a = (x[index + 1] - xi) / h[index];
            double b = (xi - x[index]) / h[index];
            return a * y[index] + b * y[index + 1] + ((a * a * a - a) * m[index] + (b * b * b - b) * m[index + 1]) * h[index] * h[index] / 6;
        }


        public void SplineInterpolation(double[] x, double[] y)
        {
            this.x = x;
            this.y = y;
            this.h = new double[x.Length - 1];
            this.m = new double[x.Length];

            for (int i = 0; i < x.Length - 1; i++)
            {
                h[i] = x[i + 1] - x[i];
            }

            double[] alpha = new double[x.Length - 1];
            for (int i = 1; i < x.Length - 1; i++)
            {
                alpha[i] = 3 * (y[i + 1] - y[i]) / h[i] - 3 * (y[i] - y[i - 1]) / h[i - 1];
            }

            double[] l = new double[x.Length];
            double[] z = new double[x.Length];
            l[0] = 1;
            z[0] = 0;
            m[0] = m[m.Length - 1] = 0;

            for (int i = 1; i < x.Length - 1; i++)
            {
                l[i] = 2 * (x[i + 1] - x[i - 1]) - h[i - 1] * z[i - 1];
                z[i] = h[i] / l[i];
                m[i] = (alpha[i] - h[i - 1] * m[i - 1]) / l[i];
            }

            for (int j = x.Length - 2; j >= 0; j--)
            {
                m[j] = m[j] - z[j] * m[j + 1];
            }
        }






        private double[] x;
        private double[] y;
        private double[] m;
        private double[] h;





        private void button8_Click(object sender, EventArgs e)
        {

           

        }

        private void UrlCB_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void BrowseTV_AfterSelect_1(object sender, TreeViewEventArgs e)
        {

        }
    }
}
