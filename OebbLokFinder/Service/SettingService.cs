using Config.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OebbLokFinder.Service
{
    public class SettingService
    {
        public ISetting Setting { get; }

        public SettingService()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LokFinder", "Config.json");
            Setting = new ConfigurationBuilder<ISetting>().UseJsonFile(path)
                                                        .Build();
        }
    }

    public interface ISetting
    {
        /// <summary>
        /// Holds the hours that need to be preloaded for a given rollingstock.
        /// </summary>
        [Option(DefaultValue = 6)]
        public int RollingStockDataPreloadMin { get; set; }

        /// <summary>
        /// Holds the hours of how often a rolling stock should be refreshed. Use 0 to turn automatic refresh off.
        /// </summary>
        [Option(DefaultValue = 0)]
        public uint RefreshRollingStockCycle { get; set; }
    }
}