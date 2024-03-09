using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToolsRent.Bll.Reservations;

namespace ToolsRent.Web.Controllers
{
    public class ReservationsController : Controller
    {
        // GET: Reservations
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ReservationsHandler( )
        {
            try
            {
                
                var result = ReservationsManager.GetReservations( );

                return Json(result);
            }
            catch (Exception e)
            {
                //ExceptionHelper.LogException(e);
                return Json(new { error = e.Message });
            }
        }
    }
}