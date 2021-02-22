using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models;

namespace KeystoneProject.Controllers.Login
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Login()
        {
            ViewBag.Message = "Your Login Page";

            return View();
        }
    [HttpPost]
        public ActionResult Login(FormCollection e)
        {
            ViewBag.Message = "Your Login Page";

            return RedirectToAction("Dashboard", "Home");
            //return View();
        }
     
	}
}