using Serilog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

                    string format = "dd/MM/yyyy HH:mm:ss";

                    DateTime dateOffer;
                    DateTime.TryParseExact(res.OfferDate, format,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out dateOffer);

                    ToolsRent.Dal.Models.Reservations reservation = new ToolsRent.Dal.Models.Reservations
                    {
                        ImePrezime = res.ImePrez,  
                        OfferDateTime = dateOffer, 
                        Note = res.Note,
                        PriceAll = res.PriceAll
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
                    Log.Error(ex, "Error occured");
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
                    string format = "dd/MM/yyyy HH:mm:ss";

                    DateTime dateTo;
                        DateTime.TryParseExact(tool.DateTo, format, 
                        System.Globalization.CultureInfo.InvariantCulture, 
                        System.Globalization.DateTimeStyles.None, out dateTo);

                    DateTime dateFrom;
                    DateTime.TryParseExact(tool.DateFrom, format,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out dateFrom);

                    ToolsRent.Dal.Models.ToolsReservations toolRes = new ToolsRent.Dal.Models.ToolsReservations
                    {
                        ReservationID = tool.ReservationID,
                        ToolID = tool.ToolID,
                        DateFrom = dateFrom,  
                        DateTo = dateTo,  
                        Price = tool.Price
                    };

                    db.ToolsReservations.Add(toolRes);

                    // Save changes to the database
                    db.SaveChanges();

                    // Return the ID of the newly created reservation
                    return toolRes.ToolReservationID;
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error occured");
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


                        var totalPrice = db.ToolsReservations
                       .Where(tr => tr.ReservationID == reservationID)
                       .Sum(tr => tr.Price) ?? 0;

                        var reservationToUpdate = db.Reservations.Find(reservationID);
                        if (reservationToUpdate != null)
                        {
                            reservationToUpdate.PriceAll = totalPrice;
                            db.Entry(reservationToUpdate).State = EntityState.Modified;
                        }



                        db.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Tool reservation not found for deletion.");
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error occured");
                }
                 
            }
        }



        public static void RefreshPriceAllsFporReservationID(int reservationID)
        {
            using (Entities db = new Entities())
            {
                try
                {
                   
                    var totalPrice = db.ToolsReservations
                    .Where(tr => tr.ReservationID == reservationID)
                    .Sum(tr => tr.Price) ?? 0;

                    var reservationToUpdate = db.Reservations.Find(reservationID);
                    if (reservationToUpdate != null)
                    {
                        reservationToUpdate.PriceAll = totalPrice;
                        db.Entry(reservationToUpdate).State = EntityState.Modified;
                    }

                    db.SaveChanges();
                    
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error occured");
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
                    throw new Exception("Error deleting tool reservations and reservation: " + ex.Message);
                }
            }
        }

        public static List<ToolReservationModel> GetToolReservationsByReservationID(int reservationID)
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
                            DateFrom = res.DateFrom.Value.ToString("dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                            DateTo = res.DateTo.Value.ToString("dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                            Price = res.Price.Value
                        }); 
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error occured");
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
                        OfferDate = data.OfferDateTime.Value.ToString("dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                        Note = data.Note,
                        PriceAll = data.PriceAll.Value
                        };
                    
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error occured");
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
                            OfferDate = res.OfferDateTime.Value.ToString("dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                            Note = res.Note,
                            PriceAll = res.PriceAll.Value
                        }); ;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error occured");
                }
                return reservationList;
            }
        }
    }
}
