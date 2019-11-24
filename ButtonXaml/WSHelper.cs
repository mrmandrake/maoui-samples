using System;
using System.Threading.Tasks;
using System.Threading;
using System.Net.WebSockets;
using System.Text;

namespace ButtonXaml
{
    public class WSHelper
    {
        public ClientWebSocket cws = null;

        public static ClientWebSocket CreateWebSocket(string protocols = "")
        {
            Console.WriteLine("WSHelper::CreateWebSocket");
            var cws = new ClientWebSocket();
            if (!string.IsNullOrEmpty(protocols))
                foreach (var p in protocols.Split(';'))
                    cws.Options.AddSubProtocol(p);

            return cws;
        }

        public async Task<int> ConnectWebSocketStatus(Uri server, string protocols)
        {
            Console.WriteLine("WSHelper::ConnectWebSocketStatus");
            cws = CreateWebSocket(protocols);
            WebSocketCloseStatus status = cws.CloseStatus ?? WebSocketCloseStatus.Empty;

            try
            {
                Task taskConnect = cws.ConnectAsync(server, CancellationToken.None);
                await taskConnect;
            }
            catch (Exception exc)
            {
                Console.WriteLine($"{exc.Message} / {exc.InnerException.Message}");
            }
            return (int)status;
        }

        public async Task<int> ConnectWebSocketStatusWithToken(Uri server, string protocols)
        {
            Console.WriteLine("WSHelper::ConnectWebSocketStatusWithToken");
            cws = CreateWebSocket(protocols);
            WebSocketCloseStatus status = cws.CloseStatus ?? WebSocketCloseStatus.Empty;
            using (var cts2 = new CancellationTokenSource(500))
            {

                try
                {
                    Task taskConnect = cws.ConnectAsync(server, cts2.Token);
                    await taskConnect;
                }
                catch (Exception exc)
                {
                    Console.WriteLine($"{exc.Message} / {exc.InnerException.Message}");
                }
            }
            return (int)status;
        }

        public async Task<WebSocketState> OpenWebSocket(Uri server, string protocols)
        {
            Console.WriteLine("WSHelper::OpenWebSocket");
            cws = CreateWebSocket(protocols);
            var state = cws.State;
            using (var cts2 = new CancellationTokenSource(500))
            {
                try
                {
                    Task taskConnect = cws.ConnectAsync(server, cts2.Token);
                    await taskConnect;
                }
                catch (Exception exc)
                {
                    Console.WriteLine($"{exc.Message} / {exc.InnerException.Message}");
                }
                finally
                {
                    state = cws.State;
                }
            }
            return state;
        }

        public async Task<WebSocketState> CloseWebSocket()
        {
            Console.WriteLine("WSHelper::CloseWebSocket");

            if (cws == null)
                Console.WriteLine("cws null");

            var state = cws.State;
            using (var cts2 = new CancellationTokenSource(500))
            {
                try
                {
                    if (cws.State == WebSocketState.Open)
                        await cws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Hic sunt Dracones!!", CancellationToken.None);
                }
                catch (Exception exc)
                {
                    Console.WriteLine($"{exc.Message} / {exc.InnerException.Message}");
                }
                finally
                {
                    state = cws.State;
                    cws = null;
                }
            }

            return state;
        }

