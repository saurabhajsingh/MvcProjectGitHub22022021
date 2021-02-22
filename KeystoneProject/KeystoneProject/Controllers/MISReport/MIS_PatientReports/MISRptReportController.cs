using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.Report
{
    public class MISRptReportController : Controller
    {
        //
        // GET: /MISRptReport/
        public ActionResult RPTMISServiceWiseCollectionReport()
        {
            return View();
        }
	}
}