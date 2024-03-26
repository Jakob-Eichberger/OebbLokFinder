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

        public OebbWebService(Database db, StationService stationService, VehicleService vehicleService)
        {
            Db = db;
            StationService = stationService;
            VehicleService = vehicleService;
        }

        public event EventHandler? FetchedData = default;

        public Database Db { get; }

        public StationService StationService { get; }

        public VehicleService VehicleService { get; }

        public async Task UpdateStopsForAllVehicles(Action<double>? vehicleLoaded = null) => await Task.Run(async () =>
        {
            vehicleLoaded?.Invoke(0);
            var rnd = new Random();
            double count = 0;
            double max = Db.Vehicles.Count();
            foreach (var vehicle in Db.Vehicles.ToList())
            {
                await UpdateStopsAsync(vehicle.ClassNumber, vehicle.SerialNumber, vehicle.Name, vehicle.AddedManually);
                await Task.Delay(new TimeSpan(0, 0, rnd.Next(1, 4)));
                vehicleLoaded?.Invoke(count++ / max);
            }
            FetchedData?.Invoke(this, null);
        });

        public async Task UpdateVehiclesFromLokfinderOebbWebsiteListe() => await Task.Run(async () =>
        {
            VehicleService.RemoveAllAutomaticallyAddedVehicles();

            HtmlDocument htmlDoc = new();
            htmlDoc.LoadHtml(await new HttpClient().GetStringAsync("https://lokfinder.oebb.at/"));

            foreach (var vehicle in htmlDoc.DocumentNode.SelectNodes("//oebb-lokfinder-lok"))
            {
                var match = new Regex(@"([0-9]{1,4})\.([0-9]{1,4})").Match(vehicle.GetAttributeValue("unit-number", ""));

                await Db.AddAsync(new Vehicle
                {
                    AddedManually = false,
                    ClassNumber = int.Parse(match.Groups[1].Value),
                    SerialNumber = int.Parse(match.Groups[2].Value),
                    Name = vehicle.GetAttributeValue("label", "")
                });
            }

            FetchedData?.Invoke(this, null);
        });

        private async Task<IEnumerable<VehicleStopDto>> GetStopsForVehicle(Vehicle vehicle) => await Task.Run(async () =>
        {
            var uri = new Uri(@$"{ApiURL}{vehicle.ClassNumber:D4}.{vehicle.SerialNumber:D4}");
            var response = await new HttpClient().GetAsync(uri);
            if (response.StatusCode is HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<VehicleStopDto>>(content) ?? [];
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
        /// <param name="vehicleName"></param>
        /// <param name="addedManually"></param>
        /// <returns></returns>
        private async Task UpdateStopsAsync(int classNumber, int serialNumber, string vehicleName = "", bool addedManually = true) => await Task.Run(async () =>
        {
            Vehicle vehicle = await VehicleService.GetOrCreatVehicleAsync(classNumber, serialNumber, vehicleName, addedManually);

            await VehicleService.RemoveStationsFromVehilce(vehicle);

            var stops = await GetStopsForVehicle(vehicle);
            vehicle.Stops.AddRange(stops.Select(e => new Stop()
            {
                Vehicle = vehicle,
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
            await VehicleService.UpdateVehilce(vehicle);
        });

        private class VehicleStopDto
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
