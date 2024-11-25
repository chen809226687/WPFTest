using Opc.Ua;
using Opc.Ua.Client;
using Sinsegye.Ide.Resources.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OPC_UA_WPF
{

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private CheckTreeView checkTreeView;

        public MainWindow()
        {
            InitializeComponent();
            checkTreeView = (CheckTreeView)DataContext;
        }


        #region Private Fields
        private NodeId[] m_referenceTypeIds = new NodeId[] { Opc.Ua.ReferenceTypeIds.HierarchicalReferences };
        public ApplicationConfiguration m_configuration = new ApplicationConfiguration()
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

        private Dictionary<Uri, EndpointDescription> m_endpoints;
        #endregion

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            //WriteVariableList.Items.Clear();
            //WriteVariableList.Items.Add("Please select a variable to wrewddcxcssdcaslease select a variable to wrewddcxcssdcascdcsadsadasdsadilease select a variable to wrewddcxcssdcascdcsadsadasdsadicdcsadsadasdsadite");
            //WriteVariableList.Items.Add("Please select 1 variable to redsadcsdcasdasdsadasdad");
            //WriteVariableList.Items.Add("Please select 2 variable to redsadcsdcasdasdsadasdad");
            //WriteVariableList.Items.Add("Please select 3 variable to redsadcsdcasdasdsadasdad");
            //WriteVariableList.Items.Add("Please select 4 variable to redsadcsdcasdasdsadasdad");

            //WriteVariableList.Items.Add("Please select 5 variable to redsadcsdcasdasdsadasdad");

            try
            {
                Task taskconnect = Task.Run(() =>
                {
                   Connect("opc.tcp://192.168.110.8", false);
              
                });
                taskconnect.Wait();
                Thread.Sleep(100);
                addroottree();
                await Task.WhenAll(tasks);
                checkTreeView.LabelText = "节点加载完成";
            }
            catch (ServiceResultException sre)
            {
            }
    
          



        }

        BrowseDescriptionCollection nodesToBrowse = new BrowseDescriptionCollection();
        ReferenceDescription reference = new ReferenceDescription();
        List<Task> tasks = new List<Task>();

        public List<CheckTreeView> AllChildrenView { get; set; } = new List<CheckTreeView>();

        private void addroottree()
        {

            try
            {
                checkTreeView.LabelText = "Root";
                Application.Current.Dispatcher.Invoke(() =>
                {
                    checkView.Items.Clear();
                });
                SortedDictionary<ExpandedNodeId, CheckTreeView> dictionary = new SortedDictionary<ExpandedNodeId, CheckTreeView>();


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


                ReferenceDescriptionCollection references = BrowsesVar(nodesToBrowse);
                // 将 ReferenceDescriptionCollection 转换为 List<ReferenceDescription>
                List<ReferenceDescription> referenceList = new List<ReferenceDescription>(references);

                // 使用排序方法对 List 进行排序
                referenceList.Sort((refDesc1, refDesc2) => string.Compare(refDesc1.DisplayName.Text, refDesc2.DisplayName.Text));

                // 将排序后的引用重新转换为 ReferenceDescriptionCollection
                references.Clear();
                foreach (ReferenceDescription refDesc in referenceList)
                {
                    references.Add(refDesc);
                }

                foreach (var reference in references)
                {
                    CheckTreeView child = new CheckTreeView() { ViewName = reference.DisplayName.Text };
                    Task task = Task.Run(() =>
                    {
                        checkTreeView.ChildrenView.Add(child);
                        addchiletree(child, reference);
                       // checkTreeView.ChildrenView.Add(child);
                        AllChildrenView.Add(child);
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            checkView.Items.Add(child);
                            checkTreeView.LabelText = "请稍等，正在加载节点数据(" + reference.DisplayName.Text + ")";
                        });
                    });


                    tasks.Add(task);

                }

            }
            catch (Exception ex)
            {

            }
        }


        private void addchiletree(CheckTreeView treeNode, ReferenceDescription referenceDescription)
        {

            try
            {
                if (referenceDescription != null)
                {

                    nodesToBrowse.Clear();
                    SortedDictionary<ExpandedNodeId, CheckTreeView> dictionary = new SortedDictionary<ExpandedNodeId, CheckTreeView>();
                    for (int ii = 0; ii < m_referenceTypeIds.Length; ii++)
                    {
                        BrowseDescription nodeToBrowse = new BrowseDescription();
                        nodeToBrowse.BrowseDirection = BrowseDirection.Forward;
                        nodeToBrowse.ReferenceTypeId = m_referenceTypeIds[ii];
                        nodeToBrowse.IncludeSubtypes = true;
                        nodeToBrowse.NodeClassMask = 0;
                        nodeToBrowse.ResultMask = (uint)BrowseResultMask.All;

                        if (referenceDescription != null)
                        {
                            nodeToBrowse.NodeId = (NodeId)referenceDescription.NodeId;
                        }
                        else
                        {
                            return;
                        }

                        nodesToBrowse.Add(nodeToBrowse);
                    }
                    ReferenceDescriptionCollection references = BrowsesVar(nodesToBrowse);
                    referenceDescription = null;
                    int count = 0;
                    // 将 ReferenceDescriptionCollection 转换为 List<ReferenceDescription>
                    List<ReferenceDescription> referenceList = new List<ReferenceDescription>(references);

                    // 使用排序方法对 List 进行排序
                    referenceList.Sort((refDesc1, refDesc2) => string.Compare(refDesc1.DisplayName.Text, refDesc2.DisplayName.Text));

                    // 将排序后的引用重新转换为 ReferenceDescriptionCollection
                    references.Clear();
                    foreach (ReferenceDescription refDesc in referenceList)
                    {
                        references.Add(refDesc);
                    }
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
                            CheckTreeView child = new CheckTreeView() { ViewName = reference.DisplayName.Text };

                            child.Tag = reference;
                            child.Parent = treeNode;
                            treeNode.ChildrenView.Add(child);
                            checkTreeView.LabelText = reference.DisplayName.Text;
                            addchiletree(child, reference);
                        }
                    }
                }

            }
            catch (Exception ex)
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



        public ReferenceDescriptionCollection BrowsesVar(BrowseDescriptionCollection nodesToBrowse)
        {
            if (nodesToBrowse.Count == 0)
            {
                return null;
            }
            //try
            //{
            ReferenceDescriptionCollection references = new ReferenceDescriptionCollection();

            while (nodesToBrowse.Count > 0)
            {
                // start the browse operation.
                BrowseResultCollection results = null;
                DiagnosticInfoCollection diagnosticInfos = null;

                ResponseHeader responseHeader = m_session.Browse(
                    null,
                    null,
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
                    m_session.BrowseNext(
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
            //}
            //catch (Exception exception)
            //{
            //    SystemInstances.Engine.MessageService.Error("浏览单个节点失败！" + exception.Message);
            //    return null;
            //}
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

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {




            CheckBox ck = (CheckBox)sender;
            try
            {
                int fid = Convert.ToInt32(ck.Tag);
                //新建存储过程 修改物标显示DISPLAY字段 fid 为对应物标id
            }
            catch (FormatException es)
            {
                //说明是群组id
                string ids = ck.Tag.ToString().Trim('.');
                //新建存储过程 修改物标显示DISPLAY字段 ids 为对应群组 id
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {



            //foreach (var item in AllChildrenView)
            //{
            //    foreach (var child in item.ChildrenView)
            //    {
            //        var asas = child.FullPath;
            //    }
            //}
            //var aaa = WriteVariableList.SelectedItems;

            //// 遍历所有选中的项
            //foreach (var selectedItem in WriteVariableList.SelectedItems.Cast<object>().ToList())
            //{
            //    // 从列表中移除选中的项
            //    WriteVariableList.Items.Remove(selectedItem);
            //}



        }
    }
}
