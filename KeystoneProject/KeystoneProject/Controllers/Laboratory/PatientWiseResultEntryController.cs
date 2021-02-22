using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Laboratory;
using KeystoneProject.Buisness_Logic.Hospital;
using System.Data;
using KeystoneProject.Buisness_Logic.Laboratory;
using System.Collections;
using System.Net;
namespace KeystoneProject.Controllers.Laboratory
{         public class PatientWiseResultEntryController : Controller
        {
    int HospitalID = 0;
    int LocationID = 0;
    int CreationID = 0;
    string Footer = "";
        private void Connect()
        {
           
            HospitalID = Convert.ToInt32(Session["HospitalID"]);
            LocationID = Convert.ToInt32(Session["LocationID"]);
            CreationID = Convert.ToInt32(Session["UserID"]);
        }
        //
        // GET: /PatientWiseResultEntry/
        public ActionResult PatientWiseResultEntry()
        {
            return View();
        }
        
        BL_Patient_Wise_Result_Entry BL_obj = new BL_Patient_Wise_Result_Entry();
        Patient_Wise_Result_Entry location = new Patient_Wise_Result_Entry();
      
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PatientWiseResultEntry(FormCollection collection, FooterData obj1, string LabNo, string ParamiterID, string TestID2, string Formula, string newValue, string NLH1)
        
        {

            string Message = "";
             //string op = collection["footer[" + 1 + "]"];
           // string[] Footersplit = WebUtility.HtmlEncode(obj1.footer.ToString()).Replace("\r\n", "<br>").Split(',');
            //location.LabNo = collection["LabNo"].ToString();
            Connect();
            //location.LabNo = collection["LabNo"].ToString();
            if (obj1.Nyvalue.ToString().Trim().Length > 0)
            {
                string[] NLH = obj1.Nyvalue.ToString().Split(',');

                Calculation(Convert.ToInt32(obj1.LNo.ToString()), obj1.PreID.ToString(), obj1.TesID.ToString(), obj1.Formu.ToString(), obj1.Nyvalue.ToString(), obj1.HLN.ToString());
                if (obj1.TesID.ToString() == "")
                {
                    TestID2 = "0";
                }
                dsCompare = BL_obj.GetPatientWiseResultforParameterValue(Convert.ToInt32(obj1.LNo.ToString()), Convert.ToInt32(dsPatientEntry.Tables[0].Rows[0]["TestID"]));

                if (dsPatientEntry.Tables.Count > 0)
                {
                    int i = 0;
                    foreach (DataRow dr in dsPatientEntry.Tables[0].Rows)
                    {
                        if (dsCompare.Tables[0].Rows[i]["ResultValue"].ToString().Trim() != dr["ResultValue"].ToString().Trim())
                        {
                            dr["CreationID"] = CreationID;
                        }
                        else
                        {
                            dr["CreationID"] = dsCompare.Tables[0].Rows[i]["CreationID"].ToString().Trim();
                        }

                        if (NLH[i] == "Blank")
                        {
                            dr["NLH"] = "";
                        }
                        else
                        {
                            dr["NLH"] = NLH[i];
                        }
                        //if (Footersplit[i] == "Blank")
                        //{
                        //    dr["Footer"] = "";
                        //}
                        //else
                        //{
                        //    dr["Footer"] =  WebUtility.HtmlEncode(Footersplit[i]);
                        //}


                        dr["CompleteBy"] = CreationID;
                        dr["Mode"] = "Edit";
                        i++;
                    }
                    if (BL_obj.Save(dsPatientEntry, dsCompare, obj1.footer))
                    {
                        ModelState.Clear();
                        TempData["Msg"] = " Record Saved Successfully";
                        return RedirectToAction("PatientWiseResultEntry", "PatientWiseResultEntry");
                       

                    }
                }

            }

            return View();
        }
       //public ActionResult GetParamater(string footer)
       // {

       //     return Json(searclist, JsonRequestBehavior.AllowGet);
       // }
        public ActionResult GetParamaterHelpValue(string ParameterID)
        {
          
            BL_Patient_Wise_Result_Entry BL_obj = new BL_Patient_Wise_Result_Entry();
            DataSet ds = new DataSet();
            List<Patient_Wise_Result_Entry> searclist = new List<Models.Laboratory.Patient_Wise_Result_Entry>();

            DataSet dsDetail = new DataSet();

            dsDetail = BL_obj.GetParamaterHelpValue(Convert.ToInt32(ParameterID));
            foreach (DataRow dr in dsDetail.Tables[0].Rows)
            {
                Patient_Wise_Result_Entry obj = new Patient_Wise_Result_Entry();
                //  dr["Formula"] = "5";
                obj.ParamiterID = dr["ParameterID1"].ToString();
                obj.HelpValueID = dr["HelpValueID"].ToString();
                obj.HelpValue = dr["HelpValue"].ToString();



                searclist.Add(obj);

            }



            return Json(searclist, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPatientWiseResultForPatientSearchReg(int PatientRegNo)
        {
            BL_Patient_Wise_Result_Entry BL_obj = new BL_Patient_Wise_Result_Entry();
            DataSet ds = new DataSet();
            List<Patient_Wise_Result_Entry> searclist = new List<Models.Laboratory.Patient_Wise_Result_Entry>();

            ds = BL_obj.GetPatientWiseResultForPatientSearchReg(PatientRegNo);

            // test.Models.Master.MasterLaboratory.Patient_Wise_Result_Entry obj = new Patient_Wise_Result_Entry();

            // searclist.Add(obj);            

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                Patient_Wise_Result_Entry obj = new Patient_Wise_Result_Entry();
                obj.LabNo = ds.Tables[0].Rows[0]["LabNo"].ToString();
                obj.RegNo = ds.Tables[0].Rows[0]["PatientRegNo"].ToString();
                obj.Patientname = ds.Tables[0].Rows[0]["PatientName"].ToString();
                obj.ConsDr = ds.Tables[0].Rows[0]["DoctorPrintName"].ToString();


                obj.TestID = dr["TestID"].ToString();
                obj.Testname = dr["TestName"].ToString();

                searclist.Add(obj);

            }
            return Json(searclist, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DgvClickGetPatientWiseResultForPatientSearch(int LabNo)
        {
            BL_Patient_Wise_Result_Entry BL_obj = new BL_Patient_Wise_Result_Entry();
            DataSet ds = new DataSet();
            List<Patient_Wise_Result_Entry> searclist = new List<Models.Laboratory.Patient_Wise_Result_Entry>();
          
            ds = BL_obj.GetPatientWiseResultForPatientSearch(LabNo);

            // test.Models.Master.MasterLaboratory.Patient_Wise_Result_Entry obj = new Patient_Wise_Result_Entry();

            // searclist.Add(obj);            

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                Patient_Wise_Result_Entry obj = new Patient_Wise_Result_Entry();
                obj.LabNo = ds.Tables[0].Rows[0]["LabNo"].ToString();
                obj.RegNo = ds.Tables[0].Rows[0]["PatientRegNo"].ToString();
                obj.Patientname = ds.Tables[0].Rows[0]["PatientName"].ToString();
                obj.ConsDr = ds.Tables[0].Rows[0]["DoctorPrintName"].ToString();
                obj.PrintRegNO = ds.Tables[0].Rows[0]["PrintRegNO"].ToString();

                obj.TestID = dr["TestID"].ToString();
                obj.Testname = dr["TestName"].ToString();

                searclist.Add(obj);

            }
            return Json(searclist, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PatientBind(DateTime FromDate, DateTime Todate)
        {
            BL_Patient_Wise_Result_Entry BL_obj = new BL_Patient_Wise_Result_Entry();
            DataSet ds = new DataSet();
            List<Patient_Wise_Result_Entry> searclist = new List<Models.Laboratory.Patient_Wise_Result_Entry>();
         
            ds = BL_obj.SearchPatientLabResultEntryByNameID( "%", FromDate, Todate);


            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Patient_Wise_Result_Entry obj = new Patient_Wise_Result_Entry();

                obj.Patientname = dr["PatientName"].ToString();
                obj.PatientType = dr["PatientType"].ToString();
                obj.LabNo = dr["LabNo"].ToString();
                obj.LabDate = dr["LAB DATE"].ToString();

                searclist.Add(obj);

            }
            return Json(searclist, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RechkCompletebtn(int LabNo, string ParamiterID, string TestID2, string Formula, string newValue, string NLH1,string col)
        {
            Connect();
            string Message = "";
            if (newValue.ToString().Trim().Length > 0)
            {
                string[] NLH = NLH1.Split(',');

                BL_Patient_Wise_Result_Entry BL_obj = new BL_Patient_Wise_Result_Entry();
                Patient_Wise_Result_Entry obj = new Patient_Wise_Result_Entry();
                Calculation(LabNo, ParamiterID, TestID2, Formula, newValue, NLH1);
                if (TestID2 == "")
                {
                    TestID2 = "0";
                }
                dsCompare = BL_obj.GetPatientWiseResultforParameterValue(Convert.ToInt32(LabNo), Convert.ToInt32(dsPatientEntry.Tables[0].Rows[0]["TestID"]));

                if (dsPatientEntry.Tables.Count > 0)
                {
                    int i = 0;
                    foreach (DataRow dr in dsPatientEntry.Tables[0].Rows)
                    {
                        if (dsCompare.Tables[0].Rows[i]["ResultValue"].ToString().Trim() != dr["ResultValue"].ToString().Trim())
                        {
                            dr["CreationID"] = CreationID;
                        }
                        else
                        {
                            dr["CreationID"] = dsCompare.Tables[0].Rows[i]["CreationID"].ToString().Trim();
                        }

                        if (NLH[i] == "Blank")
                        {
                            dr["NLH"] = "";
                        }
                        else
                        {
                            dr["NLH"] = NLH[i];
                        }
                        dr["CompleteBy"] = CreationID;
                        dr["Mode"] = "Edit";
                        i++;
                    }
                    if (BL_obj.SaveRechk(dsPatientEntry,col))
                    {
                        Message = "Record Saved successfully";
                        //PatientWiseResultEntry();
                    }
                }
            }
            return Json(Message, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Completebtn(int LabNo, string ParamiterID, string TestID2, string Formula, string newValue, string NLH1,FooterData data,FormCollection obj,string [] arraydata)
        {
            //string[] Footersplit = WebUtility.HtmlEncode(Request.Form["footer"]).Replace("\r\n", "<br>").Split(',');
            
            Connect();
           
            string Message = "";
            if (newValue.ToString().Trim().Length > 0)
            {
                string[] NLH = NLH1.Split(',');
              //  string[] Footersplit = Footer.Split(',');

                BL_Patient_Wise_Result_Entry BL_obj = new BL_Patient_Wise_Result_Entry();
               // Patient_Wise_Result_Entry obj = new Patient_Wise_Result_Entry();
                Calculation(LabNo, ParamiterID, TestID2, Formula, newValue, NLH1);
                if (TestID2 == "")
                {
                    TestID2 = "0";
                }
                dsCompare = BL_obj.GetPatientWiseResultforParameterValue(Convert.ToInt32(LabNo), Convert.ToInt32(dsPatientEntry.Tables[0].Rows[0]["TestID"]));

                if (dsPatientEntry.Tables.Count > 0)
                {
                    int i = 0;
                    foreach (DataRow dr in dsPatientEntry.Tables[0].Rows)
                    {
                        if (dsCompare.Tables[0].Rows[i]["ResultValue"].ToString().Trim() != dr["ResultValue"].ToString().Trim())
                        {
                            dr["CreationID"] = CreationID;
                        }
                        else
                        {
                            dr["CreationID"] = dsCompare.Tables[0].Rows[i]["CreationID"].ToString().Trim();
                        }

                        if (NLH[i] == "Blank")
                        {
                            dr["NLH"] = "";
                        }
                        else
                        {
                            dr["NLH"] = NLH[i];
                        }
                        //if (Footersplit[i] == "Blank")
                        //{
                        //    dr["Footer"] = "";
                        //}
                        //else
                        //{
                        //    dr["Footer"] =  WebUtility.HtmlEncode(Footersplit[i]);
                        //}

                        
                        dr["CompleteBy"] = CreationID;
                        dr["Mode"] = "Edit";
                        i++;
                    }
                    if (BL_obj.Save(dsPatientEntry, dsCompare,data.footer))
                    {
                        Message = "Record Saved successfully";
                        //PatientWiseResultEntry();
                    }
                }
            }
            return Json(Message, JsonRequestBehavior.AllowGet);
        }



        public ActionResult GetPatientLabWithParameterRecheck(int LabNo, int TestID)
        {
            List<Patient_Wise_Result_Entry> searclist = new List<Models.Laboratory.Patient_Wise_Result_Entry>();

            if (TestID > 0)
            {
                BL_Patient_Wise_Result_Entry BL_obj = new BL_Patient_Wise_Result_Entry();
                DataSet ds = new DataSet();
                DataSet dsPatientEntry = new DataSet();
                DataSet dsCompare = new DataSet();
                dsPatientEntry = BL_obj.GetPatientLabWithParameterRecheck( LabNo, TestID);
                dsCompare = BL_obj.GetPatientWiseResultforParameterValue(Convert.ToInt32(LabNo), Convert.ToInt32(TestID));
              
                     foreach (DataRow dr in dsPatientEntry.Tables[0].Rows)
                    {
                        Patient_Wise_Result_Entry obj = new Patient_Wise_Result_Entry();
                        //  dr["Formula"] = "5";
                        //obj.Formula = dr["Formula"].ToString();
                        obj.LabNo = dr["LabNo"].ToString();
                       // obj.FullName = dr["FullName"].ToString();
                        obj.NLH = dr["NLH"].ToString();
                       // obj.ParamiterName = dr["ParameterName"].ToString();
                        obj.Low = dr["ConvLow"].ToString();
                        obj.Hiegh = dr["ConvHigh"].ToString();
                        obj.ParamiterID = dr["ParameterID"].ToString();
                        obj.status = dr["Status"].ToString();
                        obj.TestID = dr["TestID"].ToString();
                        obj.ResultValue = dr["ResultValue"].ToString();
                        obj.Value1 = dr["Value1"].ToString();
                        obj.Value2 = dr["Value2"].ToString();
                        obj.Value3 = dr["Value3"].ToString();
                        searclist.Add(obj);

                    }




                


           
            }
            return Json(searclist, JsonRequestBehavior.AllowGet);
        }
    
    
    
    
    public ActionResult GetPatientWiseResultforParameterValue(int LabNo, int TestID)
        {
            List<Patient_Wise_Result_Entry> searclist = new List<Models.Laboratory.Patient_Wise_Result_Entry>();
      
            if (TestID > 0)
            {
                BL_Patient_Wise_Result_Entry BL_obj = new BL_Patient_Wise_Result_Entry();
                DataSet ds = new DataSet();
                DataSet dsPatientEntry = new DataSet();
                DataSet dsCompare = new DataSet();
                dsPatientEntry = BL_obj.GetPatientWiseResultforParameterValue(LabNo, TestID);
                dsCompare = BL_obj.GetPatientWiseResultforParameterValue(Convert.ToInt32(LabNo), Convert.ToInt32(TestID));

                if (dsPatientEntry.Tables[0].Rows.Count > 0)
                {
                if (dsPatientEntry.Tables[1].Rows[0]["TestStatus"].ToString().Trim() == "COMPLETED")
                {
                    Patient_Wise_Result_Entry objc = new Patient_Wise_Result_Entry();

                    //  ucPatientWiseResultEntry1.gbDetail.Enabled = true;
                    //ucPatientWiseResultEntry1.dgvDetails.DataSource = dsPatientEntry.Tables[2];
                    //    ucPatientWiseResultEntry1.btnComplete.BackColor = Color.Green;
                    objc.Color = "Green";
                    searclist.Add(objc);

                    foreach (DataRow dr in dsPatientEntry.Tables[2].Rows)
                    {
                        Patient_Wise_Result_Entry obj = new Patient_Wise_Result_Entry();
                        //  dr["Formula"] = "5";
                        obj.UserID = dr["UserID"].ToString();
                        obj.AuthoriserName = dr["AuthoriserName"].ToString();
                        obj.status = dr["Status"].ToString();
                        obj.Remark = dr["Remark"].ToString();
                        obj.LabNo = dr["LabNo"].ToString();
                        obj.TestID = dr["TestID"].ToString();
                        obj.TestName1 = dr["TestName"].ToString();


                        searclist.Add(obj);

                    }

                    foreach (DataRow dr in dsPatientEntry.Tables[0].Rows)
                    {
                        Patient_Wise_Result_Entry obj = new Patient_Wise_Result_Entry();
                        //  dr["Formula"] = "5";
                        obj.Formula = dr["Formula"].ToString();
                        obj.LabNo = dr["LabNo"].ToString();
                        obj.FullName = dr["FullName"].ToString();
                        obj.NLH = dr["NLH"].ToString();
                        obj.ParamiterName = dr["ParameterName"].ToString();
                        obj.Low = dr["ConvLow"].ToString();
                        obj.Hiegh = dr["ConvHigh"].ToString();
                        obj.ParamiterID = dr["ParameterID"].ToString();
                        obj.status = dr["Status"].ToString();
                        obj.TestID = dr["TestID"].ToString();
                        obj.ResultValue = dr["ResultValue"].ToString();
                        obj.Value1 = dr["Value1"].ToString();
                        obj.Value2 = dr["Value2"].ToString();
                        obj.Value3 = dr["Value3"].ToString();
                        obj.footer = dr["Footer"].ToString();

                        //  obj.fo
                        searclist.Add(obj);

                    }
                }
                else
                {
                    // ucPatientWiseResultEntry1.gbDetail.Enabled = false;
                    //   ucPatientWiseResultEntry1.btnComplete.BackColor = Color.Coral;
                    Patient_Wise_Result_Entry obj = new Patient_Wise_Result_Entry();

                    obj.Color = "Coral";
                    searclist.Add(obj);
                }



                    // ucPatientWiseResultEntry1.dgvResultEntry.DataSource = dsPatientEntry.Tables[0];
                    //ucPatientWiseResultEntry1.dgvDetails.DataSource = dsPatientEntry.Tables[2];

                    if (dsPatientEntry.Tables[1].Rows[0]["TestStatus"].ToString().Trim() != "COMPLETED")
                    {
                        foreach (DataRow dr in dsPatientEntry.Tables[2].Rows)
                        {
                            Patient_Wise_Result_Entry obj = new Patient_Wise_Result_Entry();
                            //  dr["Formula"] = "5";
                            obj.UserID = dr["UserID"].ToString();
                            obj.AuthoriserName = dr["AuthoriserName"].ToString();
                            obj.status = dr["Status"].ToString();
                            obj.Remark = dr["Remark"].ToString();
                            obj.LabNo = dr["LabNo"].ToString();
                            obj.TestID = dr["TestID"].ToString();
                            obj.TestName1 = dr["TestName"].ToString();

                            // obj.Color = "Green";
                            searclist.Add(obj);

                        }

                        foreach (DataRow dr in dsPatientEntry.Tables[0].Rows)
                        {
                            Patient_Wise_Result_Entry obj = new Patient_Wise_Result_Entry();
                            //  dr["Formula"] = "5";
                            obj.Formula = dr["Formula"].ToString();
                            obj.LabNo = dr["LabNo"].ToString();
                            obj.FullName = dr["FullName"].ToString();
                            obj.NLH = dr["NLH"].ToString();
                            obj.ParamiterName = dr["ParameterName"].ToString();
                            obj.Low = dr["ConvLow"].ToString();
                            obj.Hiegh = dr["ConvHigh"].ToString();
                            obj.ParamiterID = dr["ParameterID"].ToString();
                            obj.status = dr["Status"].ToString();
                            obj.TestID = dr["TestID"].ToString();
                            obj.ResultValue = dr["ResultValue"].ToString();
                            obj.Value1 = dr["Value1"].ToString();
                            obj.Value2 = dr["Value2"].ToString();
                            obj.Value3 = dr["Value3"].ToString();
                            obj.footer = dr["FooterParameter"].ToString();

                            //  obj.fo
                            searclist.Add(obj);

                        }
                    }
                    




                }


            }
            else
            {


                BL_Patient_Wise_Result_Entry BL_obj = new BL_Patient_Wise_Result_Entry();
                DataSet ds = new DataSet();
                DataSet dsCompare = new DataSet();
                DataSet dsPatientEntry = new DataSet();
            
                dsPatientEntry = BL_obj.GetPatientWiseResultforParameterValue(LabNo, TestID);
                dsCompare = BL_obj.GetPatientWiseResultforParameterValue(Convert.ToInt32(LabNo), Convert.ToInt32(TestID));


                foreach (DataRow dr in dsPatientEntry.Tables[0].Rows)
                {
                    Patient_Wise_Result_Entry obj = new Patient_Wise_Result_Entry();
                    //  dr["Formula"] = "5";
                    obj.Formula = dr["Formula"].ToString();
                    obj.LabNo = dr["LabNo"].ToString();
                    obj.FullName = dr["FullName"].ToString();
                    obj.NLH = dr["NLH"].ToString();
                    obj.ParamiterName = dr["ParameterName"].ToString();
                    obj.Low = dr["ConvLow"].ToString();
                    obj.Hiegh = dr["ConvHigh"].ToString();
                    obj.ParamiterID = dr["ParameterID"].ToString();
                    obj.status = dr["Status"].ToString();
                    obj.TestID = dr["TestID"].ToString();
                    obj.ResultValue = "";
                    searclist.Add(obj);

                }
            }
            return Json(searclist, JsonRequestBehavior.AllowGet);
        }
        List<Patient_Wise_Result_Entry> searclist = new List<Models.Laboratory.Patient_Wise_Result_Entry>();
        DataSet dsPatientEntry = new DataSet();
        DataSet dsCompare = new DataSet();
        public ActionResult Calculation(int LabNo, string ParamiterID, string TestID2, string Formula, string newValue, string NLH1)
            {
        
                MathParser();
            string FullFormula;
            try
            {
                BL_Patient_Wise_Result_Entry BL_obj = new BL_Patient_Wise_Result_Entry();
               
                string[] ParamiteID1 = ParamiterID.Split(',');
                string[] TestID1 = TestID2.Split(',');
                string[] Formula1 = Formula.Split(',');
                string[] newValue1 = newValue.Split(',');
            //    string[] footersplit = Footer.Split(',');
                // string[] NLH = NLH1.Split(',');
                dsPatientEntry = BL_obj.GetPatientWiseResultforParameterValue(LabNo, Convert.ToInt32(TestID1[0]));

                var a = dsPatientEntry.Tables[0].Rows.Count;
                int row = 0;
                foreach (DataRow dr in dsPatientEntry.Tables[0].Rows)
                {
                    if (ParamiteID1[row] != "")
                    {
                        dr["ParameterID"] = ParamiteID1[row];
                        dr["TestID"] = TestID1[0];
                        if (Formula1[row].ToString() == "Blank")
                        {
                            dr["Formula"] = "";
                        }
                        else
                        {
                            dr["Formula"] = Formula1[row];
                        }

                        if (newValue1[row] == "Blank")
                        {
                            dr["ResultValue"] = "";
                        }
                        else
                        {
                            dr["ResultValue"] = newValue1[row];
                        }

                        //if (footersplit[row] == "Blank")
                        //{
                        //    dr["Footer"] = "";
                        //}
                        //else
                        //{
                        //    dr["Footer"] = footersplit[row];
                        //}

                        row++;
                    }
                }

                foreach (DataRow dr in dsPatientEntry.Tables[0].Rows)
                {
                    FullFormula = dr["Formula"].ToString().Trim();
                    int ParameterID = Convert.ToInt32(dr["ParameterID"].ToString().Trim());
                    int TestID = Convert.ToInt32(dr["TestID"].ToString().Trim());
                    if (FullFormula.Trim().Length > 0)
                    {
                        // dr["ResultValue"] = Math.Round(Convert.ToDecimal(Calculate(FormulaFullName(ParameterID, FullFormula, TestID)).ToString()), 2);
                        dr["ResultValue"] = Math.Round(Convert.ToDecimal(Calculate(FormulaFullName(ParameterID, FullFormula, TestID).ToString())), 2);
                        //  dr["ResultValue"] = Math.Round(Convert.ToDecimal(Evaluate(FormulaFullName(ParameterID, FullFormula, TestID)).ToString()), 2);
                    }
                }

            }


            catch (Exception ex)
            {
                //ExceptionManager.Publish(ex);
                // ShowMessage(ex.Message, MessageTypes.ErrorMessage);
            }
            foreach (DataRow dr in dsPatientEntry.Tables[0].Rows)
            {
                Patient_Wise_Result_Entry obj = new Patient_Wise_Result_Entry();
                //  dr["Formula"] = "5";
                obj.Formula = dr["Formula"].ToString();
                obj.LabNo = dr["LabNo"].ToString();
                obj.FullName = dr["FullName"].ToString();
                obj.NLH = dr["NLH"].ToString();
                obj.ParamiterName = dr["ParameterName"].ToString();
                obj.Low = dr["ConvLow"].ToString();
                obj.Hiegh = dr["ConvHigh"].ToString();
                obj.ParamiterID = dr["ParameterID"].ToString();
                obj.status = dr["Status"].ToString();
                obj.TestID = dr["TestID"].ToString();
                obj.ResultValue = dr["ResultValue"].ToString();
                searclist.Add(obj);

            }

            return Json(searclist, JsonRequestBehavior.AllowGet);
        }
        public string FormulaFullName(int ParameterID, string FullFormula, int TestID)
        {
         
            BL_Patient_Wise_Result_Entry OBJ_bl = new BL_Patient_Wise_Result_Entry();
            DataSet dsFormula = new DataSet();
            dsFormula = OBJ_bl.GetParameterOnlyFormula( ParameterID);

            foreach (DataRow dr in dsFormula.Tables[0].Rows)
            {
                if (dr["FormulaParameterID"].ToString().Trim().Length > 0)
                {

                    DataView dv = new DataView(dsPatientEntry.Tables[0], "ParameterID = " + dr["FormulaParameterID"].ToString() + " and TestID = " + TestID + " ", "", DataViewRowState.CurrentRows);

                    if (dv.Count != 0)
                    {
                        string Value = dv[0]["ResultValue"].ToString().Trim();
                        if (Value == "null" || Value == "")
                        {
                            Value = "";
                        }
                        if (Value.Length == 0)
                        {
                            // DataView dv1 = new DataView(dsPatientEntry.Tables[0], "ParameterID = " + dv[0]["ParameterID"].ToString() + " and TestID = " + TestID + " ", "", DataViewRowState.CurrentRows);
                            string NewFormula = OBJ_bl.GetParameterOnlyFormulaParameter(Convert.ToInt32(dr["FormulaParameterID"].ToString())).Tables[0].Rows[0]["FormulaWithShortName"].ToString();

                            Value = Convert.ToString(Math.Round(Calculate(FormulaFullName(Convert.ToInt32(dv[0]["ParameterID"].ToString()), NewFormula, TestID)), 2));

                            //Value = Convert.ToString(Evaluate(FormulaFullName(Convert.ToInt32(dv[0]["ParameterID"].ToString()), NewFormula, TestID)));

                        }
                        if (Value.Length > 0)
                        {
                            FullFormula = FullFormula.Replace("" + dr["Formula"] + "", "" + Value + "");

                        }
                        else
                        {

                            throw new Exception("Please Insert Result Value.");
                            //  MessageBox.Show("Please Insert Result Value.");
                            //  break;
                        }
                    }

                }

            }
            return FullFormula;
        }
        public decimal Calculate(string Formula)//111111111111111
        {
            try
            {
                string[] arr = Formula.Split("/+-*()^".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (KeyValuePair<Microsoft.Office.Interop.Excel.Parameters, decimal> de in _Parameters)
                {
                    foreach (string s in arr)
                    {
                        if (s != de.Key.ToString() && s.EndsWith(de.Key.ToString()))
                        {
                            Formula = Formula.Replace(s, (Convert.ToDecimal(s.Replace(de.Key.ToString(), "")) * de.Value).ToString());
                        }
                    }
                    Formula = Formula.Replace(de.Key.ToString(), de.Value.ToString());
                }
                while (Formula.LastIndexOf("(") > -1)
                {
                    int lastOpenPhrantesisIndex = Formula.LastIndexOf("(");
                    int firstClosePhrantesisIndexAfterLastOpened = Formula.IndexOf(")", lastOpenPhrantesisIndex);
                    decimal result = ProcessOperation(Formula.Substring(lastOpenPhrantesisIndex + 1, firstClosePhrantesisIndexAfterLastOpened - lastOpenPhrantesisIndex - 1));
                    bool AppendAsterix = false;
                    if (lastOpenPhrantesisIndex > 0)
                    {
                        if (Formula.Substring(lastOpenPhrantesisIndex - 1, 1) != "(" && !OperationOrder.Contains(Formula.Substring(lastOpenPhrantesisIndex - 1, 1)))
                        {
                            AppendAsterix = true;
                        }
                    }

                    Formula = Formula.Substring(0, lastOpenPhrantesisIndex) + (AppendAsterix ? "*" : "") + result.ToString() + Formula.Substring(firstClosePhrantesisIndexAfterLastOpened + 1);

                }
                return ProcessOperation(Formula);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Occured While Calculating. Check Syntax", ex);
            }
        }

        private decimal CalculateByOperator(decimal number1, decimal number2, string op)//3333333333333
        {
            if (op == "/")
            {
                return number1 / number2;
            }
            else if (op == "*")
            {
                return number1 * number2;
            }
            else if (op == "-")
            {
                return number1 - number2;
            }
            else if (op == "+")
            {
                return number1 + number2;
            }
            else if (op == "^")
            {
                return Convert.ToDecimal(Math.Pow(Convert.ToDouble(number1), Convert.ToDouble(number2)));
            }
            else
            {
                return 0;
            }


        }


        private decimal ProcessOperation(string operation)//222222222222222
        {
            ArrayList arr = new ArrayList();
            string s = "";
            for (int i = 0; i < operation.Length; i++)
            {
                string currentCharacter = operation.Substring(i, 1);
                if (OperationOrder.IndexOf(currentCharacter) > -1)
                {
                    if (s != "")
                    {
                        arr.Add(s);
                    }
                    arr.Add(currentCharacter);
                    s = "";
                }
                else
                {
                    s += currentCharacter;
                }
            }
            arr.Add(s);
            s = "";
            foreach (string op in OperationOrder)
            {

                while (arr.IndexOf(op) > -1)
                {
                    int operatorIndex = arr.IndexOf(op);

                    if (arr.Count > 2)
                    {
                        if (operatorIndex > 0)
                        {
                            decimal digitBeforeOperator = Convert.ToDecimal(arr[operatorIndex - 1]);

                            decimal digitAfterOperator = 0;
                            if (arr[operatorIndex + 1].ToString() == "-")
                            {
                                arr.RemoveAt(operatorIndex + 1);
                                digitAfterOperator = Convert.ToDecimal(arr[operatorIndex + 1]) * -1;
                            }
                            else
                            {
                                digitAfterOperator = Convert.ToDecimal(arr[operatorIndex + 1]);
                            }
                            arr[operatorIndex] = CalculateByOperator(digitBeforeOperator, digitAfterOperator, op);
                            arr.RemoveAt(operatorIndex - 1);
                            arr.RemoveAt(operatorIndex);
                        }
                    }
                    else
                    {
                        arr.RemoveAt(operatorIndex);
                    }
                }

            }
            return Convert.ToDecimal(arr[0]);
        }
        private List<String> OperationOrder = new List<string>();
        private Dictionary<Microsoft.Office.Interop.Excel.Parameters, decimal> _Parameters = new Dictionary<Microsoft.Office.Interop.Excel.Parameters, decimal>();
        public void MathParser()
        {
            OperationOrder.Add("/");
            OperationOrder.Add("*");
            OperationOrder.Add("-");
            OperationOrder.Add("+");
            OperationOrder.Add("^");
        }

    // Aurthorise

        public ActionResult ValidateUser(string users, string password, string LabNo, string TestID)
        {
            Patient_Wise_Result_Entry obj = new Patient_Wise_Result_Entry();
            List<Patient_Wise_Result_Entry> listAdd = new List<Patient_Wise_Result_Entry>();
            int AuthorzerID = 0;
            BL_Patient_Wise_Result_Entry OBJ_bl = new BL_Patient_Wise_Result_Entry();
            DataSet dsUser = OBJ_bl.ValidateUser(users, password);
          
            if (dsUser.Tables[0].Rows[0]["UserValid"].ToString() == "3")
            {
                obj.Message = "Invalid Username. Please try again";


            }
            else if (dsUser.Tables[0].Rows[0]["UserValid"].ToString() == "0")
            {
                obj.Message = "Invalid Password. Please try again";

            }
            else
            {
                obj.AuthorzerID = dsUser.Tables[0].Rows[0]["UserID"].ToString();
                DataSet dsAuthorzer = OBJ_bl.GetPatientLabForAuthorizer( Convert.ToInt32(obj.AuthorzerID), Convert.ToInt32(LabNo), Convert.ToInt32(TestID));
                if (dsAuthorzer.Tables[0].Rows.Count > 0)
                {
                    obj.AuthorzeName = dsAuthorzer.Tables[0].Rows[0]["AuthoriserName"].ToString();
                    if (dsAuthorzer.Tables[1].Rows.Count > 0)
                    {
                        obj.cmbStatus = dsAuthorzer.Tables[1].Rows[0]["Status"].ToString();
                        obj.txtRemark = dsAuthorzer.Tables[1].Rows[0]["Remark"].ToString();
                        obj.btnsaveEdit = "Edit";
                    }
                    else
                    {
                        obj.btnsaveEdit = "Save";
                    }

                }
                else
                {
                    obj.Message="You are not Authoriser.";
                }
            }
            listAdd.Add(obj);
            return Json(listAdd, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IUPatientLabAuthorization(string AuthorzerID, string AuthorzeName, string stauts, string Remark, string LabNo, string TestID, string TestName, string Mode)
        {
            Patient_Wise_Result_Entry obj = new Patient_Wise_Result_Entry();
            List<Patient_Wise_Result_Entry> listAdd = new List<Patient_Wise_Result_Entry>();
           
            BL_Patient_Wise_Result_Entry OBJ_bl = new BL_Patient_Wise_Result_Entry();
            if(Mode=="Save")
            {
                Mode = "Add";
            }
            else
            {
                Mode = "Edit";
            }
            bool chk = OBJ_bl.IUPatientLabAuthorization(AuthorzerID, AuthorzeName, stauts, Remark, LabNo, TestID, TestName, Mode);
        if(chk)
        {
            obj.Message = "You are  Save Authoriser .";
        }
            else
        {
            obj.Message = "You are not Save Authoriser .";
        }
        listAdd.Add(obj);
            return Json(listAdd, JsonRequestBehavior.AllowGet);

        }

	}
}