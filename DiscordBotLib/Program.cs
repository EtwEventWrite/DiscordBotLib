using System;
using System.Threading.Tasks;
using DiscordBotLib.Core.Networking;

namespace DiscordBotLib
{
    internal class Program
    {
        private static WebSocketClient websocketclient;
        private static HttpApiClient httpclient;

        static async Task Main(string[] args)
        {
            Console.WriteLine("[+] DiscordBotLib Starting...");
            string bottoken = "YOUR_BOT_TOKEN_HERE";

            try
            {
                httpclient = new HttpApiClient(bottoken);
                websocketclient = new WebSocketClient("wss://gateway.discord.gg/?v=10&encoding=json");
                websocketclient.OnMessageReceived += HandleGatewayMessage;
                websocketclient.OnConnected += HandleConnected;
                websocketclient.OnDisconnected += HandleDisconnected;
                websocketclient.OnError += HandleError;
                await websocketclient.ConnectAsync();
                Console.WriteLine("[+] Bot is running. Press Ctrl+C to exit.");
                await Task.Delay(-1);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[-] Critical error: {ex.Message}");
            }
            finally
            {
                websocketclient?.Dispose();
                httpclient?.Dispose();
            }
        }

        private static void HandleGatewayMessage(string message)
        {
            Console.WriteLine($"[+] Gateway: {message}");
            if (message.Contains("READY"))
            {
                Console.WriteLine("[+] Bot is ready and connected!");
            }
            else if (message.Contains("MESSAGE_CREATE"))
            {
                HandleIncomingMessage(message);
            }
        }

        private static void HandleIncomingMessage(string message)
        {
            try
            {
                if (message.Contains("\"content\""))
                {
                    // basic parsing, im lazy rn and havent coded most of this
                    int contentStart = message.IndexOf("\"content\"") + 10;
                    int contentEnd = message.IndexOf("\"", contentStart);
                    string content = message.Substring(contentStart, contentEnd - contentStart);

                    Console.WriteLine($"[+] Message: {content}");
                    if (content == "!ping")
                    {
                        _ = Task.Run(async () => await SendMessageAsync("Pong! 🏓"));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Message handling error: {ex.Message}");
            }
        }

        private static async Task SendMessageAsync(string content)
        {
            try
            {
                Console.WriteLine($"[+] Would send: {content}");
                // Example REST API call:
                // await httpclient.PostAsync("/channels/CHANNEL_ID/messages", $"{{\"content\":\"{content}\"}}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[-] Send message error: {ex.Message}");
            }
        }

        private static void HandleConnected()
        {
            Console.WriteLine("[+] Connected to Discord Gateway!");
        }

        private static void HandleDisconnected()
        {
            Console.WriteLine("[-] Disconnected from Discord Gateway!");
        }

        private static void HandleError(Exception ex)
        {
            Console.WriteLine($"[-] WebSocket error: {ex.Message}");
        }
    }
}