using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsRent.Models
{
    public class ToolReservationModel
    {
        public int ToolReservationID { get; set; }

        public int ReservationID { get; set; }

        public int ToolID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int Price { get; set; }
    }
}
