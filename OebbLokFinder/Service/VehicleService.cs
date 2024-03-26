using OebbLokFinder.Infrastructure;
using OebbLokFinder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OebbLokFinder.Service
{
    public class VehicleService
    {
        public VehicleService(Database db)
        {
            Db = db;
        }

        private Database Db { get; }

        public async Task<Vehicle> GetOrCreatVehicleAsync(int classNumber, int serialNumber, string name = null, bool addedManually = true)
        {
            if (classNumber < 0 || serialNumber < 0)
                throw new ApplicationException($"Invalid paramters");

            if (Db.Vehicles.FirstOrDefault(e => e.SerialNumber == serialNumber && e.ClassNumber == classNumber) is not Vehicle vehicle)
            {
                await Db.AddAsync(new Vehicle
                {
                    Name = name,
                    ClassNumber = classNumber,
                    SerialNumber = serialNumber,
                    AddedManually = addedManually
                });

                vehicle = Db.Vehicles.FirstOrDefault(e => e.SerialNumber == serialNumber && e.ClassNumber == classNumber);
            }

            return vehicle;
        }

        public void RemoveAllAutomaticallyAddedVehicles()
        {
            var vehicles = Db.Vehicles.Where(e => !e.AddedManually);
            if (vehicles.Any())
            {
                Db.Vehicles.RemoveRange(vehicles);
            }
        }

        public async Task<Vehicle> RemoveStationsFromVehilce(Vehicle vehicle)
        {
            vehicle.Stops.Clear();
            await Db.UpdateAsync(vehicle);
            return vehicle;
        }

        public async Task UpdateVehilce(Vehicle vehicle) => await Db.UpdateAsync(vehicle);
    }
}
