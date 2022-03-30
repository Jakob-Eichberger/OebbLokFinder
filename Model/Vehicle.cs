using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Vehicle
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public VehicleClassification VehicleClassification { get; set; }

        public bool AddedManually { get; set; } = false;

        public List<Stop> Stops { get; set; } = new();
    }

}
