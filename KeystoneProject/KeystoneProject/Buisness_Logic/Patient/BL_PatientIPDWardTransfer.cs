using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using KeystoneProject.Models.Patient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace KeystoneProject.Buisness_Logic.Patient
{
    public class BL_PatientIPDWardTransfer
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


        public bool IUPatientIPDWardTransfer(KeystoneProject.Models.Patient.PatientIPDWardTransfer obj)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();

                SqlCommand cmd = new SqlCommand("IUPatientIPDWardTransfer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PatientRegNO", obj.PatientRegNO);
                cmd.Parameters.AddWithValue("@PatientIPDNO", obj.PatientIPDNO);
                cmd.Parameters.AddWithValue("@WardID", obj.WardID);
                cmd.Parameters.AddWithValue("@RoomID", obj.RoomID);
                cmd.Parameters.AddWithValue("@BedID", obj.BedID);
                cmd.Parameters.AddWithValue("@OldWardID", obj.FromWordID);
                cmd.Parameters.AddWithValue("@OldRoomID", obj.FromRoomID);
                cmd.Parameters.AddWithValue("@OldBedID", obj.FromBedID);
                cmd.Parameters.AddWithValue("@BedCharges", "0.00");
                cmd.Parameters.AddWithValue("@IsCurrentBed", 0);
                cmd.Parameters.AddWithValue("@EnterDateTime", Convert.ToDateTime(obj.EnterDateTime));
                cmd.Parameters.AddWithValue("@CreationID", UserID);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return true;
        }

        public DataSet GetPatientData(int HospitalID, int LocationID, int PatientRegNO)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@PatientRegNO", SqlDbType.Int);
                param[2].Value = PatientRegNO;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientIPDWardTransfer", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }


        #region SearchPatientIPDWardTransfer
        public DataSet SearchPatientiIPDWardTransferByNameID(string ReferenceCode, int HospitalID, int LocatioID, string PatientName)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] aParams = new SqlParameter[5];
                aParams[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                aParams[0].Value = HospitalID;
                aParams[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                aParams[1].Value = LocatioID;
                aParams[2] = new SqlParameter("@PatientRegNO", SqlDbType.Int);
                aParams[2].Value = Convert.ToInt32(0);
                aParams[3] = new SqlParameter("@PatientName", SqlDbType.VarChar, 100);
                aParams[3].Value = PatientName;
                aParams[4] = new SqlParameter("@ReferenceCode", SqlDbType.VarChar, 20);
                aParams[4].Value = ReferenceCode;

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "SearchPatientIPDWardTransferByNameID", aParams);

            }
            catch (Exception ex)
            {
                // ExceptionManager.Publish(ex);
                throw ex;

            }
            return ds;
        }
        #endregion

        public DataSet GetAllFinancialYear()
        {
            Connect();
            {
                DataSet ds = new DataSet();
                try
                {
                    SqlParameter[] param = new SqlParameter[3];
                    con.Open();
                    param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                    param[0].Value = HospitalID;
                    param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                    param[1].Value = LocationID;
                    ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetAllFinancialYear", param);
                    con.Close();
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    throw ex;
                }
                return ds;
            }
        }

        public DataSet BindConsultant(string DoctorFName)
        {
            Connect();
            SqlDataAdapter ad = new SqlDataAdapter("select  DoctorID,DoctorFName  from  Doctor where DoctorFName  like  '" + DoctorFName + "%" + "' and RowStatus =0 and HospitalID = " + HospitalID + " and LocationID = " + LocationID + " and RowStatus= 0 order by  DoctorFName asc", con);//Your data query goes here for searching the data
            DataSet ds = new DataSet();
            con.Open();
            ad.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet BindFromWard()
        {
            Connect();
            SqlDataAdapter ad = new SqlDataAdapter("select WardID,WardName  from  Ward", con);//Your data query goes here for searching the data
            DataSet ds = new DataSet();
            con.Open();
            ad.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet BindFromRoom()
        {
            Connect();
            SqlDataAdapter ad = new SqlDataAdapter("select RoomID ,RoomName from WardRooms", con);
            DataSet ds = new DataSet();
            con.Open();
            ad.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet BindFromBed()
        {
            // Note: @filter parameter must be there.
            Connect();
            SqlDataAdapter ad = new SqlDataAdapter("select BedID,BedNO from WardRoomsDetails ", con);
            DataSet ds = new DataSet();
            con.Open();
            ad.Fill(ds);
            con.Close();
            return ds;
        }

        //public DataSet BindFromBed()
        //{
        //    // Note: @filter parameter must be there.
        //    Connect();
        //    SqlDataAdapter ad = new SqlDataAdapter("select BedID,BedNO from WardRoomsDetails ", con);
        //    DataSet ds = new DataSet();
        //    con.Open();
        //    ad.Fill(ds);
        //    con.Close();
        //    return ds;
        //}

        public DataSet BindDoctor(int ID)
        {
            // Note: @filter parameter must be there.
            Connect();
            SqlDataAdapter ad = new SqlDataAdapter("select DoctorID,DoctorPrintName from Doctor where DoctorID=" + ID + " and RowStatus=0 and HospitalID=" + HospitalID + " and LocationID=" + LocationID + "", con);
            DataSet ds = new DataSet();
            con.Open();
            ad.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet BindToWard()
        {
            // Note: @filter parameter must be there.
            Connect();
            SqlDataAdapter ad = new SqlDataAdapter("select WardRoomsDetails.WardID as WardID,Ward.WardName as WardName  from  WardRoomsDetails inner join Ward on Ward.WardID = WardRoomsDetails.WardID where Ward.HospitalID = " + HospitalID + " and  Ward.LocationID = " + LocationID + " and WardRoomsDetails.RowStatus =0 and WardRoomsDetails.BedStatus =0 group by  WardRoomsDetails.WardID,Ward.WardName", con);
            DataSet ds = new DataSet();
            con.Open();
            ad.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet FillWord()
        {
            // Note: @filter parameter must be there.
            Connect();
            SqlDataAdapter ad = new SqlDataAdapter("select WardRoomsDetails.WardID as WardID,Ward.WardName as WardName  from  WardRoomsDetails inner join Ward on Ward.WardID = WardRoomsDetails.WardID where Ward.HospitalID = " + HospitalID + " and  Ward.LocationID = " + LocationID + " and WardRoomsDetails.RowStatus =0  group by  WardRoomsDetails.WardID,Ward.WardName", con);
            DataSet ds = new DataSet();
            con.Open();
            ad.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet BindToRoom(int wordID)
        {
            //Your data query goes here for searching the data
            // Note: @filter parameter must be there.
            Connect();
            SqlDataAdapter ad = new SqlDataAdapter("select  WardRoomsDetails.RoomID as RoomID ,WardRooms.RoomName as RoomName from WardRoomsDetails inner join WardRooms on WardRooms.RoomID = WardRoomsDetails.RoomID  where  WardRoomsDetails.RowStatus =0 and WardRoomsDetails.BedStatus =0 and  WardRoomsDetails.WardID = " + wordID + " group by  WardRoomsDetails.RoomID,WardRooms.RoomName ", con);
            DataSet ds = new DataSet();
            con.Open();
            ad.Fill(ds);
            con.Close();
            return ds;
        }
        public DataSet FillToRoom(int wordID)
        {
            //Your data query goes here for searching the data
            // Note: @filter parameter must be there.
            Connect();
            SqlDataAdapter ad = new SqlDataAdapter("select  WardRoomsDetails.RoomID as RoomID ,WardRooms.RoomName as RoomName from WardRoomsDetails inner join WardRooms on WardRooms.RoomID = WardRoomsDetails.RoomID  where  WardRoomsDetails.RowStatus =0  and  WardRoomsDetails.WardID = " + wordID + " group by  WardRoomsDetails.RoomID,WardRooms.RoomName ", con);
            DataSet ds = new DataSet();
            con.Open();
            ad.Fill(ds);
            con.Close();
            return ds;
        }
        public DataSet BindToBed(int WordID, int RoomID)
        {
            Connect();

            SqlDataAdapter ad = new SqlDataAdapter("select WardRoomsDetails.BedID as  BedID  , WardRoomsDetails.BedNO as BedNO from WardRoomsDetails where WardID = " + WordID + " and RoomID = " + RoomID + "   and BedStatus =0 and RowStatus= 0 order by Convert(NVARCHAR(MAX),BedNO) asc", con);
            DataSet ds = new DataSet();
            con.Open();
            ad.Fill(ds);
            con.Close();
            return ds;
        }
        public DataSet FillTOBed(int WordID, int RoomID)
        {
            Connect();

            SqlDataAdapter ad = new SqlDataAdapter("select WardRoomsDetails.BedID as  BedID  , WardRoomsDetails.BedNO as BedNO from WardRoomsDetails where WardID = " + WordID + " and RoomID = " + RoomID + "   and RowStatus= 0 order by Convert(NVARCHAR(MAX),BedNO) asc", con);
            DataSet ds = new DataSet();
            con.Open();
            ad.Fill(ds);
            con.Close();
            return ds;
        }

        #region Check Patient IPD Final Bill
        public bool CheckFinalBill(int HospitalID, int LocationID, int PatientRegNo, int OPDIPDID)
        {
            SqlParameter[] aParams = new SqlParameter[5];
            try
            {
                Connect();
                aParams[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                aParams[0].Value = HospitalID;
                aParams[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                aParams[1].Value = LocationID;
                aParams[2] = new SqlParameter("@NameExists", SqlDbType.Bit);
                aParams[2].Direction = ParameterDirection.Output;
                aParams[3] = new SqlParameter("@PatientRegNo", SqlDbType.Int);
                aParams[3].Value = PatientRegNo;
                aParams[4] = new SqlParameter("@OPDIPDID", SqlDbType.Int);
                aParams[4].Value = OPDIPDID;

                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "CheckFinalBill", aParams);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return Convert.ToBoolean(aParams[2].Value);
        }
        #endregion
        public bool CheckDischargePatient(int HospitalID, int LocationID, int PatientRegNo, int PatientIPDNo)
        {
            SqlParameter[] aParams = new SqlParameter[5];
            try
            {
                Connect();

                aParams[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                aParams[0].Value = HospitalID;
                aParams[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                aParams[1].Value = LocationID;
                aParams[2] = new SqlParameter("@NameExists", SqlDbType.Bit);
                aParams[2].Direction = ParameterDirection.Output;
                aParams[3] = new SqlParameter("@PatientRegNo", SqlDbType.Int);
                aParams[3].Value = PatientRegNo;
                aParams[4] = new SqlParameter("@PatientIPDno", SqlDbType.Int);
                aParams[4].Value = PatientIPDNo;


                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "CheckDischargePatient", aParams);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return Convert.ToBoolean(aParams[2].Value);
        }

        public DataSet GetPatientDetails(int ipdno,int regno)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@PatientRegNO", SqlDbType.Int);
                param[2].Value = regno;
                param[3] = new SqlParameter("@PatientIPDNo", SqlDbType.Int);
                param[3].Value = ipdno;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "RptAdmissionForm", param);              
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;

            HttpContext.Current.Session["HospitalID"] = HospitalID;
            HttpContext.Current.Session["LocationID"] = LocationID;
        }

        

    }


}