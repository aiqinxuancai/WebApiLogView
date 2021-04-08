using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebApiLogCore.Base;

namespace WebApiLogCore.Services.Rest
{

    public class RestServer : RestBase
    {
        const int kSurcess = 1;

        public enum REST_RESULT
        {
            ERROR = 0,
            SURCESS = 1
        }


        public RestServer(int maxThreads) : base(maxThreads)
        {
            this.JsonPost("SendLog", (root) => {
                try
                {
                    return GetRestResponseModel(REST_RESULT.SURCESS, ""); 
                }
                catch (System.Exception ex)
                {
                    return GetRestResponseModel(REST_RESULT.ERROR, ex.ToString());
                }
            });

            this.JsonPost("Test", (root) => 
            {
                //测试服务可用
                return GetRestResponseModel(REST_RESULT.SURCESS, "");
            });

        }


        private Dictionary<string, dynamic> GetRestResponseModel(REST_RESULT status, dynamic data)
        {
            Dictionary<string, dynamic> keyValuePairs = new Dictionary<string, dynamic>();
            keyValuePairs["status"] = status;
            keyValuePairs["data"] = data;
            return keyValuePairs;
        }

        protected override void ProcessHttpRequest(HttpListenerContext context)
        {

        }



    }
}
