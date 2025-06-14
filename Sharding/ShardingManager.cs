using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YellowMacaroni.Discord.API;
using YellowMacaroni.Discord.Core;
using YellowMacaroni.Discord.Extentions;
using YellowMacaroni.Discord.Websocket.Events;

namespace YellowMacaroni.Discord.Sharding
{
    public class ShardingManager
    {
        public List<Shard> shards = [];
        private readonly string token;
        private readonly int _shardCount;
        public readonly Intents intents;
        public readonly BotGateway gateway;

        public ShardingManager(string token, Intents intents, int? shardCount = null)
        {
            this.token = token;
            this.intents = intents;

            APIHandler.AddDefaultHeader("Authorization", $"Bot {token}");

            HttpResponseMessage result = APIHandler.GET("/gateway/bot").WaitFor();

            gateway = JsonConvert.DeserializeObject<BotGateway>(result.Content.ReadAsStringAsync().WaitFor()) ?? throw new Exception("Failed to retrieve gateway information. Please check your token and internet connection.");
        
            _shardCount = shardCount ?? gateway.shards;

            for (int i = 0; i < (_shardCount); i++) 
            { 
                Shard thisShard = new(token, intents, i, _shardCount);
                shards.Add(thisShard);
            }
        }

        public void Start()
        {
            if ((_shardCount) > gateway.session_start_limit.remaining) throw new Exception($"Cannot start that many shards! Remaining sessions: {gateway.session_start_limit.remaining}, resets in {Math.Round(gateway.session_start_limit.reset_after / 1000d)}s.");

            int max_concurrency = gateway.session_start_limit.max_concurrency;

            foreach (Shard shard in shards)
            {
                shard.Start();

                while (!shards.All(s => s.id > shard.id || s.client.ready)) Thread.Sleep(50);
                if ((max_concurrency == 1 || shard.id % max_concurrency == max_concurrency - 1) && shard.id != ((_shardCount) - 1))
                {
                    Thread.Sleep(5000);
                }
            }
        }

        public void Hold()
        {
            while (shards.Any(s => s.client.ready)) Thread.Sleep(50);
        }
    }

    public class BotGateway
    {
        public string url = "wss://gateway.discord.gg/?v=10&encoding=json";
        public int shards = 1;
        public SessionStartLimit session_start_limit = new();
    }

    public class SessionStartLimit
    {
        public int total = 1000;
        public int remaining = 1000;
        public int reset_after = 3600000;
        public int max_concurrency = 1;
    }
}
