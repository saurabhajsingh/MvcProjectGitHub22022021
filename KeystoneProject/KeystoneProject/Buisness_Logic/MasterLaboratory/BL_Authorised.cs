using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using KeystoneProject.Controllers.MasterLaboratory;
using KeystoneProject.Models.MasterLaboratory;

namespace KeystoneProject.Buisness_Logic.MasterLaboratory
{
    public class BL_Authorised
    {
        private SqlConnection con;
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);


        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
        }

        public bool Save(Authorised obj)
        {

            Connect();
            con.Open();

                SqlCommand cmd = new SqlCommand("IUAuthoriser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);


                if (obj.AuthoriserID == "0")
                {
                    cmd.Parameters.AddWithValue("@AuthoriserID", 0);
                    cmd.Parameters["@AuthoriserID"].Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Mode", "Add");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@AuthoriserID", obj.AuthoriserID);
                    cmd.Parameters.AddWithValue("@Mode", "Edit");
                }
                cmd.Parameters.AddWithValue("@UserID", obj.UserId);
                cmd.Parameters.AddWithValue("@AuthoriserName", obj.AuthoriserName.ToUpper());
                cmd.Parameters.AddWithValue("@MobileNo", obj.MobileNo);
                cmd.Parameters.AddWithValue("@SignatureImg",obj.Signature);
                cmd.Parameters.AddWithValue("@FileName","" );
                cmd.Parameters.AddWithValue("@Remark", obj.Remark);
                
                cmd.Parameters.AddWithValue("@CreationID", UserID);
                //  cmd.Parameters.AddWithValue("@Mode", "Add");

                int row = cmd.ExecuteNonQuery();
                //obj.AuthoriserID = cmd.Parameters["@AuthoriserID"].ToString();
                // obj.LabName = cmd.Parameters["@labName"].ToString();
                con.Close();

            
            
            return true;
        }

        public DataSet GetAuthorised(string AuthorisedName)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select  UserID,FullName from  Users where FullName  like  ''+@AuthorisedName+'%'   and RowStatus=0  order by  FullName asc", con);
            cmd.Parameters.AddWithValue("@AuthorisedName", AuthorisedName);
            //and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet GeteditAuthorised(int AuthoriserID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("GetAuthoriser", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@AuthoriserID", AuthoriserID);

                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Open();
                ad.Fill(ds);
                con.Close();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return ds;
        }
        public DataSet GetAllAuthoriser()
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("GetAllAuthoriser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Open();
                ad.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return ds;

        }
        #region Delete
        public bool DeleteAuthoriser(int AuthoriserID)
        {
            try
            {
                Connect();
                SqlParameter[] aParams = new SqlParameter[3];
                aParams[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                aParams[0].Value = HospitalID;
                aParams[1] = new SqlParameter("@AuthoriserID", SqlDbType.Int);
                aParams[1].Value = AuthoriserID;
                aParams[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                aParams[2].Value = LocationID;
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "DeleteAuthoriser", aParams);
            }
            catch (DBConcurrencyException exp)
            {
                ExceptionManager.Publish(exp);
                exp.Data.Add("returnValue", "-1");
                throw exp;
            }
            catch (Exception ex)
            {
                ex.Data.Add("returnValue", "0");
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return true;

        }
        #endregion


    }
}