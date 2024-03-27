using Microsoft.EntityFrameworkCore;
using OebbLokFinder.Infrastructure;
using OebbLokFinder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OebbLokFinder.Service
{
    public class RollingstockService
    {
        public RollingstockService(Database db)
        {
            Db = db;
        }

        private Database Db { get; }

        public event EventHandler RollingstocksAddedOrDeleted;

        public async Task<Rollingstock> GetOrCreatRollingstockAsync(int classNumber, int serialNumber, string name = "", bool addedManually = true)
        {
            if (classNumber < 0 || serialNumber < 0)
                throw new ApplicationException($"Invalid paramters");

            if (Db.Rollingstocks.FirstOrDefault(e => e.SerialNumber == serialNumber && e.ClassNumber == classNumber) is not Rollingstock rollingstock)
            {
                await Db.AddAsync(new Rollingstock
                {
                    Name = name,
                    ClassNumber = classNumber,
                    SerialNumber = serialNumber,
                    AddedManually = addedManually
                });

                rollingstock = Db.Rollingstocks.FirstOrDefault(e => e.SerialNumber == serialNumber && e.ClassNumber == classNumber);
                RollingstocksAddedOrDeleted?.Invoke(this, new());
            }

            return rollingstock;
        }

        /// <summary>
        /// Finds a <see cref="Rollingstock"/> with a given <paramref name="rollingstockId"/> and returns it. 
        /// </summary>
        /// <param name="rollingstockId"></param>
        /// <returns>Returns a <see cref="Rollingstock"/> object or null.</returns>
        public async Task<Rollingstock?> GetRollingstockByIdAsync(int rollingstockId)
        {
            return await Db.Rollingstocks.FirstOrDefaultAsync(e => e.Id == rollingstockId);
        }

        public void RemoveAllAutomaticallyAddedRollingstock()
        {
            var rollingstock = Db.Rollingstocks.Where(e => !e.AddedManually);
            if (rollingstock.Any())
            {
                Db.Rollingstocks.RemoveRange(rollingstock);
            }
        }

        public async Task<Rollingstock> RemoveStationsFromVehilce(Rollingstock rollingstock)
        {
            rollingstock.Stops.Clear();
            await Db.UpdateAsync(rollingstock);
            return rollingstock;
        }

        public async Task UpdateVehilce(Rollingstock rollingstock) => await Db.UpdateAsync(rollingstock);

        /// <summary>
        /// Gets all <see cref="Rollingstock"/> objects from the context.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Rollingstock>> GetAllVehicles()
        {
            return await Db.Rollingstocks.ToListAsync();
        }
    }
}
