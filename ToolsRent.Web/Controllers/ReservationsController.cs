using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToolsRent.Bll.Reservations;
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
        public ActionResult CreateReservation()
        {
            try
            {
                // Your reservation creation logic goes here
                ReservationModel res = new ReservationModel();
                int reservationID = ReservationsManager.CreateReservation();

                // Assuming the operation was successful, return a success message
                return Json(new { success = true, message = "Reservation created successfully" });
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
        public ActionResult CreateToolReservationModal()
        {
            return PartialView("~/Views/Reservations/_ToolReservationView.cshtml");
        }
    }
}