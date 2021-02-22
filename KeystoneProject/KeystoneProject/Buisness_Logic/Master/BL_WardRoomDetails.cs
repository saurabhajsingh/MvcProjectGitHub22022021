using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using KeystoneProject.Buisness_Logic.Master;
using KeystoneProject.Models.Master;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;


namespace KeystoneProject.Buisness_Logic.Master
{
    public class BL_WardRoomDetails
    {

        List<WardRoomsDetails> WardList = new List<WardRoomsDetails>();
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        private SqlConnection con;
        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
            HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        }

        public List<WardRoomsDetails> SelectGetAllWard()
        {
            Connect();


            SqlCommand cmd = new SqlCommand("GetAllWardRooms", con);
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
                WardList.Add(
                    new WardRoomsDetails
                    {
                        RoomID = dr["RoomID"].ToString(),
                        WardID = Convert.ToInt32(dr["WardID"]),
                        WardName = (dr["WardName"]).ToString(),
                        RoomName = (dr["RoomName"]).ToString(),
                        TotalBed = Convert.ToInt32(dr["Total Bed"]).ToString(),
                        colorname = dr["WardColor"].ToString(),
                    });
            }
            return WardList;

        }

        public List<WardRoomsDetails> GetWardData(int WardID)
        {
            Connect();
            // List<Department> dept = new List<Department>();


            SqlCommand cmd = new SqlCommand("GetWard", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.Add(new SqlParameter("@WardID", WardID));
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                WardList.Add(
                    new WardRoomsDetails
                    {
                        WardID = Convert.ToInt32(dr["WardID"]),
                        WardName = Convert.ToString(dr["WardName"]),

                    });
            }
            return WardList;
        }

        public List<WardRoomsDetails> GetWardRooms(int RoomID)
        {
            Connect();
            // List<Department> dept = new List<Department>();

            SqlCommand cmd = new SqlCommand("GetWardRooms", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.Add(new SqlParameter("@RoomID", RoomID));
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                WardList.Add(
                    new WardRoomsDetails
                    {
                        WardID = Convert.ToInt32(dr["WardID"]),
                        RoomID = (dr["RoomID"]).ToString(),
                        RoomName = Convert.ToString(dr["RoomName"]),

                    });
            }
            return WardList;
        }


        public List<WardRoomsDetails> GetWardRoomsDetails(int RoomID)
        {
            Connect();
            // List<Department> dept = new List<Department>();

            SqlCommand cmd = new SqlCommand("GetWardRoomsDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.Add(new SqlParameter("@RoomID", RoomID));
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                WardList.Add(
                    new WardRoomsDetails
                    {
                        WardID = Convert.ToInt32(dr["WardID"]),
                        RoomID = (dr["RoomID"]).ToString(),
                        BedID = Convert.ToString(dr["BedID"]),
                        BedNo = Convert.ToString(dr["BedNo"]).Trim(),
                        BedStatus=Convert.ToBoolean(dr["BedStatus"])
                    });

            }
            return WardList;
        }

        public DataSet GetAllWard(string prefix)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("Select   WardID , WardName ,GeneralCharges,EmergencyCharges from Ward where  WardName like ''+@prefix+'%' and HospitalID = '" + HospitalID + "' and LocationID = '" + LocationID + "' and RowStatus= 0", con);
            cmd.Parameters.AddWithValue("@prefix", prefix);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

     
        public string DeleteWardRoomsDetails(int RoomID, int WardID)
        {
            try
            {
                Connect();
                SqlParameter[] aParams = new SqlParameter[5];
                aParams[0] = new SqlParameter("@RoomID", SqlDbType.Int);
                aParams[0].Value = RoomID;
                aParams[1] = new SqlParameter("@WardID", SqlDbType.Int);
                aParams[1].Value = WardID;
                aParams[2] = new SqlParameter("@TableName", SqlDbType.NVarChar, 50);
                aParams[2].Direction = ParameterDirection.Output;
                aParams[3] = new SqlParameter("@HospitalID", SqlDbType.NVarChar, 50);
                aParams[3].Value = HospitalID;
                aParams[4] = new SqlParameter("@LocationID", SqlDbType.NVarChar, 50);
                aParams[4].Value = LocationID;
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "DeleteWardRoomsDetails", aParams);
                return aParams[2].Value.ToString();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
        }

        public DataSet GetWardForServiceDetail(int HospitalID, int LocationID)
        {
            try
            {
                    Connect();
                    SqlParameter[] param = new SqlParameter[2];
                    param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                    param[0].Value = HospitalID;
                    param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                    param[1].Value = LocationID;
                    return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetWardForServiceDetail", param);
                
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
        }
        public DataSet SelectServiceChargesbyID(int ServiceID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("GetServices", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID); // i will pass zero to MobileID beacause its Primary .
            cmd.Parameters.AddWithValue("@LocationID", LocationID); // i will pass zero to MobileID beacause its Primary .

            cmd.Parameters.AddWithValue("@ServiceID", ServiceID); // i will pass zero to MobileID beacause its Primary .


            DataSet ds = new DataSet();
            con.Open();

            SqlDataAdapter da = new SqlDataAdapter();

            da.SelectCommand = cmd;

            //  ds = new DataSet();

            da.Fill(ds);
            con.Close();
            return ds;

        }

        public DataSet GetAllWard1(int WardID)
        {
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@WardID", SqlDbType.Int);
                param[2].Value = WardID;
                return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetWard", param);

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

        }
        public DataSet GetWardForTestMasterDetailCharges(int HospitalID, int LocationID)
        {
            try
            {
                Connect();
                {
                    SqlParameter[] param = new SqlParameter[2];
                    param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                    param[0].Value = HospitalID;
                    param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                    param[1].Value = LocationID;
                    return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetWardForTestMasterDetailCharges", param);
                }
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
        }

        public DataSet GetServicesDetailsTPAWiseForWard(int HospitalID, int LocationID)
        {
            try
            {
                Connect();
                {
                    SqlParameter[] param = new SqlParameter[2];
                    param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                    param[0].Value = HospitalID;
                    param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                    param[1].Value = LocationID;
                    return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetServicesDetailsTPAWiseForWard", param);
                }
            }
            catch (DBConcurrencyException exp)
            {
                ExceptionManager.Publish(exp);
                exp.Data.Add("returnValue", "-1");
                throw exp;
            }

        }

           public DataSet GetTestMasterDetailsTPAWiseForWard(int HospitalID, int LocationID)
        {
            try
            {
               Connect();
                {
                    SqlParameter[] param = new SqlParameter[2];
                    param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                    param[0].Value = HospitalID;
                    param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                    param[1].Value = LocationID;
                    return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetTestMasterDetailsTPAWiseForWard", param);
                }
            }
            catch (DBConcurrencyException exp)
            {
                ExceptionManager.Publish(exp);
                exp.Data.Add("returnValue", "-1");
                throw exp;
            }

        }

        

        public bool save(WardRoomsDetails Objward)
        {
            bool flag = true;
            Connect();
            con.Open();
            int ServiceChargesID = 0;

            string Mode = "";
            SqlCommand cmd = new SqlCommand("IUWard", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            if (Objward.WardID == 0)
            {
                cmd.Parameters.AddWithValue("@WardID", 0);
                cmd.Parameters["@WardID"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@Mode", "Add");
                Mode = "Add";
            }
            else
            {
                cmd.Parameters.AddWithValue("@WardID", Objward.WardID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
                Mode = "Edit";
            }


            cmd.Parameters.AddWithValue("@WardName", Objward.WardName);
            cmd.Parameters.AddWithValue("@WardColor", Objward.colorname);
            cmd.Parameters.AddWithValue("@ReferenceCode", 1);
            cmd.Parameters.AddWithValue("@GeneralCharges", Objward.GeneralCharges);
            cmd.Parameters.AddWithValue("@EmergencyCharges", Objward.EmergencyCharges);
            cmd.Parameters.AddWithValue("@CreationID", UserID);

            int WardID = cmd.ExecuteNonQuery();
            Objward.WardID = Convert.ToInt32(cmd.Parameters["@WardID"].Value.ToString());

            
            if(Mode=="Add")
            { 
                
                    #region Add All Services TPA Wise Ward

                if (WardID > 0)
                    {
                       

                       
                        cmd = new SqlCommand("IUGetServicesForAllTpaandTest", con);
                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmd.Parameters.AddWithValue("@LocationID", LocationID);
                        cmd.Parameters.AddWithValue("@WardID", Objward.WardID);
                        cmd.Parameters.AddWithValue("@CreationID", UserID);

                        ServiceChargesID = cmd.ExecuteNonQuery();
                      
                    }
            }

                    #endregion




                        if (WardID > 0)
                        {
                           
                            Objward.WardID = Convert.ToInt32(cmd.Parameters["@WardID"].Value.ToString());
                            cmd = new SqlCommand("IUWardRooms", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmd.Parameters.AddWithValue("@LocationID", LocationID);
                            cmd.Parameters.AddWithValue("@WardID", Objward.WardID);

                            if (Objward.RoomID == null)
                            {
                                cmd.Parameters.AddWithValue("@RoomID", 0);
                                cmd.Parameters["@RoomID"].Direction = ParameterDirection.Output;
                                cmd.Parameters.AddWithValue("@Mode", "Add");
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@RoomID", Objward.RoomID);
                                cmd.Parameters.AddWithValue("@Mode", "Edit");
                            }
                            cmd.Parameters.AddWithValue("@RoomName", Objward.RoomName);
                            cmd.Parameters.AddWithValue("@ReferenceCode", Objward.ReferenceCode);
                            cmd.Parameters.AddWithValue("@CreationID", UserID);

                            int roomID = cmd.ExecuteNonQuery();
                           
                            if (roomID > 0)
                            {
                                int bedID = 0;
                               // string Mode = string.Empty;
                                #region -------------------Add Ward Room Details-------------------
                                Objward.RoomID = cmd.Parameters["@RoomID"].Value.ToString();

                                int length = 0;

                                string[] bedNo = Objward.BedNo.Split(',');
                                string[] bedstatus = Objward.BedStatus1.Split(',');
                                for (int i = 0; i < bedNo.Length; i++)
                                {
                                    
                                    cmd = new SqlCommand("IUWardRoomsDetails", con);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                                    cmd.Parameters.AddWithValue("@LocationID", LocationID);
                                    cmd.Parameters.AddWithValue("@RoomID", Objward.RoomID);
                                    cmd.Parameters.AddWithValue("@WardID", Objward.WardID);

                                    //if (length > i)
                                    //{
                                    //    cmd.Parameters.AddWithValue("@BedID",Objward.BedID);
                                    //    Objward.Mode = "Edit";
                                    //    cmd.Parameters.AddWithValue("@Mode", Objward.Mode);
                                    //}
                                    //else
                                    //{
                                    cmd.Parameters.AddWithValue("@BedID", 0);
                                    Objward.Mode = "Add";
                                    cmd.Parameters.AddWithValue("@Mode", Objward.Mode);
                                    //  }
                                    cmd.Parameters.AddWithValue("@BedNO", bedNo[i]);
                                    cmd.Parameters.AddWithValue("@GeneralCharges", Objward.GeneralCharges);
                                    cmd.Parameters.AddWithValue("@EmergencyCharges", Objward.EmergencyCharges);
                                    cmd.Parameters.AddWithValue("@BedStatus", Convert.ToBoolean(bedstatus[i]));
                                    cmd.Parameters.AddWithValue("@CreationID", UserID);

                                    bedID = cmd.ExecuteNonQuery();
                                   
                                }
                                con.Close();
                                #endregion-------------------------------

                                if (bedID > 0)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }

                            }
                        
                    
                
            }
            return true;


        }

            
        


        public bool CheckBedIDInIPDWardDetails(int BedID)
        {
            try
            {
                Connect();
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT BedID FROM PatientIPDWardDetails WHERE BedID=" + BedID, con);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        # region CheckWard

        public bool CheckWard(string WardName, int WardID)
        {

            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckWard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@WardID", WardID);
                cmd.Parameters.AddWithValue("@WardName", WardName.ToUpper());
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
        # endregion


        # region CheckWardRooms

        public bool CheckWardRooms(int WardID, int RoomID, string RoomName)
        {

            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckWardRooms", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@WardID", WardID);
                cmd.Parameters.AddWithValue("@RoomID", RoomID);
                cmd.Parameters.AddWithValue("@RoomName", RoomName.ToUpper());
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
        #endregion



    }

}


