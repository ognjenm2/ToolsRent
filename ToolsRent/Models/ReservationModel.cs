using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsRent.Models
{
    public class ReservationModel
    {
        public string  ImePrez { get; set; }
        public int ReservationID { get; set; }
        public DateTime OfferDate { get; set; }

        public  string OfferDateStr { get; set; }       

        public string  Note { get; set; }      

        public int PriceAll { get; set; }



    }
}
