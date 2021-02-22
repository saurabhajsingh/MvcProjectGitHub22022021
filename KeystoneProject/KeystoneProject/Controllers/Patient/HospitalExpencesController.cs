using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Patient;
using KeystoneProject.Buisness_Logic.Hospital;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using KeystoneProject.Buisness_Logic.Patient;
using KeystoneProject.Models.Master;
namespace KeystoneProject.Controllers.Patient
{
    public class HospitalExpencesController : Controller
    {
        int HospitalID;
        int LocationID;
        int CreationID;

        //
        // GET: /PatientSearch/
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public void HospitlLocationID()
        {
            HospitalID = Convert.ToInt32(Session["HospitalID"]);
            LocationID = Convert.ToInt32(Session["LocationID"]);
            CreationID = Convert.ToInt32(Session["UserID"]);
        }
        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }
        [HttpGet]
        public ActionResult HospitalExpences()
        {
            return View();
        }
        //
        // GET: /HospitalExpences/
        public ActionResult HospitalExpences(HospitalExpences obj)
        {

            HospitlLocationID();
            connection();
            //string s = Request.Form["expenceNametbl"].ToString();
            for (int row = 0; row < obj.ServiceGroupIDtbl.Length; row++)
            {
                string ServiceGroupIDtbl = obj.expenceNametbl[row].ToString();
               // int ServiceGroupIDtbl = Convert.ToInt32(obj.ServiceGroupIDtbl[row]);
//int ServiceGroupIDtbl = Convert.ToInt32(obj.ServiceGroupIDtbl[row]);
                SqlCommand cmd = new SqlCommand("IUHospitalExpences", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@ServiceGroupID", obj.ServiceGroupIDtbl[row]);
              
                cmd.Parameters.AddWithValue("@HospitalExpencesName", obj.expenceNametbl[row].ToString());
                cmd.Parameters.AddWithValue("@ServiceGroupName", obj.ServiceGroupNametbl[row].ToString());
                cmd.Parameters.AddWithValue("@SequenceID", 0);
                cmd.Parameters.AddWithValue("@PerAmt", obj.Expencepertbl[row]);
                cmd.Parameters.AddWithValue("@CreationID", CreationID);
                if (obj.HospitalExpenceID[row] == 0)
                {
                    cmd.Parameters.AddWithValue("@HospitalExpencesID", obj.HospitalExpenceID[row]);
                
                    cmd.Parameters["@HospitalExpencesID"].Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Mode", "Add");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@HospitalExpencesID", obj.HospitalExpenceID[row]);
                
                    cmd.Parameters.AddWithValue("@Mode", "Edit");

                }
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();
                TempData["Msg"] = "Hospital Expences Update Successfully";
            }
            return View();
        }


        public JsonResult GetHospitaExpences(int ServiceGroupID)
        {

            KeystoneProject.Buisness_Logic.Master.Bl_Services Bl_obj = new Buisness_Logic.Master.Bl_Services();
            Services AddServiceMod = new Services();
            connection();
            HospitlLocationID();
            con.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter ad;
            List<HospitalExpences> SearchList = new List<Models.Master.HospitalExpences>();

            if (ServiceGroupID > 0)
            {
                ad = new SqlDataAdapter("select*from HospitalExpences where HospitalID=" + HospitalID + " and LocationID=" + LocationID + " and Rowstatus=0 and ServiceGroupID="+ServiceGroupID+"", con);
                ad.Fill(ds);
            }
          if(ServiceGroupID == 0)
            {
               ad = new SqlDataAdapter("select*from HospitalExpences where HospitalID=" + HospitalID + " and LocationID=" + LocationID + " and Rowstatus=0", con);
                ad.Fill(ds);
            }
            else
            {
                if (ds.Tables[0].Rows.Count == 0)
                {
                  //  ad = new SqlDataAdapter("select distinct HospitalExpencesName, '0' PerAmt,'0' HospitalExpencesID,'' ServiceGroupName,'0' ServiceGroupID from HospitalExpences where HospitalID=" + HospitalID + " and LocationID=" + LocationID + " and Rowstatus=0", con);
                  //  ds.Reset();
                  //  ad.Fill(ds);
                }
            }
            con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
            {
                SearchList.Add(new HospitalExpences
                {
                    Expenceper = dr["PerAmt"].ToString(),
                    expenceName = dr["HospitalExpencesName"].ToString(),
                  HospitalExpencesID = Convert.ToInt32(dr["HospitalExpencesID"]),
                    ServiceGroupName = dr["ServiceGroupName"].ToString(),
                    ServiceGroupID = dr["ServiceGroupID"].ToString()
                });
            }
            return new JsonResult { Data = SearchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult DeleteHospitaExpences(int HospitalExpencesID)
        {
            KeystoneProject.Buisness_Logic.Master.Bl_Services Bl_obj = new Buisness_Logic.Master.Bl_Services();
            Services AddServiceMod = new Services();
            connection();
            HospitlLocationID();
            con.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter ad;
            List<HospitalExpences> SearchList = new List<Models.Master.HospitalExpences>();
            int delete = 0;
            if (HospitalExpencesID > 0)
            {
                SqlCommand cmd=new SqlCommand("update HospitalExpences set  rowstatus=1 where HospitalExpencesID ="+HospitalExpencesID+" and Rowstatus=0 and HospitalID="+HospitalID+" and LocationID="+LocationID+"",con);


               delete= cmd.ExecuteNonQuery();
               con.Close();
            }
            return new JsonResult { Data = delete, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult BindHospitaExpences(string ExpenceName)
        {
            KeystoneProject.Buisness_Logic.Master.Bl_Services Bl_obj = new Buisness_Logic.Master.Bl_Services();
            Services AddServiceMod = new Services();
            connection();
            HospitlLocationID();
            con.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter ad;
            List<HospitalExpences> SearchList = new List<Models.Master.HospitalExpences>();

           
                ad = new SqlDataAdapter("select distinct  HospitalExpencesName from HospitalExpences where HospitalID=" + HospitalID + " and LocationID=" + LocationID + " and Rowstatus=0 and HospitalExpencesName like'"+ ExpenceName + "%' ", con);
                ad.Fill(ds);
            
          
              
            con.Close();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                SearchList.Add(new HospitalExpences
                {
                   
                    expenceName = dr["HospitalExpencesName"].ToString(),
                   
                });
            }
            return new JsonResult { Data = SearchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}