using Serilog;
using System;
using System.Collections.Generic;
using ToolsRent.Dal.Models;
using ToolsRent.Dal.Reservations;
using ToolsRent.Models;
namespace ToolsRent.Bll.Reservations
{
    public class ReservationsManager
    {
        public static int CreateReservation(ReservationModel res)
        {
            int resID = 0;
            try
            {
                resID = ReservationsDao.CreateReservation(res);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occured");
            }
            return resID;
        }

        public static int CreateToolReservation(ToolReservationModel tool)
        {
            int resID = 0;
            try
            {
                resID = ReservationsDao.CreateToolReservation(tool);
                ReservationsDao.RefreshPriceAllsFporReservationID(tool.ReservationID);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occured");
            }
            return resID;
        }

        public static void DeleteReservation(int reservationID)
        {
            try
            {
                ReservationsDao.DeleteReservation(reservationID);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occured");
            }
        }

        public static void DeleteToolReservation(int toolReservationID, int reservationID)
        {
            try
            {
                ReservationsDao.DeleteToolReservation(toolReservationID, reservationID);
                ReservationsDao.RefreshPriceAllsFporReservationID(reservationID);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occured");
            }
        }

        public static object GetReservationByID(int reservationID)
        {
            ReservationModel reservations = null;
            try
            {
                reservations = ReservationsDao.GetReservationByID(reservationID);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occured");
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
                Log.Error(ex, "Error occured");
            }
            return reservations;
        }

        public static List<ToolReservationModel> GetToolReservationsByReservationID(int reservationID)
        {
            List<ToolReservationModel> reservations = new List<ToolReservationModel>();
            try
            {
                reservations = ReservationsDao.GetToolReservationsByReservationID(reservationID);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occured");
            }
            return reservations;
        }
    }
}
