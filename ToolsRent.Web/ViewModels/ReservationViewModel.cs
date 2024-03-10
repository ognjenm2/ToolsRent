using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToolsRent.Web.ViewModels
{
    public class ReservationViewModel
    {

        public string ImePrez { get; set; }
        public int ReservationID { get; set; }
        public string From { get; set; }
        public string To { get; set; }

        public int PriceAll { get; set; }

        public string Note { get; set; }

        public string ToolType { get; set; }

        /// public List<ToolReservations>
    }
}