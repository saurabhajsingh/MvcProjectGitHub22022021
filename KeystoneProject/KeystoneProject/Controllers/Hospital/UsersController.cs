using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Keystone;
using KeystoneProject.Buisness_Logic.Hospital;
using System.Data;
using System.Data.SqlClient;

namespace KeystoneProject.Controllers.Hospital
{
    public class UsersController : Controller
    {
        //
        // GET: /Users/
        BL_Users user = new BL_Users();
        public ActionResult Users()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Users(Users obj)
        {
            try
            {
                if(obj.chbAuthorizationRights=="on")
                {
                    obj.chbAuthorizationRights = "true";
                }
                else
                {
                    obj.chbAuthorizationRights = "false";
                }
                string A = Request.Form["FullName"];
                TempData["Msg"] = "";
                if (Request.Form["FullName"] != null)
                {
                    if (obj.UserID > 0)
                    {
                        if (user.Save(obj))
                        {
                            obj.FullName = "";
                            TempData["Msg"] = "Update Successfully";
                            ModelState.Clear();
                            RedirectToAction("Users", "Users");
                        }
                        RedirectToAction("Users", "Users");
                    }
                    else
                    {
                        try
                        {
                            if (user.Save(obj))
                            {
                                if (obj.UserID > 0)
                                {
                                    obj.FullName = "";
                                    TempData["Msg"] = "Update Successfully";
                                    ModelState.Clear();
                                    RedirectToAction("Users", "State");
                                }
                                TempData["Msg"] = "Save Successfully";
                                RedirectToAction("Users", "Users");
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Msg"] = ex.Message;
                return RedirectToAction("Users", "Users");
            }
            return RedirectToAction("Users", "Users");
        }

        public JsonResult GetRoleRecord(string prefix)
        {
            DataSet role = new DataSet();
            role = user.GetRole(prefix);
            List<Users> search = new List<Users>();
            foreach (DataRow dr in role.Tables[0].Rows)
            {
                search.Add(new Users
                {
                    RoleID = Convert.ToInt32(dr["RoleID"]),
                    RoleName = dr["RoleName"].ToString(),

                });
            }
            return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult DatatableBind(string prefix)
        {
            Users Obj_Model = new Models.Keystone.Users();
            Obj_Model.StoreAllUsers = user.SelectAllData();
            List<Users> serch = new List<Users>();
            foreach (DataRow dr in Obj_Model.StoreAllUsers.Tables[0].Rows)
            {
                serch.Add(new Users
                {
                    UserID = Convert.ToInt32(dr["UserID"]),
                    FullName = dr["FullName"].ToString(),

                });
            }
            return new JsonResult { Data = serch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult Fill(int UserID)
        {
            DataSet ds = new DataSet();
            List<Users> Search = new List<Users>();
            ds = user.GetUsers(UserID);
            Users addusers = new Users();
            addusers.UserID = Convert.ToInt32(ds.Tables[0].Rows[0]["UserID"].ToString());
            addusers.FullName = ds.Tables[0].Rows[0]["FullName"].ToString();
            addusers.LoginName = ds.Tables[0].Rows[0]["LoginName"].ToString();

            KeystoneProject.Buisness_Logic.Master.BL_MasterSetting obj4 = new Buisness_Logic.Master.BL_MasterSetting();
            DataSet dsMasterSetting = new DataSet();
            dsMasterSetting = obj4.GetMasterSetting();
            if (Convert.ToBoolean( dsMasterSetting.Tables[0].Rows[0]["ForAuthorization"]) == true)
            {
                addusers.chbAuthorizationRights = ds.Tables[0].Rows[0]["AuthorizationRights"].ToString();
            }
            else
            {
                addusers.chbAuthorizationRights = "false";
            }

            addusers.Password = ds.Tables[0].Rows[0]["Password"].ToString();
            addusers.Password2 = ds.Tables[0].Rows[0]["Password2"].ToString();
            addusers.RoleID2 = ds.Tables[0].Rows[0]["RoleID2"].ToString();
            addusers.EmailID = ds.Tables[0].Rows[0]["EmailID"].ToString();
            addusers.RoleName = ds.Tables[0].Rows[0]["RoleName"].ToString();
            addusers.UserDetailsID =Convert.ToInt32( ds.Tables[0].Rows[0]["UserDetailsID"]);
            if (ds.Tables[1].Rows.Count>0)
            addusers.RoleName2 = ds.Tables[1].Rows[0]["RoleName2"].ToString();
            addusers.RoleID = Convert.ToInt32(ds.Tables[0].Rows[0]["RoleID"].ToString());
            addusers.Mode = "Edit";

            Search.Add(addusers);

            return new JsonResult { Data = Search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetAllHospitalLocation(string prefix)
        {
            Users Obj_Model = new Models.Keystone.Users();
            Obj_Model.StoreAllHospital = user.GetAllHospitalLocation();
            List<Users> serch = new List<Users>();
            foreach (DataRow dr in Obj_Model.StoreAllHospital.Tables[0].Rows)
            {
                serch.Add(new Users
                {
                    HospitalID = Convert.ToInt32(dr["HospitalID"]),
                    HospitalName = dr["HospitalName"].ToString(),

                });
            }
            return new JsonResult { Data = serch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult DeleteUser(int UserID)
        {
            string data = "";
            try
            {
                KeystoneProject.Buisness_Logic.Hospital.BL_Users bl_users = new Buisness_Logic.Hospital.BL_Users();
                int a = bl_users.DeleteUser(UserID); 
            if(a == 1)
            {
                data = "User Deleted Successfully ";
            }
            }
            catch(Exception ex)
            {
               data = ex.Message;
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
	}
}