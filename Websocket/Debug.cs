using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Websocket
{
    public class ClientDebug(string message, ClientDebugType type = ClientDebugType.Info)
    {
        public string message = message;
        public ClientDebugType type = type;
    }

    public enum ClientDebugType
    {
        Info = 0,
        Warn = 1,
        Error = 2
    }
}
