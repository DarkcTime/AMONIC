using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.Model
{
    class NewShedulesList
    {
        public string Date { get; set; }

        public string Time { get; set; }

        public decimal BusinessPrice { get; set; }

        public decimal FirstClassPrice { get; set; }
        public Model.Schedules Schedules { get; set; }
    }
}
