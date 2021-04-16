using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiLogCore.Base
{
    public class LogModel
    {
        //
        //int level = root["level"].ToObject<int>();
        //string message = root["message"].ToString();

        public string Time { set; get; }

        public int Level { set; get; }

        public string LevelString { get {
                var str = Level switch
                {
                    1 => "DEBUG",
                    2 => "INFO",
                    3 => "WARN",
                    4 => "ERROR",
                    5 => "FATAL",
                    6 => "NONE",
                    _ => ""
                };
                return str;
            } }


        //删除换行符的结果
        public string MessageOneLine { get {
                if (Message.Count(f => f == '\n') > 5) {
                    return Message.Replace("\n", " "); // 被折叠了
                } 
                return Message;
            } }

        public string Message { set; get; }

        public LogModel(int level, string message)
        {
            this.Level = level;
            this.Message = message;
            this.Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

    }
}
