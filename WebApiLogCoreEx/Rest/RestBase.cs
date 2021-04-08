using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using WebApiLogCore.Base;

namespace WebApiLogCore.Services.Rest
{
    public class JsonRoute
    {
        public string FuncName { get; set; }

        public Func<JObject, dynamic> Func { get; set; }

        public JsonRoute(string funcName, Func<JObject, dynamic> func)
        {
            FuncName = funcName;
            Func = func;
        }
    }


    // HTTP服务类
    public abstract class RestBase : IDisposable
    {
        private readonly HttpListener _listener;                        // HTTP 协议侦听器
        private readonly Thread _listenerThread;                        // 监听线程
        private readonly Thread[] _workers;                             // 工作线程组
        private readonly ManualResetEvent _stop, _ready;                // 通知停止、就绪
        private Queue<HttpListenerContext> _queue;                      // 请求队列
        private event Action<HttpListenerContext> ProcessRequest;       // 请求处理委托

        private readonly List<JsonRoute> _route;

        private Action<LogModel> _callback;
        public RestBase(int maxThreads)
        {
            _workers = new Thread[maxThreads];
            _queue = new Queue<HttpListenerContext>();
            _stop = new ManualResetEvent(false);
            _ready = new ManualResetEvent(false);
            _listener = new HttpListener();
            _listenerThread = new Thread(HandleRequests);
            _route = new List<JsonRoute>();
        }

        public bool Start(int port, Action<LogModel> callback)

        {
            _callback = callback;
            // 注册处理函数
            try
            {
	            ProcessRequest += ProcessHttpRequest;

                // 启动Http服务

                var url = String.Format("http://{0}:{1}/", RestManager.GetAddressIP(), port);
                Console.WriteLine(url);

                _listener.Prefixes.Add(url);
	            _listener.Start();
	            _listenerThread.Start();
	
	            // 启动工作线程
	            for (int i = 0; i < _workers.Length; i++)
	            {
	                _workers[i] = new Thread(Worker);
	                _workers[i].Start();
	            }
                return true;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public virtual void JsonPost(string func, Func<JObject, dynamic> action)
        {
            //this.Get<object>(path, action, condition, name);
            _route.Add(new JsonRoute(func, action));
        }

        // 请求处理函数
        protected abstract void ProcessHttpRequest(HttpListenerContext ctx);

        // 释放资源
        public void Dispose()
        {
            Stop();
        }

        // 停止服务
        public void Stop()
        {
            _stop.Set();
            _listenerThread.Join();
            foreach (Thread worker in _workers)
            {
                worker.Join();
            }
            _listener.Stop();
        }

        // 处理请求
        private void HandleRequests()
        {
            while (_listener.IsListening)
            {
                var context = _listener.BeginGetContext(ContextReady, null);
                if (0 == WaitHandle.WaitAny(new[] { _stop, context.AsyncWaitHandle }))
                {
                    return;
                }
            }
        }

        // 请求就绪加入队列
        private void ContextReady(IAsyncResult ar)
        {
            try
            {
                lock (_queue)
                {
                    _queue.Enqueue(_listener.EndGetContext(ar));
                    _ready.Set();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("[HttpServerBase::ContextReady]err:{0}", e.Message));
            }
        }

        // 处理一个任务
        private void Worker()
        {
            WaitHandle[] wait = new[] { _ready, _stop };
            while (0 == WaitHandle.WaitAny(wait))
            {
                HttpListenerContext context;
                lock (_queue)
                {
                    if (_queue.Count > 0)
                        context = _queue.Dequeue();
                    else
                    {
                        _ready.Reset();
                        continue;
                    }
                }

                try
                {
                    
                    ProcessRequest(context);

                    if (context.Request.HttpMethod.ToUpper() == "POST" )
                    {
                        RunJsonRoute(context);
                    } 
                    else
                    {
                        byte[] buffer = Encoding.UTF8.GetBytes("Online");
                        context.Response.StatusCode = 200;
                        context.Response.ContentLength64 = buffer.Length;
                        context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                        context.Response.OutputStream.Close();
                        context.Response.Close();
                        "收到GET请求".WriteSuccessLine();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(string.Format("[HttpServerBase::Worker]err:{0}", e.Message));
                }
            }
        }

        private void RunJsonRoute(HttpListenerContext context)
        {

            dynamic response = new Dictionary<string, dynamic>() {
                { "message" , "Not Find Action"}
            };

            if (context.Request.HttpMethod.ToUpper() == "POST")
            {
                string text;
                using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                {
                    text = reader.ReadToEnd();
                }

                Debug.WriteLine(text);

                if (JsonHelper.IsJsonFormat(text))
                {
                    JObject root = JObject.Parse(text);
                    string funcName = root["funcName"].ToString();
                    foreach (JsonRoute item in _route)
                    {
                        if (item.FuncName == funcName)
                        {
                            response = item.Func(root);
                            break;
                        }
                    }
                }
            }

            byte[] buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
            context.Response.StatusCode = 200;
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();
            context.Response.Close();
        }


    }

}
