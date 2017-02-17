using Org.Common;
using Org.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Org.SuperMall.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        ISubjectGradeService subService = ServiceUtil.GetService<ISubjectGradeService>();

        public ActionResult Index()
        {
            var model = subService.GetSudentGrades();
            return View(model);
        }

    }
}
