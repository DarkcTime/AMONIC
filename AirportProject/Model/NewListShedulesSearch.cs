using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.Model
{
    class NewListShedulesSearch
    {
        public Model.Schedules Schedule { get; set; } 
        public string Date { get; set; }
        public string Time { get; set;  }
        public decimal CabinType { get; set; }
        public int NumberOfStops { get; set; }
    }
}
