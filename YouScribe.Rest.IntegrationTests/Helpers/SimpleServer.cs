using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace YouScribe.Rest.IntegrationTests.Helpers
{
    public class SimpleServer : IDisposable
    {
        readonly HttpListener _listener;
        readonly Action<HttpListenerContext> _handler;
        Thread _processor;

        public static SimpleServer Create(string url, Action<HttpListenerContext> handler)
        {
            var server = new SimpleServer(new HttpListener { Prefixes = { url } }, handler);
            server.Start();
            return server;
        }

        SimpleServer(HttpListener listener, Action<HttpListenerContext> handler)
        {
            _listener = listener;
            _handler = handler;
        }

        public void Start()
        {
            if (!_listener.IsListening)
            {
                _listener.Start();

                _processor = new Thread(() =>
                {
                    while (_listener.IsListening)
                    {
                        var context = _listener.GetContext();
                        _handler(context);
                        context.Response.Close();
                    }
                }) { Name = "WebServer" };
                _processor.Start();
            }
        }

        public void Dispose()
        {
            _processor.Abort();
            _listener.Stop();
            _listener.Close();
        }
    }
}
