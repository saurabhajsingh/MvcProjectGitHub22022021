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
    public class BL_MedicineLibrary
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<MedicineLibrary> MedicineLibraryList = new List<MedicineLibrary>();

        private SqlConnection con;
        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }


        public DataSet GetMedicines(string prefix)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("select  DrugID,DrugName from  Drug where DrugName like ''+@prefix+'%' and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + " order by  DrugName asc", con);
                cmd.Parameters.AddWithValue("@prefix", prefix);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            { }
            return ds;
        }

        public DataSet GetMedicineType(string prefix)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("select  MedicineTypeID,MedicineTypeName from  MedicineType where MedicineTypeName like ''+@prefix+'%' and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + " order by  MedicineTypeName asc", con);
                cmd.Parameters.AddWithValue("@prefix", prefix);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            { }
            return ds;
        }
        public DataSet GetUnit(string prefix)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("select  UnitID,UnitName from  Unit where UnitName like ''+@prefix+'%' and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + " order by  UnitName asc", con);
                cmd.Parameters.AddWithValue("@prefix", prefix);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            { }
            return ds;
        }
        public List<MedicineLibrary> SelectAllData()
        {
            Connect();


            SqlCommand cmd = new SqlCommand("GetAllMedicinesLibrary", con);
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
                MedicineLibraryList.Add(
                    new MedicineLibrary
                    {
                        MedicineLibraryID = Convert.ToInt32(dr["MedicineLibraryID"]),
                        Type = Convert.ToString(dr["Type"]),
                        Medicines = Convert.ToString(dr["Medicines"]),
                        txtdeteils = Convert.ToString(dr["Discription"]),
                        Unit = Convert.ToString(dr["Unit"]),
                        Time = Convert.ToString(dr["Time"]),
                        Instruction = Convert.ToString(dr["Instruction"]),
                        Frequency = Convert.ToString(dr["Frequency"]),
                        Days = Convert.ToString(dr["Days"]),
                        Strength = Convert.ToString(dr["Strength"]),
                        Quantity = Convert.ToString(dr["Quantity"]),
                        CompositionName = Convert.ToString(dr["CompositionName"]),
                       
                    });
            }
            return MedicineLibraryList;
        }


        public List<MedicineLibrary> GetMedicineLibrary(int MedicineLibraryID)
        {
            Connect();
            // List<Department> dept = new List<Department>();

            SqlCommand cmd = new SqlCommand("GetMedicineLibrary", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MedicineLibraryID", MedicineLibraryID));
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                MedicineLibraryList.Add(
                    new MedicineLibrary
                    {

                        //MedicineLibraryID = Convert.ToInt32(dr["MedicineLibraryID"]),
                        //Type = Convert.ToString(dr["Type"]),
                        //Medicines = Convert.ToString(dr["Medicines"]),
                        //Dosages = Convert.ToString(dr["Dosages"]),
                        //Unit = Convert.ToString(dr["Unit"]),
                        //Time = Convert.ToString(dr["Time"]),
                        //Instruction = Convert.ToString(dr["Instruction"]),
                        //Frequency = Convert.ToString(dr["frequency"]),
                        //Duration = Convert.ToString(dr["duration"]),
                        //Quantity = Convert.ToString(dr["Quantity"]),
                        //CompositionName = Convert.ToString(dr["CompositionName"]),
                        //Packages = Convert.ToString(dr["Packages"]),
                    });
            }
            return MedicineLibraryList;
        }



        public bool Save(MedicineLibrary obj)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("IUMedicineLibrary", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            if (obj.MedicineLibraryID == 0)
            {
                cmd.Parameters.AddWithValue("@MedicineLibraryID", 0);
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@MedicineLibraryID", obj.MedicineLibraryID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }
            cmd.Parameters.AddWithValue("@Medicines", obj.Medicines);



            if (obj.Type == null)
                cmd.Parameters.AddWithValue("@Type", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@Type", obj.Type);

           

            if (obj.Unit == null)
                cmd.Parameters.AddWithValue("@Unit", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@Unit", obj.Unit);

            if (obj.Time == null)
                cmd.Parameters.AddWithValue("@Time", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@Time", obj.Time);

            if (obj.Instruction == null)
                cmd.Parameters.AddWithValue("@Instruction", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@Instruction", obj.Instruction);

            if (obj.Frequency1 == null)
                cmd.Parameters.AddWithValue("@Frequency1", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@Frequency1", obj.Frequency1);


            if (obj.txtdeteils == null)
                cmd.Parameters.AddWithValue("@Frequency2", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@Frequency2", obj.txtdeteils);


            if (obj.Frequency3 == null)
                cmd.Parameters.AddWithValue("@Frequency3", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@Frequency3", obj.Frequency3);


            if (obj.Frequency4 == null)
                cmd.Parameters.AddWithValue("@Frequency4", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@Frequency4", obj.Frequency4);

            if (obj.Strength == null)
                cmd.Parameters.AddWithValue("@Strength", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@Strength", obj.Strength);

            if (obj.Days == null)
                cmd.Parameters.AddWithValue("@Days", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@Days", obj.Days);

            if (obj.Quantity == null)
                cmd.Parameters.AddWithValue("@Quantity", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@Quantity", obj.Quantity);

            if (obj.CompositionName == null)
                cmd.Parameters.AddWithValue("@CompositionName", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@CompositionName", obj.CompositionName);

          

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


       

         public DataSet GetPackages(string prefix)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("select PackagesID ,upper(Packages.PackagesName) as PackagesName from Packages where PackagesName like''+@prefix+'%' and RowStatus = 0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + " order by PackagesName asc", con);
                cmd.Parameters.AddWithValue("@prefix", prefix);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            { }
            return ds;
        }


        public bool CheckMedicineLibrary(int MedicineLibraryID, string Medicines)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckMedicineLibrary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@MedicineLibraryID", MedicineLibraryID);
                cmd.Parameters.AddWithValue("@Medicines", Medicines);
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
            catch (Exception)
            {
                return false;
            }
        }

        public bool CheckPackages(int PackagesID, string PackagesName)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckPackages", con);
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
            catch (Exception)
            {
                return false;
            }
        }


        public string Delete(int MedicineLibraryID)
        {
            string Table = string.Empty;
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("DeleteMedicineLibrary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MedicineLibraryID", MedicineLibraryID);
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

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
    }

}