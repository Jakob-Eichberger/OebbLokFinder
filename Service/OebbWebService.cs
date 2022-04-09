using HtmlAgilityPack;
using Infrastructure;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Service
{
    public class OebbWebService : BaseService
    {
        public OebbWebService(Database db, StationService stationService, VehicleService vehicleService) : base(db)
        {
            StationService = stationService;
            VehicleService = vehicleService;
        }

        public StationService StationService { get; }

        public VehicleService VehicleService { get; }

        private const string ApiURL = "https://konzern-apps.web.oebb.at/lok/index/";

        public event EventHandler FetchedData;

        /// <summary>
        /// Updates the <see cref="Stop"/>s for a given <paramref name="classNumber"/> and <paramref name="serialNumber"/> combination in the database.
        /// </summary>
        /// <param name="classNumber"></param>
        /// <param name="serialNumber"></param>
        /// <param name="vehicleName"></param>
        /// <param name="addedManually"></param>
        /// <returns></returns>
        public async Task UpdateStopsAsync(int classNumber, int serialNumber, string vehicleName = null, bool addedManually = true) => await Task.Run(async () =>
        {
            Vehicle vehicle = await VehicleService.GetOrCreatVehicleAsync(classNumber, serialNumber, vehicleName, addedManually);

            await VehicleService.RemoveStationsFromVehilce(vehicle);

            vehicle.Stops.AddRange((await Task.WhenAll((await GetVehicleJsonMapListAsync(classNumber, serialNumber)).Select(async e => new Stop()
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
            }))));

            await VehicleService.UpdateVehilce(vehicle);
            FetchedData?.Invoke(null, null);
        });

        public async Task UpdateStopsForAllVehicles(Action<double>? vehicleLoaded = null) => await Task.Run(async () =>
        {
            vehicleLoaded?.Invoke(0);
            var rnd = new Random();
            double count = 0;
            double max = Db.Vehicles.Count();
            foreach (var vehicle in Db.Vehicles.ToList())
            {
                await UpdateStopsAsync(vehicle.ClassNumber, vehicle.SerialNumber, vehicle.Name, vehicle.AddedManually);
                await Task.Delay(new TimeSpan(0, 0, rnd.Next(4, 10)));
                vehicleLoaded?.Invoke(count++ / max);
            }
            FetchedData?.Invoke(null, null);
        });

        private async Task<IEnumerable<VehicleJsonMap>> GetVehicleJsonMapListAsync(int classNumber, int serialNumber) => await Task.Run(async () =>
        {
            var uri = new Uri(@$"{ApiURL}{classNumber:D4}.{serialNumber:D4}");
            var response = await new HttpClient().GetStringAsync(uri);
            IEnumerable<VehicleJsonMap> List = JsonConvert.DeserializeObject<List<VehicleJsonMap>>(response);
            FetchedData?.Invoke(null, null);
            return List;
        });

        public async Task UpdateVehilcesFromLokfinderOebbWebsiteListe() => await Task.Run(async () =>
        {
            VehicleService.RemoveAllAutomaticallyAddedVehicles();

            HtmlDocument htmlDoc = new();
            htmlDoc.LoadHtml(await new HttpClient().GetStringAsync("https://lokfinder.oebb.at/"));

            List<Vehicle> vehicles = new();
            foreach (var vehicle in htmlDoc.DocumentNode.SelectNodes("//oebb-lokfinder-lok"))
            {
                var match = new Regex(@"([0-9]{1,4})\.([0-9]{1,4})").Match(vehicle.GetAttributeValue("unit-number", ""));

                vehicles.Add(new Vehicle
                {
                    AddedManually = false,
                    ClassNumber = int.Parse(match.Groups[1].Value),
                    SerialNumber = int.Parse(match.Groups[2].Value),
                    Name = vehicle.GetAttributeValue("label", "")
                });
            }

            VehicleService.AddVehicleRange(vehicles);
            FetchedData?.Invoke(null, null);
        });

        private class VehicleJsonMap
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
