using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using KeystoneProject.Models.Patient;
using KeystoneProject.Models.Master;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using System.Net;

namespace KeystoneProject.Buisness_Logic.Master
{
    public class BL_Template
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

        private SqlConnection con;

        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public bool Save(Template objmodel)
        {
            Connect();
            try
            {

                //  string[] Investigation = objmodel.Investigations.Split(',');

                string Newid = Convert.ToString(objmodel.TemplateID);

                SqlCommand cmd1 = new SqlCommand("IUTemplate", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd1.Parameters.AddWithValue("@LocationID", LocationID);

                if (objmodel.TemplateID != "" && objmodel.TemplateID != null)
                {
                    cmd1.Parameters.AddWithValue("@TemplateID", objmodel.TemplateID);
                }
                else
                {
                    cmd1.Parameters.AddWithValue("@TemplateID", 0);
                    cmd1.Parameters["@TemplateID"].Direction = ParameterDirection.Output;
                }
                cmd1.Parameters.AddWithValue("@TemplateName", objmodel.TemplateName);
                cmd1.Parameters.AddWithValue("@ChiefComplaint", objmodel.ChiefComplaint);
                cmd1.Parameters.AddWithValue("@Allergies", objmodel.Allergies);
                cmd1.Parameters.AddWithValue("@ClinicalFindings", objmodel.ClinicalFinding);
                cmd1.Parameters.AddWithValue("@ProvisionalDiagonosis", objmodel.ProvitionalDiagnosis);
                cmd1.Parameters.AddWithValue("@Investigations", objmodel.Investigation);
                cmd1.Parameters.AddWithValue("@TreatmentAdvice", objmodel.TreatmentAdvice); 
                cmd1.Parameters.AddWithValue("@CreationID", UserID);
                if (objmodel.TemplateID != "" && objmodel.TemplateID != null)
                {
                    cmd1.Parameters.AddWithValue("@Mode", "Edit");
                }
                else
                {
                    cmd1.Parameters.AddWithValue("@Mode", "Add");
                }

                con.Open();
                int i = cmd1.ExecuteNonQuery();

                con.Close();


                objmodel.TemplateID = Convert.ToString(cmd1.Parameters["@TemplateID"].Value);

                string[] Medicines = objmodel.Medicines.Split(',');
                Medicines = Medicines.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                string[] Strength = objmodel.Strength.Split(',');
                string[] Frequency = objmodel.Frequency.Split(',');
                string[] Days = objmodel.Days.Split(',');
                string[] Instruction = objmodel.Instruction.Split(',');
                string[] MedicinesID = objmodel.MedicineLibraryID.Split(',');




                for (int k = 0; k < Medicines.Length; k++)
                {
                    if (Medicines[k].ToString() != "")
                    {
                        #region IUTemplateMedicineDetails

                        SqlCommand cmd = new SqlCommand("IUTemplateMedicineDetails", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmd.Parameters.AddWithValue("@LocationID", LocationID);
                        cmd.Parameters.AddWithValue("@TemplateID", objmodel.TemplateID);
                        cmd.Parameters.AddWithValue("@TemplateMedicineDetailsID", 0);
                        cmd.Parameters["@TemplateMedicineDetailsID"].Direction = ParameterDirection.Output; 
                        
                        cmd.Parameters.AddWithValue("@MedicineLibraryID", MedicinesID[k]);

                        cmd.Parameters.AddWithValue("@Medicines", Medicines[k]);
                        cmd.Parameters.AddWithValue("@Strength", Strength[k]);
                        cmd.Parameters.AddWithValue("@Frequency", Frequency[k]);
                        cmd.Parameters.AddWithValue("@Instruction", Instruction[k]);
                        cmd.Parameters.AddWithValue("@Days", Days[k]);
                        cmd.Parameters.AddWithValue("@CreationID", UserID);
                        cmd.Parameters.AddWithValue("@Mode", "Add");
                        con.Open();
                        int j = cmd.ExecuteNonQuery();
                        con.Close();

                        #endregion
                    }
                }


            }
            catch (Exception ex)
            {

            }
            return true;
        }

        public DataSet GetAllOldTemplate( int HospitalID, int LocationID)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllOldTemplate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter();
                ad.SelectCommand = cmd;
                ad.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            { }
            return ds;
        }

        public DataSet GetOLDTemplate(string OldTemplateID, int HospitalID, int LocationID)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetOLDTemplateDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@TemplateID", OldTemplateID);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Close();
                ad.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            { }
            return ds;
        }

        public DataSet GetTemplateMedicine(string TemplateID, int HospitalID, int LocationID)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetTemplateMedicine", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@TemplateID", TemplateID);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Close();
                ad.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            { }
            return ds;
        }
    }
}