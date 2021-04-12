
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
            string nowTime = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

            Console.WriteLine("正在启动服务...");
            Console.WriteLine(Process.GetCurrentProcess().MainModule.FileName);
 

            RestManager.Start(LogCallback);
            //Console.WriteLine("以下IP可访问：");
            //RestManager.GetAddressIP();
            "等待日志传入...".WriteSuccessLine();
            //Console.ReadKey();

            FileStream ostrm;
            StreamWriter writer;
            TextWriter oldOut = Console.Out;
            try
            {
                ostrm = new FileStream($"./{nowTime}.txt", FileMode.OpenOrCreate, FileAccess.Write);
                writer = new StreamWriter(ostrm);
                writer.AutoFlush = true;
                Console.SetOut(writer);
                Console.SetError(writer);
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot open Redirect.txt for writing");
                Console.WriteLine(e.Message);
                return;
            }


            bool stop = false;
            while (true)
            {
                if (stop)
                {
                    break;
                }
                string cmd = Console.ReadLine();
                switch (cmd)
                {
                    case "save":
                        {
                            Console.WriteLine("已存储");
                            break;
                        }
                    case "exit":
                        {
                            stop = true;
                            break;
                        }
                }

            }
            writer.Close();
            ostrm.Close();
            Console.WriteLine("已经退出");
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
