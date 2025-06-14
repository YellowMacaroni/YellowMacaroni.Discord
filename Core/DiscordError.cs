using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowMacaroni.Discord.Extentions;

namespace YellowMacaroni.Discord.Core
{
    public class DiscordError
    {
        public int code = -1;
        public string message = "";
        public List<DiscordErrorDetails> stack = [];
        public List<string> errors = [];

        public DiscordError(DiscordErrorResponse response)
        {
            code = response.code;
            message = response.message;

            GetStack(response.errors ?? []);
        }

        private void GetStack(Dictionary<string, object> dict, List<string>? st = null)
        {
            List<string> errorStack = st ?? [];

            foreach (var error in dict)
            {
                if (error.Key == "_errors")
                {
                    if (error.Value is List<DiscordErrorDetails> details)
                    {
                        stack.AddRange(details);
                        foreach (DiscordErrorDetails detail in details)
                        {
                            errors.Add(errorStack.Join(" > ") + $" > {detail.message}");
                        }
                    }
                }
                else
                {
                    if (error.Value is Dictionary<string, object> nextDict)
                    {
                        errorStack.Add(error.Key);
                        GetStack(nextDict, errorStack);
                    }
                }
            }
        }

        public override string ToString()
        {
            return $"[{code}] {message}\n> {errors.Join("\n> ")}";
        }
    }

    public class DiscordErrorResponse
    {
        public int code = -1;
        public string message = "";
        public Dictionary<string, object>? errors = [];
    }

    public class DiscordErrorDetails
    {
        public string code = "";
        public string message = "";
    }
}
