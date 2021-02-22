using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Keystone;
using KeystoneProject.Buisness_Logic.Hospital;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
namespace KeystoneProject.Controllers.Hospital
{
    public class UserRightsController : Controller
    {
        private SqlConnection con;
        int HospitalID = 0;
            int LocationID=0;
                 int UserID=0;
                 BL_RolesRights RolesRights = new BL_RolesRights();
                 RolesRights rolesRightModel = new RolesRights();
                 BL_UsersRight BLUsersRight = new BL_UsersRight();
                 UserRights usersRight = new UserRights();
        private void connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
            HospitalID = Convert.ToInt32(Session["HospitalID"]);
            LocationID = Convert.ToInt32(Session["LocationID"]);
            UserID = Convert.ToInt32(Session["UserID"]);
        }
       
      
        RolesRights RolesRightModel = new RolesRights();
        //
        // GET: /UserRights/
        [HttpPost]
        public ActionResult UserRights(FormCollection formCollection)
        {


            //int columncount = Request.Form.Count;
            //int[] rowscount = new int[columncount];
            //for (int a = 0; a < columncount; a++)
            //{
            //     rowscount[a] = Request.Form[a].Split(',').Length;

            //}






            Rights right = new Rights();
            rolesRightModel.RoleName = formCollection["RoleName"];
            rolesRightModel.RoleID = formCollection["RoleID"];
            usersRight.UserID = Convert.ToInt32(formCollection["UserID"]);
            BLUsersRight.GetUserRightPassword(usersRight.UserID);
            EditUserRight(usersRight.UserID.ToString());
            usersRight.HospitalID = Convert.ToInt32(formCollection["HospitalName"]);
            usersRight.LocationID = Convert.ToInt32(formCollection["LocationName"]);

           

            //String parentLevel = formCollection["parentLevel"];
            String parentLevel = formCollection["parentLevel4Parent"];
            String parentLevel1 = formCollection["parentLevel1"];
            String parentLevel2 = formCollection["parentLevel2"];
            String parentLevel3 = formCollection["parentLevel3"];
            String parentLevel4 = formCollection["parentLevel4"];
            int length = 0;
            int Number = 0;
            if (right.OLDRightID == null)
            {
                length = 0;
            }
            else
            {
                length = right.OLDRightID.Length;
            }

            if (formCollection["parentLevel4Parent"] != null)
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
            //   right.ParentLevel0ID = ParentModuleID;
            //right.ParentLevel1ID = ModuleID;
            //right.ParentLevel2ID = ChildModuleID;
            // right.ParentLevel3ID = SubChildModuleID;
            // right.ParentLevel4ID = RightID;

          
            for (int m = 0; m < right.ParentLevel4ID.Length; m++)
                {
                   
                           // right.ParentLevel1ID = right.ParentLevel4ID.Except(right.OLDRightID).ToArray();

                         string[] parentwithIDs = right.ParentLevel4ID[m].Split('+');
                                usersRight.Mode = "Add";
                                BLUsersRight.add(usersRight, parentwithIDs, Number);
                            
                        
                       
                       
                   
                }
            
            //if (RolesRights.addRolesRight(rolesRightModel, right) == true)
            //{
            //    return RedirectToAction("AddRolesRight", "RolesRight");
            //}

            //if (BLUsersRight.adduserRight(usersRight) == true)
            //{
           
            //else
            //{
               rolesRightModel.GetRoleAndRights = RolesRights.GetRoleAndRights();
                return View(rolesRightModel);
            //}
        
           
        }

