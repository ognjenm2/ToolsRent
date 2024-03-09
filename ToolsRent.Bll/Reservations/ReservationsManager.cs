using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ToolsRent.Dal.Models;
namespace ToolsRent.Bll.Reservations
{
    public class ReservationsManager
    {
        public static DTResult<ReservationsModel> GetActivities()
        {
            using (var db = new TestDBToolsReservationEntities())
            {

                Expression<Func<Activity, bool>> filter = null;
                if (!String.IsNullOrWhiteSpace(param.Search.Value))
                {
                    filter = (p =>
                            p.ActivityCode.Contains(param.Search.Value) ||
                            p.Description.Contains(param.Search.Value)
                        );
                }
                //
                var data = AdministrationQueries.GetActivities(db, param.SortOrder, param.Start, param.Length, filter);
                int count = AdministrationQueries.CountActivities(db, filter);
                //
                var result = new DTResult<CodeListModel>
                {
                    draw = param.Draw,
                    data = AdministrationTranslator.Translate(data),
                    recordsFiltered = count,
                    recordsTotal = count
                };

                return result;
            }
        }
    }
}
