using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OebbLokFinder.Model
{
    public class Vehicle
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        [MaxLength(4)]
        public int ClassNumber { get; set; } = 0;

        [MaxLength(4)]
        public int SerialNumber { get; set; } = 0;

        public bool AddedManually { get; set; } = false;

        public List<Stop> Stops { get; set; } = [];

        public int Value => int.Parse($"{ClassNumber:D4}{SerialNumber:D4}");

        public override bool Equals(object obj) => obj is Vehicle vehicle && ClassNumber == vehicle.ClassNumber && SerialNumber == vehicle.SerialNumber;

        public override int GetHashCode() => HashCode.Combine(ClassNumber, SerialNumber);
    }

}
