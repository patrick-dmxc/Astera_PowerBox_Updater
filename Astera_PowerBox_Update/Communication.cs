using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TitanPowerBoxUpdate
{
    public class Communication
    {
        private static HttpClient client;
        private static HttpClient createClient()
        {
            var _client = new HttpClient(new HttpClientHandler() { }, false) { Timeout = new TimeSpan(0, 0, 0, 20, 0) };
            _client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36");
            _client.DefaultRequestHeaders.Add("Accept", "*/*");
            _client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
            _client.DefaultRequestHeaders.Add("Accept-Language", "de-DE,de;q=0.9,en-US;q=0.8,en;q=0.7");
            return _client;
        }

        public static async Task SendUpdate(string ip, string file)
        {
            byte[] data = File.ReadAllBytes(file);
            await SendRequest(ip, "flash/upload", HttpMethod.Post, data);
        }
        public static async Task SendReboot(string ip)
        {
            await SendRequest(ip, "flash/reboot", HttpMethod.Get);
        }

        internal static async Task SendRequest(string ip, string endpoint, HttpMethod methode, byte[] data = null)
        {
            if (client == null)
                client = createClient();

            string address = string.Empty;
            address = $"http://{ip}/{endpoint}";

            HttpResponseMessage response = null;
            //for (byte i = 0; i < 10; i++)
            {
                try
                {
                    var req = new HttpRequestMessage(methode, address) { };
                    req.Headers.Add("Host", $"ip");
                    req.Headers.Add("Origin", $"http://{ip}");
                    req.Headers.Add("Referer", $"http://{ip}/flash/index.html");
                    if (data != null)
                    {
                        req.Content = new ByteArrayContent(data);
                        req.Content.Headers.Clear();
                        req.Content.Headers.Add("Content-type", "text/plain");
                    }
                    response = await client.SendAsync(req);
                }
                catch (TaskCanceledException e)
                {
                    // Drop Timeout
                }
                catch (TimeoutException)
                {
                    // Drop Timeout
                }

                catch (HttpRequestException)
                {
                    client?.Dispose();
                    await Task.Delay(300);
                    client = createClient();
                }
                await Task.Delay(300);
            }
        }
    }
}