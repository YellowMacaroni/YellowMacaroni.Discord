using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Core
{
    public class Poll
    {
        public PollMedia? question;
        public List<PollAnswer>? answers;
        public string? expiry;
        public bool allow_multiselect;
        public PollLayoutType layout_type;
        public PollResults? results;
    }

    public class PollMedia
    {
        public string? text;
        public Emoji? emoji;
    }

    public class PollAnswer
    {
        public int answer_id;
        public PollMedia? poll_media;
    }

    public class PollResults
    {
        public bool is_finalized;
        public List<AnswerCount>? answer_counts;
    }

    public class AnswerCount
    {
        public int id;
        public int count;
        public bool me_voted;
    }

    public enum PollLayoutType
    {
        Default = 1
    }
}
