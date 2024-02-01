using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Thorium.Shared.DTOs;
using Thorium.Shared.Messages;

namespace Thorium.Shared
{
    public class FunctionServerHttp
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly HttpListener listener;
        private readonly Dictionary<string, Action<HttpListenerContext>> functions = [];

        public FunctionServerHttp(HttpListener listener)
        {
            this.listener = listener;
        }

        public void AddFunction(string name, Action<HttpListenerContext> action)
        {
            functions[name.ToLowerInvariant()] = action;
        }

        public void Start()
        {
            listener.Start();
            listener.BeginGetContext(HandleNewApiRequest, null);
        }

        private void HandleNewApiRequest(IAsyncResult ar)
        {
            var context = listener.EndGetContext(ar);
            var request = context.Request;
            var response = context.Response;

            var functionName = request.Url.AbsolutePath[1..];

            if (functions.TryGetValue(functionName, out var func))
            {
                try
                {
                    func(context);
                    context.Response.Close();
                }
                catch(Exception ex)
                {
                    logger.Error(ex.ToString());
                    response.StatusCode = 500;
                    byte[] text = Encoding.UTF8.GetBytes("Error 500"); //TODO: more?
                    response.OutputStream.Write(text);
                    context.Response.Close();
                }
            }
            else
            {
                response.StatusCode = 404;
                byte[] text = Encoding.UTF8.GetBytes("Error 404"); //TODO: more?
                response.OutputStream.Write(text);
                context.Response.Close();
            }

            listener.BeginGetContext(HandleNewApiRequest, null);
        }
    }
}
