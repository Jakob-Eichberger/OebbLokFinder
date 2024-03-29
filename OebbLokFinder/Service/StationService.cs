using OebbLokFinder.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OebbLokFinder.Service
{
    public class StationService
    {
        public StationService(Database db, OebbWebService oebbWebService, SettingService settingService)
        {
            Db = db;
            OebbWebService = oebbWebService;
            SettingService = settingService;
        }

        private Database Db { get; }

        private OebbWebService OebbWebService { get; }

        private SettingService SettingService { get; }

        /// <summary>
        /// Updates the stops for all rolling stock in the database that meet the criteria. 
        /// </summary>
        /// <returns></returns>
        public async Task UpdateStopsForRollingStockAsync() => await Task.Run(async () =>
        {
            var stock = Db.Rollingstocks.AsEnumerable().Where(e => SettingService.Setting.RefreshRollingStockCycle > 0 || e.Stops.Count == 0 || (e.Stops.Count > 0 && (e.Stops.Max(e => e.Departure) - DateTime.Now).TotalHours < SettingService.Setting.RollingStockDataPreloadMin));
            foreach (var item in stock)
            {
                await OebbWebService.UpdateStopsAsync(item);
                await Task.Delay(TimeSpan.FromSeconds(3));
            }
        });
    }
}