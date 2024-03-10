using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        public static List<ReservationModel> GetReservations(string sortOrder, int start, int length)
        {
            using (Entities db = new Entities())
            {
                List<ReservationModel> reservationList = new List<ReservationModel>();
                try
                {
                    var query = db.Reservations.AsQueryable();

                    //var data = query.OrderBy(x=>x.ReservationID).Skip(start).Take(length).ToList();
                    var data = query.ToList();
                    foreach (ToolsRent.Dal.Models.Reservations res in data)
                    {
                        reservationList.Add(new ReservationModel
                        {
                            ImePrez = res.ImePrezime,
                            OfferDate = res.OfferDateTime.Value,
                            OfferDateStr = res.OfferDateTime.Value.ToString("dd.MM.yyyy."),
                            Note = res.Note
                        }); ;
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
