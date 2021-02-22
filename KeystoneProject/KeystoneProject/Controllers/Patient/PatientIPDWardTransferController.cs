using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Patient;
using KeystoneProject.Buisness_Logic.Hospital;
using KeystoneProject.Buisness_Logic.Patient;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace KeystoneProject.Controllers.Patient
{
    public class PatientIPDWardTransferController : Controller
    {
        int HospitalID;
        int LocationID;
        int CreationID;
        BL_PatientIPDWardTransfer BL_obj = new BL_PatientIPDWardTransfer();

        //
        // GET: /PatientIPDWardTransfer/
        [HttpGet]
        public ActionResult PatientIPDWardTransfer()
        {
            HospitlLocationID();
            return View();
        }
        public void HospitlLocationID()
        {
            HospitalID = Convert.ToInt32(Session["HospitalID"]);
            LocationID = Convert.ToInt32(Session["LocationID"]);
            CreationID = Convert.ToInt32(Session["UserID"]);
        }

        [HttpPost]
        public ActionResult PatientIPDWardTransfer(PatientIPDWardTransfer obj)
        {
            BL_PatientIPDWardTransfer BL_obj = new BL_PatientIPDWardTransfer();

            HospitlLocationID();
            obj.PatientRegNO = Convert.ToInt32(Request.Form["regNo"]);
            obj.PatientIPDNO = Convert.ToInt32(Request.Form["IPD"]);
            #region New Word
            obj.WardID = Request.Form["to_ward_ID"];
            obj.RoomID = Request.Form["RoomID"];
            obj.BedID = Request.Form["to_bed_ID"];

            #endregion
            #region OLD word


            obj.FromWordID = Request.Form["wordID1"];
            obj.FromRoomID = Request.Form["from_room1"];
            obj.FromBedID = Request.Form["from_bed_no1"];
            #endregion

            obj.EnterDateTime = Request.Form["transfer_date"];

            obj.TransferTime = Request.Form["transfer_time"];

            obj.EnterDateTime = Request.Form["transfer_date"] + " " + Request.Form["transfer_time"];

            if (BL_obj.IUPatientIPDWardTransfer(obj))
            {
                TempData["Msg"] = "Record Saved Sussefully";
            }
            else
            {
                TempData["Msg"] = null;
            }
            return RedirectToAction("PatientIPDWardTransfer", "PatientIPDWardTransfer");
        }

        public JsonResult GetAllFinancialYear()
        {
            DataSet ds = BL_obj.GetAllFinancialYear();
            List<PatientIPDWardTransfer> searchList = new List<PatientIPDWardTransfer>();
            DataView dvTest = new DataView(ds.Tables[0], "FinancialYearID = " + ds.Tables[1].Rows[0]["FinancialYearID"].ToString() + " ", "", DataViewRowState.CurrentRows);
            dvTest.ToTable().Rows[0]["FinancialYear"].ToString();
            foreach (DataRow dr in dvTest.ToTable().Rows)
            {
                searchList.Add(new PatientIPDWardTransfer
                {
                    FinancialYear = dr["FinancialYear"].ToString(),
                    FinancialYearID = Convert.ToInt32(dr["FinancialYearID"]).ToString()
                });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult BindName(string prefix)
        {
            HospitlLocationID();

            BL_PatientIPDWardTransfer BL_obj = new BL_PatientIPDWardTransfer();

            DataSet ds = BL_obj.SearchPatientiIPDWardTransferByNameID("%", HospitalID, LocationID, prefix);

            List<PatientIPDWardTransfer> lists = new List<PatientIPDWardTransfer>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientIPDWardTransfer obj = new PatientIPDWardTransfer();

                obj.PatientRegNO = Convert.ToInt32(dr["PatientRegNO"].ToString());
                obj.PatientName = dr["Name"].ToString();

                lists.Add(obj);
            }

            return Json(lists, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FillData(string prefix)
        {
            BL_PatientIPDWardTransfer BL_obj = new BL_PatientIPDWardTransfer();
            List<PatientIPDWardTransfer> lists = new List<PatientIPDWardTransfer>();
            HospitlLocationID();
            try
            {
                DataSet dsDetails = BL_obj.GetPatientData(HospitalID, LocationID, Convert.ToInt32(prefix));

                if (dsDetails.Tables[0].Rows.Count > 0)
                {
                    if (BL_obj.CheckFinalBill(HospitalID, LocationID, Convert.ToInt32(prefix), Convert.ToInt32(dsDetails.Tables[0].Rows[0]["PatientIPDNo"].ToString())) == false)
                    {
                        if (BL_obj.CheckDischargePatient(HospitalID, LocationID, Convert.ToInt32(prefix), Convert.ToInt32(dsDetails.Tables[0].Rows[0]["PatientIPDNo"].ToString())) == false)
                        {


                            foreach (DataRow dr in dsDetails.Tables[0].Rows)
                            {


                                foreach (DataRow dr1 in dsDetails.Tables[1].Rows)
                                {
                                    PatientIPDWardTransfer obj = new PatientIPDWardTransfer();
                                    obj.Message = "";
                                    if (dr1["ExitDateTime"].ToString() != null && dr1["ExitDateTime"].ToString() != "")
                                    {
                                        // ucPatientIPDWardTransfer1.dgvBedDetails.Rows.Add(dsDetails.Tables[1].Rows[i]["WardName"].ToString(), dsDetails.Tables[1].Rows[i]["RoomName"].ToString(),
                                        //  dsDetails.Tables[1].Rows[i]["BedNO"].ToString(), dsDetails.Tables[1].Rows[i]["EnterDateTime"].ToString(), dsDetails.Tables[1].Rows[i]["ExitDateTime"].ToString());
                                        obj.WardNameFill = dr1["WardName"].ToString();
                                        obj.RoomNameFill = dr1["RoomName"].ToString();
                                        obj.BedNoFill = dr1["BedNO"].ToString();
                                        obj.EnterDateTimeFill = dr1["EnterDateTime"].ToString();
                                        obj.ExitTime = dr1["ExitDateTime"].ToString();

                                    }
                                    else if (dr1["ExitDateTime"].ToString() == null || dr1["ExitDateTime"].ToString() == "")
                                    {
                                        // ucPatientIPDWardTransfer1.dgvBedDetails.Rows.Add(dsDetails.Tables[1].Rows[i]["WardName"].ToString(), dsDetails.Tables[1].Rows[i]["RoomName"].ToString(), dsDetails.Tables[1].Rows[i]["BedNO"].ToString(),
                                        // dsDetails.Tables[1].Rows[i]["EnterDateTime"].ToString(), "Present");
                                        obj.WardNameFill = dr1["WardName"].ToString();
                                        obj.RoomNameFill = dr1["RoomName"].ToString();
                                        obj.BedNoFill = dr1["BedNO"].ToString();
                                        obj.EnterDateTimeFill = dr1["EnterDateTime"].ToString();
                                        obj.ExitTime = dr1["ExitDateTime"].ToString() + "Present";

                                    }

                                    obj.PatientRegNO = Convert.ToInt32(dr["PatientRegNO"].ToString());
                                    obj.PatientName = dr["PatientName"].ToString();
                                    //ucPatientIPDWardTransfer1.txtPatientName.Text = dsDetails.Tables[0].Rows[0]["PatientName"].ToString();

                                    obj.PatientIPDNO = Convert.ToInt32(dsDetails.Tables[0].Rows[0]["PatientIPDNO"].ToString());

                                    DataSet ds = BL_obj.FillWord();

                                    DataView dv = new DataView(ds.Tables[0], "WardID=" + dsDetails.Tables[0].Rows[0]["WardID"].ToString() + "", "", DataViewRowState.CurrentRows);

                                    DataSet dsroom = BL_obj.FillToRoom(Convert.ToInt32(dsDetails.Tables[0].Rows[0]["WardID"]));

                                    DataView dvroom = new DataView(dsroom.Tables[0], "RoomID=" + dsDetails.Tables[0].Rows[0]["RoomID"].ToString() + "", "", DataViewRowState.CurrentRows);

                                    DataSet dsBed = BL_obj.FillTOBed(Convert.ToInt32(dsDetails.Tables[0].Rows[0]["WardID"]), Convert.ToInt32(dsDetails.Tables[0].Rows[0]["RoomID"]));
                                    DataView dvBed = new DataView(dsBed.Tables[0], "BedID=" + dsDetails.Tables[0].Rows[0]["BedID"].ToString() + "", "", DataViewRowState.CurrentRows);

                                    obj.GuardianName = dsDetails.Tables[0].Rows[0]["GuardianName"].ToString();

                                    if (dsDetails.Tables[0].Rows[0]["DoctorID"].ToString() != "" && dsDetails.Tables[0].Rows[0]["DoctorID"].ToString() != "0")
                                    {
                                        obj.DoctorName = BL_obj.BindDoctor(Convert.ToInt32(dsDetails.Tables[0].Rows[0]["DoctorID"])).Tables[0].Rows[0]["DoctorPrintName"].ToString();
                                    }
                                    obj.DoctorID = dsDetails.Tables[0].Rows[0]["DoctorID"].ToString();

                                    obj.OrganizationID = dsDetails.Tables[0].Rows[0]["OrganizationID"].ToString();

                                    obj.WardID = dsDetails.Tables[0].Rows[0]["WardID"].ToString();
                                    if (dv.ToTable().Rows.Count > 0)
                                    {
                                        obj.WardName = dv.ToTable().Rows[0]["WardName"].ToString();
                                    }
                                    obj.RoomID = dsDetails.Tables[0].Rows[0]["RoomID"].ToString();
                                    if (dvroom.ToTable().Rows.Count > 0)
                                    {
                                        obj.RoomName = dvroom.ToTable().Rows[0]["RoomName"].ToString();
                                    }
                                    obj.BedID = dsDetails.Tables[0].Rows[0]["BedID"].ToString();
                                    if (dvBed.ToTable().Rows.Count > 0)
                                    {
                                        obj.BedName = dvBed.ToTable().Rows[0]["BedNO"].ToString();
                                    }
                                    obj.EnterDateTime = Convert.ToDateTime(dsDetails.Tables[0].Rows[0]["EnterDateTime"]).ToString("yyyy-MM-dd");

                                    obj.time = Convert.ToDateTime(dsDetails.Tables[0].Rows[0]["EnterDateTime"]).ToString("hh:mm:ss");

                                    //obj.FinancialYearID = dsDetails.Tables[0].Rows[0]["FinancialYearID"].ToString();

                                    //obj.FinancialYear = dsDetails.Tables[0].Rows[0]["FinancialYear"].ToString();
                                    lists.Add(obj);
                                }
                            }
                            //lists.Add(obj);
                        }
                        else
                        {
                            PatientIPDWardTransfer obj = new PatientIPDWardTransfer();
                            obj.Message = "Patient Already Discharge";
                            lists.Add(obj);
                        }

                    }
                    else
                    {
                        PatientIPDWardTransfer obj = new PatientIPDWardTransfer();
                        //lists.Add("Update is successful.");
                        obj.Message = "Final Bill Generated";
                        lists.Add(obj);


                        //  Response.Write(@"<script language='javascript'>alert('Update is successful.')</script>");

                    }
                }



            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(lists, JsonRequestBehavior.AllowGet);

        }

        public JsonResult BindTable(string RegNo)
        {
            BL_PatientIPDWardTransfer BL_obj = new BL_PatientIPDWardTransfer();
            List<PatientIPDWardTransfer> lists = new List<PatientIPDWardTransfer>();
            HospitlLocationID();

          
            try
            {
                DataSet dsDetails = BL_obj.GetPatientData(HospitalID, LocationID, Convert.ToInt32(RegNo));

                if (dsDetails.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in dsDetails.Tables[1].Rows)
                    {
                        lists.Add(new PatientIPDWardTransfer
                        {
                            WardName = dr["WardName"].ToString(),
                            RoomName = dr["RoomName"].ToString(),
                            BedNo = dr["BedNO"].ToString(),
                            //EnterDateTime = Convert.ToDateTime(dr["EnterDateTime"]).ToString("yyyy-MM-dd"),
                            //time = Convert.ToDateTime(dr["EnterDateTime"]).ToString("hh:mm"),
                            EnterDateTimeFill = dr["EnterDateTime"].ToString(),
                            ExitTime = dr["ExitDateTime"].ToString(),

                    });
                    }
                }

            }
            catch (Exception exe)
            {

                throw;
            }

            return Json(lists, JsonRequestBehavior.AllowGet);

        }


        public JsonResult BindToWord(string prefix)
        {
            HospitlLocationID();

            BL_PatientIPDWardTransfer BL_obj = new BL_PatientIPDWardTransfer();

            DataSet ds = BL_obj.BindToWard();

            List<PatientIPDWardTransfer> lists = new List<PatientIPDWardTransfer>();

            //DataView dv=new DataView(ds.Tables[0],"WardName like "+prefix+"","",DataViewRowState.CurrentRows);


            //  DataView dv1 = new DataView(dsPatientBillsDetails.Tables[0], " ServiceID = " + ServiceID + " ", "", DataViewRowState.CurrentRows);   

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientIPDWardTransfer obj = new PatientIPDWardTransfer();

                obj.WardID = dr["WardID"].ToString();
                obj.WardName = dr["WardName"].ToString();

                lists.Add(obj);
            }

            return Json(lists, JsonRequestBehavior.AllowGet);
        }


        public JsonResult BindToRoom(string prefix, int TowordID)
        {


            BL_PatientIPDWardTransfer BL_obj = new BL_PatientIPDWardTransfer();

            DataSet ds = BL_obj.BindToRoom(TowordID);

            List<PatientIPDWardTransfer> lists = new List<PatientIPDWardTransfer>();

            //DataView dv=new DataView(ds.Tables[0],"WardName like "+prefix+"","",DataViewRowState.CurrentRows);


            //  DataView dv1 = new DataView(dsPatientBillsDetails.Tables[0], " ServiceID = " + ServiceID + " ", "", DataViewRowState.CurrentRows);   

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientIPDWardTransfer obj = new PatientIPDWardTransfer();

                obj.RoomID = dr["RoomID"].ToString();
                obj.RoomName = dr["RoomName"].ToString();

                lists.Add(obj);
            }

            return Json(lists, JsonRequestBehavior.AllowGet);
        }


        public JsonResult BindToBed(int WordID, int RoomID)
        {


            BL_PatientIPDWardTransfer BL_obj = new BL_PatientIPDWardTransfer();

            DataSet ds = BL_obj.BindToBed(WordID, RoomID);

            List<PatientIPDWardTransfer> lists = new List<PatientIPDWardTransfer>();

            //DataView dv=new DataView(ds.Tables[0],"WardName like "+prefix+"","",DataViewRowState.CurrentRows);


            //  DataView dv1 = new DataView(dsPatientBillsDetails.Tables[0], " ServiceID = " + ServiceID + " ", "", DataViewRowState.CurrentRows);   

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientIPDWardTransfer obj = new PatientIPDWardTransfer();

                obj.BedID = dr["BedID"].ToString();
                obj.BedNo = dr["BedNO"].ToString();

                lists.Add(obj);
            }

            return Json(lists, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPatientDetails(int ipdno,int regno)
        {
            var opdno = "";
            DataSet ds = BL_obj.GetPatientDetails(Convert.ToInt32(ipdno),Convert.ToInt32(regno));

            List<PatientIPDWardTransfer> lists = new List<PatientIPDWardTransfer>();

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                PatientIPDWardTransfer obj = new PatientIPDWardTransfer();

                opdno = dr["PatientRegNO"].ToString();

                lists.Add(obj);
            }

            Session["ipdno"] = opdno;
            Session["opdno"] = ipdno;

            return Json(lists, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Print_sticker(string qty)
        {
            Session["HospitalIDReport"] = Session["HospitalID"];
            Session["LocationIDReport"] = Session["LocationID"];

            Session["PatientRegNoReport"] = Session["ipdno"];
            Session["PatientIPDNoReport"] = Session["opdno"];
            Session["ReportType"] = "SaveView";
            Session["WardName"] = "";
            Session["Quantity"] = qty;

            return new JsonResult { Data = qty, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

    }
}