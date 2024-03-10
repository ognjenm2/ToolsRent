using System;
using System.Collections.Generic;
using ToolsRent.Dal.Reservations;
using ToolsRent.Models;
namespace ToolsRent.Bll.Reservations
{
    public class ReservationsManager
    {
        public static List<ReservationModel> GetReservations(DTParameters param)
        {
            List<ReservationModel> reservations = new List<ReservationModel>();
            try
            {
                
                reservations = ReservationsDao.GetReservations(param.SortOrder, param.Start, param.Length);
            }
            catch (Exception ex)
            {
                //error handler
            }
            return reservations;
        }
    }
}
