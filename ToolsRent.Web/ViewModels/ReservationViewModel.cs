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
        public string OfferDate { get; set; }

        public string Note { get; set; }

        public decimal PriceAll { get; set; }

        public List<ToolReservationViewModel> ToolReservations { get; set; }
    }
}