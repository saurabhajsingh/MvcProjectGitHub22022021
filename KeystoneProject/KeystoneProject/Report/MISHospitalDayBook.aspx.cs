using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
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

namespace KeystoneProject.Report
{
    public partial class MISHospitalDayBook : System.Web.UI.Page
    {
        ReportDocument rd = new ReportDocument();
        ReportDocument rd1 = new ReportDocument();
        ReportDocument rd2 = new ReportDocument();

        protected void Page_Load(object sender, EventArgs e)
        {

           

            int HospitalID = Convert.ToInt32(Session["HospitalID"].ToString());
            int LocationID = Convert.ToInt32(Session["LocationID"].ToString());
            string date =    Convert.ToDateTime(Session["DateFrom"]).ToString();
            string date1 = Convert.ToDateTime(Session["Date"]).ToString();

            string Type1 = Session["Type1"].ToString();


            if (Type1 == "IPD")
            {
                SqlConnection con;
                string constring1 = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
                con = new SqlConnection(constring1);
                SqlCommand cmd = new SqlCommand("RptMISHospitalDayBookForIPD", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 500;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(date1));

                SqlDataAdapter da1 = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                da1.Fill(ds);

                List<byte[]> files = new List<byte[]>();


                if (ds.Tables[0].Rows.Count != 0)
                {


                    rd1.Load(Path.Combine(Server.MapPath("~/Report"), "RptMISHospitalDayBookForIPD.rpt"));
                    rd1.SetDataSource(ds);


                    rd1.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "RptMISHospitalDayBookForIPD.pdf");
                    Response.End();
                }


            }
            if (Type1 == "OPD")
            {
                SqlConnection con;
                string constring1 = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
                con = new SqlConnection(constring1);
                SqlCommand cmd = new SqlCommand("RptMISHospitalDayBookForOPD", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 500;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(date1));

                SqlDataAdapter da1 = new SqlDataAdapter(cmd);
                DataSet ds2 = new DataSet();

                da1.Fill(ds2);

                List<byte[]> files = new List<byte[]>();


                if (ds2.Tables[0].Rows.Count != 0)
                {


                    rd2.Load(Path.Combine(Server.MapPath("~/Report"), "RptMISHospitalDayBookForOPD.rpt"));
                    rd2.SetDataSource(ds2);


                    rd2.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "RptMISHospitalDayBookForOPD.pdf");
                    Response.End();
                }
            }   
            if (Type1 == "All")
            {
                SqlConnection con1;
                string constring1 = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
                con1 = new SqlConnection(constring1);
                SqlCommand cmd1 = new SqlCommand("RptMISHospitalDayBook", con1);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandTimeout = 500;
                cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                cmd1.Parameters.AddWithValue("@Date", Convert.ToDateTime(date1));

                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                DataSet ds1 = new DataSet();

                da1.Fill(ds1);

                List<byte[]> files = new List<byte[]>();


                if (ds1.Tables[0].Rows.Count != 0)
                {


                    rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptMISHospitalDayBook.rpt"));
                    rd.SetDataSource(ds1);


                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "RptMISHospitalDayBook.pdf");
                    Response.End();



                }

            }
           
           

       
        }
    }
}