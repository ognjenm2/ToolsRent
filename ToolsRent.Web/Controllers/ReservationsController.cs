using System;
using System.Collections.Generic;
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
        public ActionResult CreateReservationModal()
        {
            //return PartialView("~/Views/Administration/Shared/_EmployeeView.cshtml", viewModel);
            return PartialView("~/Views/Reservations/_ReservationView.cshtml");
        }


        [HttpGet]
        public ActionResult CreateToolReservationModal(int reservationID)
        {
            // Pass the reservation ID to the partial view
            ViewBag.ReservationID = reservationID;

            return PartialView("~/Views/Reservations/_ToolReservationView.cshtml");
        }

        [HttpGet]
        public ActionResult GetToolTypes()
        {
            using (var db = new Entities()) // Adjust the DbContext name
            {
                var toolTypes = db.Tools
                                  .Select(t => new {
                                      t.ID,
                                      t.ToolKind,
                                      t.Price
                                  })
                                  .Take(1000).ToList();

                return Json(toolTypes, JsonRequestBehavior.AllowGet);
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