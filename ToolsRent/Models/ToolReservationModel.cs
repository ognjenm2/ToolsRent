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

        public string ToolType { get; set; }

        public int ToolID { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public decimal Price { get; set; }
    }
}
