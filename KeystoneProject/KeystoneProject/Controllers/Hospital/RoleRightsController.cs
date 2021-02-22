using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Keystone;
using KeystoneProject.Buisness_Logic.Hospital;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace KeystoneProject.Controllers.Hospital
{


    public class RoleRightsController : Controller
    {
        BL_RolesRights RolesRights = new BL_RolesRights();
        RolesRights RolesRightModel = new RolesRights();
        Rights right = new Rights();
        public static bool flag = false;
        public static int result = 0;
        public static string strResult = string.Empty;
        public string Mode = string.Empty;
        private SqlConnection con;
        //saurabh
        private void connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
        }
        //
        // GET: /RoleRights/
        public ActionResult RoleRights()
        {
            BL_RolesRights BLobj = new BL_RolesRights();
            RolesRights obj = new RolesRights();
            ModelState.Clear();
            obj.GetRoleAndRights = BLobj.GetRoleAndRights();
            obj.GetRoles = RolesRights.GetAllRols();

            return View(obj);
        }
        public ActionResult RoleRightstwo(int RoleID)
        {
            BL_RolesRights BLobj = new BL_RolesRights();
            RolesRights obj = new RolesRights();
            ModelState.Clear();
            obj.GetRoleAndRights = BLobj.GetRoleAndRights();
            obj.GetRoles = RolesRights.GetAllRols();

            return View(obj);
        }
        [HttpPost]
        public ActionResult RoleRightstwo(FormCollection formCollection)
        {
            RolesRightModel.RoleName = formCollection["RoleName"];
            RolesRightModel.RoleID = formCollection["RoleID"];
            String parentLevel = formCollection["parentLevel"];
            String parentLevel1 = formCollection["parentLevel1"];
            String parentLevel2 = formCollection["parentLevel2"];
            String parentLevel3 = formCollection["parentLevel3"];
            String parentLevel4 = formCollection["parentLevel4"];

            if (formCollection["parentLevel"] != null)
            {
                String[] ParentModuleID = parentLevel.Split(',');
                right.ParentLevel0ID = ParentModuleID;
            }


            if (formCollection["parentLevel1"] != null)
            {
                String[] ModuleID = parentLevel1.Split(',');
                right.ParentLevel1ID = ModuleID;
            }

            if (formCollection["parentLevel2"] != null)
            {
                String[] ChildModuleID = parentLevel2.Split(',');
                right.ParentLevel2ID = ChildModuleID;
            }
            if (formCollection["parentLevel3"] != null)
            {
                String[] SubChildModuleID = parentLevel3.Split(',');
                right.ParentLevel3ID = SubChildModuleID;
            }
            if (formCollection["parentLevel4"] != null)
            {
                String[] RightID = parentLevel4.Split(',');
                right.ParentLevel4ID = RightID;
            }

            right.OLDRightID = null;
            if (RolesRights.addRolesRight(RolesRightModel, right) == true)
            {
                return RedirectToAction("AddRolesRight", "RolesRight");
            }
            else
            {
                RolesRightModel.GetRoleAndRights = RolesRights.GetRoleAndRights();
                return View(RolesRightModel);
            }

            return View();
        }
        public ActionResult RoleRightsLevel2(int Level1ModuleID)
        {
            List<RolesRights> objSearch = new List<Models.Keystone.RolesRights>();
            BL_RolesRights BLobj = new BL_RolesRights();
            RolesRights obj = new RolesRights();
            ModelState.Clear();
            obj.GetRoleAndRights = BLobj.GetRoleAndRights();
            DataView dataView3 = new DataView(obj.GetRoleAndRights.Tables[0], " ParentModuleID = " + Level1ModuleID + "", "", DataViewRowState.CurrentRows);
            foreach (DataRow dr in dataView3.ToTable().Rows)
            {
                objSearch.Add(new RolesRights {
                    ModuleID = Convert.ToInt32( dr["ModuleID"]),
                    ModuleName = dr["ModuleName"].ToString(),
                });
            }
            return new JsonResult { Data = objSearch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public ActionResult RoleRights(FormCollection formCollection)
        {
            RolesRightModel.RoleName = formCollection["RoleName"];
            RolesRightModel.RoleID = formCollection["RoleID"];
            String parentLevel = formCollection["parentLevel"];
            String parentLevel1 = formCollection["parentLevel1"];
            String parentLevel2 = formCollection["parentLevel2"];
            String parentLevel3 = formCollection["parentLevel3"];
            String parentLevel4 = formCollection["parentLevel4"];

            if (formCollection["parentLevel"] != null)
            {
                String[] ParentModuleID = parentLevel.Split(',');
                right.ParentLevel0ID = ParentModuleID;
            }


            if (formCollection["parentLevel1"] != null)
            {
                String[] ModuleID = parentLevel1.Split(',');
                right.ParentLevel1ID = ModuleID;
            }

            if (formCollection["parentLevel2"] != null)
            {
                String[] ChildModuleID = parentLevel2.Split(',');
                right.ParentLevel2ID = ChildModuleID;
            }
            if (formCollection["parentLevel3"] != null)
            {
                String[] SubChildModuleID = parentLevel3.Split(',');
                right.ParentLevel3ID = SubChildModuleID;
            }
            if (formCollection["parentLevel4"] != null)
            {
                String[] RightID = parentLevel4.Split(',');
                right.ParentLevel4ID = RightID;
            }

            right.OLDRightID = null;
            if (RolesRights.addRolesRight(RolesRightModel, right) == true)
            {
                return RedirectToAction("AddRolesRight", "RolesRight");
            }
            else
            {
                RolesRightModel.GetRoleAndRights = RolesRights.GetRoleAndRights();
                return View(RolesRightModel);
            }
        }
        [HttpGet]
        public ActionResult EditRolesRight(int RoleID)
        {
            ModelState.Clear();
            RolesRightModel.GetRoleAndRights = RolesRights.GetRoleAndRights();
            RolesRightModel.GetRights = RolesRights.GetRoleAndRightsByRoleID(RoleID);
            // RolesRightModel.GetRoleAndRights = RolesRights.GetRoleAndRights();
            RolesRightModel.GetRoles = RolesRights.GetAllRols();
            return View(RolesRightModel);
        }

        [HttpPost]
        public ActionResult EditRolesRight(FormCollection formCollection)
        {
            RolesRightModel.RoleName = formCollection["RoleName"];
            RolesRightModel.RoleID = formCollection["RoleID"];
            String parentLevel = formCollection["parentLevel"];
            String parentLevel1 = formCollection["parentLevel1"];
            String parentLevel2 = formCollection["parentLevel2"];
            String parentLevel3 = formCollection["parentLevel3"];
            String parentLevel4 = formCollection["parentLevel4"];
            String OldValue = formCollection["OldValue"];
            String RoleRightID = formCollection["RoleRightID"];
            String[] ParentModuleID = parentLevel.Split(',');
            String[] ModuleID = parentLevel1.Split(',');
            String[] ChildModuleID = parentLevel2.Split(',');
            String[] SubChildModuleID = parentLevel3.Split(',');
            String[] RightID = parentLevel4.Split(',');
            //String[] OLDRightIDs = OldValue.Split(',');
            // String[] RoleRightIDs = RoleRightID.Split(',');
            right.ParentLevel0ID = ParentModuleID;
            right.ParentLevel1ID = ModuleID;
            right.ParentLevel2ID = ChildModuleID;
            right.ParentLevel3ID = SubChildModuleID;
            right.ParentLevel4ID = RightID;
            // right.OLDRightID = OLDRightIDs;
            //  right.RolesRightID = RoleRightIDs;
            if (RolesRights.addRolesRight(RolesRightModel, right) == true)
            {
                return RedirectToAction("AddRolesRight", "RolesRight");
            }
            else
            {
                int roleID = Convert.ToInt32(formCollection["RoleID"]);
                RolesRightModel.GetRoleAndRights = RolesRights.GetRoleAndRights();
                RolesRightModel.GetRights = RolesRights.GetRoleAndRightsByRoleID(roleID);
                return View(RolesRightModel);
            }
        }
    }
}