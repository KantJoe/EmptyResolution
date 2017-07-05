using Org.Common;
using Org.Common.Mvc;
using Org.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Org.SuperMall.Controllers
{
    public class HomeController : BaseController
    {
        ISubjectGradeService sgService = ServiceUtil.GetService<ISubjectGradeService>();
        public ActionResult Index()
        {
            var model = sgService.GetSudentGrades();
            return View(model.Result);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}