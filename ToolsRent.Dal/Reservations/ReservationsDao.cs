using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ToolsRent.Dal.Models;
using ToolsRent.Models;

namespace ToolsRent.Dal.Reservations
{
    public class ReservationsDao
    {
        public static int CreateReservation(ReservationModel res)
        {
            using (Entities db = new Entities())
            {
                 
                try
                {

                      ToolsRent.Dal.Models.Reservations reservation = new ToolsRent.Dal.Models.Reservations
                    {
                        ImePrezime = res.ImePrez,  
                        OfferDateTime = res.OfferDate, 
                        Note = res.Note 
                    };

                    // Add the new reservation entity to the database context
                    db.Reservations.Add(reservation);

                    // Save changes to the database
                    db.SaveChanges();

                    // Return the ID of the newly created reservation
                    return reservation.ReservationID;
                }
                catch (Exception ex)
                {
                    return 0;
                }

            }
        }

        public static int CreateToolReservation(ToolReservationModel tool)
        {
            using (Entities db = new Entities())
            {
                try
                {
                    ToolsRent.Dal.Models.ToolsReservations toolRes = new ToolsRent.Dal.Models.ToolsReservations
                    {
                        ReservationID = tool.ReservationID,
                        ToolID = tool.ToolID,
                        DateFrom = DateTime.Parse(tool.DateFrom),  
                        DateTo = DateTime.Parse(tool.DateTo),  
                        Price = tool.Price
                    };

                    // Add the new reservation entity to the database context
                    db.ToolsReservations.Add(toolRes);

                    // Save changes to the database
                    db.SaveChanges();

                    // Return the ID of the newly created reservation
                    return toolRes.ToolReservationID;
                }
                catch (Exception ex)
                {
                    return 0;
                }

            }
        }

        public static void DeleteToolReservation(int toolReservationID, int reservationID)
        {
            using (Entities db = new Entities())
            {
                
                try
                {
                var toolReservation = db.ToolsReservations
               .Where(tr => tr.ToolReservationID == toolReservationID && tr.ReservationID == reservationID)
               .FirstOrDefault();

                    if (toolReservation != null)
                    {
                        
                        db.ToolsReservations.Remove(toolReservation);
                        db.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Tool reservation not found for deletion.");
                    }
                }
                catch (Exception ex)
                {
                    //Error  hand
                }
                 
            }
        }


        public static void DeleteReservation(int reservationID)
        {
            using (Entities db = new Entities())
            {
                try
                {
                    // Find and delete all tool reservations associated with the given reservationID
                    var toolReservationsToDelete = db.ToolsReservations.Where(tr => tr.ReservationID == reservationID);
                    db.ToolsReservations.RemoveRange(toolReservationsToDelete);

                    // Find and delete the reservation itself
                    var reservationToDelete = db.Reservations.Find(reservationID);
                    if (reservationToDelete != null)
                    {
                        db.Reservations.Remove(reservationToDelete);
                    }

                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    // Handle the exception
                    throw new Exception("Error deleting tool reservations and reservation: " + ex.Message);
                }
            }
        }

        public static List<ToolReservationModel> GetReseGetToolReservationsByReservationIDrvations(int reservationID)
        {
            using (Entities db = new Entities())
            {
                List<ToolReservationModel> reservationList = new List<ToolReservationModel>();
                try
                {
                    var query = db.ToolsReservations.AsQueryable();

                    //var data = query.OrderBy(x=>x.ReservationID).Skip(start).Take(length).ToList();
                    var data = query.Where(x => x.ReservationID == reservationID).ToList();
                    foreach (ToolsRent.Dal.Models.ToolsReservations res in data)
                    {
                        reservationList.Add(new ToolReservationModel
                        {
                            ToolReservationID = res.ToolReservationID,
                            ReservationID = res.ReservationID.Value,
                            ToolID = res.ToolID.Value,
                            ToolType = res.Tools.ToolKind,
                            DateFrom = res.DateFrom.Value.ToString("dd.MM.yyyy."),
                            DateTo = res.DateTo.Value.ToString("dd.MM.yyyy."),
                            Price = res.Price.Value
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

        
        public static ReservationModel GetReservationByID(int reservationID)
        {
            using (Entities db = new Entities())
            {
                ReservationModel reservationList = new ReservationModel();
                try
                {
                    var query = db.Reservations.AsQueryable();
                    var data = query.Where(x => x.ReservationID == reservationID).SingleOrDefault();                   
                     return new ReservationModel
                        {
                            ReservationID = data.ReservationID,
                            ImePrez = data.ImePrezime,
                            OfferDate = data.OfferDateTime.Value,
                            OfferDateStr = data.OfferDateTime.Value.ToString("dd.MM.yyyy."),
                            Note = data.Note
                        };
                    
                }
                catch (Exception ex)
                {
                    //Error  hand
                }
                return reservationList;
            }
        }

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
                            ReservationID = res.ReservationID,
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
