using System;
using System.Collections.Generic;
using Exiled.API.Features;

namespace BanHammer
{
    public class Methods
    {
        public Dictionary<Player, string> CustomReasons = new Dictionary<Player, string>();

        public string ParseTime(long duration)
        {
            TimeSpan time = TimeSpan.FromMinutes(duration);

            string durationString = "";
            
            if (time.TotalDays >= 365)
                durationString = $"{time.TotalDays / 365} years";
            else if(time.TotalDays >= 30)
                durationString = $"{time.TotalDays / 30} months";
            else if (time.TotalDays >= 1)
                durationString = $"{time.TotalDays} days";
            else if (time.Hours > 0)
                durationString = $"{time.TotalHours} hours";
            
            if (time.Minutes > 0)
                durationString = $"{time.TotalMinutes} minutes";
            if (time.Seconds > 0)
                durationString = $"{time.TotalSeconds} seconds";

            return string.IsNullOrEmpty(durationString) ? "Kick" : durationString;
        }

        public string GetCustomReason(Player p)
        {
            if (CustomReasons.TryGetValue(p, out string r))
            {
                return r;
            }
            return "none";
        }
    }
}