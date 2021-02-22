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
    public class RoleRightstwoController : Controller
    {
        BL_RolesRights RolesRights = new BL_RolesRights();
        RolesRights RolesRightModel = new RolesRights();
        Rights right = new Rights();
        public static bool flag = false;
        public static int result = 0;
        public static string strResult = string.Empty;
        public string Mode = string.Empty;
        private SqlConnection con;

        private void connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
        }
        //
        // GET: /RoleRightstwo/
        public ActionResult RoleRightstwo(int RoleID)
        {
            BL_RolesRights BLobj = new BL_RolesRights();
            RolesRights obj = new RolesRights();
           
            ModelState.Clear();
            obj.GetRoleAndRights = BLobj.GetRoleAndRights();
            obj.GetRoles = RolesRights.GetAllRols();
            if (RoleID > 0)
            {
                obj.GetRights = RolesRights.GetRoleAndRightsByRoleID(RoleID);
                obj.RoleID = obj.GetRights.Tables[0].Rows[0]["RoleID"].ToString();
                obj.RoleName = obj.GetRights.Tables[0].Rows[0]["RoleName"].ToString();
            
            }
            else
            {
              //  obj.RoleName = "";
              //  obj.RoleID = "0";
            }
            return View(obj);
        }

        public ActionResult RoleRightsLevelChk(int RoleID)
        {
            List<RolesRights> SerchAdd = new List<Models.Keystone.RolesRights>();
            List<RolesRights> objSearch = new List<Models.Keystone.RolesRights>();
            BL_RolesRights BLobj = new BL_RolesRights();
            RolesRights obj = new RolesRights();
            if (RoleID > 0)
            {

                obj.GetRights = RolesRights.GetRoleAndRightsByRoleID(RoleID);
                if (obj.GetRights.Tables[0].Rows.Count > 0)
                {
                    obj.RoleID = obj.GetRights.Tables[0].Rows[0]["RoleID"].ToString();
                    obj.RoleName = obj.GetRights.Tables[0].Rows[0]["RoleName"].ToString();
                    foreach (DataRow dr in obj.GetRights.Tables[0].Rows)
                    {
                        SerchAdd.Add(new RolesRights
                        {
                            ModuleIDchk = Convert.ToInt32(dr["ModuleIDchk"]),
                            ModuleID = Convert.ToInt32(dr["ParentModuleID"]),
                            // ModuleName = dr["ModuleName"].ToString(),
                            RightCode = Convert.ToInt32(dr["RightCode"])
                            //=obj.GetRights.Tables[0].Rows[0]["ModuleID"].ToString();
                        });
                    }
                }
            }
            return new JsonResult { Data = SerchAdd, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult EditRoleRightsLevel2( int RoleID)
        {
            List<RolesRights> SerchAdd = new List<Models.Keystone.RolesRights>();
            List<RolesRights> objSearch = new List<Models.Keystone.RolesRights>();
            BL_RolesRights BLobj = new BL_RolesRights();
            RolesRights obj = new RolesRights();
            if (RoleID > 0)
            {

                obj.GetRights = RolesRights.GetRoleAndRightsByRoleID(RoleID);
                obj.RoleID = obj.GetRights.Tables[0].Rows[0]["RoleID"].ToString();
                obj.RoleName = obj.GetRights.Tables[0].Rows[0]["RoleName"].ToString();
                foreach (DataRow dr in obj.GetRights.Tables[0].Rows)
                {
                    SerchAdd.Add(new RolesRights
                    {
                        ModuleIDchk = Convert.ToInt32(dr["ModuleIDchk"]),
                        ModuleID = Convert.ToInt32(dr["ModuleID"]),
                        // ModuleName = dr["ModuleName"].ToString(),
                        RightCode = Convert.ToInt32(dr["RightCode"])
                        //=obj.GetRights.Tables[0].Rows[0]["ModuleID"].ToString();
                    });
                }
               
            }
            return new JsonResult { Data = SerchAdd, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult RoleRightsLevel2(int Level1ModuleID)
        {
            List<RolesRights> SerchAdd = new List<Models.Keystone.RolesRights>();
            List<RolesRights> objSearch = new List<Models.Keystone.RolesRights>();
            BL_RolesRights BLobj = new BL_RolesRights();
            RolesRights obj = new RolesRights();
            ModelState.Clear();
            obj.GetRoleAndRights = BLobj.GetRoleAndRights();
            DataView dataView3 = new DataView(obj.GetRoleAndRights.Tables[0], " ParentModuleID = " + Level1ModuleID + "", "", DataViewRowState.CurrentRows);
            foreach (DataRow dr in dataView3.ToTable().Rows)
            {
                objSearch.Add(new RolesRights
                {
                    ModuleID = Convert.ToInt32(dr["ModuleID"]),
                    ModuleName = dr["ModuleName"].ToString(),
                });
            }
           
            return new JsonResult { Data = objSearch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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
            if (formCollection["beforAfterchk"] != null)
            {
                String[] RightID = formCollection["beforAfterchk"].Split(',');
                right.beforAfterchk = RightID;
            }
            right.OLDRightID = null;
            if (RolesRights.addRolesRight(RolesRightModel, right) == true)
            {
                RolesRightModel.GetRoleAndRights = RolesRights.GetRoleAndRights();
                RolesRightModel.GetRoles = RolesRights.GetAllRols();
                return RoleRightstwo( Convert.ToInt32( RolesRightModel.RoleID));
            }
            else
            {
                RolesRightModel.GetRoleAndRights = RolesRights.GetRoleAndRights();
                RolesRightModel.GetRoles = RolesRights.GetAllRols();
                return RoleRightstwo(Convert.ToInt32(RolesRightModel.RoleID));
            }

         
        }
	}
}