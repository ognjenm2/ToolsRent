using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ToolsRent.Dal.Models;
using ToolsRent.Dal.Reservations;
using ToolsRent.Models;
namespace ToolsRent.Bll.Reservations
{
    public class ReservationsManager
    {
        public static List<ReservationModel> GetReservations()
        {
            List<ReservationModel> reservations = new List<ReservationModel>();
            try
            {
                reservations = ReservationsDao.GetReservations();
            }
            catch (Exception ex)
            {
                //error handler
            }
            return reservations;
        }
    }
}
