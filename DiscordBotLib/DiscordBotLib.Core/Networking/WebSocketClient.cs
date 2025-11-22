using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordBotLib.Core.Networking
{
    public class WebSocketClient
    {
        private ClientWebSocket websocket;
        private CancellationTokenSource cancellationtokensource;
        private readonly Uri websocketuri;
        private readonly int buffersize = 4096;

        public event Action<string> OnMessageReceived;
        public event Action OnConnected;
        public event Action OnDisconnected;
        public event Action<Exception> OnError;

        public WebSocketClient(string uri)
        {
            websocketuri = new Uri(uri);
        }

        public async Task ConnectAsync()
        {
            try
            {
                cancellationtokensource = new CancellationTokenSource();
                websocket = new ClientWebSocket();

                await websocket.ConnectAsync(websocketuri, cancellationtokensource.Token);
                OnConnected?.Invoke();

                _ = Task.Run(ReceiveLoopAsync);
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex);
                throw;
            }
        }

        public async Task DisconnectAsync()
        {
            try
            {
                cancellationtokensource?.Cancel();

                if (websocket?.State == WebSocketState.Open)
                {
                    await websocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Disconnecting", CancellationToken.None);
                }

                websocket?.Dispose();
                OnDisconnected?.Invoke();
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex);
            }
        }

        public async Task SendAsync(string message)
        {
            if (websocket?.State != WebSocketState.Open)
                throw new InvalidOperationException("WebSocket is not connected");

            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await websocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, cancellationtokensource.Token);
        }

        private async Task ReceiveLoopAsync()
        {
            var buffer = new byte[buffersize];
            var messagebuffer = new MemoryStream();

            try
            {
                while (websocket.State == WebSocketState.Open && !cancellationtokensource.Token.IsCancellationRequested)
                {
                    WebSocketReceiveResult result = await websocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationtokensource.Token);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await DisconnectAsync();
                        break;
                    }

                    messagebuffer.Write(buffer, 0, result.Count);

                    if (result.EndOfMessage)
                    {
                        string message = Encoding.UTF8.GetString(messagebuffer.ToArray());
                        OnMessageReceived?.Invoke(message);
                        messagebuffer.SetLength(0);
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex);
            }
        }

        public WebSocketState State => websocket?.State ?? WebSocketState.None;

        public void Dispose()
        {
            DisconnectAsync().GetAwaiter().GetResult();
        }
    }
}