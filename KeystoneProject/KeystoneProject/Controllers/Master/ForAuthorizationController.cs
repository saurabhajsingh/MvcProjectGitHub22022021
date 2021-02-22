using KeystoneProject.Models.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.Master
{
    public class ForAuthorizationController : Controller
    {
        // GET: ForAuthorization
        public ActionResult ForAuthorization()
        {
            return View();
        }
       
        public ActionResult GetAllForAuthorization( DateTime DateFrom,DateTime DateTo)
         {
            KeystoneProject.Buisness_Logic.Master.BL_ForAuthorization obj = new Buisness_Logic.Master.BL_ForAuthorization();

            ForAuthorization location = new ForAuthorization();
            location.StoreAllForAuthorization = obj.StoreAllForAuthorization(DateFrom,DateTo);


            List<ForAuthorization> searchList = new List<ForAuthorization>();
            foreach (DataRow dr in location.StoreAllForAuthorization.Tables[0].Rows)
            {
                searchList.Add(new ForAuthorization
                {
                    AuthorizationID =  dr["AuthorizationID"].ToString(),
                    UserName = dr["UserName"].ToString(),
                    PatientRegNo = dr["PatientRegNo"].ToString(),
                    PatientName = dr["PatientName"].ToString(),
                    BillNo = dr["BillNo"].ToString(),
                    PatientAccountRowID = dr["PatientAccountRowID"].ToString(),
                    BillType = dr["BillType"].ToString(),
                    Date = dr["Date"].ToString(),
                    AuthorationReason = dr["AuthorationReason"].ToString(),
                    Authorise = dr["Authorise"].ToString(),

                });

            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult GetDetailsBeforeAfterAuthorization(int PatientRegNo, int BillNo, int AuthorizationID, string PatientAccountRowID)
        {
            KeystoneProject.Buisness_Logic.Master.BL_ForAuthorization obj = new Buisness_Logic.Master.BL_ForAuthorization();
            ForAuthorization location = new ForAuthorization();
            List<ForAuthorization> searchList = new List<ForAuthorization>();
            location.StoreAllForAuthorization = obj.GetDetailsBeforeAfterAuthorization(PatientRegNo, BillNo, AuthorizationID, PatientAccountRowID);
            if (BillNo == 0)
            {
                if (location.StoreAllForAuthorization.Tables[0].Rows[0]["BillType"].ToString() == "IPD PreBalanceBills")
                {


                    foreach (DataRow dr in location.StoreAllForAuthorization.Tables[0].Rows)
                    {
                        searchList.Add(new ForAuthorization
                        {
                            AfterData = "pre1",
                            BillType = dr["BillType"].ToString(),
                            PatientAccountRowID = dr["PatientAccountRowID"].ToString(),
                            CrAmount = dr["CrAmount"].ToString(),
                            DrAmount = dr["DrAmount"].ToString(),
                            Rowstatus = dr["RowStatus"].ToString(),
                        });

                    }
                    foreach (DataRow dr in location.StoreAllForAuthorization.Tables[1].Rows)
                    {
                        searchList.Add(new ForAuthorization
                        {
                            Rowstatus = dr["RowStatus"].ToString(),
                            AfterData = "pre2",
                            BillType = dr["BillType"].ToString(),
                            PatientAccountRowID = dr["PatientAccountRowID"].ToString(),
                            CrAmount = dr["CrAmount"].ToString(),
                            DrAmount = dr["DrAmount"].ToString()


                        });


                    }

                }
            }
            else
            {
                foreach (DataRow dr in location.StoreAllForAuthorization.Tables[0].Rows)
                {
                    searchList.Add(new ForAuthorization
                    {
                        BeforeData = "true",
                        BillNo = dr["BillNo"].ToString(),
                        ServiceName = dr["ServiceName"].ToString(),
                        Rate = dr["Rate"].ToString(),
                        Quantity = dr["Quantity"].ToString(),
                        TotalAmount = Convert.ToDecimal(dr["TotalAmount"]),
                        Rowstatus = dr["RowStatus"].ToString(),

                    });

                }
                foreach (DataRow dr in location.StoreAllForAuthorization.Tables[1].Rows)
                {
                    searchList.Add(new ForAuthorization
                    {
                        AfterData = "true",
                        BillNo = dr["BillNo"].ToString(),
                        ServiceName = dr["ServiceName"].ToString(),
                        Rate = dr["Rate"].ToString(),
                        Quantity = dr["Quantity"].ToString(),
                        TotalAmount = Convert.ToDecimal(dr["TotalAmount"]),
                        Rowstatus = dr["RowStatus"].ToString(),

                    });
                }
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public ActionResult IUAfterAuthorization(int PatientRegNo, int BillNo, int AuthorizationID, string PatientAccountRowID,string AuthorizedRemark)
        {
            KeystoneProject.Buisness_Logic.Master.BL_ForAuthorization obj = new Buisness_Logic.Master.BL_ForAuthorization();
            int count= obj.IUAfterAuthorization(PatientRegNo, BillNo, AuthorizationID, PatientAccountRowID, AuthorizedRemark);
            string mess = "";
            if(count>0)
            {
                mess = "Save";
            }
            else
            {
                mess = "Not";
            }
            return new JsonResult { Data = "Save", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}