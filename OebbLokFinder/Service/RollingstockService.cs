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

        public async Task<Rollingstock> GetOrCreatRollingstockAsync(int classNumber, int serialNumber, string name = null, bool addedManually = true)
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
            }

            return rollingstock;
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
    }
}
