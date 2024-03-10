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