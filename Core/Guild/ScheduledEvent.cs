using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Core
{
    public class ScheduledEvent (string eventId)
    {
        public string id = eventId;
        public string? guild_id;
        public string? channel_id;
        public string? creator_id;
        public string? name;
        public string? description;
        public string? scheduled_start_time;
        public string? scheduled_end_time;
        public EventPrivacyLevel? privacy_level;
        public EventStatus? status;
        public ScheduledEntityType? entity_type;
        public string? entity_id;
        public EntityMetadata? entity_metadata;
        public User? creator;
        public int? user_count;
        public string? image;
        public RecurrenceRule? recurrence_rule;
    }

    public class RecurrenceRule
    {
        public string? start;
        public string? end;
        public RecurrenceFrequency? frequency;
        public int? interval;
        public List<RecurrenceWeekday>? by_weekday;
        public List<RecurrenceNWeekday>? by_n_weekday;
        public List<RecurrenceMonth>? by_month;
        public List<int>? by_month_day;
        public List<int>? by_year_day;
        public int? count;
    }

    public enum EventPrivacyLevel
    {
        GuildOnly = 2
    }

    public enum ScheduledEntityType
    {
        StageInstance = 1,
        Voice = 2,
        External = 3
    }

    public enum EventStatus
    {
        Scheduled = 1,
        Active = 2,
        Completed = 3,
        Canceled = 4
    }

    public class EntityMetadata
    {
        public string? location;
    }

    public enum RecurrenceFrequency
    {
        Yearly = 0,
        Monthly = 1,
        Weekly = 2,
        Daily = 3,
    }

    public enum RecurrenceWeekday
    {
        Monday = 0,
        Tuesday = 1,
        Wednesday = 2,
        Thursday = 3,
        Friday = 4,
        Saturday = 5,
        Sunday = 6
    }

    public class RecurrenceNWeekday
    {
        public int? n;
        public RecurrenceWeekday? day;
    }

    public enum RecurrenceMonth
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }
}
