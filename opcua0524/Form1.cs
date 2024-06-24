using Opc.Ua.Client;
using Opc.Ua;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace opcua0524
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



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
        private void button1_Click(object sender, EventArgs e)
        {

           Connect(@"opc.tcp://192.168.110.8", false).GetAwaiter().GetResult();


            VariableNode NodeDate = (VariableNode)m_session.ReadNode("ns=4;s=|var|Sinsegye-x86_64-Linux-SM-CNC.Application.G.LM_IO");
           var aaa= TypeInfo.GetBuiltInType(NodeDate.DataType.ToString());

            DataValue value = m_session.ReadValue("ns=4;s=|var|Sinsegye-x86_64-Linux-SM-CNC.Application.G.LM_IO");

        }


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

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
