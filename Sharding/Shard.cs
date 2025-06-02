using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowMacaroni.Discord.Core;

namespace YellowMacaroni.Discord.Sharding
{
    public class Shard
    {
        public readonly int id;
        public readonly int totalShards;
        public readonly Client client;

        public Shard(string token, Intents intents, int id, int totalShards)
        {
            this.id = id;
            this.totalShards = totalShards;
            this.client = new Client(token, intents, this);
        }

        public void Start()
        {
            client.Start();
        }
    }
}
