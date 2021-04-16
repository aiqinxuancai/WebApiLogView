using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using WebApiLogCore.Services.Rest;
using System.Net.NetworkInformation;
using System.IO.MemoryMappedFiles;
using System.Diagnostics;
//using NetFwTypeLib;
using WebApiLogCore.Base;

namespace WebApiLogCore.Services
{
    public static class RestManager
    {
        const int START_PORT = 12100;

        static int _servicePort = 0;

        static MemoryMappedFile _memoryMappedPort = null;

        static RestServer _httpServer = new RestServer(5);

        /// <summary>
        /// 返回端口号
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static int Start (Action<LogModel> callback)
        {
            //端口自动
            for (int i = START_PORT; i < START_PORT + 100; i++)
            {
                if (PortInUse(i) == false)
                {
                    Console.WriteLine("使用端口{0}", i);
                    //NetFwAddApps("WebApiLogView", i);
                    //NetFwAddApps("WebApiLogView", i);
                    //无占用 启动
                    if (_httpServer.Start(i, callback))
                    {
                        _servicePort = i;
                    }
                    break;
                }
            }
            return _servicePort;
        }
        public static void Stop()
        {
            _httpServer.Stop();
        }

        

        /// <summary>
        /// 检查本地某个TCP端口是否在使用中（监听中）
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool PortInUse(int port)
        {
            bool inUse = false;

            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();

            foreach (IPEndPoint endPoint in ipEndPoints)
            {
                if (endPoint.Port == port)
                {
                    inUse = true;
                    break;
                }
            }
            return inUse;
        }

        /// <summary>
        /// 获取本地IP地址信息
        /// </summary>
        public static string GetAddressIP()
        {
            string localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }

            return localIP;
        }

        ///// <summary>
        ///// 将应用程序添加到防火墙例外
        ///// </summary>
        ///// <param name="name">应用程序名称</param>
        ///// <param name="executablePath">应用程序可执行文件全路径</param>
        //public static void NetFwAddApps(string name, int port)
        //{
        //    INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
        //    foreach (INetFwRule item in firewallPolicy.Rules)
        //    {
        //        if (port.ToString() == item.LocalPorts)
        //        {
        //            //已经存在
        //            return;
        //        }
        //    }

        //    INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
        //    firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
        //    firewallRule.Description = "WebApiLogView."; //<✅DEBUG><Default>
        //    firewallRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
        //    firewallRule.Enabled = true;
        //    firewallRule.InterfaceTypes = "All";
        //    firewallRule.Name = name;
        //    //firewallRule.ApplicationName = Process.GetCurrentProcess().MainModule.FileName;
        //    firewallRule.Protocol = (int)NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP;
        //    firewallRule.LocalPorts = port.ToString();

        //    firewallPolicy.Rules.Add(firewallRule);



        //}
    }

}
