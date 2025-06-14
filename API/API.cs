using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowMacaroni.Discord.Core;

namespace YellowMacaroni.Discord.API
{
    internal static class APIHandler
    {
        public static readonly HttpClient client = new();

        private static string baseUrl = "https://discord.com/api/v10";


        /// <summary>
        /// Automatically updates the base URL to use Discord's API on the specified version.
        /// </summary>
        /// <param name="apiVersion"></param>
        public static void UseVersion(int apiVersion) => baseUrl = $"https://discord.com/api/v{apiVersion}/";
        /// <summary>
        /// Updates the base URL to use with API requests.
        /// </summary>
        /// <param name="baseUrl"></param>
        public static void UseBaseUrl(string _baseUrl) => baseUrl = _baseUrl;

        public static string GetBaseUrl() => baseUrl;


        /// <summary>
        /// Adds a default header to the HTTP client. If the header already exists, it will be replaced with the new value.
        /// </summary>
        /// <param name="name">The name of the header.</param>
        /// <param name="content">The content of the header.</param>
        public static void AddDefaultHeader (string name, string content)
        {
            if (client.DefaultRequestHeaders.Contains(name))
            {
                client.DefaultRequestHeaders.Remove(name);
            }
            client.DefaultRequestHeaders.Add(name, content);
        }
        /// <summary>
        /// Removes a default header from the HTTP client. If the header does not exist, nothing will happen.
        /// </summary>
        /// <param name="name">The name of the header.</param>
        public static void RemoveDefaultHeader(string name)
        {
            if (client.DefaultRequestHeaders.Contains(name))
            {
                client.DefaultRequestHeaders.Remove(name);
            }
        }

        public static (T?, DiscordError?) DeserializeResponse<T>(HttpResponseMessage result) where T : class
        {
            if (result.IsSuccessStatusCode)
            {
                return (JsonConvert.DeserializeObject<T>(result.Content.ReadAsStringAsync().Result) ?? default, null);
            }
            else
            {
                return (null, new DiscordError(JsonConvert.DeserializeObject<DiscordErrorResponse>(result.Content.ReadAsStringAsync().Result) ?? new()));
            }
        }


        /// <summary>
        /// Send a GET request to Discord (or the base URL provided) with the specified endpoint and headers.
        /// </summary>
        /// <param name="endpoint">The endpoint to send the request to. The default base URL is 'https://discord.com/api/v10/'.</param>
        /// <param name="headers">The headers to send with the request.</param>
        /// <returns><see cref="Task"/>&lt;<see cref="HttpResponseMessage"/>&gt; for a GET request using the information provided to Discord (or the base URL provided).</returns>
        public static async Task<HttpResponseMessage> GET(string endpoint, Dictionary<string, string>? headers = null)
        {
            HttpRequestMessage message = new(HttpMethod.Get, baseUrl + endpoint);
            
            if (headers is not null)
            {
                foreach (var item in headers)
                {
                    message.Headers.Add(item.Key, item.Value);
                }
            }

            HttpResponseMessage response = await client.SendAsync(message);
            return response;
        }

        /// <summary>
        /// Send a POST request to Discord (or the base URL provided) with the specified endpoint, content, headers, and reason.
        /// </summary>
        /// <param name="endpoint">The endpoint to send the request to. The default base URL is 'https://discord.com/api/v10/'</param>
        /// <param name="content">The content to send along side the POST request.</param>
        /// <param name="headers">The headers to send with the request.</param>
        /// <param name="reason">The reason for Discord's audit logs using the X-Audit-Log-Reason header.</param>
        /// <returns><see cref="Task"/>&lt;<see cref="HttpResponseMessage"/>&gt; for a POST request using the information provided to Discord (or the base URL provided).</returns>
        public static async Task<HttpResponseMessage> POST(string endpoint, HttpContent content, Dictionary<string, string>? headers = null, string? reason = null)
        {
            HttpRequestMessage message = new(HttpMethod.Post, baseUrl + endpoint)
            {
                Content = content
            };

            if (headers is not null)
            {
                foreach (var item in headers)
                {
                    message.Headers.Add(item.Key, item.Value);
                }
            }
            if (reason is not null)
            {
                message.Headers.Add("X-Audit-Log-Reason", reason);
            }            

            HttpResponseMessage response = await client.SendAsync(message);
            
            return response;
        }

        /// <summary>
        /// Send a PUT request to Discord (or the base URL provided) with the specified endpoint, content, headers, and reason.
        /// </summary>
        /// <param name="endpoint">The endpoint to send the request to. The default base URL is 'https://discord.com/api/v10/'</param>
        /// <param name="headers">The headers to send with the request.</param>
        /// <param name="reason">The reason for Discord's audit logs using the X-Audit-Log-Reason header.</param>
        /// <returns><see cref="Task"/>&lt;<see cref="HttpResponseMessage"/>&gt; for a DELETE request using the information provided to Discord (or the base URL provided).</returns>
        public static async Task<HttpResponseMessage> DELETE(string endpoint, Dictionary<string, string>? headers = null, string? reason = null)
        {
            HttpRequestMessage message = new(HttpMethod.Delete, baseUrl + endpoint);

            if (headers is not null)
            {
                foreach (var item in headers)
                {
                    message.Headers.Add(item.Key, item.Value);
                }
            }
            if (reason is not null)
            {
                message.Headers.Add("X-Audit-Log-Reason", reason);
            }

            HttpResponseMessage response = await client.SendAsync(message);
            return response;
        }

        /// <summary>
        /// Send a PATCH request to Discord (or the base URL provided) with the specified endpoint, content, headers, and reason.
        /// </summary>
        /// <param name="endpoint">The endpoint to send the request to. The default base URL is 'https://discord.com/api/v10/'</param>
        /// <param name="content">The content to send along side the PATCH request.</param>
        /// <param name="headers">The headers to send with the request.</param>
        /// <param name="reason">The reason for Discord's audit logs using the X-Audit-Log-Reason header.</param>
        /// <returns><see cref="Task"/>&lt;<see cref="HttpResponseMessage"/>&gt; for a PATCH request using the information provided to Discord (or the base URL provided).</returns>
        public static async Task<HttpResponseMessage> PATCH(string endpoint, HttpContent content, Dictionary<string, string>? headers = null, string? reason = null)
        {
            HttpRequestMessage message = new(HttpMethod.Patch, baseUrl + endpoint)
            {
                Content = content
            };

            if (headers is not null)
            {
                foreach (var item in headers)
                {
                    message.Headers.Add(item.Key, item.Value);
                }
            }
            if (reason is not null)
            {
                message.Headers.Add("X-Audit-Log-Reason", reason);
            }

            HttpResponseMessage response = await client.SendAsync(message);
            return response;
        }
    }
}
