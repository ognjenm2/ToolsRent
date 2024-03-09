using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsRent.Dal.Models;
using ToolsRent.Models;

namespace ToolsRent.Dal.Reservations
{
    public class ReservationsDao
    {
        public static List<ReservationModel> GetReservations()
        {
            using (TestDBToolsReservationEntities db = new TestDBToolsReservationEntities())
            {
                List<ReservationModel> reservationList = new List<ReservationModel>();
                try
                {
                    var query = db.ToolReservations.AsQueryable();

                    var data = query.ToList();

                    foreach (ToolReservations toolRes in data)
                    {
                        reservationList.Add(new ReservationModel
                        {
                            ImePrez = toolRes.ImePrezime,
                            From = toolRes.DateTimeFrom.Value,
                            To = toolRes.DateTimeTo.Value,
                            Note = toolRes.Note,
                            ToolType = toolRes.ToolType
                        });
                    }
                }
                catch (Exception ex)
                {
                    //Error  hand
                }
                return reservationList;
            }
        }
    }
}
