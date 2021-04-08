using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiLogCore.Base
{
    /// <summary>
    /// 控制台帮助类
    /// </summary>
    public static class ConsoleHelper
    {
        static object obj = new object();
        static void WriteColorLine(string str, ConsoleColor color)
        {
            lock (obj){
                //Console.BackgroundColor = ConsoleColor.White;
                ConsoleColor currentForeColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"));
                Console.ForegroundColor = currentForeColor;

                Console.Write("  ");
                currentForeColor = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Console.Write(str);
                Console.ForegroundColor = currentForeColor;

                Console.Write("\r\n");

            }


        }

        /// <summary>
        /// 打印错误信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="color">想要打印的颜色</param>
        public static void WriteErrorLine(this string str, ConsoleColor color = ConsoleColor.Red)
        {
            WriteColorLine(str, color);
        }

        /// <summary>
        /// 打印警告信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="color">想要打印的颜色</param>
        public static void WriteWarningLine(this string str, ConsoleColor color = ConsoleColor.Yellow)
        {
            WriteColorLine(str, color);
        }
        /// <summary>
        /// 打印正常信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="color">想要打印的颜色</param>
        public static void WriteInfoLine(this string str, ConsoleColor color = ConsoleColor.White)
        {
            WriteColorLine(str, color);
        }
        /// <summary>
        /// 打印成功的信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="color">想要打印的颜色</param>
        public static void WriteSuccessLine(this string str, ConsoleColor color = ConsoleColor.Green)
        {
            WriteColorLine(str, color);
        }
    }

}
