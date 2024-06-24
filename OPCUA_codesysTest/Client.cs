using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Export;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;

namespace OPCUA_codesysTest
{
    public sealed class Client : IDisposable
    {
        public Session m_session;
        private ApplicationConfiguration m_configuration = new ApplicationConfiguration()
        {
            ClientConfiguration = new ClientConfiguration(),
        };
        /// <summary>
        /// The discover timeout in ms.
        /// </summary>
        public int DiscoverTimeout { get; set; } = 15000000;
        public IUserIdentity UserIdentity { get; set; }
        public string[] PreferredLocales { get; set; } = { "zh-CN", "en" };


        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="serverUrl"></param>
        /// <param name="useSecurity"></param>
        /// <param name="sessionTimeout"></param>
        /// <returns></returns>
        public async Task<Session> Connect(
             string serverUrl,
             bool useSecurity = true,
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
                "ApplicationNam",
                (uint)DiscoverTimeout,
                UserIdentity,
                null);


            // return the new session.
            return m_session;
        }







        private NodeId[] m_referenceTypeIds = new NodeId[] { Opc.Ua.ReferenceTypeIds.HierarchicalReferences };

        ByteStringCollection continuationPoints = new ByteStringCollection();
        public List<ReferenceDescription> BrowseNodeId(BrowseDescriptionCollection nodesToBrowse)
        {

            if (nodesToBrowse.Count == 0)
            {
                return null;
            }
            List<ReferenceDescription> list = new List<ReferenceDescription>();
            byte[] revisedContinuationPoint = null;
            do
            {
                ReferenceDescriptionCollection references;
                if (revisedContinuationPoint == null)
                {
                    m_session.Browse(null, null, nodesToBrowse[0].NodeId, 0u, BrowseDirection.Forward, nodesToBrowse[0].ReferenceTypeId, includeSubtypes: true, 0u, out revisedContinuationPoint, out references);
                }
                else
                {
                    m_session.BrowseNext(null, releaseContinuationPoint: false, revisedContinuationPoint, out revisedContinuationPoint, out references);
                }
                if (references != null || references.Count != 0)
                {

                    list.AddRange(references);
                    if (revisedContinuationPoint == null)
                    {
                        return list;
                    }

                }
                if (list.Count == 10000)
                {

                }

            }
            while (revisedContinuationPoint != null);
            return list;



        }

        //public ReferenceDescriptionCollection BrowseNodeId(BrowseDescriptionCollection nodesToBrowse)
        //{

        //    // start the browse operation.
        //    BrowseResultCollection results = null;
        //    DiagnosticInfoCollection diagnosticInfos = null;

        //    ResponseHeader responseHeader = m_session.Browse(
        //               null,
        //               null,
        //               0,
        //               nodesToBrowse,
        //               out results,
        //               out diagnosticInfos);

        //    return new ReferenceDescriptionCollection();

        //}







        public void Dispose()
        {


            throw new NotImplementedException();
        }
    }
}
