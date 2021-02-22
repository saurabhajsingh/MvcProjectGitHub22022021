using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Hospital;
using KeystoneProject.Models.Hospital;
using System.Data;
using System.IO;


namespace KeystoneProject.Controllers.Hospital
{
    public class ChangePasswordController : Controller
    {
        BL_ChangePassword bl_pswd = new BL_ChangePassword();
        ChangePassword mod_pswd = new ChangePassword();

        public ActionResult ChangePassword()
        {
            return View();
        }




        public JsonResult Bind_User()
        {
            return new JsonResult { Data = bl_pswd.SelectAlluser(), JsonRequestBehavior = new JsonRequestBehavior() };
        }

        public ActionResult AjaxMethod_OldPSWD(string id)
        {
            List<string> searchList = new List<string>();

            DataTable dt = new DataTable();
            dt = bl_pswd.GetUSer(id);

            mod_pswd.oldpswd = dt.Rows[0]["Password"].ToString();

            searchList.Add(mod_pswd.oldpswd);

            return Json(searchList);
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePassword obj, FormCollection fc)
        {
            try
            {
                if (bl_pswd.SAVE(obj))
                {
                    ModelState.Clear();
                    TempData["Msg"] = "Password Updated Successfully !";
                    return RedirectToAction("ChangePassword", "ChangePassword");
                }
                else
                {
                    ModelState.Clear();
                    TempData["Msg"] = "Password Not Updated !";
                    return RedirectToAction("ChangePassword", "ChangePassword");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("ChangePassword", "ChangePassword");
            }
        }


    }
}