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
        /// Holds the hours of how many hours need to be preloaded for a given rollingstock.
        /// </summary>
        [Option(DefaultValue = 3)]
        public int RollingStockDataPreloadMin { get; set; }
    }
}