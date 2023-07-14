using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WebApiLogViewGUI.Model;

using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;

namespace WebApiLogViewGUI.Service
{
    class LogManager
    {
        private static LogManager instance;



        public static LogManager GetInstance()
        {
            // 如果类的实例不存在则创建，否则直接返回
            if (instance == null)
            {
                instance = new LogManager();
            }
            return instance;
        }



        public ObservableCollection<LogModel> Logs { get; }


        public string IP { get; } = "";
        public int Port { get; set; }

        public const string kNewLogModel = "kNewLogModel";


        LogManager()
        {
            Logs = new ObservableCollection<LogModel>();

            Task.Run(() =>
            {

                // 创建UdpClient对象，并指定要监听的端口号
                UdpClient udpClient = new UdpClient(12111);

                try
                {
                    // 接收数据
                    while (true)
                    {
                        IPEndPoint remoteEP = null;
                        byte[] data = udpClient.Receive(ref remoteEP);
                        Debug.WriteLine("Received data from {0}:", remoteEP.ToString());
                        Debug.WriteLine(Encoding.ASCII.GetString(data));
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
                finally
                {
                    udpClient.Close();
                }

            });


            Task.Run(async () => 
            {
                await Task.Delay(3000);
                // 创建UdpClient对象，并指定要监听的端口号
                UdpClient udpClient = new UdpClient();

                try
                {
                    UdpClient client = new UdpClient();
                    byte[] data = System.Text.Encoding.UTF8.GetBytes("Hello, world!");
                    client.Send(data, data.Length, "172.16.1.24", 12111);
                    client.Close();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
                finally
                {
                    udpClient.Close();
                }

            });

        }

        public string GetAddress()
        {
            return $"{IP}:{Port}";
        }

        public void Stop()
        {
            //RestManager.Stop();
        }

        public void Test()
        {

            Logs.Add(new LogModel(4, $"test{2222}"));
            Logs.Add(new LogModel(5, $"test{1111}"));
            for (int i = 0; i < 100; i++)
            {
                // Logs.Add();

                LogCallback(new LogModel(1, $"test{i}"));
            }
        }

        public void ClearAll()
        {
            Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
            {
                Logs.Clear();
            });
        }

        public string Save()
        {
            string fileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".log";
            using (FileStream fsWrite = new FileStream(fileName, FileMode.Append))
            {
                foreach (var item in Logs)
                {
                    byte[] line = System.Text.Encoding.UTF8.GetBytes($"{item.Time} [{item.LevelString}] {item.Message} \r\n" );
                    fsWrite.Write(line, 0, line.Length);
                }

            };
            return fileName;
        }

        void LogCallback(LogModel model)
        {

            Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
            {
                Logs.Add(model);
            });

            GlobalNotification.Default.Post(kNewLogModel, model);

            //Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action<LogModel>((x) =>
            //    {
            //        Logs.Add(model);
            //    }), model);

            //    Dispatcher.CurrentDispatcher.Invoke(() =>
            //    {
            //        Logs.Add(model);
            //    }, DispatcherPriority.Normal);

            //switch (level)
            //{
            //    case 1:
            //        {
            //            message.WriteInfoLine();
            //            break;
            //        };
            //    case 2:
            //        {
            //            message.WriteInfoLine();
            //            break;
            //        }
            //    case 3:
            //        {
            //            message.WriteWarningLine();
            //            break;
            //        }
            //    case 4:
            //        {
            //            message.WriteErrorLine();
            //            break;
            //        }
            //    case 5:
            //        {
            //            message.WriteErrorLine();
            //            break;
            //        }
            //    default:
            //        message.WriteInfoLine();
            //        break;
            //}

        }




    }
}
