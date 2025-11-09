using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using YellowMacaroni.Discord.Extentions;

namespace YellowMacaroni.Discord.Core
{
    public class DiscordError
    {
        public int code = -1;
        public string message = "";
        public string stack = "";
        public List<string> errors = [];

        public DiscordError(DiscordErrorResponse response)
        {
            code = response.code;
            message = response.message;

            GetStack(response.errors ?? []);
        }

        private void GetStack(Dictionary<string, dynamic> dict)
        {
            StringBuilder sb = new();

            

            sb.AppendLine($"[{code}]: {message}");

            void checkDict(Dictionary<string, dynamic> d, int depth = 1)
            {
                foreach (var kvp in d)
                {
                    if (kvp.Key == "_errors")
                    {
                        foreach (var err in kvp.Value.ToObject<List<DiscordErrorDetails>>())
                        {
                            if (err is DiscordErrorDetails errorDetails)
                            {
                                sb.AppendLine($"{"".PadLeft(depth, '│')}├ [{errorDetails.code}]: {errorDetails.message}");
                                errors.Add($"[{errorDetails.code}]: {errorDetails.message}");
                            }
                        }
                    }
                    else
                    {
                        sb.AppendLine($"{"".PadLeft(depth, '│')}├ {kvp.Key}");
                        checkDict(kvp.Value.ToObject<Dictionary<string, dynamic>>(), depth + 1);
                    }
                }
            }

            checkDict(dict);

            stack = sb.ToString();
        }

        public override string ToString()
        {
            return stack;
        }
    }

    public class DiscordErrorResponse
    {
        public int code = -1;
        public string message = "";
        public Dictionary<string, dynamic>? errors = [];
    }
    public class DiscordErrorList
    {
        public List<DiscordErrorDetails> _errors = [];
    }

    public class DiscordErrorDetails
    {
        public string code = "";
        public string message = "";
    }
}
