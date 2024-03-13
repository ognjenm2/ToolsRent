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
                return Json(new { success = true, message = "Tool reservation deleted successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
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
            catch (Exception ex)            {
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
                    Note =  model.Note,
                    PriceAll = model.PriceAll
                });

                return Json(new { success = true, message = "Reservation created successfully", reservationID = reservationID });
            }
            catch (Exception e)
            {
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
                    DateFrom = model.DateFrom,
                    DateTo = model.DateTo,
                    Price = model.Price
                };

                int result = ReservationsManager.CreateToolReservation(tool);
                return Json(new { success = true, message = "Tool Reservation created successfully", reservationID = model.ReservationID });
            }
            catch (Exception e)
            {
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
            ViewBag.ReservationID = reservationID;
            return PartialView("~/Views/Reservations/_ToolReservationView.cshtml");
        }

        [Obsolete]
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
                int pageSize = 50;
                int skipCount = (page - 1) * pageSize;
                var toolTypesQuery = db.Tools.Select(t => new
                {
                    id = t.ID,
                    text = t.ToolKind + "        €" + t.Price
                });

                if (!string.IsNullOrEmpty(search))
                {
                    toolTypesQuery = toolTypesQuery.Where(t => t.text.Contains(search));
                }

                var toolTypes = toolTypesQuery.OrderBy(t => t.text)
                                              .Skip(skipCount)
                                              .Take(pageSize)
                                              .ToList();

                var jsonResponse = new
                {
                    results = toolTypes,
                    pagination = new
                    {
                        more = toolTypes.Count == pageSize
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