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

        public int Level { set; get; }

        public string Message { set; get; }

        public LogModel(int level, string message)
        {
            this.Level = level;
            this.Message = message;
        }

    }
}