        public async Task<WebSocketState> ReceiveHostCloseWebSocket()
        {
            Console.WriteLine("WSHelper::ReceiveHostCloseWebSocket");
            //cws = CreateWebSocket(protocols);
            var state = cws.State;
            using (var cts2 = new CancellationTokenSource(500))
            {
                try
                {
                    // Task taskConnect = cws.ConnectAsync(server, cts2.Token);
                    // await taskConnect;
                    if (cws.State == WebSocketState.Open)
                    {
                        var sndBuffer = Encoding.UTF8.GetBytes("closeme");
                        await cws.SendAsync(new ArraySegment<byte>(sndBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
                        var rcvBuffer = new ArraySegment<byte>(new byte[4096]);
                        var r = await cws.ReceiveAsync(rcvBuffer, CancellationToken.None);
                    }
                }
                catch (Exception exc)
                {
                    Console.WriteLine($"{exc.Message} / {exc.InnerException.Message}");
                }
                finally
                {
                    state = cws.State;
                    cws = null;
                }
            }
            return state;
        }

        public async Task<string> CloseStatusDescCloseWebSocket(Uri server, string protocols)
        {
            Console.WriteLine("WSHelper::CloseStatusDescCloseWebSocket");
            cws = CreateWebSocket(protocols);
            var description = $"{{ \"code\": \"{cws.CloseStatus}\", \"desc\": \"{cws.CloseStatusDescription}\" }}";
            using (var cts2 = new CancellationTokenSource(500))
            {
                try
                {
                    Task taskConnect = cws.ConnectAsync(server, cts2.Token);
                    await taskConnect;
                    if (cws.State == WebSocketState.Open)
                    {
                        var sndBuffer = Encoding.UTF8.GetBytes("closeme");
                        await cws.SendAsync(new ArraySegment<byte>(sndBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
                        var rcvBuffer = new ArraySegment<byte>(new byte[4096]);
                        var r = await cws.ReceiveAsync(rcvBuffer, CancellationToken.None);
                    }

                }
                catch (Exception exc)
                {
                    Console.WriteLine($"{exc.Message} / {exc.InnerException.Message}");
                }
                finally
                {
                    description = $"{{ \"code\": \"{cws.CloseStatus}\", \"desc\": \"{cws.CloseStatusDescription}\" }}";
                    cws = null;
                }
            }
            return description;
        }

        public async Task<string> WebSocketSendText(string text)
        {
            Console.WriteLine("WSHelper::WebSocketSendText");

            if (cws == null) {
                Console.WriteLine("cws null");
                return "cws null";
            }                

            try
            {
                Console.WriteLine("WS sendasync");
                await cws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(text)), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception exc)
            {
                Console.WriteLine($"{exc.Message} / {exc.InnerException.Message}");
            }

            return "SomethingWentWrong";
        }

        public async Task<string> WebSocketSendText(Uri server, string protocols, string text)
        {
            Console.WriteLine("WSHelper::WebSocketSendText");
            cws = CreateWebSocket(protocols);
            using (var cts2 = new CancellationTokenSource(500))
            {
                try
                {
                    Console.WriteLine("connectasync");
                    Task taskConnect = cws.ConnectAsync(server, cts2.Token);
                    await taskConnect;
                    if (cws.State == WebSocketState.Open)
                    {
                        Console.WriteLine("WS open");
                        var sndBuffer = Encoding.UTF8.GetBytes(text);
                        Console.WriteLine("sendasync");
                        await cws.SendAsync(new ArraySegment<byte>(sndBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
                        var rcvBuffer = new ArraySegment<byte>(new byte[4096]);
                        Console.WriteLine("recvasync");
                        var r = await cws.ReceiveAsync(rcvBuffer, CancellationToken.None);
                        Console.WriteLine("received!");
                        return Encoding.UTF8.GetString(rcvBuffer.Array, rcvBuffer.Offset, r.Count);
                    }

                }
                catch (Exception exc)
                {
                    Console.WriteLine($"{exc.Message} / {exc.InnerException.Message}");
                }
            }
            return "SomethingWentWrong";
        }

        public async Task<string> WebSocketRecvText()
        {
            Console.WriteLine("WSHelper::WebSocketRecvText");
            try
            {
                if (cws.State == WebSocketState.Open)
                {
                    Console.WriteLine("WS read");
                    var rcvBuffer = new ArraySegment<byte>(new byte[4096]);
                    Console.WriteLine("recvasync");
                    var r = await cws.ReceiveAsync(rcvBuffer, CancellationToken.None);
                    Console.WriteLine("received!");
                    return Encoding.UTF8.GetString(rcvBuffer.Array, rcvBuffer.Offset, r.Count);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine($"{exc.Message} / {exc.InnerException.Message}");
            }
            return "SomethingWentWrong";
        }        

        public async Task<string> WebSocketSendBinary(Uri server, string protocols, string text)
        {
            Console.WriteLine("WSHelper::WebSocketSendBinary");
            cws = CreateWebSocket(protocols);
            using (var cts2 = new CancellationTokenSource(500))
            {
                try
                {
                    Task taskConnect = cws.ConnectAsync(server, cts2.Token);
                    await taskConnect;
                    if (cws.State == WebSocketState.Open)
                    {
                        var sndBuffer = Encoding.UTF8.GetBytes(text);
                        await cws.SendAsync(new ArraySegment<byte>(sndBuffer), WebSocketMessageType.Binary, true, CancellationToken.None);
                        var rcvBuffer = new ArraySegment<byte>(new byte[4096]);
                        var r = await cws.ReceiveAsync(rcvBuffer, CancellationToken.None);
                        return Encoding.UTF8.GetString(rcvBuffer.Array, rcvBuffer.Offset, r.Count);
                    }

                }
                catch (Exception exc)
                {
                    Console.WriteLine($"{exc.Message} / {exc.InnerException.Message}");
                }
            }
            return "SomethingWentWrong";
        }

        public async Task<string> WebSocketSendBinary(Uri server, string protocols, byte[] buffer)
        {
            Console.WriteLine("WSHelper::WebSocketSendBinary");
            cws = CreateWebSocket(protocols);
            using (var cts2 = new CancellationTokenSource(500))
            {
                try
                {
                    Task taskConnect = cws.ConnectAsync(server, cts2.Token);
                    await taskConnect;
                    if (cws.State == WebSocketState.Open)
                    {
                        await cws.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Binary, true, CancellationToken.None);
                        var rcvBuffer = new ArraySegment<byte>(new byte[4096]);
                        var r = await cws.ReceiveAsync(rcvBuffer, CancellationToken.None);
                        return Encoding.UTF8.GetString(rcvBuffer.Array, rcvBuffer.Offset, r.Count);
                    }

                }
                catch (Exception exc)
                {
                    Console.WriteLine($"{exc.Message} / {exc.InnerException.Message}");
                }
            }
            return "SomethingWentWrong";
        }

        public async Task<WebSocketState> ConnectWebSocket(Uri server, string protocols)
        {
            Console.WriteLine("WSHelper::ConnectWebSocket");            
            cws = CreateWebSocket(protocols);
            var state = cws.State;
            using (var cts2 = new CancellationTokenSource(4000))
            {
                try
                {
                    Task taskConnect = cws.ConnectAsync(server, cts2.Token);
                    await taskConnect;
                }
                catch (Exception exc)
                {
                    Console.WriteLine($"{exc.Message} / {exc.InnerException.Message}");
                }
                finally
                {
                    state = cws.State;
                }
            }
            return state;
        }
    }
}
