using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToolsRent.Bll.Reservations;
using ToolsRent.Dal.Models;
using ToolsRent.Models;
using ToolsRent.Web.Translators;
using ToolsRent.Web.ViewModels;

namespace ToolsRent.Web.Controllers
{
    public class ReservationsController : Controller
    {
        // GET: Reservations
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ReservationsHandler(DTParameters param)
        {
            try
            {
                
                var reservations = ReservationsManager.GetReservations(param);

                var result = new DTResult<ReservationModel>
                {
                    draw = param.Draw,
                    //data = ReservationsTranslator.Translate(reservations)
                    data = reservations
                };

                return Json(result);
            }
            catch (Exception e)
            {
                return Json(new { error = e.Message });
            }
        }

        
        public JsonResult GetReservationByID(int reservationID)
        {
            try
            {
                var reservation = ReservationsManager.GetReservationByID(reservationID);

                 

                return Json(reservation, JsonRequestBehavior.AllowGet);

            
            }
            catch (Exception e)
            {
                return Json(new { error = e.Message });
            }
        }

        [HttpGet]
        public ActionResult DeleteToolReservationFromReservation(int toolReservationID, int reservationID)
        {
            try
            {
                ReservationsManager.DeleteToolReservation(toolReservationID, reservationID);

                // Assuming successful deletion
                return Json(new { success = true, message = "Tool reservation deleted successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Handle exception if deletion fails
                return Json(new { success = false, message = "Failed to delete tool reservation: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult DeleteReservation(int reservationID)
        {
            try
            {
                ReservationsManager.DeleteReservation(reservationID);

                return Json(new { success = true, message = "Reservation and associated tool reservations deleted successfully." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while deleting reservation: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult CreateReservation(ReservationViewModel model)
        {
            try
            {
               
                ReservationModel res = new ReservationModel();
                int reservationID = ReservationsManager.CreateReservation(new ReservationModel
                {
                    ImePrez = model.ImePrez,
                    OfferDate = model.OfferDate,
                    Note =  model.Note
                });

                // Assuming the operation was successful, return a success message
                return Json(new { success = true, message = "Reservation created successfully", reservationID = reservationID });
            }
            catch (Exception e)
            {
                // If there's an exception, return an error message
                return Json(new { success = false, message = "Failed to create reservation: " + e.Message });
            }
        }

        [HttpPost]
        public ActionResult CreateToolReservation(ToolReservationViewModel model)
        {
            try
            {
                ToolReservationModel tool = new ToolReservationModel
                {
                    ReservationID = model.ReservationID,
                    ToolID = model.ToolID,
                    DateFrom = model.DateFrom.ToString("dd.MM.yyyy."),
                    DateTo = model.DateTo.ToString("dd.MM.yyyy."),
                    Price = model.Price
                };

                int result = ReservationsManager.CreateToolReservation(tool);

                // Assuming the operation was successful, return a success message
                return Json(new { success = true, message = "Tool Reservation created successfully", reservationID = model.ReservationID });
            }
            catch (Exception e)
            {
                // If there's an exception, return an error message
                return Json(new { success = false, message = "Failed to create reservation: " + e.Message });
            }
        }


        [HttpGet]
        public ActionResult InjectReservationView()
        {
           return PartialView("~/Views/Reservations/_ReservationView.cshtml");
        }


        [HttpGet]
        public ActionResult InjectToolReservationView(int reservationID)
        {
            // Pass the reservation ID to the partial view
            ViewBag.ReservationID = reservationID;

            return PartialView("~/Views/Reservations/_ToolReservationView.cshtml");
        }

        [HttpGet]
        public ActionResult GetToolTypes()
        {
            using (var db = new Entities()) 
            {
                var toolTypes = db.Tools
                                  .Select(t => new {
                                      t.ID,
                                      t.ToolKind,
                                      t.Price
                                  })
                                  .Take(5000).ToList();

                return Json(toolTypes, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetToolTypesSelect2(string search, int page)
        {
            using (var db = new Entities())
            {
                // Determine the number of items to skip based on the page number
                int pageSize = 50; // Adjust the page size as needed
                int skipCount = (page - 1) * pageSize;

                // Query for tool types based on the search term (if provided)
                var toolTypesQuery = db.Tools.Select(t => new
                {
                    id = t.ID,
                    text = t.ToolKind + "        €" + t.Price
                });

                // Filter by search term
                if (!string.IsNullOrEmpty(search))
                {
                    toolTypesQuery = toolTypesQuery.Where(t => t.text.Contains(search));
                }

                // Perform pagination by skipping and taking the appropriate number of results
                var toolTypes = toolTypesQuery.OrderBy(t => t.text)
                                              .Skip(skipCount)
                                              .Take(pageSize)
                                              .ToList();

                // Construct the JSON response with the 'results' key
                var jsonResponse = new
                {
                    results = toolTypes,
                    pagination = new
                    {
                        more = toolTypes.Count == pageSize // Indicates if more results are available
                    }
                };

                return Json(jsonResponse, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult GetToolReservationsByReservationID(DTParameters param)
        {
            try
            {
                List<ToolReservationModel> reservations = ReservationsManager.GetToolReservationsByReservationID(param.reservationID);

                var result = new DTResult<ToolReservationModel>
                {
                    draw = param.Draw,
                     
                    data = reservations
                };

                return Json(result);
            }
            catch (Exception e)
            {
                return Json(new { error = e.Message });
            }
        }
    }
}