        [HttpGet]
        public ActionResult UserRights()
        {
            //HospitalLocation Location = new HospitalLocation();
            //collection
            //return View();
            UserRights location = new UserRights();


            BL_RolesRights BLobj = new BL_RolesRights();
            RolesRights obj = new RolesRights();
            ModelState.Clear();
            obj.GetRoleAndRights = BLobj.GetRoleAndRights();
            obj.GetRoles = BLobj.GetAllRols();

            return View(obj);
        }
        public JsonResult GetUsers(string prefix)
        {
            BL_RolesRights co = new BL_RolesRights();
            connect();
            List<UserRights> usersRight = new List<UserRights>();
            try
            {
                DataSet ds=new DataSet();
                SqlCommand cmd = new SqlCommand("select upper(LoginName) as LoginName,upper(FullName) as FullName,UserID from Users where LoginName like '" + prefix + "%' and RowStatus = 0", con);
               
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter();
                ad.SelectCommand = cmd;
                ad.Fill(ds);
                foreach (DataRow rdr in ds.Tables[0].Rows)
                {
                    UserRights user = new UserRights();
                    user.UserID = Convert.ToInt32(rdr["UserID"]);
                    user.LoginName = rdr["LoginName"].ToString();
                    user.FullName = rdr["FullName"].ToString();
                    usersRight.Add(user);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                ex.ToString();
            }
            return new JsonResult { Data = usersRight, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public void EditUserRight(string UserID)
        {
            connect();
           
            SqlCommand cmd = new SqlCommand("update UserRights  set RowStatus=1 where UserID='" + UserID + "' and HospitalID='" + HospitalID + "' and LocationID='" + LocationID + "'", con);
            con.Open();
            int a = cmd.ExecuteNonQuery();
            con.Close();
        }
        public JsonResult GetUserData(int UserID)
        {
            connect();
            DataSet ds = new DataSet();
            List<UserRights> usersRight = new List<UserRights>();
            try
            {
                SqlCommand cmd = new SqlCommand("GetUsersForUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", UserID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    UserRights user = new UserRights();
                    user.HospitalID = Convert.ToInt32(row["HospitalID"]);
                    user.LocationID = Convert.ToInt32(row["LocationID"]);
                    user.RoleID = Convert.ToInt32(row["RoleID"]);
                    user.RoleName = row["RoleName"].ToString();
                    user.HospitalName = row["HospitalName"].ToString();
                    user.LocationName = row["LocationName"].ToString();
                    usersRight.Add(user);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                ex.ToString();
            }
            return new JsonResult { Data = usersRight, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetRoleDataForUserRights(int UserID)
        {
            connect();
            DataSet ds = new DataSet();
            List<RolesRights> usersRight = new List<RolesRights>();
            try
            {
                SqlCommand cmd = new SqlCommand("GetRoleDataForUserRights", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    RolesRights role = new RolesRights();
                    role.ModuleID = Convert.ToInt32(dr["ModuleID"]);
                    role.RightCode = Convert.ToInt32(dr["Rights"]);
                    //  role.RightCode = Convert.ToInt32(dr["RightCode"]);
                    usersRight.Add(role);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                ex.ToString();
            }
            // return Json(usersRight, JsonRequestBehavior.AllowGet); //new JsonResult { Data = GetRoleRights, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            var jsonResult = Json(usersRight, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

            //return new JsonResult { Data = usersRight, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult HideModuls()
        {
            //  TempData["Msg"]="yed";



            RolesRights obj = new RolesRights();
            obj.GetRoleAndRights = new DataSet();
            connect();
            string n = Session["Password"].ToString();
            string m = Session["Password2"].ToString();

            List<RolesRights> serch = new List<RolesRights>();
            DataSet _dsReturnDataSet = new DataSet();
            string CompanyID = "0";
            string UserID = Session["UserID"].ToString();
            if (UserID == "1")
            {
                CompanyID = "1";
            }
            if (UserID != "1")
            {
                SqlCommand cmd = new SqlCommand("GetModulesByParentModuleIDNew1", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@CompanyID", HospitalID);
                cmd.Parameters.AddWithValue("@CompanyLocationID", LocationID);
                cmd.Parameters.AddWithValue("@Password", Session["Password"].ToString());
                cmd.Parameters.AddWithValue("@Password2", Session["Password2"].ToString());
                cmd.Parameters.AddWithValue("@FYStartDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@FYEndDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@ParentModuleID", 0);
                cmd.Parameters.AddWithValue("@iCustomerID", 1);
                SqlDataAdapter ad = new SqlDataAdapter();
                ad.SelectCommand = cmd;
                ad.Fill(obj.GetRoleAndRights);
          foreach (DataRow dr in obj.GetRoleAndRights.Tables[0].Rows)
                {

                    RolesRights obj1 = new RolesRights();
                    obj1.LeafModuleName = dr["LeafModuleName"].ToString();
                    obj1.ModuleName = dr["ModuleName"].ToString();
                    obj1.SubModuleName = dr["SubModuleName"].ToString();
                    serch.Add(obj1);
                }
            }
            else
            {
                return Json("All", JsonRequestBehavior.AllowGet);
            }
            return Json(serch, JsonRequestBehavior.AllowGet);
        }
	}
}