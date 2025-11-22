using System;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DiscordBotLib.Core.Networking
{
    public class HttpApiClient
    {
        private readonly HttpClient httpclient;
        private readonly string basetoken;
        private readonly string baseurl = "https://discord.com/api/v10";

        public HttpApiClient(string token)
        {
            basetoken = token;
            httpclient = new HttpClient();
            httpclient.DefaultRequestHeaders.Add("Authorization", $"Bot {basetoken}");
            httpclient.DefaultRequestHeaders.Add("User-Agent", "DiscordBotLib/1.0");
        }

        public async Task<string> GetAsync(string endpoint)
        {
            var response = await httpclient.GetAsync($"{baseurl}{endpoint}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PostAsync(string endpoint, string jsoncontent)
        {
            var content = new StringContent(jsoncontent, Encoding.UTF8, "application/json");
            var response = await httpclient.PostAsync($"{baseurl}{endpoint}", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PutAsync(string endpoint, string jsoncontent)
        {
            var content = new StringContent(jsoncontent, Encoding.UTF8, "application/json");
            var response = await httpclient.PutAsync($"{baseurl}{endpoint}", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DeleteAsync(string endpoint)
        {
            var response = await httpclient.DeleteAsync($"{baseurl}{endpoint}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public void SetTimeout(TimeSpan timeout)
        {
            httpclient.Timeout = timeout;
        }

        public void Dispose()
        {
            httpclient?.Dispose();
        }
    }
}