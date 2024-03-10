using System;
using System.Collections.Generic;
using ToolsRent.Dal.Models;
using ToolsRent.Dal.Reservations;
using ToolsRent.Models;
namespace ToolsRent.Bll.Reservations
{
    public class ReservationsManager
    {
        public static object CreateReservation(ReservationModel res)
        {
            throw new NotImplementedException();
        }

        public static int CreateReservation()
        {
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
