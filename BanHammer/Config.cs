using System.ComponentModel;
using Exiled.API.Interfaces;

namespace BanHammer
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        public bool Debug { get; set; } = false;

        [Description("Different ban durations for the hammer. In minutes.")]
        public long[] BanDurations { get; set; } =
        {
            0,
            1,
            5,
            15,
            30,
            60,
            180,
            300,
            480,
            720,
            1440,
            4320,
            10080,
            20160,
            43200,
            144000,
            525600,
            26280000
        };
    }
}