using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using KeystoneProject.Models.Master;

namespace KeystoneProject.Buisness_Logic.Master
{
    public class BL_MedicinePackage
    {

        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<MedicinePackage> MedicinesList = new List<MedicinePackage>();
        List<MedicinePackage> PackagesList = new List<MedicinePackage>();
        List<MedicinePackage> PackagesMedList = new List<MedicinePackage>();

        private SqlConnection con;
        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public string Delete(int PackagesID)
        {
            string Table = string.Empty;
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("DeletePackages", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PackagesID", PackagesID);
               

                con.Open();
                int i = cmd.ExecuteNonQuery();

                con.Close();
                if (i > 0)
                {
                    return Table;
                }
                else
                {
                    return Table;
                }
            }
            catch (Exception)
            {
                return Table;
            }
        }
        public List<MedicinePackage> GetMedicinePackage(int PackagesID)
        {
            Connect();
            // List<Department> dept = new List<Department>();

            SqlCommand cmd = new SqlCommand("GetMedicinePackage", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PackagesID", PackagesID);
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                PackagesMedList.Add(
                    new MedicinePackage
                    {
                        PackagesID = Convert.ToInt32(dr["PackagesID"]),
                        Package = Convert.ToString(dr["PackagesName"]),
                        Medicines = Convert.ToString(dr["Medicines"]),
                        MedicinesID = Convert.ToString(dr["MedicineID"]),
                    });
            }
            return PackagesMedList;
        }

        public bool Save(MedicinePackage obj)
        {
            Connect();
            try
            {
                #region IUMedicinePackage

                SqlCommand cmd = new SqlCommand("IUPackages", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PackagesID", 0);
                cmd.Parameters["@PackagesID"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@PackagesName", obj.Package); 
                cmd.Parameters.AddWithValue("@CreationID", UserID);
                cmd.Parameters.AddWithValue("@Mode", "Add");
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                #endregion

                int PackagesID = Convert.ToInt32(cmd.Parameters["@PackagesID"].Value.ToString());


                if (PackagesID > 0)
                {
                    if (obj.Package != null)
                    {
                        string[] Medicines = obj.Medicines.Split(',');
                        string[] MedicinesID = obj.MedicinesID.Split(',');
                        for (int k = 0; k < Medicines.Length; k++)
                        {
                            if (Medicines[k].ToString() != "")
                            {
                                #region IUPackagesDetails

                                SqlCommand cmd1 = new SqlCommand("IUPackagesDetail", con);
                                cmd1.CommandType = CommandType.StoredProcedure;
                                cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                                cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                                cmd1.Parameters.AddWithValue("@PackagesID", PackagesID);
                                cmd1.Parameters.AddWithValue("@PackagesDetailsID", 0);
                                cmd1.Parameters["@PackagesDetailsID"].Direction = ParameterDirection.Output;
                                cmd1.Parameters.AddWithValue("@PackagesName", obj.Package);
                                cmd1.Parameters.AddWithValue("@MedicinesID", MedicinesID[k]); 
                                cmd1.Parameters.AddWithValue("@Medicines", Medicines[k]); 
                                cmd1.Parameters.AddWithValue("@CreationID", UserID);
                                cmd1.Parameters.AddWithValue("@Mode", "Add");
                                con.Open();
                                int j = cmd1.ExecuteNonQuery();
                                con.Close();

                                #endregion
                            }
                        }
                    }

                    return true;

                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Edit(MedicinePackage obj)
        {
            Connect();
            try
            {
                #region IUMedicinePackage

                SqlCommand cmd = new SqlCommand("IUPackages", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PackagesID", obj.PackagesID);

                cmd.Parameters.AddWithValue("@PackagesName", obj.Package);
              
                cmd.Parameters.AddWithValue("@CreationID", UserID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                #endregion

                int PackagesID = Convert.ToInt32(cmd.Parameters["@PackagesID"].Value.ToString());

                if (i > 0)
                {
                    if (obj.Medicines != null)
                    {
                        string[] Medicines = obj.Medicines.Split(',');
                        string[] MedicinesID = obj.MedicinesID.Split(',');

                        for (int k = 0; k < Medicines.Length; k++)
                        {
                            if (Medicines[k].ToString() != "")
                            {
                                #region IUPackagesDetail

                                SqlCommand cmd1 = new SqlCommand("IUPackagesDetail", con);
                                cmd1.CommandType = CommandType.StoredProcedure;
                                cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                                cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                                cmd1.Parameters.AddWithValue("@PackagesID", PackagesID);
                                cmd1.Parameters.AddWithValue("@PackagesDetailsID", 0);
                                cmd1.Parameters["@PackagesDetailsID"].Direction = ParameterDirection.Output;
                                cmd1.Parameters.AddWithValue("@PackagesName", obj.Package);
                                cmd1.Parameters.AddWithValue("@MedicinesID", MedicinesID[k]);
                                cmd1.Parameters.AddWithValue("@Medicines", Medicines[k]);
                                cmd1.Parameters.AddWithValue("@CreationID", UserID);
                                cmd1.Parameters.AddWithValue("@Mode", "Add");
                                con.Open();
                                int j = cmd1.ExecuteNonQuery();
                                con.Close();

                                #endregion
                            }
                        }
                    }

                    return true;

                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CheckMedicinePackages(int PackagesID, string PackagesName)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckPackagesMedicines", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PackagesID", PackagesID);
                cmd.Parameters.AddWithValue("@PackagesName", PackagesName);
                cmd.Parameters.Add("@NameExists", SqlDbType.NVarChar, 50);
                cmd.Parameters["@NameExists"].Direction = ParameterDirection.Output;
                con.Open();
                int i = cmd.ExecuteNonQuery();
                Table = (string)cmd.Parameters["@NameExists"].Value;
                if (Convert.ToInt32(Table) == 1)
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

        public List<MedicinePackage> SelectAllData()
        {
            Connect();


            SqlCommand cmd = new SqlCommand("GetAllMedicines", con);
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
                MedicinesList.Add(
                    new MedicinePackage
                    {
                        MedicineLibraryID = Convert.ToInt32(dr["MedicineLibraryID"]),
                        Medicines = Convert.ToString(dr["Medicines"]),
                       
                    });
            }
            return MedicinesList;
        }
        public DataSet ShowAllPackages()
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllPackages", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                con.Close();

                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }


        }
    }
}