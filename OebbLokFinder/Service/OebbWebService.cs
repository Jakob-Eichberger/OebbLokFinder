using HtmlAgilityPack;
using Newtonsoft.Json;
using OebbLokFinder.Infrastructure;
using OebbLokFinder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OebbLokFinder.Service
{
    public class OebbWebService
    {
        private const string ApiURL = "https://konzern-apps.web.oebb.at/lok/index/";

        public OebbWebService(Database db, StationService stationService, RollingstockService rollingstockService)
        {
            Db = db;
            StationService = stationService;
            RollingstockService = rollingstockService;
        }

        public event EventHandler? FetchedData = default;

        public Database Db { get; }

        public StationService StationService { get; }

        public RollingstockService RollingstockService { get; }

        public async Task UpdateStopsForAllRollingstock(Action<double>? rollingstockLoaded = null) => await Task.Run(async () =>
        {
            rollingstockLoaded?.Invoke(0);
            var rnd = new Random();
            double count = 0;
            double max = Db.Rollingstocks.Count();
            foreach (var rollingstock in Db.Rollingstocks.ToList())
            {
                await UpdateStopsAsync(rollingstock.ClassNumber, rollingstock.SerialNumber, rollingstock.Name, rollingstock.AddedManually);
                await Task.Delay(new TimeSpan(0, 0, rnd.Next(1, 4)));
                rollingstockLoaded?.Invoke(count++ / max);
            }
            FetchedData?.Invoke(this, null);
        });

        public async Task UpdateRollingstockFromLokfinderOebbWebsiteListe() => await Task.Run(async () =>
        {
            RollingstockService.RemoveAllAutomaticallyAddedRollingstock();

            HtmlDocument htmlDoc = new();
            htmlDoc.LoadHtml(await new HttpClient().GetStringAsync("https://lokfinder.oebb.at/"));

            foreach (var rollingstock in htmlDoc.DocumentNode.SelectNodes("//oebb-lokfinder-lok"))
            {
                var match = new Regex(@"([0-9]{1,4})\.([0-9]{1,4})").Match(rollingstock.GetAttributeValue("unit-number", ""));

                await Db.AddAsync(new Rollingstock
                {
                    AddedManually = false,
                    ClassNumber = int.Parse(match.Groups[1].Value),
                    SerialNumber = int.Parse(match.Groups[2].Value),
                    Name = rollingstock.GetAttributeValue("label", "")
                });
            }

            FetchedData?.Invoke(this, null);
        });

        private async Task<IEnumerable<RollingstockStopDto>> GetStopsForRollingstock(Rollingstock rollingstock) => await Task.Run(async () =>
        {
            var uri = new Uri(@$"{ApiURL}{rollingstock.ClassNumber:D4}.{rollingstock.SerialNumber:D4}");
            var response = await new HttpClient().GetAsync(uri);
            if (response.StatusCode is HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<RollingstockStopDto>>(content) ?? [];
            }
            else
            {
                return [];
            }
        });

        /// <summary>
        /// Updates the <see cref="Stop"/>s for a given <paramref name="classNumber"/> and <paramref name="serialNumber"/> combination in the database.
        /// </summary>
        /// <param name="classNumber"></param>
        /// <param name="serialNumber"></param>
        /// <param name="rollingstockName"></param>
        /// <param name="addedManually"></param>
        /// <returns></returns>
        private async Task UpdateStopsAsync(int classNumber, int serialNumber, string rollingstockName = "", bool addedManually = true) => await Task.Run(async () =>
        {
            Rollingstock rollingstock = await RollingstockService.GetOrCreatRollingstockAsync(classNumber, serialNumber, rollingstockName, addedManually);

            await RollingstockService.RemoveStationsFromVehilce(rollingstock);

            var stops = await GetStopsForRollingstock(rollingstock);
            rollingstock.Stops.AddRange(stops.Select(e => new Stop()
            {
                Rollingstock = rollingstock,
                Station = new Station
                {
                    StationName = e.Name.Trim(),
                    StationNameAbbr = e.Abbr.Trim(),
                    Lat = decimal.Parse(e.Lat),
                    Lng = decimal.Parse(e.Lng),
                },
                Arrival = e.Arrival,
                Departure = e.Departure
            }));
            await RollingstockService.UpdateVehilce(rollingstock);
        });

        private class RollingstockStopDto
        {
            [JsonProperty("abbr")]
            public string Abbr { get; set; }

            [JsonProperty("arrival")]
            public DateTime? Arrival { get; set; }

            [JsonProperty("departure")]
            public DateTime Departure { get; set; }

            [JsonProperty("lat")]
            public string Lat { get; set; }

            [JsonProperty("lng")]
            public string Lng { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("train_number")]
            public string TrainNumber { get; set; }

            [JsonProperty("unit_number")]
            public string UnitNumber { get; set; }
        }

    }
}
