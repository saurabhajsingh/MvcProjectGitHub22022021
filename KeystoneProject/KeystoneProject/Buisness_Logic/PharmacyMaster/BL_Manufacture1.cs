using System;
using KeystoneProject.Models.PharmacyMaster;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using KeystoneProject.Controllers.PharmacyMaster;

namespace KeystoneProject.Buisness_Logic.PharmacyMaster
{
    public class BL_Manufacture1
    {
       
            int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
            int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
            int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
            private SqlConnection con;

            private void connect()
            {
                string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
                con = new SqlConnection(constring);
            }

            public List<Manufacture> SelectAllData()
            {
                connect();
                List<Manufacture> Manufacturelist = new List<Manufacture>();

                SqlCommand cmd = new SqlCommand("GetAllManufacture", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                SqlDataAdapter sd = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                con.Open();
                sd.Fill(dt);
                con.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    Manufacturelist.Add(
                       new Manufacture
                       {
                           ManufactureID = Convert.ToInt32(dr["ManufactureID"]),
                           ManufactureName = Convert.ToString(dr["ManufactureName"]),

                       });
                }
                return Manufacturelist;

            }
            public bool CheckManufacture(int ManufactureID, string ManufactureName)
            {
                connect();
                string table;
                bool flag;
                try
                {
                    SqlCommand cmd = new SqlCommand("CheckManufacture", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd.Parameters.AddWithValue("@LocationID", LocationID);

                    cmd.Parameters.AddWithValue("@ManufactureID", ManufactureID);
                    cmd.Parameters.AddWithValue("@ManufactureName", ManufactureName.ToUpper());
                    cmd.Parameters.Add("@NameExists", SqlDbType.NVarChar, 50);
                    cmd.Parameters["@NameExists"].Direction = ParameterDirection.Output;
                    con.Open();
                    int i = cmd.ExecuteNonQuery();

                    table = (string)cmd.Parameters["@NameExists"].Value;
                    if (Convert.ToInt32(table) == 1)
                    {
                        flag = false;
                    }
                    else
                    {
                        flag = true;
                    }
                    return flag;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        public List<Manufacture> GETtManufacture(string ManufactureID)
        {
            connect();
            List<Manufacture> Manufacturelist = new List<Manufacture>();

            SqlCommand cmd = new SqlCommand("GetManufacture", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
            cmd.Parameters.Add(new SqlParameter("@LocationID", LocationID));
            cmd.Parameters.Add(new SqlParameter("@ManufactureID ", ManufactureID));
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                Manufacturelist.Add(
                      new Manufacture
                      {
                          ManufactureName = (dr["ManufactureName"].ToString()),
                          ManufactureID = Convert.ToInt32(dr["ManufactureID"]),


                      });
            }
            return Manufacturelist;

        }
        public bool DeleteManufacture(int ManufactureID)
            {
                try
                {

                    connect();
                    SqlParameter[] aParams = new SqlParameter[3];
                    aParams[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                    aParams[0].Value = HospitalID;
                    aParams[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                    aParams[1].Value = LocationID;
                    aParams[2] = new SqlParameter("@ManufactureID", SqlDbType.Int);
                    aParams[2].Value = ManufactureID;
                    SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "DeleteManufacture", aParams);
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

            public bool Save(Manufacture obj)
            {
                connect();
                SqlCommand cmd = new SqlCommand("[IUManufacture]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                if (obj.ManufactureID == 0)
                {
                    cmd.Parameters.AddWithValue("@ManufactureID", 0);
                    cmd.Parameters.AddWithValue("@Mode", "Add");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ManufactureID", obj.ManufactureID);
                    cmd.Parameters.AddWithValue("@Mode", "Edit");
                }
                cmd.Parameters.AddWithValue("@ManufactureName", obj.ManufactureName);

                cmd.Parameters.AddWithValue("@CreationID", UserID);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    
}