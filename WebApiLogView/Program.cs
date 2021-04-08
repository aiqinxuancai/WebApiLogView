
using System;
using System.IO;
using System.Diagnostics;
using WebApiLogCore.Services;
using WebApiLogCore.Base;

namespace WebApiLogView
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("正在启动服务...");
            Console.WriteLine(Process.GetCurrentProcess().MainModule.FileName);
 

            RestManager.Start(LogCallback);
            //Console.WriteLine("以下IP可访问：");
            //RestManager.GetAddressIP();
            "等待日志传入...".WriteSuccessLine();
            Console.ReadKey();
        }

        static void LogCallback(LogModel model)
        {
            int level = model.Level;
            string message = model.Message;

            switch (level)
            {
                case 1:
                    {
                        message.WriteInfoLine();
                        break;
                    };
                case 2:
                    {
                        message.WriteInfoLine();
                        break;
                    }
                case 3:
                    {
                        message.WriteWarningLine();
                        break;
                    }
                case 4:
                    {
                        message.WriteErrorLine();
                        break;
                    }
                case 5:
                    {
                        message.WriteErrorLine();
                        break;
                    }
                default:
                    message.WriteInfoLine();
                    break;
            }

        }

    }
}
