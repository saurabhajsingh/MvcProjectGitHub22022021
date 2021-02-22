using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Patient;
using KeystoneProject.Buisness_Logic.Patient;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;


namespace KeystoneProject.Controllers.Patient
{
    public class PatientOPDController : Controller
    {
        private SqlConnection con;
        int HospitalID = Convert.ToInt32(System.Web.HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(System.Web.HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]);
        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        BL_PatientOPD _OPDBill = new BL_PatientOPD();
        PatientOPD obj = new PatientOPD();
        //
        // GET: /PatientOPD/

        public JsonResult GetAllFinancialYear()
        {
            DataSet ds = _OPDBill.GetAllFinancialYear();
            List<PatientOPD> searchList = new List<PatientOPD>();
            if(ds.Tables[0].Rows.Count>0)
            { 
            DataView dvTest = new DataView(ds.Tables[0], "FinancialYearID = " + ds.Tables[1].Rows[0]["FinancialYearID"].ToString() + " ", "", DataViewRowState.CurrentRows);
            dvTest.ToTable().Rows[0]["FinancialYear"].ToString();
                foreach (DataRow dr in dvTest.ToTable().Rows)
                {
                    searchList.Add(new PatientOPD
                    {
                        FinancialYear = dr["FinancialYear"].ToString(),
                        FinancialYearID = Convert.ToInt32(dr["FinancialYearID"]).ToString()
                    });
                }
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult PatientCategory(string prefix)
        {
            return new JsonResult { Data = _OPDBill.patientCategory(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetServiceDetailsPackage(string prefix)
        {
            return new JsonResult { Data = _OPDBill.GetServiceDetailsPackage(prefix), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult WardName(string prefix)
        {
            return new JsonResult { Data = _OPDBill.WardName(HospitalID, LocationID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult RoomNo(string Roomname, string WardID)
        {
            return new JsonResult { Data = _OPDBill.RoomNo(Roomname, WardID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult BindBedNo(string BedNo, string RoomID)
        {
            return new JsonResult { Data = _OPDBill.BedNo(BedNo, RoomID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult BindOrganization(string prefix, string OPDIPDTYPE)
        {
            //if(OPDIPDTYPE=="IPD")
            {
                
            return new JsonResult { Data = _OPDBill.OrganizationName(prefix), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            //else
            //{
            //    return new JsonResult { Data = _OPDBill.Bind_TPA_Name(prefix), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //}
        }
        public JsonResult BindTPA(string prefix)
        {
            return new JsonResult { Data = _OPDBill.Bind_TPA_Name(prefix), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }    
        public JsonResult BindInsurance(string prefix)
        {
            return new JsonResult { Data = _OPDBill.Bind_InsuranceCompny(prefix), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetIPDPatient(string prefix, string outside)
        {
          //  BL_Patient_IPDBill _IPDBill = new BL_Patient_IPDBill();
            DataSet ds = _OPDBill.GetIPDPatient(prefix, outside);
            List<PatientOPDBill> searchList = new List<PatientOPDBill>();
            // if (outside == false)
            //  {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new PatientOPDBill
                {
                    patientregNo = (dr["PatientRegNO"].ToString()),
                    patientname = dr["PatientName"].ToString(),
                    address = (dr["Address"].ToString()),
                    contactno = dr["MobileNo"].ToString(),
                    PrintRegNO = dr["PrintRegNO"].ToString()
                });
            }
            //}
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult BindPatientPrefix(string prefix)
        {
            return new JsonResult { Data = _OPDBill.BindPrefixPatient(prefix), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }        
        public JsonResult Department(string prefix)
        {
            return new JsonResult { Data = _OPDBill.BindDepartment(prefix, "%"), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ConsultantDoctor(string prefix)
        {
            return new JsonResult { Data = _OPDBill.BindConsultant(prefix), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult ConsultantJoinDoctor(string prefix)
        {
            return new JsonResult { Data = _OPDBill.BindConsultant(prefix), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult RefferedDoctor(string prefix)
        {
            return new JsonResult { Data = _OPDBill.BindReferredByDoctorID(prefix), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        //public JsonResult RegisteredPatient( string FinancialYearID)
        //{

        //    return new JsonResult { Data = _OPDBill.BindPatient(FinancialYearID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}

        public JsonResult BindPatientName(string prefix)
        {

                return new JsonResult { Data = _OPDBill.GetPatientName(prefix), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult City(string prefix)
        {
           return new JsonResult { Data = _OPDBill.BindCity(prefix, "%"), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        public JsonResult GetCountryStateID(int CityID)
        {
            return new JsonResult { Data = _OPDBill.GetCountryStateID(CityID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        public ActionResult GetRegistrationDate(string id,string Type)
        {
            return new JsonResult { Data = _OPDBill.GetRegistrationDate(id,Type), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
         //   return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        public JsonResult FillRevisit(int PatientRegNO)
        {

            return new JsonResult { Data = _OPDBill.EditPatient_OPDDetail(PatientRegNO, "Revisit"), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }    
        public JsonResult GetAllPatient()
        {
            return new JsonResult { Data = _OPDBill.GetAllPatient(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetOPDPatientAppointment()
        {
            return new JsonResult { Data = _OPDBill.GetOPDPatientAppointment(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetOPDPatientAppointmentDetails(string PatientAppointID)
        {
            return new JsonResult { Data = _OPDBill.GetOPDPatientAppointmentDetails(PatientAppointID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetCategory()
        {
            return new JsonResult { Data = _OPDBill.GetCategory(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult EditTPAWiseBills(string PatientRegNo, string PatientIPDNo, string TPAID, string WardID,string type,PatientOPD obj)
        {
            DataSet dsTotalBills = new DataSet();
            DataSet dsPatientBills = new DataSet();
            DataSet dsPatientBillsDetail = new DataSet();
            DataSet dsAllTpa = new DataSet();
            DataSet dsAllLabTPA = new DataSet();
            DataSet dsservicerate = new DataSet();
            string message ="";
           

                dsTotalBills = _OPDBill.GetPatientTotalBillsNO(PatientRegNo, PatientIPDNo,type);


            if (TPAID != null)
            {
                if (type == "IPD")
                {

                if (WardID != "")
                {

                    dsAllTpa = _OPDBill.GetAllServicesTPAWiseForIPDBills(Convert.ToInt32(WardID), Convert.ToInt32(TPAID));

                    if (dsAllTpa.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow drBillno in dsTotalBills.Tables[0].Rows)
                        {

                            dsPatientBillsDetail = _OPDBill.GetPatientBillsDetails(Convert.ToInt32(drBillno["BillNo"].ToString()));
                            //  DataSet dt1 = (DataSet)Session["GetPatientBillsDetails"];
                            double GTotal = 0;
                            string TPARate1;
                            foreach (DataRow drPatientBillsDetail in dsPatientBillsDetail.Tables[0].Rows)
                            {
                                if ((drPatientBillsDetail["ServiceType"].ToString() == "IPD") || (drPatientBillsDetail["ServiceType"].ToString() == "IPDFinalBill") || (drPatientBillsDetail["ServiceType"].ToString() == "OPD") || (drPatientBillsDetail["ServiceType"].ToString() == "IPDPackageBills"))
                                {

                                    DataView dvServices = new DataView(dsAllTpa.Tables[0], "ServiceID = " + drPatientBillsDetail["ServiceID"].ToString() + " ", "", DataViewRowState.CurrentRows);
                                    if (dvServices.Count != 0)
                                    {
                                        //if (dvServices[0]["RecommendedByDoctor"].ToString() == "No")
                                        //{
                                        drPatientBillsDetail["Rate"] = dvServices[0]["GeneralCharges"].ToString();
                                        drPatientBillsDetail["TotalAmount"] = (Convert.ToDecimal(dvServices[0]["GeneralCharges"]) * Convert.ToDecimal(drPatientBillsDetail["Quantity"]));
                                        //   obj.TPARate = dvServices[0]["GeneralCharges"].ToString();
                                        // obj.TPARate = TPARate1;
                                        //  obj.TotalAmount = (Convert.ToDecimal(dvServices[0]["GeneralCharges"]) * Convert.ToDecimal(drPatientBillsDetail["Quantity"])).ToString();
                                        // drPatientBillsDetail[""]= (Convert.ToDecimal(dvServices[0]["GeneralCharges"]) * Convert.ToDecimal(drPatientBillsDetail["Quantity"])).ToString();

                                        GTotal = GTotal + (Convert.ToDouble(dvServices[0]["GeneralCharges"]) * Convert.ToDouble(drPatientBillsDetail["Quantity"]));
                                        obj.GTotal = GTotal.ToString();
                                        //}
                                        //else
                                        //{
                                        //    // obj.TPARate = (Convert.ToDouble(drPatientBillsDetail["Rate"]) * Convert.ToDouble(drPatientBillsDetail["Quantity"])).ToString();
                                        //    GTotal = GTotal + (Convert.ToDouble(drPatientBillsDetail["Rate"]) * Convert.ToDouble(drPatientBillsDetail["Quantity"]));
                                        //    obj.GTotal = GTotal.ToString();
                                        //}

                                    }
                                }
                                else if ((drPatientBillsDetail["ServiceType"].ToString() == "IPDLabBills") || (drPatientBillsDetail["ServiceType"].ToString() == "IPDLabFinalBill") || (drPatientBillsDetail["ServiceType"].ToString() == "OPDLabBills"))
                                {
                                    DataView dvTest = new DataView(dsAllTpa.Tables[1], "TestID = " + drPatientBillsDetail["ServiceID"].ToString() + " ", "", DataViewRowState.CurrentRows);
                                    if (dvTest.Count != 0)
                                    {
                                        //if (dvTest[0]["RecommendedByDoctor"].ToString() == "No")
                                        //{
                                        drPatientBillsDetail["Rate"] = dvTest[0]["GeneralCharges"].ToString();
                                        drPatientBillsDetail["TotalAmount"] = (Convert.ToDecimal(dvTest[0]["GeneralCharges"]) * Convert.ToDecimal(drPatientBillsDetail["Quantity"]));
                                        //  obj.TPARate = dvTest[0]["GeneralCharges"].ToString();
                                        // obj.TPARate=
                                        // obj.TotalAmount = (Convert.ToDecimal(dvTest[0]["GeneralCharges"]) * Convert.ToDecimal(drPatientBillsDetail["Quantity"])).ToString();
                                        GTotal = GTotal + (Convert.ToDouble(dvTest[0]["GeneralCharges"]) * Convert.ToDouble(drPatientBillsDetail["Quantity"]));
                                        obj.GTotal = GTotal.ToString();
                                        //}
                                        //else
                                        //{
                                        //    //  obj.TPARate= (Convert.ToDouble(drPatientBillsDetail["Rate"]) * Convert.ToDouble(drPatientBillsDetail["Quantity"])).ToString();
                                        //    GTotal = GTotal + Convert.ToDouble(drPatientBillsDetail["Rate"]) * Convert.ToDouble(drPatientBillsDetail["Quantity"]);
                                        //    obj.GTotal = GTotal.ToString();
                                        //}
                                    }

                                }


                            }
                            dsPatientBills = _OPDBill.GetPatientBills(Convert.ToInt32(drBillno["BillNo"].ToString()));
                            foreach (DataRow drPatientBills in dsPatientBills.Tables[0].Rows)
                            {
                                //obj.ServiceTax = Convert.ToDouble(drPatientBills["TaxPercent"].ToString());
                                //double TotalAmount = 0;

                                //if (obj.ServiceTax > 0)
                                //{
                                //    obj.TaxAmount = Convert.ToDouble(obj.GTotal) * (obj.ServiceTax / 100);
                                //}
                                //drPatientBills["TaxAmount"] = obj.TaxAmount.ToString();
                                //TotalAmount = GTotal + obj.TaxAmount;
                                //double TotalAmount1 = Convert.ToDouble(TotalAmount) + Convert.ToDouble(drPatientBills["PreBalanceAmount"]);
                                //obj.TotalAmount = TotalAmount1.ToString();
                                //obj.Discount = Convert.ToDouble(drPatientBills["DiscountAmount"]);
                                ////double Prebalance = Convert.ToDouble(drPatientBills["PaidAmount"]) - TotalAmount1  ;
                                ////obj.PreBalance = Prebalance;
                                //obj.NetPayableAmount = Convert.ToDouble(obj.TotalAmount) - Convert.ToDouble(drPatientBills["DiscountAmount"]) - Convert.ToDouble(drPatientBills["PaidAmount"]);
                                //obj.BalAmount = obj.NetPayableAmount - Convert.ToDouble(drPatientBills["Paidamount"]);


                                drPatientBills["GrossAmount"] = GTotal;
                                double ServiceTax = Convert.ToDouble(drPatientBills["TaxPercent"].ToString());
                                double TaxAmount = 0.00;
                                double TotalAmount = 0;
                                double Discount = 0;
                                Double BalAmount = 0;
                                if (ServiceTax > 0)
                                {
                                    TaxAmount = GTotal * (ServiceTax / 100);
                                }
                                drPatientBills["TaxAmount"] = TaxAmount.ToString();
                                TotalAmount = GTotal + TaxAmount;
                                TotalAmount = TotalAmount + Convert.ToDouble(drPatientBills["PreBalanceAmount"]);
                                drPatientBills["TotalAmount"] = TotalAmount.ToString();
                                Discount = Convert.ToDouble(drPatientBills["DiscountAmount"]);
                                drPatientBills["NetPayableAmount"] = Convert.ToDouble(drPatientBills["TotalAmount"]) - Convert.ToDouble(drPatientBills["DiscountAmount"]);
                                BalAmount = Convert.ToDouble(drPatientBills["NetPayableAmount"]) - Convert.ToDouble(drPatientBills["Paidamount"]);
                                drPatientBills["BalanceAmount"] = BalAmount.ToString();
                                drPatientBills["Mode"] = "Edit";

                            }
                            _OPDBill.EditTPAWiseBills(dsPatientBills, obj);
                            _OPDBill.DeletePatientBillDetails(Convert.ToInt32(dsPatientBills.Tables[0].Rows[0]["BillNo"].ToString()));
                            _OPDBill.EditTPAWiseBills1(dsPatientBillsDetail, obj);

                        }
                    }
                }

            }
            else
            {
                    dsAllTpa = _OPDBill.GetAllServicesTPAWiseForOPDBills(Convert.ToInt32(TPAID));

                    if (dsAllTpa.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow drBillno in dsTotalBills.Tables[0].Rows)
                        {

                            dsPatientBillsDetail = _OPDBill.GetPatientBillsDetails(Convert.ToInt32(drBillno["BillNo"].ToString()));
                            //  DataSet dt1 = (DataSet)Session["GetPatientBillsDetails"];
                            double GTotal = 0;
                            string TPARate1;
                            foreach (DataRow drPatientBillsDetail in dsPatientBillsDetail.Tables[0].Rows)
                            {
                                if ((drPatientBillsDetail["ServiceType"].ToString() == "OPD"))
                                {

                                    DataView dvServices = new DataView(dsAllTpa.Tables[0], "ServiceID = " + drPatientBillsDetail["ServiceID"].ToString() + " ", "", DataViewRowState.CurrentRows);
                                    if (dvServices.Count != 0)
                                    {
                                       
                                        drPatientBillsDetail["Rate"] = dvServices[0]["GeneralCharges"].ToString();
                                        drPatientBillsDetail["TotalAmount"] = (Convert.ToDecimal(dvServices[0]["GeneralCharges"]) * Convert.ToDecimal(drPatientBillsDetail["Quantity"]));
                                       
                                        GTotal = GTotal + (Convert.ToDouble(dvServices[0]["GeneralCharges"]) * Convert.ToDouble(drPatientBillsDetail["Quantity"]));
                                        obj.GTotal = GTotal.ToString();
                                        

                                    }
                                }
                                else if ((drPatientBillsDetail["ServiceType"].ToString() == "OPDLabBills"))
                                {
                                    DataView dvTest = new DataView(dsAllTpa.Tables[1], "TestID = " + drPatientBillsDetail["ServiceID"].ToString() + " ", "", DataViewRowState.CurrentRows);
                                    if (dvTest.Count != 0)
                                    {
                                        
                                        drPatientBillsDetail["Rate"] = dvTest[0]["GeneralCharges"].ToString();
                                        drPatientBillsDetail["TotalAmount"] = (Convert.ToDecimal(dvTest[0]["GeneralCharges"]) * Convert.ToDecimal(drPatientBillsDetail["Quantity"]));
                                   
                                        GTotal = GTotal + (Convert.ToDouble(dvTest[0]["GeneralCharges"]) * Convert.ToDouble(drPatientBillsDetail["Quantity"]));
                                        obj.GTotal = GTotal.ToString();
                                       
                                    }

                                }


                            }
                            dsPatientBills = _OPDBill.GetPatientBills(Convert.ToInt32(drBillno["BillNo"].ToString()));
                            foreach (DataRow drPatientBills in dsPatientBills.Tables[0].Rows)
                            {
                                //obj.ServiceTax = Convert.ToDouble(drPatientBills["TaxPercent"].ToString());
                                //double TotalAmount = 0;

                                //if (obj.ServiceTax > 0)
                                //{
                                //    obj.TaxAmount = Convert.ToDouble(obj.GTotal) * (obj.ServiceTax / 100);
                                //}
                                //drPatientBills["TaxAmount"] = obj.TaxAmount.ToString();
                                //TotalAmount = GTotal + obj.TaxAmount;
                                //double TotalAmount1 = Convert.ToDouble(TotalAmount) + Convert.ToDouble(drPatientBills["PreBalanceAmount"]);
                                //obj.TotalAmount = TotalAmount1.ToString();
                                //obj.Discount = Convert.ToDouble(drPatientBills["DiscountAmount"]);
                                ////double Prebalance = Convert.ToDouble(drPatientBills["PaidAmount"]) - TotalAmount1  ;
                                ////obj.PreBalance = Prebalance;
                                //obj.NetPayableAmount = Convert.ToDouble(obj.TotalAmount) - Convert.ToDouble(drPatientBills["DiscountAmount"]) - Convert.ToDouble(drPatientBills["PaidAmount"]);
                                //obj.BalAmount = obj.NetPayableAmount - Convert.ToDouble(drPatientBills["Paidamount"]);


                                drPatientBills["GrossAmount"] = GTotal;
                                double ServiceTax = Convert.ToDouble(drPatientBills["TaxPercent"].ToString());
                                double TaxAmount = 0.00;
                                double TotalAmount = 0;
                                double Discount = 0;
                                Double BalAmount = 0;
                                if (ServiceTax > 0)
                                {
                                    TaxAmount = GTotal * (ServiceTax / 100);
                                }
                                drPatientBills["TaxAmount"] = TaxAmount.ToString();
                                TotalAmount = GTotal + TaxAmount;
                                TotalAmount = TotalAmount + Convert.ToDouble(drPatientBills["PreBalanceAmount"]);
                                drPatientBills["TotalAmount"] = TotalAmount.ToString();
                                Discount = Convert.ToDouble(drPatientBills["DiscountAmount"]);
                                drPatientBills["NetPayableAmount"] = Convert.ToDouble(drPatientBills["TotalAmount"]) - Convert.ToDouble(drPatientBills["DiscountAmount"]);
                                BalAmount = Convert.ToDouble(drPatientBills["NetPayableAmount"]) - Convert.ToDouble(drPatientBills["Paidamount"]);
                                drPatientBills["BalanceAmount"] = BalAmount.ToString();
                                drPatientBills["Mode"] = "Edit";

                            }
                            _OPDBill.EditTPAWiseBills(dsPatientBills, obj);
                            _OPDBill.DeletePatientBillDetails(Convert.ToInt32(dsPatientBills.Tables[0].Rows[0]["BillNo"].ToString()));
                            _OPDBill.EditTPAWiseBills1(dsPatientBillsDetail, obj);

                        }
                    }

                }
            }
            if (TPAID == null)
            {
                if (type == "IPD")
                {
                    if (WardID != "")
                    {


                        dsservicerate = _OPDBill.GetAllServicesWiseForIPDBills(Convert.ToInt32(WardID));
                    }
                    foreach (DataRow dr in dsTotalBills.Tables[0].Rows)
                    {

                        dsPatientBillsDetail = _OPDBill.GetPatientBillsDetails(Convert.ToInt32(dr["BillNo"].ToString()));

                        double GTotal = 0;
                        string TPARate1;
                        foreach (DataRow drPatientBillsDetail in dsPatientBillsDetail.Tables[0].Rows)
                        {
                            if ((drPatientBillsDetail["ServiceType"].ToString() == "IPD") || (drPatientBillsDetail["ServiceType"].ToString() == "IPDFinalBill") || (drPatientBillsDetail["ServiceType"].ToString() == "OPD") || (drPatientBillsDetail["ServiceType"].ToString() == "IPDPackageBills"))
                            {

                                DataView dv = new DataView(dsservicerate.Tables[0], "ServiceID = " + drPatientBillsDetail["ServiceID"].ToString() + " ", "", DataViewRowState.CurrentRows);
                                if (dv.Count != 0)
                                {

                                    drPatientBillsDetail["Rate"] = dv[0]["GeneralCharges"].ToString();
                                    drPatientBillsDetail["TotalAmount"] = (Convert.ToDecimal(dv[0]["GeneralCharges"]) * Convert.ToDecimal(drPatientBillsDetail["Quantity"]));

                                    GTotal = GTotal + (Convert.ToDouble(dv[0]["GeneralCharges"]) * Convert.ToDouble(drPatientBillsDetail["Quantity"]));
                                    obj.GTotal = GTotal.ToString();


                                }
                            }
                            else if ((drPatientBillsDetail["ServiceType"].ToString() == "IPDLabBills") || (drPatientBillsDetail["ServiceType"].ToString() == "IPDLabFinalBill") || (drPatientBillsDetail["ServiceType"].ToString() == "OPDLabBills"))
                            {
                                DataView dvTest = new DataView(dsservicerate.Tables[1], "TestID = " + drPatientBillsDetail["ServiceID"].ToString() + " ", "", DataViewRowState.CurrentRows);
                                if (dvTest.Count != 0)
                                {
                                    if (dvTest[0]["RecommendedByDoctor"].ToString() == "No")
                                    {
                                        drPatientBillsDetail["Rate"] = dvTest[0]["GeneralCharges"].ToString();
                                        drPatientBillsDetail["TotalAmount"] = (Convert.ToDecimal(dvTest[0]["GeneralCharges"]) * Convert.ToDecimal(drPatientBillsDetail["Quantity"]));

                                        GTotal = GTotal + (Convert.ToDouble(dvTest[0]["GeneralCharges"]) * Convert.ToDouble(drPatientBillsDetail["Quantity"]));
                                        obj.GTotal = GTotal.ToString();
                                    }
                                    else
                                    {

                                        GTotal = GTotal + Convert.ToDouble(drPatientBillsDetail["Rate"]) * Convert.ToDouble(drPatientBillsDetail["Quantity"]);
                                        obj.GTotal = GTotal.ToString();
                                    }
                                }

                            }


                        }
                        dsPatientBills = _OPDBill.GetPatientBills(Convert.ToInt32(dr["BillNo"].ToString()));
                        foreach (DataRow drPatientBills in dsPatientBills.Tables[0].Rows)
                        {

                            drPatientBills["GrossAmount"] = GTotal;
                            double ServiceTax = Convert.ToDouble(drPatientBills["TaxPercent"].ToString());
                            double TaxAmount = 0.00;
                            double TotalAmount = 0;
                            double Discount = 0;
                            Double BalAmount = 0;
                            if (ServiceTax > 0)
                            {
                                TaxAmount = GTotal * (ServiceTax / 100);
                            }
                            drPatientBills["TaxAmount"] = TaxAmount.ToString();
                            TotalAmount = GTotal + TaxAmount;
                            TotalAmount = TotalAmount + Convert.ToDouble(drPatientBills["PreBalanceAmount"]);
                            drPatientBills["TotalAmount"] = TotalAmount.ToString();
                            Discount = Convert.ToDouble(drPatientBills["DiscountAmount"]);
                            drPatientBills["NetPayableAmount"] = Convert.ToDouble(drPatientBills["TotalAmount"]) - Convert.ToDouble(drPatientBills["DiscountAmount"]);
                            BalAmount = Convert.ToDouble(drPatientBills["NetPayableAmount"]) - Convert.ToDouble(drPatientBills["Paidamount"]);
                            drPatientBills["BalanceAmount"] = BalAmount.ToString();
                            drPatientBills["Mode"] = "Edit";

                        }
                        _OPDBill.EditTPAWiseBills(dsPatientBills, obj);
                        _OPDBill.DeletePatientBillDetails(Convert.ToInt32(dsPatientBills.Tables[0].Rows[0]["BillNo"].ToString()));
                        _OPDBill.EditTPAWiseBills1(dsPatientBillsDetail, obj);

                    }
                }

                else
                {
                    dsservicerate = _OPDBill.GetAllServicesWiseForOPDBills();

                    foreach (DataRow dr in dsTotalBills.Tables[0].Rows)
                    {

                        dsPatientBillsDetail = _OPDBill.GetPatientBillsDetails(Convert.ToInt32(dr["BillNo"].ToString()));

                        double GTotal = 0;
                        string TPARate1;
                        foreach (DataRow drPatientBillsDetail in dsPatientBillsDetail.Tables[0].Rows)
                        {
                            if ((drPatientBillsDetail["ServiceType"].ToString() == "OPD"))
                            {

                                DataView dv = new DataView(dsservicerate.Tables[0], "ServiceID = " + drPatientBillsDetail["ServiceID"].ToString() + " ", "", DataViewRowState.CurrentRows);
                                if (dv.Count != 0)
                                {

                                    drPatientBillsDetail["Rate"] = dv[0]["GeneralCharges"].ToString();
                                    drPatientBillsDetail["TotalAmount"] = (Convert.ToDecimal(dv[0]["GeneralCharges"]) * Convert.ToDecimal(drPatientBillsDetail["Quantity"]));

                                    GTotal = GTotal + (Convert.ToDouble(dv[0]["GeneralCharges"]) * Convert.ToDouble(drPatientBillsDetail["Quantity"]));
                                    obj.GTotal = GTotal.ToString();


                                }
                            }
                            else if ((drPatientBillsDetail["ServiceType"].ToString() == "OPDLabBills"))
                            {
                                DataView dvTest = new DataView(dsservicerate.Tables[1], "TestID = " + drPatientBillsDetail["ServiceID"].ToString() + " ", "", DataViewRowState.CurrentRows);
                                if (dvTest.Count != 0)
                                {
                                    //if (dvTest[0]["RecommendedByDoctor"].ToString() == "No")
                                    //{
                                    drPatientBillsDetail["Rate"] = dvTest[0]["GeneralCharges"].ToString();
                                    drPatientBillsDetail["TotalAmount"] = (Convert.ToDecimal(dvTest[0]["GeneralCharges"]) * Convert.ToDecimal(drPatientBillsDetail["Quantity"]));

                                    GTotal = GTotal + (Convert.ToDouble(dvTest[0]["GeneralCharges"]) * Convert.ToDouble(drPatientBillsDetail["Quantity"]));
                                    obj.GTotal = GTotal.ToString();
                                    //}
                                    //else
                                    //{

                                    //    GTotal = GTotal + Convert.ToDouble(drPatientBillsDetail["Rate"]) * Convert.ToDouble(drPatientBillsDetail["Quantity"]);
                                    //    obj.GTotal = GTotal.ToString();
                                    //}
                                }

                            }


                        }
                        dsPatientBills = _OPDBill.GetPatientBills(Convert.ToInt32(dr["BillNo"].ToString()));
                        foreach (DataRow drPatientBills in dsPatientBills.Tables[0].Rows)
                        {

                            drPatientBills["GrossAmount"] = GTotal;
                            double ServiceTax = Convert.ToDouble(drPatientBills["TaxPercent"].ToString());
                            double TaxAmount = 0.00;
                            double TotalAmount = 0;
                            double Discount = 0;
                            Double BalAmount = 0;
                            if (ServiceTax > 0)
                            {
                                TaxAmount = GTotal * (ServiceTax / 100);
                            }
                            drPatientBills["TaxAmount"] = TaxAmount.ToString();
                            TotalAmount = GTotal + TaxAmount;
                            TotalAmount = TotalAmount + Convert.ToDouble(drPatientBills["PreBalanceAmount"]);
                            drPatientBills["TotalAmount"] = TotalAmount.ToString();
                            Discount = Convert.ToDouble(drPatientBills["DiscountAmount"]);
                            drPatientBills["NetPayableAmount"] = Convert.ToDouble(drPatientBills["TotalAmount"]) - Convert.ToDouble(drPatientBills["DiscountAmount"]);
                            BalAmount = Convert.ToDouble(drPatientBills["NetPayableAmount"]) - Convert.ToDouble(drPatientBills["Paidamount"]);
                            drPatientBills["BalanceAmount"] = BalAmount.ToString();
                            drPatientBills["Mode"] = "Edit";

                        }
                        _OPDBill.EditTPAWiseBills(dsPatientBills, obj);
                        _OPDBill.DeletePatientBillDetails(Convert.ToInt32(dsPatientBills.Tables[0].Rows[0]["BillNo"].ToString()));
                        _OPDBill.EditTPAWiseBills1(dsPatientBillsDetail, obj);

                    }
                }
            }
            message = "Save Changes";

            return View(obj);
        }
        public JsonResult EditPatient_OPDDetail(int PatientRegNO,string PatientType)
        {
            return new JsonResult { Data = _OPDBill.EditPatient_OPDDetail1(PatientRegNO, PatientType), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetSecurityDeposite(int PatientRegNO)
        {


            return new JsonResult { Data = _OPDBill.GetSecurityDeposite(PatientRegNO), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetPatientIPDWardDetailsForEdit(int PatientRegNO,int PatientIPDNO)
        {


            return new JsonResult { Data = _OPDBill.GetPatientIPDWardDetailsForEdit(PatientRegNO, PatientIPDNO), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult BindCaculteAge(string date)
        {
            BL_PatientOPDBill db = new BL_PatientOPDBill();
            string aa = Convert.ToDateTime(date).ToString("yyyy-MM-dd");
            return new JsonResult { Data = aa, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult getFill_Department(string DepartMentID)
        {

            List<string> searchList = new List<string>();
            if (DepartMentID != "0")
            {
                DataTable td = new DataTable();
                td = _OPDBill.GetDoctorDepartment(DepartMentID);
                obj.DepartmentID = td.Rows[0]["DepartmentName"].ToString();
                searchList.Add(obj.DepartmentID);
            }
            return Json(searchList);
        }
        public JsonResult imageuplod(PatientOPD model)
        {
            string path = "";
            //PatientMRD_Model Model
            // PatientMRD_Model obj = new PatientMRD_Model();
            var file = model.ImageFile;
            if (file != null)
            {
                //var path = "";
                var filename = Path.GetFileName(file.FileName);
                var extention = Path.GetExtension(file.FileName);
                var filenamethoutextenction = Path.GetFileNameWithoutExtension(file.FileName);
                //  file.SaveAs(Server.MapPath("~/MRDFiles/" + file.FileName));
                //  Session["Paper"] = "/MRDFiles/" + file.FileName;

                path = Server.MapPath("~/") + "MRDFiles/" + file.FileName;

                file.SaveAs(path);
                // Session["Paper"] = path;
                ViewData["ImagePath"] = "/MRDFiles/" + file.FileName; 
                  path=  "/MRDFiles/" + file.FileName; 
            }
            return new JsonResult { Data = path, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult PatientOPD()
        {
            DataSet dsFinancialYear = _OPDBill.Bind_FinancialYear();
            if (dsFinancialYear.Tables[0].Rows.Count > 0)
            {
                obj.FinancialYear = dsFinancialYear.Tables[0].Rows[0]["FinancialYear"].ToString();

                obj.PrintOPDNo = _OPDBill.BindPatientRegNoOPD(Convert.ToInt32(dsFinancialYear.Tables[0].Rows[0]["FinancialYearID"])).ToString();
                obj.PrintIPDNO = _OPDBill.BindPatientRegNoIPD(Convert.ToInt32(dsFinancialYear.Tables[0].Rows[0]["FinancialYearID"])).ToString();
                obj.printRegNo = _OPDBill.BindPatientPrintRegNo(Convert.ToInt32(dsFinancialYear.Tables[0].Rows[0]["FinancialYearID"]));
            }
            return View(obj);
        }
        [HttpPost]
        public ActionResult PatientOPD(PatientOPD obj1, FormCollection fc)
        {
            try
            {
                obj1.PatientIPDNO = Request.Form["PatientIPDNO"].ToString();
                obj1.PatientOPDNO = Request.Form["PatientOPDNO"].ToString();
                obj1.PatientRegNo = Convert.ToInt32(Request.Form["RegNo"].ToString());
                obj1.chkIPDOPD = Request.Form["colorRadio"].ToString();
                if (obj1.chkIPDOPD == "IPD")
                {
                    
                    //if (Request.Form["TPANameID"].ToString() == null || Request.Form["TPANameID"].ToString() == "")
                    //{
                       
                    //}
                    //else
                    //{
                    //    obj1.TPAStatus = "true";
                    //}
                }

                if (Request.Form["PatientCancelID"] == null || Request.Form["PatientCancelID"].ToString() == "")
                {
                    obj1.PatientCancel = "false";
                }
                else
                {
                    obj1.PatientCancel = "true";
                }
                
                obj1.PatientCancelReason = Request.Form["PatientCancelText"].ToString();
                
                if (Request.Form["TemporaryDoctorID"] == null || Request.Form["TemporaryDoctorID"].ToString() == "")
                {
                    obj1.TemporaryDoctorStatus = "false";
                }
                else
                {
                    obj1.TemporaryDoctorStatus = "true";
                }

                if (Request.Form["RefferedTemporaryDoctorID"] == null || Request.Form["RefferedTemporaryDoctorID"].ToString() == "")
                {
                    obj1.RefferedTemporaryDoctorStatus = "false";
                }
                else
                {
                    obj1.RefferedTemporaryDoctorStatus = "true";
                }

                if(Request.Form["TPAName"] == "" || Request.Form["TPAName"] == null)
                {
                    obj1.TPA_ID = "";
                    obj1.TPA_Name = "";
                    obj1.TPAStatus = "false";
                }
                else
                {
                    obj1.TPA_ID = Request.Form["TPANameID"].ToString();
                    obj1.TPA_Name = Request.Form["TPAName"].ToString();
                    obj1.TPAStatus = "true";
                }


                // string nm = Request.Form["PFPatientNameID"].ToString();
                if (Request.Form["PFPatientNameID"] == "" || Request.Form["PFPatientNameID"] == null)
                {

                    obj1.PFPatientName = "";
                }
                else
                {
                    obj1.PFPatientName = Request.Form["PFPatientNameID"].ToString();
                }
                obj1.PFirstName = Request.Form["FirstName"].ToString();
                obj1.PMiddleName = Request.Form["MiddleName"].ToString();
                obj1.PLastName = Request.Form["LastName"].ToString();
                obj1.DepartmentID = Request.Form["DepartMentID"].ToString();
                obj1.Source = Request.Form["Source"].ToString();
                obj1.MediclaimCardID = Request.Form["MediclaimcardID"].ToString();
                obj1.OrganizationID = Request.Form["OrganizationID"].ToString();
                obj1.OrganizationName = Request.Form["OrganizationName"].ToString();

                obj1.OrganizationName = Request.Form["OrganizationName"].ToString();
                obj1.TemporaryDoctor = Request.Form["TemporaryDoctorText"].ToString();

                obj1.RefferedTemporaryDoctor = Request.Form["RefferedTemporaryDoctorText"].ToString();
                obj1.ReferenceCode = Request.Form["PatientAppointmentID"].ToString();
                obj1.Photo = Request.Form["OPDImage3"].ToString();
                obj1.Reason = Request.Form["Reason"].ToString();
                obj1.ReferredByDoctorID = Request.Form["ReferredByDoctorID"].ToString();
                obj1.DoctorID = Request.Form["ConsultantDoctorID"].ToString();
                obj1.GuardianName = Request.Form["GuardianName"].ToString();
                obj1.PFGuardianName = Request.Form["PFGuardianNameID"].ToString();
                obj1.Gender = Request.Form["Gender"].ToString();
                obj1.AadhaarNo = Request.Form["AadhaarNo"].ToString();
                obj1.EmailID = Request.Form["EmailID"].ToString();
                obj1.BloodPressure = Request.Form["BP"].ToString();
                obj1.PanCardNo = Request.Form["PanCardNo"].ToString();
                obj1.PinCode = Request.Form["Pincode"].ToString();
                obj1.Age = Request.Form["Year"].ToString();
                obj1.Height = Request.Form["Height"].ToString();
                obj1.Weight = Request.Form["Weight"].ToString();
                obj1.Address = Request.Form["Address"].ToString();
                obj1.MobileNo = Request.Form["MobileNo"].ToString();
                obj1.CityID = Request.Form["CityID"].ToString();
                obj1.StateID = Request.Form["StateID"].ToString();
                obj1.CountryID = Request.Form["CountryID"].ToString();
                obj1.CityName = Request.Form["CityName"].ToString();
                obj1.StateName = Request.Form["State"].ToString();
                obj1.CountryName = Request.Form["Country"].ToString();

                if (obj1.chkIPDOPD == "IPD")
                {
                    obj1.WardName = Request.Form["Wardname"].ToString();
                    obj1.RoomNo = Request.Form["RoomNo"].ToString();
                    obj1.BedNO = Request.Form["BedNo"].ToString();


                    //obj1.WardName = obj1.WardID;
                    //obj1.RoomNo =obj.RoomID;
                    //obj1.BedNO = obj.BedNO;


                    obj1.ApplyWardID = Request.Form["ApplicableClassID"].ToString();
                    obj1.SecurityDeposityID = Request.Form["SecurityDeposityID"].ToString();
                    obj1.CrAmount = Request.Form["SecurityDeposit"].ToString();
                    obj1.AdvanceAmt = Request.Form["DepositeAmt"].ToString();
                    if (Request.Form["payment_type"] == "" || Request.Form["payment_type"] == null)
                    {
                        obj1.PaymentType = null;
                    }
                    else
                    {

                        obj1.PaymentType = Request.Form["payment_type"].ToString();
                    }
                    obj1.Number = Request.Form["Number"].ToString();
                    obj1.Name = Request.Form["Name"].ToString();
                    obj1.Remark = Request.Form["Remarks"].ToString();
                    obj1.Date = Request.Form["paymentDate"].ToString();
                  
                }
                obj1.InsuranceCompnyID = Request.Form["InsuranceID"].ToString();
                obj1.InsuranceCompny = Request.Form["InsuranceName"].ToString();

                obj1.PhoneNo = Request.Form["PhoneNumber"].ToString();


                if (Request.Form["ClaimNo"].ToString() == "" || Request.Form["ClaimNo"].ToString() == null)
                {
                    obj1.CCNClaimNo = "";
                }
                else
                {
                    obj1.CCNClaimNo = Request.Form["ClaimNo"].ToString();
                }

                obj1.PolicyNo = Request.Form["PolicyNo"].ToString();




                // SecurityDeposit

                //obj1.PatientRegistrationDate = Request.Form["RegistrationDate"];
                //obj1.RegistrationTime = Request.Form["RegistrationTime"];

                //obj.PatientRegistrationDate = Request.Form["RegistrationDate"] + " " + Request.Form["RegistrationTime"];

                obj1.PatientRegistrationDate = fc["RegistrationDate"] + " " + fc["RegistrationTime"];

                obj1.chkIPDOPD = Request.Form["colorRadio"].ToString();
                if (obj1.chkIPDOPD == "OPD")
                {
                    obj1.chkIPDOPD = "OPD";
                }
                else
                {
                    obj1.chkIPDOPD = "IPD";
                }
                if (obj1.chkIPDOPD == "OPD")
                {
                    string a = Request.Form["Regnew"].ToString();
                    if (Request.Form["Regnew"] == "New Registration")
                    {
                        obj1.PatientType = "NEWOPD";
                        obj1.NewOrRevisit = "New Registration";
                    }
                    else
                    {
                        if (Request.Form["Regnew"] == "Revisit")
                        {
                            obj1.PatientRegNo = Convert.ToInt32(Request.Form["RegNo"].ToString());
                            obj1.PatientType = "ReVisit";
                            obj1.PatientType = "REVISIT";
                        }
                    }
                }
                else
                {
                    if (Request.Form["Regnew"] == "New Registration")
                    {
                        obj1.NewOrRevisit = "New Registration";
                        obj1.PatientType = "NEWIPD";

                    }
                    else
                    {
                        //if (Request.Form["RevisitOPD_OPD_No"] != null)
                        //{
                        string o = Request.Form["RevisitOPD_OPD_No"];

                        if (Request.Form["RevisitOPD_OPD_No"] != "" && Request.Form["RevisitOPD_OPD_No"] != "0")
                        {

                            obj1.PatientRegNo = Convert.ToInt32(Request.Form["RegNo"].ToString());
                            obj1.PatientOPDNO = Request.Form["RevisitOPD_OPD_No"].ToString();
                            obj1.PatientType = "FROMOPD";
                        }
                        else
                        {
                            obj1.PatientType = "READDMISSION";
                        }
                    }
                }


                int RegNo = obj1.PatientRegNo;
                  if (_OPDBill.save(obj1))
                {
                    if(RegNo > 0)
                    {
                        ModelState.Clear();
                        ViewData["flag"] = "Done";
                      //  TempData["Msg"] = "Patient Details Updated Succussfully";
                        if (obj1.chkIPDOPD == "IPD")
                        {

                            Session["HospitalIDReport"] = HospitalID;
                            Session["LocationIDReport"] = LocationID;
                            Session["Registration"] = obj1.PatientRegNo;
                            Session["IPDNo"] = obj1.PatientIPDNO;
                            Session["Advanceamt"] = obj1.AdvanceAmt;
                            return RedirectToAction("RptAdmissionFormNew", "PatientReport");
                        }
                        else
                        {

                            Session["HospitalIDReport"] = HospitalID;
                            Session["LocationIDReport"] = LocationID;
                            Session["Registration"] = obj1.PatientRegNo;
                            Session["OPDNo"] = obj1.PatientOPDNO;
                            return RedirectToAction("RptPatientPrescriptionForOPD", "PatientReport");
                        }
                    }
                    else
                    {
                        ModelState.Clear();
                        ViewData["flag"] = "Done";
                    //    TempData["Msg"] = "Patient Details Saved Succussfully";
                        if (obj1.chkIPDOPD == "IPD")
                        {

                            Session["HospitalIDReport"] = HospitalID;
                            Session["LocationIDReport"] = LocationID;
                            Session["Registration"] = obj1.PatientRegNo;
                            Session["IPDNo"] = obj1.PatientIPDNO;
                            Session["Advanceamt"] = obj1.AdvanceAmt;
                            return RedirectToAction("RptAdmissionFormNew", "PatientReport");
                        }
                        else
                        {
                            Session["HospitalIDReport"] = HospitalID;
                            Session["LocationIDReport"] = LocationID;
                            Session["Registration"] = obj1.PatientRegNo;
                            Session["OPDNo"] = obj1.PatientOPDNO;
                            return RedirectToAction("RptPatientPrescriptionForOPD", "PatientReport");
                        }
                    }
                   // return RedirectToAction("RptPatientPrescriptionForOPD", "PatientReport");
                }
                else
                {
                    ViewData["flag"] = "Error";

                }

            }
            catch (Exception ex)
            {

            }
            //DataSet dsFinancialYear = _OPDBill.Bind_FinancialYear();
            //obj.FinancialYear = dsFinancialYear.Tables[0].Rows[0]["FinancialYear"].ToString();

            //obj.PrintOPDNo = _OPDBill.BindPatientRegNoOPD(Convert.ToInt32(dsFinancialYear.Tables[0].Rows[0]["FinancialYearID"])).ToString();
            //obj.PrintIPDNO = _OPDBill.BindPatientRegNoIPD(Convert.ToInt32(dsFinancialYear.Tables[0].Rows[0]["FinancialYearID"])).ToString();

            return View(obj);
        }
        public JsonResult CalGetDaysAge(string datebirth)
        {
            DateTime dtpDateOfBirth;
            if (datebirth != "")
            {


                dtpDateOfBirth = Convert.ToDateTime(datebirth);
            }
            else
            {
                dtpDateOfBirth = System.DateTime.Now;
            }
                List<PatientOPD> add = new List<Models.Patient.PatientOPD>();
                PatientOPD obj = new Models.Patient.PatientOPD();
                DateTime dtToday = new System.DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                DateTime dtBeforeDate = new System.DateTime(dtpDateOfBirth.Year, dtpDateOfBirth.Month, dtpDateOfBirth.Day, 0, 0, 0);
                int Year = 0;
                int Month = 0;
                int Day = 0;

                dtBeforeDate = dtBeforeDate.AddYears(1);
                while (dtToday >= dtBeforeDate)
                {
                    dtBeforeDate = dtBeforeDate.AddYears(1);
                    Year++;
                }
                dtBeforeDate = new System.DateTime(dtpDateOfBirth.Year, dtpDateOfBirth.Month, dtpDateOfBirth.Day, 0, 0, 0);
                dtBeforeDate = dtBeforeDate.AddMonths(1);
                while (dtToday >= dtBeforeDate)
                {
                    dtBeforeDate = dtBeforeDate.AddMonths(1);
                    Month++;
                }

                dtBeforeDate = new System.DateTime(dtpDateOfBirth.Year, dtpDateOfBirth.Month, dtpDateOfBirth.Day, 0, 0, 0);

                dtBeforeDate = dtBeforeDate.AddMonths(Month);

                var a = dtBeforeDate;
                //if(a== getTime())
                //{

                //}
                TimeSpan diffResult = dtToday.Date - dtBeforeDate.Date;

                int TotalDay = Convert.ToInt32(diffResult.TotalDays);
                int TotalMonth = Month % 12;

                obj.day = TotalDay.ToString();

                obj.Month = TotalMonth.ToString();

                obj.year = Year.ToString();
                add.Add(obj);

                return new JsonResult { Data = add, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            
        }
        public JsonResult GetPrintNo_ToRegNo(string PrintRegNo)
        {
            BL_PatientOPD BL_Reg = new BL_PatientOPD();
            string RegNo = BL_Reg.GetPrintNo_ToRegNo(PrintRegNo);
            return new JsonResult { Data = RegNo, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        public JsonResult GetPatientPrintNo_ToRegNo(string PrintRegNo,string PatientType)
        {
            BL_PatientOPD BL_Reg = new BL_PatientOPD();
           string RegNo = BL_Reg.GetPatientPrintNo_ToRegNo(PrintRegNo, PatientType);
            return new JsonResult { Data = RegNo, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        
        //public ActionResult Print_sticker(string ipdno)
        //{
        //    Session["HospitalIDReport"] = HospitalID;
        //    Session["LocationIDReport"] = LocationID;



        //    Session["PatientRegNoReport"] = ipdno;
        //    //Session["PatientIPDNoReport"] = Request.Form["PatientIPDNO"].ToString(); 
        //    //Session["ReportType"] = "SaveView";
        //    //Session["WardName"] = "";
        //    //Session["Quantity"] = Request.Form["sticker"].ToString();

        //    return RedirectToAction("Rpt_A4stickerprint", "PatientReport");
        //}


        public JsonResult Print_sticker(string ipdno, string regno, string qty)
        {
            Session["HospitalIDReport"] = HospitalID;
            Session["LocationIDReport"] = LocationID;
            
            Session["PatientRegNoReport"] = regno;
            Session["PatientIPDNoReport"] = ipdno;
            Session["ReportType"] = "SaveView";
            Session["WardName"] = "";
            Session["Quantity"] = qty;

            return new JsonResult { Data = ipdno, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

    }
}