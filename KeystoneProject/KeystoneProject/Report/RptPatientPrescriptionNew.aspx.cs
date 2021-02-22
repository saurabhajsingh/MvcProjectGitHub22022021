using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using KeystoneProject.Models;
using KeystoneProject.Models.Patient;
using CrystalDecisions.Shared;

namespace KeystoneProject.Report
{
    public partial class RptPatientPrescriptionNew : System.Web.UI.Page
    {
        ReportDocument rd = new ReportDocument();
        RptPatientPrescriptionNew1 rd1 = new RptPatientPrescriptionNew1();

        protected void Page_Load(object sender, EventArgs e)
        {
          
        }

       
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (this.rd != null)
            {
                this.rd.Close();
                this.rd.Dispose();
            }
        }

        protected void btnprint_Click(object sender, EventArgs e)
        {
            // int strHospital = Convert.ToInt32(Session["HospitalIDReport"].ToString());
            //  int strLocation = Convert.ToInt32(Session["LocationIDReport"].ToString());

            string lang = ddllang.SelectedValue;


            int strHospital = 1;
            int strLocation = 1;
            int PatientRegNo = Convert.ToInt32(Session["PatientRegNoReport"].ToString());
            int PatientPrescriptionNewID = Convert.ToInt32(Session["PatientPrescriptionNewIDReport"].ToString());
            int OPDIPDID = Convert.ToInt32(Session["PatientOPDIPDReport"].ToString());


            SqlConnection con;
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);

            #region language
            SqlCommand cmd1 = new SqlCommand("GetPatientPrescriptionLang", con);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@HospitalID", strHospital);
            cmd1.Parameters.AddWithValue("@LocationID", strLocation);
           
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            DataSet ds1 = new DataSet();
            //con.Open();
            da1.Fill(ds1);




            #endregion


            SqlCommand cmd = new SqlCommand("RptPatientPrescriptionNew", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", strHospital);
            cmd.Parameters.AddWithValue("@LocationID", strLocation);
            cmd.Parameters.AddWithValue("@PatientRegNo", PatientRegNo);
            cmd.Parameters.AddWithValue("@PatientPrescriptionNewID", PatientPrescriptionNewID);
            cmd.Parameters.AddWithValue("@OPDIPDID", OPDIPDID);


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            //con.Open();
            da.Fill(ds);


            if (ds.Tables[0].Rows.Count != 0)
            {



                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    string ChiefComplaint = WebUtility.HtmlDecode(dr["ChiefComplaint"].ToString());
                    string Allergies = WebUtility.HtmlDecode(dr["Allergies"].ToString());
                    string ClinicalFindings = WebUtility.HtmlDecode(dr["ClinicalFindings"].ToString());
                    string ProvisionalDiagonosis = WebUtility.HtmlDecode(dr["ProvisionalDiagonosis"].ToString());
                    string Investigations = WebUtility.HtmlDecode(dr["Investigations"].ToString());
                    string TreatmentAdvice = WebUtility.HtmlDecode(dr["TreatmentAdvice"].ToString());

                    dr["ChiefComplaint"] = ChiefComplaint.ToString();
                    dr["Allergies"] = Allergies.ToString();
                    dr["ClinicalFindings"] = ClinicalFindings.ToString();
                    dr["ProvisionalDiagonosis"] = ProvisionalDiagonosis.ToString();
                    dr["Investigations"] = Investigations.ToString();
                    dr["TreatmentAdvice"] = TreatmentAdvice.ToString();

                    ds.Tables[1].AcceptChanges();



                }


                #region language

                foreach(DataRow dr in ds.Tables[2].Rows)
                {
                    foreach(DataRow dr1 in ds1.Tables[0].Rows)
                    {
                        if(dr["Instruction"].ToString().ToUpper()==dr1["Text"].ToString())
                        {
                            if(lang=="Hindi")
                            {
                                string language = dr1["Hindi"].ToString();
                                dr["Instruction"] = language;
                                ds.Tables[2].AcceptChanges();
                            }
                            else if(lang=="Marathi")
                            {
                                string language = dr1["Marathi"].ToString();
                                dr["Instruction"] = language;
                                ds.Tables[2].AcceptChanges();
                            }
                            else if (lang == "Tamil")
                            {
                                string language = dr1["Tamil"].ToString();
                                dr["Instruction"] = language;
                                ds.Tables[2].AcceptChanges();
                            }
                            else if (lang == "Gujarati")
                            {
                                string language = dr1["Gujarati"].ToString();
                                dr["Instruction"] = language;
                                ds.Tables[2].AcceptChanges();
                            }
                            else if (lang == "Urdu")
                            {
                                string language = dr1["Urdu"].ToString();
                                dr["Instruction"] = language;
                                ds.Tables[2].AcceptChanges();
                            }
                        }
                    }
                }
                ds.Tables[2].AcceptChanges();
                #endregion
                ds.AcceptChanges();

                String str = "";
                try
                {
                    str = "1";
                    str = "2";
                  
                    //rd.SetDatabaseLogon("","", "SCANDENT-NEW\\SQLEXPRESS", "KeystoneMVCMain");
                    rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptPatientPrescriptionNew1.rpt"));
                    str = "3";

                    //  report context = new report();

                    rd.SetDataSource(ds);

   
                    //rd.Database.Tables["Table"].SetDataSource(ds.Tables[0]);
                    //rd.Database.Tables["Table1"].SetDataSource(ds.Tables[1]);
                    //rd.Database.Tables["Table2"].SetDataSource(ds.Tables[2]);
                    //rd.Database.Tables["Table3"].SetDataSource(ds.Tables[3]);


                    str = "4";

                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Crystal.pdf");
           //         Response.End();
                    //CrystalReportViewer1.ReportSource = rd;
                    str = "5";
                    // CrystalReportViewer1.DataBind();
                    str = "6";
                    //   CrystalReportViewer1.RefreshReport();
                    str = "7";

                }
                catch (Exception ex)
                {
                    Response.Write(str);
                    //throw;
                }

            }

        }

        
    }
}