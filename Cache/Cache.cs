using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YellowMacaroni.Discord.API;
using YellowMacaroni.Discord.Core;
using YellowMacaroni.Discord.Extentions;

namespace YellowMacaroni.Discord.Cache
{
    public static class DiscordCache
    {
        public static readonly Collection<Guild> Guilds = new(Collection<Guild>.DiscordFetch("https://discord.com/api/v10/guilds/{key}"));
        public static readonly Collection<User> Users = new(Collection<User>.DiscordFetch("https://discord.com/api/v10/users/{key}"));
        public static readonly Collection<Channel> Channels = new(Collection<Channel>.DiscordFetch("https://discord.com/api/v10/channels/{key}"));
    }
    
    public class Collection<T> where T : class
    {
        public static Func<string, List<string>, Task<T?>> DiscordFetch(string baseUrl, HttpMethod? method = null, Dictionary<string, string>? headers = null)
        {
            return async (key, args) =>
            {
                string url = baseUrl.Replace("{key}", key);

                if (args is not null)
                {
                    for (int i = 0; i < args.Count; i++)
                    {
                        url = url.Replace($"{{i + 1}}", args[i]);
                    }
                }

                HttpRequestMessage message = new(method ?? HttpMethod.Get, url);

                foreach (KeyValuePair<string, string> header in headers ?? [])
                {
                    message.Headers.Add(header.Key, header.Value);
                }

                HttpResponseMessage result = await APIHandler.client.SendAsync(message);

                if (result.IsSuccessStatusCode)
                {
                    T? value = JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());
                    return value;
                }
                else
                {
                    return null;
                }
            };
        }

        private readonly Dictionary<string, T> _cache = [];
        
        private readonly Func<string, List<string>, Task<T?>> _fetchFunc;

        /// <summary>
        /// A 'collection' which stores cached items which can be searched through and added to.
        /// </summary>
        /// <typeparam name="T">The type to assign the values to.</typeparam>
        /// <param name="client">The parent client.</param>
        /// <param name="fetchEndpoint">The endpoint to send requests to when a fetch is initiated.</param>
        /// <param name="fetchMethod">The method to use when sending the request, null for the default (GET).</param>"
        /// <param name="fetchHeaders">Headers to send along side a fetch.</param>
        public Collection(Func<string, List<string>, Task<T?>> func, Dictionary<string, T>? cache = null)
        {
            _fetchFunc = func;
            if (cache is not null)
            {
                _cache = cache;
            }
        }

        /// <summary>
        /// Get the <see cref="T"/> from the cache with the given key.
        /// </summary>
        /// <param name="key">The key to get the value from.</param>
        /// <returns><see cref="T"/> represented by the given key.</returns>
        public T? FromCache(string key, List<string>? args = null)
        {
            return _cache.GetValueOrDefault(GenerateKey(key, args));
        }

        public static string GenerateKey(string key, List<string>? args = null)
        {
            string k = "";

            if (args is not null)
            {
                k = args.Join("-") + "-";
            }

            k += key;

            return k;
        }

        /// <summary>
        /// Fetch the <see cref="T"/> from the given <see cref="fetchEndpoint"/> using a GET request and then given headers.
        /// </summary>
        /// <param name="key">The key to assign the value to.</param>
        /// <param name="cache">Whether the value should be cached.</param>
        /// <returns><see cref="T"/> represented by the given key.</returns>
        public async Task<T?> Fetch(string key, List<string>? args = null, bool cache = true)
        {
            try
            {
                T? value = await _fetchFunc(key, args ?? []);
                if (value is not null && cache)
                {
                    Insert(key, value, args);
                }
                return value;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get the <see cref="T"/> from the cache with the given key, otherwise, make a fetch request to get and cache the <see cref="T"/>.
        /// </summary>
        /// <param name="key">The key to get the value from or assign to.</param>
        /// <returns><see cref="T"/> represented by the given key</returns>
        public async Task<T?> Get(string key)
        {
            
            T? cachedValue = FromCache(key);
            if (cachedValue is not null) return cachedValue;
            
            return await Fetch(key);
        }

        /// <summary>
        /// Find a <see cref="T"/> from the cache using the given predicate.
        /// </summary>
        /// <param name="predicate">A function which takes an input of a <see cref="T"/> and its key which returns a boolean value of whether it should be returned.</param>
        /// <returns><see cref="T"/> which matches the predicate.</returns>
        public T? Find(Func<T, string, bool> predicate)
        {
            T? value = null;
            foreach (KeyValuePair<string, T> KVP in _cache)
            {
                if (predicate(KVP.Value, KVP.Key))
                {
                    value = KVP.Value;
                    break;
                }
            }
            return value;
        }

        /// <summary>
        /// Find multiple <see cref="T"/> from the cache using the given predicate.
        /// </summary>
        /// <param name="predicate">A function which takes an input of a <see cref="T"/> and its key which returns a boolean value of whether it should be returned.</param>
        /// <returns>A list of <see cref="T"/>s which match the predicate.</returns>
        public List<T> FindMany(Func<T, string, bool> predicate)
        {
            List<T> values = [];
            foreach (KeyValuePair<string, T> KVP in _cache)
            {
                if (predicate(KVP.Value, KVP.Key))
                {
                    values.Add(KVP.Value);
                }
            }
            return values;
        }

        /// <summary>
        /// Find multiple <see cref="T"/> from the cache using the given predicate.
        /// </summary>
        /// <param name="predicate">A function which takes an input of a <see cref="T"/> and its key which returns a boolean value of whether it should be returned.</param>
        /// <returns>A dictionary of key <see cref="string"/> and value <see cref="T"/>s which match the predicate.</returns>
        public Dictionary<string, T> FindManyDict(Func<T, string, bool> predicate)
        {
            Dictionary<string, T> values = [];
            foreach (KeyValuePair<string, T> KVP in _cache)
            {
                if (predicate(KVP.Value, KVP.Key))
                {
                    values.Add(KVP.Key, KVP.Value);
                }
            }
            return values;
        }

        /// <summary>
        /// Insert a <see cref="T"/> with the given key into the cache.
        /// </summary>
        /// <param name="key">The key to use.</param>
        /// <param name="value">The value to use.</param>
        public void Insert(string key, T value, List<string>? args = null)
        {
            _cache[GenerateKey(key, args)] = value;
        }

        public void UpdateOrInsert(string key, T value, List<string>? args = null, bool updateWhenNull = false)
        {
            string cacheKey = GenerateKey(key, args);
            
            if (_cache.TryGetValue(cacheKey, out T? oldValue))
            {
                if (oldValue is null) return;

                PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (PropertyInfo property in properties)
                {
                    if (!property.CanWrite || !property.CanRead) continue;

                    var newValue = property.GetValue(value);
                    if (newValue is not null || updateWhenNull)
                    {
                        property.SetValue(oldValue, newValue);
                    }
                }
            }
            else
            {
                _cache.TryAdd(cacheKey, value);
            }
        }

        /// <summary>
        /// Remove a <see cref="T"/> from the cache with the given key.
        /// </summary>
        /// <param name="key">The key to use.</param>
        public void Remove(string key, List<string>? args = null)
        {
            _cache.Remove(GenerateKey(key, args));
        }

        /// <summary>
        /// Remove all keys and <see cref="T"/> from the cache.
        /// </summary>
        public void Clear()
        {
            _cache.Clear();
        }
    }
}
