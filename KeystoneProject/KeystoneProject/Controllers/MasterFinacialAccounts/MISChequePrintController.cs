using KeystoneProject.Buisness_Logic.MasterFinacialAccounts;
using KeystoneProject.Models.MasterFinacialAccounts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.MasterFinacialAccounts
{
    public class MISChequePrintController : Controller
    {
        int HospitalID;
        int LocationID;
        int UserID;

        public void HospitlLocationID()
        {
            HospitalID = Convert.ToInt32(HttpContext.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Session["UserID"]);
        }

        MISChequePrint misMISChequePrint = new MISChequePrint();

        private SqlConnection con;
        string Constring = "";

        public static Nullable<DateTime> DateFrom { get; set; }
        public static Nullable<DateTime> DateTo { get; set; }

        private void Connect()
        {
            Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
        }
        [HttpGet]
        public ActionResult MISChequePrint()
        {
            MISChequePrint obj = new MISChequePrint();
            return View();
        }


        BL_MISChequePrint BlReport = new  BL_MISChequePrint();

        [HttpPost]
        public ActionResult MISChequePrint(FormCollection form)
        {
            try
            {

                misMISChequePrint.DateFrom = Convert.ToDateTime(form["FromDate"]);
                misMISChequePrint.DateTo = Convert.ToDateTime(form["ToDate"]);
                misMISChequePrint.BankID = form["BankID"];
                misMISChequePrint.BankName = form["BankName"];
                misMISChequePrint.BookName = form["BookName"];
                misMISChequePrint.PayeeName = form["PayeeName"];
                misMISChequePrint.AccountName = form["AccountName"];
                misMISChequePrint.ChequeStatus = form["ChequeStatus"];
                misMISChequePrint.ChequeNo = form["ChequeNo"];

                if (misMISChequePrint.BankID == "" || misMISChequePrint.BankID == null || misMISChequePrint.BankID == "All")
                {
                    misMISChequePrint.BankID = "%";

                }
                if (misMISChequePrint.BankName == "" || misMISChequePrint.BankName == null || misMISChequePrint.BankName == "All")
                {
                    misMISChequePrint.BankName = "%";

                }
                if (misMISChequePrint.BookName == "" || misMISChequePrint.BookName == null || misMISChequePrint.BookName == "All")
                {
                    misMISChequePrint.BookName = "%";

                }
                if (misMISChequePrint.PayeeName == "" || misMISChequePrint.PayeeName == null || misMISChequePrint.PayeeName == "All")
                {
                    misMISChequePrint.PayeeName = "%";

                }
                if (misMISChequePrint.AccountName == "" || misMISChequePrint.AccountName == null || misMISChequePrint.AccountName == "All")
                {
                    misMISChequePrint.AccountName = "%";

                }
                if (misMISChequePrint.ChequeStatus == "" || misMISChequePrint.ChequeStatus == null || misMISChequePrint.ChequeStatus == "All")
                {
                    misMISChequePrint.ChequeStatus = "%";

                }

               

               
                DateFrom = misMISChequePrint.DateFrom;
                DateTo = misMISChequePrint.DateTo;

                misMISChequePrint.dsPatientReport = MisChequePrint(misMISChequePrint);

                return View(misMISChequePrint);
            }
            catch (Exception ex)
            {

                return View(misMISChequePrint);
            }
          
           
        }

        public DataSet MisChequePrint(MISChequePrint mischequePrint)
        {
            DataSet ds = new DataSet();

            Connect();
            try
            {
                HospitlLocationID();
                SqlCommand cmd = new SqlCommand("ReportMisChequePrint", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@FromDate", mischequePrint.DateFrom);
                cmd.Parameters.AddWithValue("@ToDate", mischequePrint.DateTo);
                cmd.Parameters.AddWithValue("@BankName", mischequePrint.BankName);
                cmd.Parameters.AddWithValue("@BookName", mischequePrint.BookName);
                cmd.Parameters.AddWithValue("@PayeeName", mischequePrint.PayeeName);
                cmd.Parameters.AddWithValue("@AccountName", mischequePrint.AccountName);
                cmd.Parameters.AddWithValue("@ChequeStatus", mischequePrint.ChequeStatus);

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception)
            {

                return ds;
            }
        }
        public JsonResult GetAllBank()
        {
            DataSet ds = BlReport.GetAllBank();
            List<MISChequePrint> searchlist = new List<MISChequePrint>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new MISChequePrint
                {
                    BankID = dr["BankID"].ToString(),
                    BankName = dr["BankName"].ToString()
                });
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetAllBookName()
        {
            DataSet ds = BlReport.GetAllChequeBook();
            List<MISChequePrint> searchlist = new List<MISChequePrint>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new MISChequePrint
                {
                    ChequeBookID = dr["ChequeBookID"].ToString(),
                    BookName = dr["BookName"].ToString()
                });
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        

        //public JsonResult ShowAllChequePrint()
        //{
        //    BL_MISChequePrint db = new BL_MISChequePrint();

        //    return new JsonResult { Data = db.SelectAllChequePrint(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}

        public JsonResult ShowAllCheque()
        {
            DataSet ds = BlReport.GetAllChequeNumber();
            List<MISChequePrint> searchlist = new List<MISChequePrint>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new MISChequePrint
                {
                    ChequeNo = dr["ChequeNo"].ToString(),
                    PayeeName = dr["PayeeName"].ToString(),
                  
                });
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetAllAccount()
        {
            DataSet ds = BlReport.GetAllAccountName();
            List<MISChequePrint> searchlist = new List<MISChequePrint>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new MISChequePrint
                {
                    AccountName = dr["AccountName"].ToString()
                });
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

	}

}