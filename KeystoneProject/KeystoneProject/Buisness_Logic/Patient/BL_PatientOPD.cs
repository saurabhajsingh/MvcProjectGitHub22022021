using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using KeystoneProject.Buisness_Logic.Patient;
using KeystoneProject.Models.Patient;

namespace KeystoneProject.Buisness_Logic.Patient
{
    public class BL_PatientOPD
    {
        private SqlConnection con;
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<PatientOPD> PatientOPDList = new List<PatientOPD>();
        PatientOPD objPatientOPd = new PatientOPD();

        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
        }


        public List<PatientOPD> GetServiceDetailsPackage(string ServiceName)
        {
            Connect();
            SqlCommand cmd;


            cmd = new SqlCommand("select distinct  BillCharges,Services. ServiceName,Services.ServiceID,ServiceGroupID,UnitID,(select UnitName from Unit where UnitID = Services.UnitID) as UnitName,        cast( GeneralCharges    as decimal(18,2)) as GeneralCharges ,  cast( EmergencyCharges as decimal(18,2))  as EmergencyCharges   from ServiceDetailsPackage   left  join Services on Services.ServiceID = ServiceDetailsPackage.ServiceID where Services.RowStatus = 0 and Services.HospitalID = " + HospitalID + " and Services.LocationID = " + LocationID + " AND ServiceName like '" + ServiceName + "%' ", con);

            
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataSet ds = new DataSet();
            con.Open();
            sd.Fill(ds);
            con.Close();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientOPDList.Add(
                    new PatientOPD
                    {
                        NameOfPackageID = dr["ServiceID"].ToString(),
                        NameOfPackage = dr["ServiceName"].ToString(),

                    });
            }
            return PatientOPDList;
        }


        public List<PatientOPD> WardName(int HospitalID, int LocationID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select WardRoomsDetails.WardID as WardID,Ward.WardName as WardName  from  WardRoomsDetails inner join Ward on Ward.WardID = WardRoomsDetails.WardID where Ward.HospitalID = " + HospitalID + " and  Ward.LocationID = " + LocationID + " and WardRoomsDetails.RowStatus =0  and Ward.RowStatus=0 group by  WardRoomsDetails.WardID,Ward.WardName", con);//Your data query goes here for searching the data
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                PatientOPDList.Add(
                    new PatientOPD
                    {
                        WardID = Convert.ToInt32(dr["WardID"]).ToString(),
                        WardName = Convert.ToString(dr["WardName"]),

                    });
            }
            return PatientOPDList;
        }


        public List<PatientOPD> RoomNo(string Roomname, string WardID)
          {
            Connect();
            if (WardID != "")
            {
                SqlCommand cmd = new SqlCommand("select WardRoomsDetails.RoomID as RoomID ,WardRooms.RoomName as RoomName from WardRoomsDetails inner join WardRooms on WardRooms.RoomID = WardRoomsDetails.RoomID and WardRooms.HospitalID=" + HospitalID + " and WardRooms.LocationID=" + LocationID + "  where  WardRoomsDetails.RowStatus =0 and WardRooms.RowStatus =0 and WardRoomsDetails.BedStatus =0 and WardRooms.WardID=" + WardID + " and WardRooms.RoomName like ''+@Roomname+'%' and WardRoomsDetails.HospitalID=" + HospitalID + " and WardRoomsDetails.LocationID=" + LocationID + " group by  WardRoomsDetails.RoomID,WardRooms.RoomName", con);
                //SqlCommand cmd = new SqlCommand("select WardRoomsDetails.RoomID as RoomID ,WardRooms.RoomName as RoomName from WardRoomsDetails inner join WardRooms on WardRooms.RoomID = WardRoomsDetails.RoomID  where  WardRoomsDetails.RowStatus =0 and WardRoomsDetails.BedStatus =0 and  WardRoomsDetails.WardID = " + WardID + " and WardRooms.RoomName like ''+@Roomname+'%'  group by  WardRoomsDetails.RoomID,WardRooms.RoomName", con);
                cmd.Parameters.AddWithValue("@Roomname", "%");
                DataSet ds = new DataSet();
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                    con.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    PatientOPDList.Add(
                        new PatientOPD
                        {
                            RoomID = Convert.ToInt32(dr["RoomID"]).ToString(),
                            RoomNo = Convert.ToString(dr["RoomName"]),

                        });
                }
            }
            return PatientOPDList;
        }

        public List<PatientOPD> BedNo(string BedNo, string RoomID)
        {
            BedNo = "%";
            Connect();
            if (RoomID != "")
            {
                SqlCommand cmd = new SqlCommand("select WardRoomsDetails.BedID as  BedID  , WardRoomsDetails.BedNO as BedNO from WardRoomsDetails where BedNO like '" + BedNo + "%" + "' and RoomID = " + RoomID + "  and BedStatus =0 and RowStatus= 0", con);//Your data query goes here for searching the data

                DataSet ds = new DataSet();
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    PatientOPDList.Add(
                        new PatientOPD
                        {
                            BedID = Convert.ToInt32(dr["BedID"]).ToString(),
                            BedNO = Convert.ToString(dr["BedNO"]),

                        });
                }
            }
            return PatientOPDList;
        }
        public void DeletePatientBillDetails(int BillNo)
        {
            Connect();
            try
            {
                SqlParameter[] aParams = new SqlParameter[3];
                aParams[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                aParams[0].Value = HospitalID;
                aParams[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                aParams[1].Value = LocationID;
                aParams[2] = new SqlParameter("@BillsNo", SqlDbType.Int);
                aParams[2].Value = BillNo;
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "DeletePatientBillDetails", aParams);

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

        public List<PatientOPD> OrganizationName(string prefix)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select OrganizationID,OrganizationName from   Organization where OrganizationType = 'Organization' and HospitalID = " + HospitalID + " and LocationID = " + LocationID + " and RowStatus= 0 and OrganizationName like '" + "%" + prefix + "%" + "'  order by OrganizationID asc", con);//Your data query goes here for searching the data
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                PatientOPDList.Add(
                    new PatientOPD
                    {
                        OrganizationID = Convert.ToInt32(dr["OrganizationID"]).ToString(),
                        OrganizationName = Convert.ToString(dr["OrganizationName"]),

                    });
            }
            return PatientOPDList;

        }


        public List<PatientOPD> Bind_TPA_Name(string prefix)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select OrganizationID,OrganizationName from   Organization where OrganizationType = 'T.P.A' and HospitalID = " + HospitalID + " and LocationID = " + LocationID + " and RowStatus= 0 and OrganizationName like '"+"%" + prefix + "%" + "' order by OrganizationID asc", con);//Your data query goes here for searching the data
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                PatientOPDList.Add(
                    new PatientOPD
                    {
                        OrganizationID = Convert.ToInt32(dr["OrganizationID"]).ToString(),
                        OrganizationName = Convert.ToString(dr["OrganizationName"]),

                    });
            }
            return PatientOPDList;

        }

        public List<PatientOPD> Bind_InsuranceCompny(string prefix)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select OrganizationID,OrganizationName from   Organization where OrganizationType = 'Insurance' and HospitalID = " + HospitalID + " and LocationID = " + LocationID + " and RowStatus= 0 and OrganizationName like '" + "%" + prefix + "%" + "' order by OrganizationID asc", con);//Your data query goes here for searching the data
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                PatientOPDList.Add(
                    new PatientOPD
                    {
                        OrganizationID = Convert.ToInt32(dr["OrganizationID"]).ToString(),
                        OrganizationName = Convert.ToString(dr["OrganizationName"]),

                    });
            }
            return PatientOPDList;

        }


        public List<PatientOPD> BindPrefixPatient(string prefix)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select PrefixID,PrefixName,Gender from   PatientPrefix where PrefixName like '" +"%"+ prefix + "' and  HospitalID = '" + HospitalID + "' and LocationID = '" + LocationID + "' and RowStatus= 0 order by PrefixID asc", con);//Your data query goes here for searching the data
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                PatientOPDList.Add(
                    new PatientOPD
                    {
                        PrefixID = Convert.ToInt32(dr["PrefixID"]).ToString(),
                        PrefixName = Convert.ToString(dr["PrefixName"]),
                        Gender = dr["Gender"].ToString(),

                    });
            }
            return PatientOPDList;

        }

        public List<PatientOPD> BindDepartment(string DepartmentName, string DepartmentID)
        {
            Connect();
            // SqlCommand cmd = new SqlCommand("select DepartmentID,DepartmentName from  Department where  HospitalID = " + HospitalID + " and LocationID = " + LocationID + " and RowStatus= 0  order by  DepartmentName asc");//Your data query goes here for searching the data

            SqlCommand cmd = new SqlCommand("select  DepartmentID,DepartmentName from  Department where HospitalID='" + HospitalID + "' and LocationID ='" + LocationID + "' and  DepartmentID like '" + DepartmentID + "%" + "' and   DepartmentName  like  @filter+'%' and RowStatus=0  order by  DepartmentName asc", con);
            cmd.Parameters.AddWithValue("@filter", DepartmentName);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                PatientOPDList.Add(
                    new PatientOPD
                    {
                        DepartmentID = Convert.ToInt32(dr["DepartmentID"]).ToString(),
                        DepartmentName = Convert.ToString(dr["DepartmentName"]),

                    });
            }
            return PatientOPDList;

        }


        public DataTable GetDoctorDepartment(string DepartmentID)
        {
            Connect();


            SqlDataAdapter da = new SqlDataAdapter("select DepartmentName from Department where DepartmentID='" + DepartmentID + "' and RowStatus=0", con);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();
            return dt;
        }
        public DataSet GetIPDPatient(string GetIPDPatient,string outside)
        {
            Connect();
            if(outside=="IPD")
            {
                SqlCommand cmd = new SqlCommand("GetPatientDischarge", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                DataSet ds = new DataSet();
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            else
            {
                SqlCommand cmd = new SqlCommand("select PatientRegNo, PatientName ,Address, MobileNo,PrintRegNO from patient where PatientName like ''+@GetIPDPatient+'%'  and PatientType ='OPD' and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "", con);
                cmd.Parameters.AddWithValue("@GetIPDPatient", GetIPDPatient);
                DataSet ds = new DataSet();
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
           
           

           
        }
        public List<PatientOPD> BindReferredByDoctorID(string prefix)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select   DoctorID,DoctorFName +' '+ DoctorLName as 'DoctorPrintName'   from  Doctor where  DoctorType ='Referred' and DoctorPrintName  like '" + "%" + prefix + "%" + "'  and HospitalID = " + HospitalID + " and LocationID = " + LocationID + " and RowStatus= 0 order by  DoctorFName asc", con);//Your data query goes here for searching the data
            cmd.Parameters.AddWithValue("@prefix", prefix);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                PatientOPDList.Add(
                    new PatientOPD
                    {
                        DoctorID = Convert.ToInt32(dr["DoctorID"]).ToString(),
                        DoctorPrintName = Convert.ToString(dr["DoctorPrintName"]),

                    });
            }
            return PatientOPDList;

        }

        public List<PatientOPD> GetCategory()
        {
            List<KeystoneProject.Models.Patient.PatientOPD> serachlist = new List<PatientOPD>();

            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllPatientType1", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Open();
                ad.Fill(ds);
                con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    serachlist.Add(new KeystoneProject.Models.Patient.PatientOPD
                    {
                        PatientTypeID = dr["PatientTypeID"].ToString(),
                        PatientType = dr["PatientType"].ToString()

                    });

                }
            }
            catch (Exception ex)
            {
                return serachlist;
            }
            return serachlist;
        }
        public List<PatientOPD> BindConsultant(string prefix)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select  DoctorID,DoctorFName +' '+ DoctorLName as 'DoctorPrintName' ,DepartmentID  from  Doctor where  DoctorType ='Consultant' and DoctorPrintName  like '" + "%" + prefix+"%"+"' and RowStatus =0 and HospitalID = " + HospitalID + " and LocationID = " + LocationID + "  order by  DoctorFName asc", con);//Your data query goes here for searching the data
          //  cmd.Parameters.AddWithValue("@prefix", prefix);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                PatientOPDList.Add(
                    new PatientOPD
                    {
                        DoctorID = Convert.ToInt32(dr["DoctorID"]).ToString(),
                        DoctorPrintName = Convert.ToString(dr["DoctorPrintName"]),
                        DepartmentID = dr["DepartmentID"].ToString(),

                    });
            }
            return PatientOPDList;

        }

        public int CityID = 0;
        public List<PatientOPD> BindCity(string CityName, string City)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select  CityID,CityName from City where  CityName like '" + CityName + "%" + "' and   CityID  like  '" + City + "%" + "' and RowStatus=0 and hospitalid = '"+ HospitalID+"' and locationid = '"+ LocationID  +"' order by  CityName asc", con);//Your data query goes here for searching the data
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                PatientOPDList.Add(
                    new PatientOPD
                    {
                        CityID = Convert.ToInt32(dr["CityID"]).ToString(),
                        CityName = Convert.ToString(dr["CityName"]),

                    });
            }
            return PatientOPDList;

        }

        public List<PatientOPD> GetCountryStateID(int CityID)
        {
            Connect();

            SqlCommand cmd = new SqlCommand("getCountryStateID", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CityID", CityID);
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                PatientOPDList.Add(

                    new PatientOPD
                    {
                        StateID = dr["StateID"].ToString(),
                        CountryID = dr["CountryID"].ToString(),
                        StateName = dr["StateName"].ToString(),
                        CountryName = dr["CountryName"].ToString(),
                    });



            }

            return PatientOPDList;
        }

        string IPDBillWardCharge = "";
        public List<PatientOPD> GetPatientIPDWardDetailsForEdit(int PatientRegNO, int PatientIPDNO)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("GetPatientIPDWardDetailsForEdit", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@PatientRegNO", PatientRegNO);
            cmd.Parameters.AddWithValue("@PatientIPDNO", PatientIPDNO);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet dt = new DataSet();
          
            con.Open();
            da.Fill(dt);
            con.Close();
            if (dt.Tables[1].Rows.Count>0)
            {
                IPDBillWardCharge = dt.Tables[1].Rows[0]["ServiceName"].ToString();
            }
            foreach (DataRow dr in dt.Tables[0].Rows)
            {
                PatientOPDList.Add(

                    new PatientOPD
                    {
                        PatientRegNo = Convert.ToInt32(dr["PatientRegNO"].ToString()),
                        PatientIPDNO = dr["PatientIPDNO"].ToString(),
                        ApplyWardID = dr["ApplyWardID"].ToString(),
                        ApplyWardName = dr["ApplyWardName"].ToString(),
                        WardID = dr["WardID"].ToString(),
                        RoomID = dr["RoomID"].ToString(),
                        BedID = dr["BedID"].ToString(),
                        WardName = dr["WardName"].ToString(),
                        RoomNo = dr["RoomName"].ToString(),
                        BedNO = dr["BedNO"].ToString(),
                        BedCharges= IPDBillWardCharge,
                        //BedCharges = dr["BedCharges"].ToString(),
                        EnterDateTime = Convert.ToDateTime(dr["EnterDateTime"].ToString()),
                        //  IsCurrentBed = Convert.ToBoolean(dr["IsCurrentBed"].ToString()),

                    });



            }
           // PatientOPDList[2].PatientRegistrationDate = Convert.ToDateTime(PatientOPDList[1].PatientRegistrationDate).ToString("yyyy-MM-dd");
            return PatientOPDList;
        }



        public List<PatientOPD> GetPatientIPDDetails(int PatientRegNO)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("GetPatientIPDDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@PatientRegNO", PatientRegNO);


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            con.Close();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientOPDList.Add(

                    new PatientOPD
                    {
                        PatientRegNo = Convert.ToInt32(dr["PatientRegNO"].ToString()),
                        printRegNo = Convert.ToInt32(dr["PrintRegNO"].ToString()),
                        PatientIPDNO = dr["PatientIPDNO"].ToString(),
                        PrintIPDNO = dr["PrintIPDNO"].ToString(),
                        PatientOPDNO = dr["PatientOPDNO"].ToString(),
                        PatientType = dr["PatientType"].ToString(),
                        AddmissionType = dr["AddmissionType"].ToString(),
                        PatientRegistrationDate = Convert.ToDateTime(dr["AddmissionDate"]).ToString("yyyy-MM-dd"),
                        time = Convert.ToDateTime(dr["AddmissionDate"]).ToString("hh:mm"),
                        // EnterDateTime = Convert.ToDateTime(dr["AddmissionDate"]),
                        TPA_ID = dr["TPA_ID"].ToString(),
                        MediclaimCardID=dr["MediclaimCardID"].ToString(),
                        TPA_Name = dr["TPAName"].ToString(),

                        TPAStatus = dr["TPAStatus"].ToString(),
                        CCNClaimNo = dr["CCNClaimNo"].ToString(),

                        OrganizationID = dr["OrganisationName"].ToString(),
                        OrganizationName = dr["Oragnisation"].ToString(),

                        InsuranceCompnyID = dr["InsuranceCompny"].ToString(),
                        InsuranceCompny = dr["InsuranceName"].ToString(),

                        PolicyNo = dr["PolicyNo"].ToString(),
                        DoctorID = dr["ConsultantDrID"].ToString(),

                        TemporaryDoctorStatus = dr["TemporaryDoctorStatus"].ToString(),
                        TemporaryDoctor = dr["TemporaryDoctor"].ToString(),

                        RefferedTemporaryDoctorStatus = dr["RefferedTemporaryDoctorStatus"].ToString(),
                        RefferedTemporaryDoctor = dr["RefferedTemporaryDoctor"].ToString(),


                        DoctorPrintName = dr["ConsultantDoctor"].ToString(),
                        DepartmentID = dr["DepartmentID"].ToString(),
                        DepartmentName = dr["DepartmentName"].ToString(),
                        ReferredByDoctorID = dr["ReferredByDoctorID"].ToString(),
                        ReferredByDoctorName = dr["RefferedDoctor"].ToString(),


                        Weight = dr["Weight"].ToString(),
                        BloodPressure = dr["BloodPressure"].ToString(),
                        Reason = dr["Reason"].ToString(),


                        AdvanceAmt = dr["AdvanceAmount"].ToString(),


                        PaymentType = dr["PaymentType"].ToString(),
                        Number = dr["Number"].ToString(),
                        Name = dr["Name"].ToString(),
                        Date = dr["Date"].ToString(),
                        Remark = dr["Remark"].ToString(),
                        Elective_Surgery = dr["Elective_Surgery"].ToString(),
                        Medical_Management = dr["Medical_Management"].ToString(),
                        ConsDr1 = dr["ConsDr1"].ToString(),
                        NameOfPackage = dr["NameOfPackage"].ToString(),
                        PatientCategory = dr["PatientCategoryID"].ToString(),
                    });



            }


            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                PatientOPDList.Add(

                    new PatientOPD
                    {
                        PatientRegNo = Convert.ToInt32(dr["PatientRegNO"].ToString()),
                        PatientIPDNO = dr["PatientIPDNO"].ToString(),
                        PrintIPDNO = dr["PrintIPDNO"].ToString(),


                    });



            }



            return PatientOPDList;
        }



        public List<PatientOPD> GetSecurityDeposite(int PatientRegNO)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("GetSecurityDeposite", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@PatientRegNO", PatientRegNO);


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            con.Close();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientOPDList.Add(

                    new PatientOPD
                    {
                        PatientRegNo = Convert.ToInt32(dr["PatientRegNO"].ToString()),
                        SecurityDeposityID = dr["SecurityDeposityID"].ToString(),
                        PrintSecurityDeposite = dr["PrintSecurityDeposite"].ToString(),
                        OPDIPDID = Convert.ToInt32(dr["OPD/IPDID"].ToString()),
                        CrAmount = dr["CrAmount"].ToString(),
                        DrAmount = dr["DrAmount"].ToString(),
                        BillType = dr["BillType"].ToString(),
                        FinancialYearID = dr["FinancialYearID"].ToString(),



                    });



            }





            return PatientOPDList;
        }


        public List<PatientOPD> GetPatientName(string PatientName)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select PatientName,PatientRegNO,PrintRegNO from Patient where HospitalID='" + HospitalID + "' and LocationID='" + LocationID + "' and PatientName like '" + PatientName + '%' + "' and RowStatus=0", con);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                PatientOPDList.Add(
                    new PatientOPD
                    {
                        PatientRegNo = Convert.ToInt32(dr["PatientRegNO"].ToString()),
                        PatientName = Convert.ToString(dr["PatientName"]),
                        printRegNo=Convert.ToInt32(dr["PrintRegNO"].ToString()),

                    });
            }


            return PatientOPDList;

        }


        public List<PatientOPD> EditPatient_OPDDetail1(int PatientRegNo,string PatientType)
        {
            DataSet dss = Bind_FinancialYear();
            string FinancialYearID = dss.Tables[0].Rows[0]["FinancialYearID"].ToString();

              PatientOPDList = EditPatient_OPDDetail(PatientRegNo, "Edit");
            //  objPatientOPd.PatientType = PatientOPDList[0].PatientType;
            objPatientOPd.PatientType = PatientType;
             objPatientOPd.Message = "";
            if (objPatientOPd.PatientType == "OPD")
            {
                
               DataSet dsPatient = new DataSet();
                Connect();
                DataSet dsPatientOPDDetails = new DataSet();
                con.Open();
                SqlCommand cmd1 = new SqlCommand("GetPatientOPDDetails", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                cmd1.Parameters.AddWithValue("@PatientRegNO", PatientRegNo);
                SqlDataAdapter ad1 = new SqlDataAdapter(cmd1);
                DataTable dt = new DataTable();
                // ad1.SelectCommand = cmd1;

                ad1.Fill(dsPatientOPDDetails);
                con.Close();
             
                foreach (DataRow dr in dsPatientOPDDetails.Tables[0].Rows)
                {
                    PatientOPDList.Add(
                       new PatientOPD
                       {
                           PatientRegNo = Convert.ToInt32(dr["PatientRegNO"].ToString()),
                           PatientOPDNO = Convert.ToString(dr["PatientOPDNO"]),
                           PrintOPDNo = Convert.ToString(dr["PrintOPDNo"]),
                           PatientRegistrationDate = Convert.ToDateTime(dr["PatientRegistrationDate"]).ToString("yyyy-MM-dd"),
                           time = Convert.ToDateTime(dr["PatientRegistrationDate"]).ToString("hh:mm"),
                           PatientType = dr["PatientType"].ToString(),
                           TPA_ID = dr["TPA_ID"].ToString(),
                           MediclaimCardID = dr["MediclaimCardID"].ToString(),
                           OrganizationID = dr["TPA_ID"].ToString(),
                           TemporaryDoctorStatus = dr["TemporaryDoctorStatus"].ToString(),
                           TemporaryDoctor = dr["TemporaryDoctor"].ToString(),

                           RefferedTemporaryDoctorStatus = dr["RefferedTemporaryDoctorStatus"].ToString(),
                           RefferedTemporaryDoctor = dr["RefferedTemporaryDoctor"].ToString(),

                           DoctorID = dr["DoctorID"].ToString(),

                           DoctorPrintName = dr["ConsultantDoctor"].ToString(),
                           DepartmentID = dr["DepartmentID"].ToString(),
                           DepartmentName = dr["DepartmentName"].ToString(),
                           ReferredByDoctorID = dr["ReferredByDoctorID"].ToString(),
                           ReferredByDoctorName = dr["RefferedDoctor"].ToString(),
                           Weight = dr["Weight"].ToString(),
                           BloodPressure = dr["BloodPressure"].ToString(),
                           Reason = dr["Reason"].ToString(),

                           RegistrationCharge = dr["RegistrationCharges"].ToString(),
                           ConsultionCharges = dr["ConsultionCharges"].ToString(),
                           DiscountInPer = dr["DiscountInPer"].ToString(),
                           DiscountInRS = dr["DiscountInRS"].ToString(),
                           TotalAmount = dr["TotalAmount"].ToString(),
                           PaymentType = dr["PaymentType"].ToString(),
                           Number = dr["Number"].ToString(),
                           Name = dr["Name"].ToString(),
                           Date = dr["Date"].ToString(),
                           DiscountReason = dr["DiscountReason"].ToString(),

                       });

                }
            }

            else
            {
                if (objPatientOPd.PatientType == "IPD" || CheckDischargePatient(PatientRegNo))
                {
                    PatientOPDList = GetPatientIPDDetails(PatientRegNo);

                    //PatientOPDList = GetPatientIPDWardDetailsForEdit(PatientRegNo, Convert.ToInt32(PatientOPDList[2].PatientIPDNO));

                    //PatientOPDList = GetSecurityDeposite(PatientRegNo);




                }
                else
                {
                    PatientOPDList.Add(
                             new PatientOPD
                             {
                                 Message = "Patient already Exists in IPD. So you can not revisit without discharge."
                             });
                }
            }

            return PatientOPDList;
        }

        public DataSet GetAllServicesTPAWiseForIPDBills( int WardID, int OrganizationID)
        {
            Connect();
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@WardID", SqlDbType.Int);
                param[2].Value = WardID;
                param[3] = new SqlParameter("@OrganizationID", SqlDbType.Int);
                param[3].Value = OrganizationID;
                return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetAllServicesTPAWiseForIPDBills", param);
            }
            catch (DBConcurrencyException exp)
            {
                ExceptionManager.Publish(exp);
                exp.Data.Add("returnValue", "-1");
                throw exp;
            }

        }
        public DataSet GetAllServicesTPAWiseForOPDBills(int OrganizationID)
        {
            Connect();
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@OrganizationID", SqlDbType.Int);
                param[2].Value = OrganizationID;
                return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetAllServicesTPAWiseForOPDBills", param);
            }
            catch (DBConcurrencyException exp)
            {
                ExceptionManager.Publish(exp);
                exp.Data.Add("returnValue", "-1");
                throw exp;
            }

        }
        public DataSet GetPatientTotalBillsNO(string PatientRegNO, string PatientIPDNO,string type)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@PatientRegNO", SqlDbType.Int);
                param[0].Value = PatientRegNO;
                param[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[1].Value = HospitalID;
                param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[2].Value = LocationID;
                param[3] = new SqlParameter("@PatientIPDNO", SqlDbType.NVarChar);
                param[3].Value = PatientIPDNO;
                param[4] = new SqlParameter("@PatientType", SqlDbType.NVarChar);
                param[4].Value = type;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientTotalBillsNO", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public DataSet GetPatientBillsDetails(int BillNo)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@BillNo", SqlDbType.Int);
                param[0].Value = BillNo;
                param[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[1].Value = HospitalID;
                param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[2].Value = LocationID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientBillsDetails", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public DataSet GetPatientBills(int BillNo)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@BillNo", SqlDbType.Int);
                param[0].Value = BillNo;
                param[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[1].Value = HospitalID;
                param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[2].Value = LocationID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientBills", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
        public void data()
        {


        }

        public List<PatientOPD> EditPatient_OPDDetail(int PatientRegNO, string Mode)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("GetPatient", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@PatientRegNO", PatientRegNO);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            if (ds.Tables[0].Rows.Count >0)
            {          
            // searchList.Add(PatientRegNO, PatientOPDNO);
            if (CheckDischargePatient(PatientRegNO) || ds.Tables[0].Rows[0]["PatientType"].ToString() == "OPD" || Mode == "Edit")
            {

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    PatientOPDList.Add(
                        new PatientOPD
                        {
                            PFPatientName = dr["PFPatientName"].ToString(),
                            PatientName = dr["PatientName"].ToString(),
                            PFGuardianName = dr["PFGuardianName"].ToString(),
                            PFirstName = dr["PFirstName"].ToString(),
                            PLastName = dr["PLastName"].ToString(),
                            PMiddleName = dr["PMiddleName"].ToString(),
                            Gender = dr["Gender"].ToString(),
                            Age = dr["Age"].ToString(),
                            AgeType = dr["AgeType"].ToString(),
                            DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]).ToString("yyyy-MM-dd"),
                            Source = dr["Source"].ToString(),
                            Height = dr["Height"].ToString(),
                            BloodPressure = dr["BloodPressure"].ToString(),
                            // Source = dr["Source"].ToString(),
                            //Reason = dr["Reason"].ToString(),
                            Address = dr["Address"].ToString(),
                            CityID = dr["CityID"].ToString(),
                            StateID = dr["StateID"].ToString(),
                            CountryID = dr["CountryID"].ToString(),
                            CityName = dr["CityName"].ToString(),
                            StateName = dr["StateName"].ToString(),
                            CountryName = dr["CountryName"].ToString(),
                            PhoneNo = dr["PhoneNo"].ToString(),
                            MobileNo = dr["MobileNo"].ToString(),
                            PinCode = dr["PinCode"].ToString(),
                            EmailID = dr["EmailID"].ToString(),
                            Photo = (dr["Photo"].ToString()),
                            FinancialYearID = dr["FinancialYearID"].ToString(),
                            GuardianName = dr["GuardianName"].ToString(),
                            PatientRegNo = Convert.ToInt32(dr["PatientRegNO"].ToString()),
                            PatientType = dr["PatientType"].ToString(),
                            PanCardNo = dr["PanCardNo"].ToString(),
                            AadhaarNo = dr["AadhaarNo"].ToString(),
                            PatientCancel = dr["PatientCancel"].ToString().ToLower(),
                            PatientCancelReason = dr["PatientCancelReason"].ToString(),
                        });



                }
                    if (objPatientOPd.PatientType == "OPD" ||  Mode == "Revisit")
                    {

                        DataSet dsPatient = new DataSet();
                        Connect();
                        DataSet dsPatientOPDDetails = new DataSet();
                        con.Open();
                        SqlCommand cmd1 = new SqlCommand("GetPatientOPDDetails", con);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                        cmd1.Parameters.AddWithValue("@PatientRegNO", PatientRegNO);
                        SqlDataAdapter ad1 = new SqlDataAdapter(cmd1);
                        DataTable dt = new DataTable();
                        // ad1.SelectCommand = cmd1;

                        ad1.Fill(dsPatientOPDDetails);
                        con.Close();

                        foreach (DataRow dr in dsPatientOPDDetails.Tables[0].Rows)
                        {
                            PatientOPDList.Add(
                               new PatientOPD
                               {
                                   PatientRegNo = Convert.ToInt32(dr["PatientRegNO"].ToString()),
                                   PatientOPDNO = Convert.ToString(dr["PatientOPDNO"]),
                                   PrintOPDNo = Convert.ToString(dr["PrintOPDNo"]),
                                   //PatientRegistrationDate = Convert.ToDateTime(dr["PatientRegistrationDate"]).ToString("yyyy-MM-dd"),
                                   //time = Convert.ToDateTime(dr["PatientRegistrationDate"]).ToString("hh:mm"),
                                   PatientType = dr["PatientType"].ToString(),
                                 //  TPA_ID = dr["TPA_ID"].ToString(),
                                 //  MediclaimCardID = dr["MediclaimCardID"].ToString(),
                                 //  OrganizationID = dr["TPA_ID"].ToString(),
                                  // TemporaryDoctorStatus = dr["TemporaryDoctorStatus"].ToString(),
                                   TemporaryDoctor = dr["TemporaryDoctor"].ToString(),

                                   RefferedTemporaryDoctorStatus = dr["RefferedTemporaryDoctorStatus"].ToString(),
                                   RefferedTemporaryDoctor = dr["RefferedTemporaryDoctor"].ToString(),

                                   DoctorID = dr["DoctorID"].ToString(),

                                   DoctorPrintName = dr["ConsultantDoctor"].ToString(),
                                   DepartmentID = dr["DepartmentID"].ToString(),
                                   DepartmentName = dr["DepartmentName"].ToString(),
                                   ReferredByDoctorID = dr["ReferredByDoctorID"].ToString(),
                                   ReferredByDoctorName = dr["RefferedDoctor"].ToString(),
                                   Weight = dr["Weight"].ToString(),
                                   BloodPressure = dr["BloodPressure"].ToString(),
                                   Reason = dr["Reason"].ToString(),

                                   //RegistrationCharge = dr["RegistrationCharges"].ToString(),
                                   //ConsultionCharges = dr["ConsultionCharges"].ToString(),
                                   //DiscountInPer = dr["DiscountInPer"].ToString(),
                                   //DiscountInRS = dr["DiscountInRS"].ToString(),
                                   //TotalAmount = dr["TotalAmount"].ToString(),
                                   //PaymentType = dr["PaymentType"].ToString(),
                                   //Number = dr["Number"].ToString(),
                                   //Name = dr["Name"].ToString(),
                                   //Date = dr["Date"].ToString(),
                                   //DiscountReason = dr["DiscountReason"].ToString(),

                               });

                        }
                    }

                    objPatientOPd.PatientType = PatientOPDList[0].PatientType;
                if (objPatientOPd.PatientType == "OPD" && Mode!="Revisit")
                {

                    DataSet dsPatient = new DataSet();
                    Connect();
                    DataSet dsPatientOPDDetails = new DataSet();
                    con.Open();
                    SqlCommand cmd1 = new SqlCommand("GetPatientOPDDetails", con);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                    cmd1.Parameters.AddWithValue("@PatientRegNO", PatientRegNO);
                    SqlDataAdapter ad1 = new SqlDataAdapter(cmd1);
                    DataTable dt = new DataTable();
                    // ad1.SelectCommand = cmd1;

                    ad1.Fill(dsPatientOPDDetails);
                    con.Close();




                    foreach (DataRow dr in dsPatientOPDDetails.Tables[0].Rows)
                    {
                        PatientOPDList.Add(
                           new PatientOPD
                           {
                               PatientRegNo = Convert.ToInt32(dr["PatientRegNO"].ToString()),
                               PatientOPDNO = Convert.ToString(dr["PatientOPDNO"]),
                               PrintOPDNo = Convert.ToString(dr["PrintOPDNo"]),
                               PatientRegistrationDate = Convert.ToDateTime(dr["PatientRegistrationDate"]).ToString("yyyy-MM-dd"),
                               time = Convert.ToDateTime(dr["PatientRegistrationDate"]).ToString("hh:mm"),
                               PatientType = dr["PatientType"].ToString(),


                               TPA_ID = dr["TPA_ID"].ToString(),
                               MediclaimCardID = dr["MediclaimCardID"].ToString(),
                               TPA_Name = dr["TPAName"].ToString(),

                               
                               CCNClaimNo = dr["CCNClaimNo"].ToString(),

                               OrganizationID = dr["OrganisationName"].ToString(),
                               OrganizationName = dr["Oragnisation"].ToString(),

                               InsuranceCompnyID = dr["InsuranceCompny"].ToString(),
                               InsuranceCompny = dr["InsuranceName"].ToString(),

                               PolicyNo = dr["PolicyNo"].ToString(),

                               TemporaryDoctorStatus = dr["TemporaryDoctorStatus"].ToString(),
                               TemporaryDoctor = dr["TemporaryDoctor"].ToString(),

                               RefferedTemporaryDoctorStatus = dr["RefferedTemporaryDoctorStatus"].ToString(),
                               RefferedTemporaryDoctor = dr["RefferedTemporaryDoctor"].ToString(),

                               DoctorID = dr["DoctorID"].ToString(),

                               DoctorPrintName = dr["ConsultantDoctor"].ToString(),
                               DepartmentID = dr["DepartmentID"].ToString(),
                               DepartmentName = dr["DepartmentName"].ToString(),
                               ReferredByDoctorID = dr["ReferredByDoctorID"].ToString(),
                               ReferredByDoctorName = dr["RefferedDoctor"].ToString(),
                               Weight = dr["Weight"].ToString(),
                               BloodPressure = dr["BloodPressure"].ToString(),
                               Reason = dr["Reason"].ToString(),

                               RegistrationCharge = dr["RegistrationCharges"].ToString(),
                               ConsultionCharges = dr["ConsultionCharges"].ToString(),
                               DiscountInPer = dr["DiscountInPer"].ToString(),
                               DiscountInRS = dr["DiscountInRS"].ToString(),
                               TotalAmount = dr["TotalAmount"].ToString(),
                               PaymentType = dr["PaymentType"].ToString(),
                               Number = dr["Number"].ToString(),
                               Name = dr["Name"].ToString(),
                               Date = dr["Date"].ToString(),
                               DiscountReason = dr["DiscountReason"].ToString(),


                           });

                    }
                }

                else
                {
                    //if (objPatientOPd.PatientType == "IPD" && Mode != "Revisit")
                    //{
                    //    PatientOPDList = GetPatientIPDDetails(PatientRegNO);

                    //    PatientOPDList = GetPatientIPDWardDetailsForEdit(PatientRegNO, Convert.ToInt32(PatientOPDList[1].PatientIPDNO));

                    //    PatientOPDList = GetSecurityDeposite(PatientRegNO);
                    //    PatientOPDList[1].BedCharges= IPDBillWardCharge;

                    //  // objPatientOPd.BedCharges
                    //}
                    if (objPatientOPd.PatientType == "IPD" && Mode == "Revisit")
                    {
                        PatientOPDList = GetPatientIPDDetails(PatientRegNO);

                        PatientOPDList = GetPatientIPDWardDetailsForEdit(PatientRegNO, Convert.ToInt32(PatientOPDList[1].PatientIPDNO));

                        PatientOPDList = GetSecurityDeposite(PatientRegNO);
                        PatientOPDList[1].BedCharges = IPDBillWardCharge;

                        // objPatientOPd.BedCharges
                    }
                }


            }
            else
            {

                PatientOPDList.Add(
                          new PatientOPD
                          {
                              Message = "Patient already Exists in IPD. So you can not revisit without discharge."
                          });
            }

            }

            return PatientOPDList;
        }

        public List<PatientOPD> GetOPDPatientAppointment()
        {
            Connect();
            SqlCommand cmd = new SqlCommand("GetOPDPatientAppointment", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@Date", DateTime.Now);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientOPDList.Add(
                    new PatientOPD
                    {

                        PatientAppointID = dr["PatientID"].ToString(),
                        PatientName = dr["PatientName"].ToString(),
                        ReferenceCode = dr["PatientAppointmentID"].ToString()
                        // PatientAppointmentScheduleID = dr["PatientAppointmentScheduleID"].ToString(),


                    });
            }
            return PatientOPDList;
        }

        public List<PatientOPD> GetOPDPatientAppointmentDetails(string PatientAppointID)
        {
            Connect();
            string PatientRegNo;
            SqlCommand cmd = new SqlCommand("GetOPDPatientAppointmentDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@PatientAppointID", PatientAppointID);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["PatientRegNo"].ToString() == "")
                {
                    PatientRegNo = "0";
                }
                else
                {
                    PatientRegNo = (dr["PatientRegNo"].ToString());
                }

                PatientOPDList.Add(
                new PatientOPD
                {

                    PatientAppointID = dr["PatientID"].ToString(),
                    PatientName = dr["PatientName"].ToString(),
                    PFPatientName = dr["PFPatientName"].ToString(),

                    PatientRegNo = Convert.ToInt32(PatientRegNo.ToString()),


                        Age =dr["Age"].ToString(),
                        Gender=dr["Gender"].ToString(),
                        DateOfBirth=Convert.ToDateTime(dr["DateOfBirth"]).ToString("yyyy-MM-dd"),
                        MobileNo=dr["MobileNo"].ToString(),
                        PFirstName = dr["PFirstName"].ToString(),
                        PLastName = dr["PLastName"].ToString(),
                        PMiddleName = dr["PMiddleName"].ToString(),
                        DoctorID = dr["DoctorID"].ToString(),
                        DoctorPrintName = dr["DoctorPrintName"].ToString(),
                        ReferenceCode = dr["PatientAppointmentID"].ToString()
                        // PatientAppointmentScheduleID = dr["PatientAppointmentScheduleID"].ToString(),


                    });
            }
            return PatientOPDList;
        }
        public DataSet GetPatientDataForIPDFormOPD(int PatientRegNO, int PatientOPDNO)
        {
            SqlCommand cmd = new SqlCommand("GetPatientDataForIPDFormOPD", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@PatientRegNO", PatientRegNO);
            cmd.Parameters.AddWithValue("@PatientOPDNO", PatientOPDNO);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.Fill(ds);
            con.Close();

            return ds;
        }


        public bool CheckDischargePatient(int PatientRegNo)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckDischargeForPatientIPDRegistration", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@PatientRegNo", PatientRegNo);

                cmd.Parameters.Add("@NameExists", SqlDbType.NVarChar, 50);
                cmd.Parameters["@NameExists"].Direction = ParameterDirection.Output;
                con.Open();
                int i = cmd.ExecuteNonQuery();
                Table = (string)cmd.Parameters["@NameExists"].Value;
                if (Convert.ToInt32(Table) == 1)
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
                return flag;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public DataSet Bind_FinancialYear()
        {
            DataSet ds = new DataSet();
            Connect();
            SqlCommand cmd = new SqlCommand("select FinancialYear.FinancialYear,MasterSetting.FinancialYearID from FinancialYear   inner join  MasterSetting on  MasterSetting.FinancialYearID=FinancialYear.FinancialYearID and MasterSetting.RowStatus=0 and MasterSetting.HospitalID = " + HospitalID + " and MasterSetting.LocationID = " + LocationID + " ", con);//Your data query goes here for searching the data
            // Note: @filter parameter must be there.
            SqlDataAdapter ad = new SqlDataAdapter();
            ad.SelectCommand = cmd;
            con.Open();

            ad.Fill(ds);
            //   con.Close();

            return ds;
        }

        public List<PatientOPD> patientCategory()
        {
            DataSet ds = new DataSet();
            Connect();
            SqlCommand cmd = new SqlCommand("select*from PatientType1 where RowStatus=0 and HospitalID = " + HospitalID + " and LocationID = " + LocationID + " ", con);//Your data query goes here for searching the data
            // Note: @filter parameter must be there.
            SqlDataAdapter ad = new SqlDataAdapter();
            ad.SelectCommand = cmd;
            con.Open();
            ad.Fill(ds);
            //   con.Close();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientOPDList.Add(
                    new PatientOPD
                    {
                        PatientCategory = dr["PatientType"].ToString(),
                       

                    });
            }
            return PatientOPDList;
           
        }

        public int BindPatientRegNoIPD(int FinancialYearID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("GetNextPatientIPDNO", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@PatientIPDNO", 0);
            cmd.Parameters["@PatientIPDNO"].Direction = ParameterDirection.Output;
            cmd.Parameters.AddWithValue("@FinancialYearID", FinancialYearID);
            con.Open();
            cmd.ExecuteNonQuery();
            int IPDNO = Convert.ToInt32(cmd.Parameters["@PatientIPDNO"].Value);

            return IPDNO;
        }



        public int BindPatientRegNoOPD(int FinancialYearID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("GetNextPatientOPDNO", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@PatientOPDNO", 0);
            cmd.Parameters["@PatientOPDNO"].Direction = ParameterDirection.Output;
            cmd.Parameters.AddWithValue("@FinancialYearID", FinancialYearID);
            con.Open();
            cmd.ExecuteNonQuery();
            int OPDNO = Convert.ToInt32(cmd.Parameters["@PatientOPDNO"].Value);

            return OPDNO;
        }
        public int BindPatientPrintRegNo(int FinancialYearID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("GetNextPatientPrintRegNo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@PatientRegNO", 0);
            cmd.Parameters["@PatientRegNO"].Direction = ParameterDirection.Output;
            cmd.Parameters.AddWithValue("@FinancialYearID", FinancialYearID);
            con.Open();
            cmd.ExecuteNonQuery();
            int Regno = Convert.ToInt32(cmd.Parameters["@PatientRegNO"].Value);

            return Regno;
        }
        public DataSet GetAllServicesWiseForIPDBills(int WardID)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@WardID", SqlDbType.Int);
                param[0].Value = WardID;
                param[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[1].Value = HospitalID;
                param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[2].Value = LocationID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetAllServicesWiseForIPDBills", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public DataSet GetAllServicesWiseForOPDBills()
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                //param[0] = new SqlParameter("@WardID", SqlDbType.Int);
                //param[0].Value = WardID;
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetAllServicesWiseForOPDBills", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
        public List<PatientOPD>GetRegistrationDate(string d, string type)
        {
      
            Connect();
           
            DataSet ds = new DataSet();
            try
            {
              
                SqlCommand cm = new SqlCommand("GetPatientRegistrationDetails", con);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@HospitalID", HospitalID);
                cm.Parameters.AddWithValue("@LocationID", LocationID);
                cm.Parameters.AddWithValue("@PatientRegNo", d);
                cm.Parameters.AddWithValue("@PatientType", type);

                SqlDataAdapter ad = new SqlDataAdapter();
                ad.SelectCommand = cm;
                con.Open();
                ad.Fill(ds);

                foreach (DataRow dr in ds.Tables[1].Rows)
                {

                    PatientOPDList.Add(new PatientOPD
                    {
                        PatientRegistrationDate =Convert.ToDateTime((dr["BillDate"])).ToString("yyyy-MM-dd"),
                       
                    });



                }
                con.Close();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return PatientOPDList;
        }
        #region GetAllPatient
        public List<PatientOPD> GetAllPatient()
        {

            DataSet ds = new DataSet();
            try
            {
                Connect();

                SqlCommand cm = new SqlCommand("GetAllPatient", con);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@HospitalID", HospitalID);
                cm.Parameters.AddWithValue("@LocationID", LocationID);

                SqlDataAdapter ad = new SqlDataAdapter();
                ad.SelectCommand = cm;
                con.Open();
                ad.Fill(ds);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    PatientOPDList.Add(new PatientOPD
                    {
                        PatientRegNo = Convert.ToInt32(dr["PatientRegNO"].ToString()),
                        printRegNo = Convert.ToInt32(dr["PrintRegNO"].ToString()),
                        PatientName = dr["PatientName"].ToString(),
                        PatientType = dr["PatientType"].ToString(),
                    });



                }
                   con.Close();


            }

            catch (Exception ex)
            {


            }
            return PatientOPDList;
        }

        #endregion

        public bool EditTPAWiseBills1(DataSet dsPatientBillsDetail,PatientOPD obj)
        {
            Connect();
            con.Open();
           
            if (dsPatientBillsDetail.Tables[0].Rows.Count>0)
            {
               


                    foreach (DataRow dr in dsPatientBillsDetail.Tables[0].Rows)
                    {
                    //if (obj.TPARate != null)
                    //{
                    //    for (int x = 0; x < obj.TPARate.Length; x++)
                    //    {



                            SqlCommand cmd1 = new SqlCommand("IUPatientBillsDetails", con);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                            cmd1.Parameters.AddWithValue("@BillNo", dr["BillNo"]);
                            cmd1.Parameters.AddWithValue("@ServiceID", dr["ServiceID"]);
                            cmd1.Parameters.AddWithValue("@ServiceName", dr["ServiceName"]);
                            cmd1.Parameters.AddWithValue("@UnitID", dr["UnitID"]);
                            cmd1.Parameters.AddWithValue("@ChargesType", dr["ChargesType"]);
                            cmd1.Parameters.AddWithValue("@Rate", dr["Rate"]);
                            cmd1.Parameters.AddWithValue("@Quantity", dr["Quantity"]);
                            cmd1.Parameters.AddWithValue("@Discount", dr["Discount"]);
                            cmd1.Parameters.AddWithValue("@Discount_Service", dr["Discount_Service"]);
                            cmd1.Parameters.AddWithValue("@DiscountServiceType", dr["DiscountServiceType"]);
                            cmd1.Parameters.AddWithValue("@Commisson", dr["Commisson"]);
                            cmd1.Parameters.AddWithValue("@DoctorCharges", dr["DoctorCharges"]);
                            cmd1.Parameters.AddWithValue("@ReffCommission", dr["ReffCommission"]);
                            cmd1.Parameters.AddWithValue("@ServiceType", dr["ServiceType"]);
                            cmd1.Parameters.AddWithValue("@TotalAmount",dr["TotalAmount"]);
                            cmd1.Parameters.AddWithValue("@HideInBilling", dr["HideInBilling"]);
                            cmd1.Parameters.AddWithValue("@DoctorID", dr["DoctorID"]);
                            cmd1.Parameters.AddWithValue("@CreationID", UserID);
                            cmd1.Parameters.AddWithValue("@ForAuthorization", dr["ForAuthorization"]);
                            cmd1.Parameters.AddWithValue("@Mode", "Add");
                            cmd1.ExecuteNonQuery();
                    //    }
                    //}
                }
            }

           
            con.Close();
            return true;
        }

        public bool EditTPAWiseBills(DataSet dsPatientBills, PatientOPD obj)
        {
            Connect();
            con.Open();
            if (dsPatientBills.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsPatientBills.Tables[0].Rows)
                {

                    SqlCommand cmd = new SqlCommand("IUPatientBills", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd.Parameters.AddWithValue("@LocationID", LocationID);
                    cmd.Parameters.AddWithValue("@BillNo", dr["BillNo"]);
                    cmd.Parameters.AddWithValue("@BillDate", dr["BillDate"]);
                    cmd.Parameters.AddWithValue("@PatientRegNO", dr["PatientRegNO"]);
                    cmd.Parameters.AddWithValue("@OPDIPDID", dr["OPDIPDID"]);
                    cmd.Parameters.AddWithValue("@BillType", dr["BillType"]);
                    cmd.Parameters.AddWithValue("@GrossAmount", dr["GrossAmount"]);
                    cmd.Parameters.AddWithValue("@TaxPercent", dr["TaxPercent"]);
                    cmd.Parameters.AddWithValue("@TaxAmount", dr["TaxAmount"]);
                    cmd.Parameters.AddWithValue("@Commisson", dr["Commisson"]);
                    cmd.Parameters.AddWithValue("@ReffCommission",dr["ReffCommission"]);
                    cmd.Parameters.AddWithValue("@TotalAmount", dr["TotalAmount"]);
                    cmd.Parameters.AddWithValue("@NonMedicalExp", dr["NonMedicalExp"]);
                    cmd.Parameters.AddWithValue("@DiscountPercent", dr["DiscountPercent"]);
                    cmd.Parameters.AddWithValue("@DiscountAmount", dr["DiscountAmount"]);
                    cmd.Parameters.AddWithValue("@DiscountReason", dr["DiscountReason"]);
                    cmd.Parameters.AddWithValue("@NetPayableAmount", dr["NetPayableAmount"]);
                    cmd.Parameters.AddWithValue("@BalanceAmount", dr["BalanceAmount"]);
                    cmd.Parameters.AddWithValue("@PreBalanceAmount", dr["PreBalanceAmount"]);
                    cmd.Parameters.AddWithValue("@IsPaid", dr["IsPaid"]);
                    cmd.Parameters.AddWithValue("@PaidAmount", dr["PaidAmount"]);
                    cmd.Parameters.AddWithValue("@DipositAmount", dr["DipositAmount"]);
                    cmd.Parameters.AddWithValue("@PaymentType", dr["PaymentType"]);
                    cmd.Parameters.AddWithValue("@Number", dr["Number"]);
                    cmd.Parameters.AddWithValue("@Name", dr["Name"]);
                    cmd.Parameters.AddWithValue("@Date", dr["Date"]);
                    cmd.Parameters.AddWithValue("@Remarks", dr["Remarks"]);
                    cmd.Parameters.AddWithValue("@SancationAmount", dr["SancationAmount"]);
                    cmd.Parameters.AddWithValue("@FinancialYearID",dr["FinancialYearID"]);
                    cmd.Parameters.AddWithValue("@CreationID", UserID);
                    cmd.Parameters.AddWithValue("@ForAuthorization", dr["ForAuthorization"]);
                    cmd.Parameters.AddWithValue("@Mode", "Edit");
                    cmd.ExecuteNonQuery();
                }
            }
            con.Close();
            return true;
        }


        public bool save(PatientOPD obj)
        {
            //   obj = new PatientOPD();
            //DataSet ds = GetAllFinancialYear();
            DataSet ds = Bind_FinancialYear();
            int PatientOPDNO;
            string FinancialYearID = ds.Tables[0].Rows[0]["FinancialYearID"].ToString();

            int RegNo = obj.PatientRegNo;

            if (obj.PatientOPDNO != null && obj.PatientOPDNO != "")
            {
                PatientOPDNO = Convert.ToInt32(obj.PatientOPDNO);
                //PatientOPDNO = 0;
            }
            else
            {
                //PatientOPDNO = Convert.ToInt32(obj.PatientOPDNO);
                PatientOPDNO = 0;
            }

            Connect();

            #region IUPatient

            bool flag = true;
            Connect();
            con.Open();
            if (obj.NewOrRevisit == "New Registration")
            {

                SqlCommand cmd = new SqlCommand("IUPatient", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                // cmd.Parameters.AddWithValue("@PatientRegNO", 0);
                //cmd.Parameters["@PatientRegNO"].Direction = ParameterDirection.Output;

                if (obj.PatientRegNo == 0)
                {
                    cmd.Parameters.AddWithValue("@PatientRegNO", 0);
                    cmd.Parameters["@PatientRegNO"].Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Mode", "Add");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PatientRegNO", obj.PatientRegNo);
                    cmd.Parameters.AddWithValue("@Mode", "Edit");
                }


                cmd.Parameters.AddWithValue("@PatientType", obj.chkIPDOPD);

                cmd.Parameters.AddWithValue("@ReferenceCode", obj.ReferenceCode);
                cmd.Parameters.AddWithValue("@PFPatientName", obj.PFPatientName);
                cmd.Parameters.AddWithValue("@PFirstName", obj.PFirstName);
                cmd.Parameters.AddWithValue("@PMiddleName", obj.PMiddleName);
                cmd.Parameters.AddWithValue("@PLastName", obj.PLastName);
                if(obj.PMiddleName=="")
                {
                    cmd.Parameters.AddWithValue("@PatientName", obj.PFirstName + " " + obj.PLastName);
                }
                else if(obj.PLastName=="")
                {
                    cmd.Parameters.AddWithValue("@PatientName", obj.PFirstName + " "+obj.PMiddleName);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PatientName", obj.PFirstName + " " + obj.PMiddleName +" "+ obj.PLastName);
                }
               
                cmd.Parameters.AddWithValue("@PFGuardianName", obj.PFGuardianName);
                cmd.Parameters.AddWithValue("@GuardianName", obj.GuardianName);

                if (obj.Gender == null || obj.Gender == "")
                {
                    cmd.Parameters.AddWithValue("@Gender", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Gender", obj.Gender);
                }
                cmd.Parameters.AddWithValue("@Age", obj.Age);
                cmd.Parameters.AddWithValue("@AgeType", "Year");
                cmd.Parameters.AddWithValue("@DateOfBirth", obj.DateOfBirth);
                cmd.Parameters.AddWithValue("@Source", obj.Source);

                if (obj.Height == null)
                {
                    cmd.Parameters.AddWithValue("@Height", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Height", obj.Height);

                }

                cmd.Parameters.AddWithValue("@BloodGroup", "");
                cmd.Parameters.AddWithValue("@BloodPressure", obj.BloodPressure);
                cmd.Parameters.AddWithValue("@Address", obj.Address);

                if (obj.CityID == null)
                {
                    cmd.Parameters.AddWithValue("@CityID", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CityID", obj.CityID);

                }


                if (obj.StateID == null)
                {
                    cmd.Parameters.AddWithValue("@StateID", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@StateID", obj.StateID);

                }

                if (obj.CountryID == null)
                {
                    cmd.Parameters.AddWithValue("@CountryID", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CountryID", obj.CountryID);

                }




                cmd.Parameters.AddWithValue("@PinCode", obj.PinCode);
                cmd.Parameters.AddWithValue("@PhoneNo", obj.PhoneNo);

                if (obj.MobileNo == null)
                {
                    cmd.Parameters.AddWithValue("@MobileNo", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@MobileNo", obj.MobileNo);

                }

                // cmd.Parameters.AddWithValue("@MobileNo", obj.MobileNo);
                cmd.Parameters.AddWithValue("@PanCardNo", obj.PanCardNo);
                cmd.Parameters.AddWithValue("@AadhaarNo", obj.AadhaarNo);
                cmd.Parameters.AddWithValue("@EmailID", obj.EmailID);
                cmd.Parameters.AddWithValue("@Photo", obj.Photo);


                cmd.Parameters.AddWithValue("@PatientCancel", obj.PatientCancel);


                if (obj.PatientCancelReason == null || obj.PatientCancelReason == "")
                {
                    cmd.Parameters.AddWithValue("@PatientCancelReason", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PatientCancelReason", obj.PatientCancelReason);

                }


                cmd.Parameters.AddWithValue("@FinancialYearID", obj.FinancialYearID);
                cmd.Parameters.AddWithValue("@CreationID", UserID);
                //  cmd.Parameters.AddWithValue("@Mode", "Add");
                // con.Open();
                cmd.ExecuteNonQuery();

                //   con.Close();

                RegNo = Convert.ToInt32(cmd.Parameters["@PatientRegNO"].Value.ToString());
                obj.PatientRegNo = RegNo;


            }

            #region PatientOPDDetails


            if (obj.chkIPDOPD == "OPD")
            {
                SqlCommand cmdPatientOPDDetail = new SqlCommand("IUPatientOPDDetails", con);
                cmdPatientOPDDetail.CommandType = CommandType.StoredProcedure;
                cmdPatientOPDDetail.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmdPatientOPDDetail.Parameters.AddWithValue("@LocationID", LocationID);
                cmdPatientOPDDetail.Parameters.AddWithValue("@PatientRegNO", RegNo);



                if (obj.PatientOPDNO == "0" || obj.PatientOPDNO == "")
                {
                    cmdPatientOPDDetail.Parameters.AddWithValue("@PatientOPDNO", 0);
                    cmdPatientOPDDetail.Parameters["@PatientOPDNO"].Direction = ParameterDirection.Output;
                    cmdPatientOPDDetail.Parameters.AddWithValue("@Mode", "Add");
                }
                else
                {
                    cmdPatientOPDDetail.Parameters.AddWithValue("@PatientOPDNO", obj.PatientOPDNO);
                    cmdPatientOPDDetail.Parameters.AddWithValue("@Mode", "Edit");
                }


                cmdPatientOPDDetail.Parameters.AddWithValue("@PatientRegistrationDate", obj.PatientRegistrationDate);// ucPatientOPD1.dtpRegDate.Text + " " + ucPatientOPD1.dtpTime.Text;


                cmdPatientOPDDetail.Parameters.AddWithValue("@PatientType", obj.PatientType);

                if (obj.TPA_ID == null || obj.TPA_ID == "" || obj.TPA_ID == "0")
                {
                    cmdPatientOPDDetail.Parameters.AddWithValue("@TPA_ID", "");
                    cmdPatientOPDDetail.Parameters.AddWithValue("@CCNClaimNo","");
                    cmdPatientOPDDetail.Parameters.AddWithValue("@OrganisationName", 0);
                    cmdPatientOPDDetail.Parameters.AddWithValue("@PolicyNo", 0);
                    cmdPatientOPDDetail.Parameters.AddWithValue("@InsuranceCompny", 0);
                  
                }
                else
                {
                    cmdPatientOPDDetail.Parameters.AddWithValue("@TPA_ID", obj.TPA_ID);
                    cmdPatientOPDDetail.Parameters.AddWithValue("@CCNClaimNo", obj.CCNClaimNo);
                    cmdPatientOPDDetail.Parameters.AddWithValue("@OrganisationName", obj.OrganizationID);
                    cmdPatientOPDDetail.Parameters.AddWithValue("@PolicyNo", obj.PolicyNo);
                    cmdPatientOPDDetail.Parameters.AddWithValue("@InsuranceCompny", obj.InsuranceCompnyID);

              
                }

                //if (obj.OrganizationID == null)
                //{
                //    cmdPatientOPDDetail.Parameters.AddWithValue("@TPA_ID", 0);
                //}
                //else
                //{
                //    cmdPatientOPDDetail.Parameters.AddWithValue("@TPA_ID", obj.OrganizationID);

                //}



                if (obj.MediclaimCardID == null || obj.MediclaimCardID=="")
                {
                    cmdPatientOPDDetail.Parameters.AddWithValue("@MediclaimCardID", 0);
                }
                else
                {
                    cmdPatientOPDDetail.Parameters.AddWithValue("@MediclaimCardID", obj.MediclaimCardID);

                }

                cmdPatientOPDDetail.Parameters.AddWithValue("@TemporaryDoctorStatus", obj.TemporaryDoctorStatus);
                cmdPatientOPDDetail.Parameters.AddWithValue("@TemporaryDoctor", obj.TemporaryDoctor);

                cmdPatientOPDDetail.Parameters.AddWithValue("@RefferedTemporaryDoctorStatus", obj.RefferedTemporaryDoctorStatus);

                cmdPatientOPDDetail.Parameters.AddWithValue("@RefferedTemporaryDoctor", obj.RefferedTemporaryDoctor);

                cmdPatientOPDDetail.Parameters.AddWithValue("@DoctorID", obj.DoctorID);
                cmdPatientOPDDetail.Parameters.AddWithValue("@DepartmentID", obj.DepartmentID);
                cmdPatientOPDDetail.Parameters.AddWithValue("@ReferredByDoctorID", obj.ReferredByDoctorID);

                if (obj.Weight == null)
                {
                    cmdPatientOPDDetail.Parameters.AddWithValue("@Weight", 0);
                }
                else
                {
                    cmdPatientOPDDetail.Parameters.AddWithValue("@Weight", obj.Weight);

                }

                //  cmdPatientOPDDetail.Parameters.AddWithValue("@Weight", obj.Weight);
                cmdPatientOPDDetail.Parameters.AddWithValue("@BloodPressure", obj.BloodPressure);
                cmdPatientOPDDetail.Parameters.AddWithValue("@Reason", obj.Reason);
                cmdPatientOPDDetail.Parameters.AddWithValue("@RegistrationCharges", "00.0");
                cmdPatientOPDDetail.Parameters.AddWithValue("@ConsultionCharges", "00.0");
                cmdPatientOPDDetail.Parameters.AddWithValue("@DiscountInPer", "00.0");
                cmdPatientOPDDetail.Parameters.AddWithValue("@DiscountInRS", "00.0");
                cmdPatientOPDDetail.Parameters.AddWithValue("@TotalAmount", "00.0");

                if (obj.PaymentType != null && obj.PaymentType!= "CASHLESS")
                {
                    cmdPatientOPDDetail.Parameters.AddWithValue("@PaymentType", obj.PaymentType);
                    cmdPatientOPDDetail.Parameters.AddWithValue("@Number", obj.Number);
                    cmdPatientOPDDetail.Parameters.AddWithValue("@Name", obj.Name);
                    cmdPatientOPDDetail.Parameters.AddWithValue("@Date", Convert.ToDateTime(obj.Date));
                    cmdPatientOPDDetail.Parameters.AddWithValue("@Remark", obj.Remark);
                }
                else
                {
                    cmdPatientOPDDetail.Parameters.AddWithValue("@PaymentType", "");
                    cmdPatientOPDDetail.Parameters.AddWithValue("@Number", "");
                    cmdPatientOPDDetail.Parameters.AddWithValue("@Name", "");
                    cmdPatientOPDDetail.Parameters.AddWithValue("@Date", System.DateTime.Now);
                    cmdPatientOPDDetail.Parameters.AddWithValue("@Remark", "");
                }

                //cmdPatientOPDDetail.Parameters.AddWithValue("@Source", obj.Source);
                cmdPatientOPDDetail.Parameters.AddWithValue("@DiscountReason", "");
                cmdPatientOPDDetail.Parameters.AddWithValue("@CreationID", UserID);
                cmdPatientOPDDetail.Parameters.AddWithValue("@FinancialYearID", FinancialYearID);

                //    cmdPatientOPDDetail.Parameters.AddWithValue("@Mode", "Add");
                // con.Open();

                int OPDvalue = cmdPatientOPDDetail.ExecuteNonQuery();

                //   con.Close();

                PatientOPDNO = Convert.ToInt32(cmdPatientOPDDetail.Parameters["@PatientOPDNO"].Value.ToString());

                obj.PatientOPDNO = PatientOPDNO.ToString();

            }

            #endregion

            #region PatientIPDDetails
            string Mode = "";
            if (obj.chkIPDOPD == "IPD")
            {

                SqlCommand PatientIPDDetails = new SqlCommand("IUPatientIPDDetails", con);
                PatientIPDDetails.CommandType = CommandType.StoredProcedure;
                PatientIPDDetails.Parameters.AddWithValue("@HospitalID", HospitalID);
                PatientIPDDetails.Parameters.AddWithValue("@LocationID", LocationID);
                PatientIPDDetails.Parameters.AddWithValue("@PatientRegNO", RegNo);





                if (obj.PatientIPDNO == "0" || obj.PatientIPDNO == "")
                {
                    PatientIPDDetails.Parameters.AddWithValue("@PatientIPDNO", 0);
                    PatientIPDDetails.Parameters["@PatientIPDNO"].Direction = ParameterDirection.Output;
                    PatientIPDDetails.Parameters.AddWithValue("@Mode", "Add");
                    Mode = "Add";
                }
                else
                {
                    PatientIPDDetails.Parameters.AddWithValue("@PatientIPDNO", obj.PatientIPDNO);
                    PatientIPDDetails.Parameters.AddWithValue("@Mode", "Edit");
                    Mode = "Edit";
                }



                PatientIPDDetails.Parameters.AddWithValue("@PatientOPDNO", PatientOPDNO);


                PatientIPDDetails.Parameters.AddWithValue("@PatientType", obj.PatientType);
                PatientIPDDetails.Parameters.AddWithValue("@AddmissionDate", obj.PatientRegistrationDate);


                PatientIPDDetails.Parameters.AddWithValue("@ConsDr1", obj.ConsDr1);
                PatientIPDDetails.Parameters.AddWithValue("@NameOfPackage", obj.NameOfPackage);
                PatientIPDDetails.Parameters.AddWithValue("@Medical_Management", obj.Medical_Management);
                PatientIPDDetails.Parameters.AddWithValue("@Elective_Surgery", obj.Elective_Surgery);
                PatientIPDDetails.Parameters.AddWithValue("@PatientCategoryID", obj.PatientCategory);

                if (obj.MediclaimCardID == null)
                {
                    PatientIPDDetails.Parameters.AddWithValue("@MediclaimCardID", 0);
                }
                else
                {
                    PatientIPDDetails.Parameters.AddWithValue("@MediclaimCardID", obj.MediclaimCardID);

                }

                if (obj.TPA_ID == null || obj.TPA_ID == "" || obj.TPA_ID == "0")
                {
                    PatientIPDDetails.Parameters.AddWithValue("@TPA_ID", "");
                    PatientIPDDetails.Parameters.AddWithValue("@CCNClaimNo", "");
                    PatientIPDDetails.Parameters.AddWithValue("@OrganisationName", 0);
                    PatientIPDDetails.Parameters.AddWithValue("@PolicyNo", 0);
                    PatientIPDDetails.Parameters.AddWithValue("@InsuranceCompny", 0);
                    PatientIPDDetails.Parameters.AddWithValue("@AddmissionType", "General");
                }
                else
                {
                    PatientIPDDetails.Parameters.AddWithValue("@TPA_ID", obj.TPA_ID);
                    PatientIPDDetails.Parameters.AddWithValue("@CCNClaimNo", obj.CCNClaimNo);
                    PatientIPDDetails.Parameters.AddWithValue("@AddmissionType", "TPA");
                    PatientIPDDetails.Parameters.AddWithValue("@OrganisationName", obj.OrganizationID);
                    PatientIPDDetails.Parameters.AddWithValue("@PolicyNo", obj.PolicyNo);

                    //if (obj.InsuranceCompnyID == null)
                    //{
                    //    PatientIPDDetails.Parameters.AddWithValue("@InsuranceCompny", 0);
                    //}
                    //else
                    //{
                    PatientIPDDetails.Parameters.AddWithValue("@InsuranceCompny", obj.InsuranceCompnyID);

                    //    }

                }

                PatientIPDDetails.Parameters.AddWithValue("@TPAStatus", obj.TPAStatus);

                if (obj.TemporaryDoctorStatus == "" || obj.TemporaryDoctorStatus == null)
                {
                    PatientIPDDetails.Parameters.AddWithValue("@TemporaryDoctorStatus", false);
                }
                else
                {
                    PatientIPDDetails.Parameters.AddWithValue("@TemporaryDoctorStatus", obj.TemporaryDoctorStatus);
                }


                if (obj.TemporaryDoctor == "" || obj.TemporaryDoctor == null)
                {
                    PatientIPDDetails.Parameters.AddWithValue("@TemporaryDoctor", "");
                }
                else
                {
                    PatientIPDDetails.Parameters.AddWithValue("@TemporaryDoctor", obj.TemporaryDoctor);
                }



                if (obj.RefferedTemporaryDoctorStatus == "" || obj.RefferedTemporaryDoctorStatus == null)
                {
                    PatientIPDDetails.Parameters.AddWithValue("@RefferedTemporaryDoctorStatus", false);
                }
                else
                {
                    PatientIPDDetails.Parameters.AddWithValue("RefferedTemporaryDoctorStatus", obj.RefferedTemporaryDoctorStatus);
                }



                if (obj.RefferedTemporaryDoctor == "" || obj.RefferedTemporaryDoctor == null)
                {
                    PatientIPDDetails.Parameters.AddWithValue("@RefferedTemporaryDoctor", "");
                }
                else
                {
                    PatientIPDDetails.Parameters.AddWithValue("RefferedTemporaryDoctor", obj.RefferedTemporaryDoctor);
                }





                PatientIPDDetails.Parameters.AddWithValue("@ConsultantDrID", obj.DoctorID);
                PatientIPDDetails.Parameters.AddWithValue("@DepartmentID", obj.DepartmentID);
                PatientIPDDetails.Parameters.AddWithValue("@ReferredByDoctorID", obj.ReferredByDoctorID);
                PatientIPDDetails.Parameters.AddWithValue("@Weight", obj.Weight);
                PatientIPDDetails.Parameters.AddWithValue("@BloodPressure", obj.BloodPressure);
                PatientIPDDetails.Parameters.AddWithValue("@Reason", obj.Reason);

                if (obj.AdvanceAmt == null || obj.AdvanceAmt == "")
                {
                    PatientIPDDetails.Parameters.AddWithValue("@AdvanceAmount", "0.00");
                }
                else
                {
                    PatientIPDDetails.Parameters.AddWithValue("@AdvanceAmount", obj.AdvanceAmt);
                }
                PatientIPDDetails.Parameters.AddWithValue("@AdmissionCharges", "0.00");
                PatientIPDDetails.Parameters.AddWithValue("@ConsultantCharges", "0.00");
                PatientIPDDetails.Parameters.AddWithValue("@BedCharges", "00.0");
                PatientIPDDetails.Parameters.AddWithValue("@PaidAmount", "0.00");

                if (obj.AdvanceAmt == null || obj.AdvanceAmt == "")
                {
                    PatientIPDDetails.Parameters.AddWithValue("@BalanceAmount", "0.00");
                }
                else
                {
                    PatientIPDDetails.Parameters.AddWithValue("@BalanceAmount", (-Convert.ToDecimal(obj.AdvanceAmt)));
                }
                PatientIPDDetails.Parameters.AddWithValue("@DiscountPercentage", "0.00");
                PatientIPDDetails.Parameters.AddWithValue("@DiscountAmount", "0.00");
                PatientIPDDetails.Parameters.AddWithValue("@DiscountReason", "");
                //if (obj.PaymentType != null)
                //{
                //    PatientIPDDetails.Parameters.AddWithValue("@PaymentType", obj.PaymentType);
                //}
                //else
                //{
                //    PatientIPDDetails.Parameters.AddWithValue("@PaymentType", "");
                //}
                if (obj.PaymentType != null)
                {
                    PatientIPDDetails.Parameters.AddWithValue("@PaymentType", obj.PaymentType);
                    PatientIPDDetails.Parameters.AddWithValue("@Number", obj.Number);
                    PatientIPDDetails.Parameters.AddWithValue("@Name", obj.Name);
                    PatientIPDDetails.Parameters.AddWithValue("@Date", System.DateTime.Now);
                    PatientIPDDetails.Parameters.AddWithValue("@Remark", obj.Remark);
                }
                else
                {
                    PatientIPDDetails.Parameters.AddWithValue("@PaymentType", "");
                    PatientIPDDetails.Parameters.AddWithValue("@Number", "");
                    PatientIPDDetails.Parameters.AddWithValue("@Name", "");
                    PatientIPDDetails.Parameters.AddWithValue("@Date", System.DateTime.Now);
                    PatientIPDDetails.Parameters.AddWithValue("@Remark", obj.Remark);
                }
                //switch (obj.PaymentType)
                //{
                //    case "Cheque":
                //        PatientIPDDetails.Parameters.AddWithValue("@Number", obj.Number);
                //        PatientIPDDetails.Parameters.AddWithValue("@Name", obj.Name);
                //        PatientIPDDetails.Parameters.AddWithValue("@Date", obj.PaymentDate);
                //        PatientIPDDetails.Parameters.AddWithValue("@Remark", obj.Remark);
                //        break;
                //    case "Debit Card":
                //        PatientIPDDetails.Parameters.AddWithValue("@Number", obj.Number);
                //        PatientIPDDetails.Parameters.AddWithValue("@Name", obj.Name);
                //        PatientIPDDetails.Parameters.AddWithValue("@Date", obj.PaymentDate);
                //        PatientIPDDetails.Parameters.AddWithValue("@Remark", obj.Remark);
                //        break;
                //    case "Credit Card":
                //        PatientIPDDetails.Parameters.AddWithValue("@Number", obj.Number);
                //        PatientIPDDetails.Parameters.AddWithValue("@Name", obj.Name);
                //        PatientIPDDetails.Parameters.AddWithValue("@Date", obj.PaymentDate);
                //        PatientIPDDetails.Parameters.AddWithValue("@Remark", obj.Remark);
                //        break;
                //    case "E-Money":
                //        PatientIPDDetails.Parameters.AddWithValue("@Number", obj.Number);
                //        PatientIPDDetails.Parameters.AddWithValue("@Name", obj.Name);
                //        PatientIPDDetails.Parameters.AddWithValue("@Date", obj.PaymentDate);
                //        PatientIPDDetails.Parameters.AddWithValue("@Remark", obj.Remark);
                //        break;
                //    case "EFT":
                //        PatientIPDDetails.Parameters.AddWithValue("@Number", obj.Number);
                //        PatientIPDDetails.Parameters.AddWithValue("@Name", obj.Name);
                //        PatientIPDDetails.Parameters.AddWithValue("@Date", obj.PaymentDate);
                //        PatientIPDDetails.Parameters.AddWithValue("@Remark", obj.Remark);
                //        break;
                //    default:
                //        PatientIPDDetails.Parameters.AddWithValue("@Number", "");
                //        PatientIPDDetails.Parameters.AddWithValue("@Name", "");
                //        // PatientIPDDetails.Parameters.AddWithValue("@Date", "");//ucPatientOPD1.dtpRegDate.Text + " " + ucPatientOPD1.dtpTime.Text;
                //        //PatientIPDDetails.Parameters.Add(new SqlParameter("@Date", DateTime.ParseExact(obj.AddmissionDate, "dd/MM/yyyy hh:mm:ss", null)));

                //        PatientIPDDetails.Parameters.Add(new SqlParameter("@Date", DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), "dd/MM/yyyy hh:mm:ss", null)));

                //        PatientIPDDetails.Parameters.AddWithValue("@Remark", "");
                //        break;
                //}


                PatientIPDDetails.Parameters.AddWithValue("@FinancialYearID", FinancialYearID);
                PatientIPDDetails.Parameters.AddWithValue("@CreationID", UserID);
                // PatientIPDDetails.Parameters.AddWithValue("@LastModificationID", 1);

                // PatientIPDDetails.Parameters.AddWithValue("@Mode", "Add");
                // con.Open();

                int IPDValue = PatientIPDDetails.ExecuteNonQuery();

                //     con.Close();
                int PatientIPDNO = Convert.ToInt32(PatientIPDDetails.Parameters["@PatientIPDNO"].Value.ToString());
                obj.PatientIPDNO = (PatientIPDNO).ToString();

            #endregion


                #region PatientIPDWardDetails

                if (IPDValue > 0)
                {
                    SqlCommand cmdPatientIPDwardDetails = new SqlCommand("IUPatientIPDWardDetails", con);
                    cmdPatientIPDwardDetails.CommandType = CommandType.StoredProcedure;
                    cmdPatientIPDwardDetails.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmdPatientIPDwardDetails.Parameters.AddWithValue("@LocationID", LocationID);
                    cmdPatientIPDwardDetails.Parameters.AddWithValue("@PatientRegNO", RegNo);
                    cmdPatientIPDwardDetails.Parameters.AddWithValue("@PatientIPDNO", obj.PatientIPDNO);

                    if (obj.ApplyWardID != "" && obj.ApplyWardID != null)
                    {
                        cmdPatientIPDwardDetails.Parameters.AddWithValue("@ApplyWardID", obj.ApplyWardID);
                    }
                    else
                    {
                        cmdPatientIPDwardDetails.Parameters.AddWithValue("@ApplyWardID", 0);
                    }

                    if (obj.WardName != "" && obj.WardName != null)
                    {
                        cmdPatientIPDwardDetails.Parameters.AddWithValue("@WardID", obj.WardID);
                    }
                    if (obj.RoomNo != "" && obj.RoomNo != null)
                    {
                        cmdPatientIPDwardDetails.Parameters.AddWithValue("@RoomID", obj.RoomID);
                    }

                    if (obj.BedNO != "" && obj.BedNO != null)
                    {
                        cmdPatientIPDwardDetails.Parameters.AddWithValue("@BedID", obj.BedID);
                    }


                    cmdPatientIPDwardDetails.Parameters.AddWithValue("@BedCharges", "0.00");


                    cmdPatientIPDwardDetails.Parameters.AddWithValue("EnterDateTime", Convert.ToDateTime(obj.PatientRegistrationDate));

                    cmdPatientIPDwardDetails.Parameters.AddWithValue("IsCurrentBed", 0);
                    cmdPatientIPDwardDetails.Parameters.AddWithValue("CreationID", UserID);
                    cmdPatientIPDwardDetails.Parameters.AddWithValue("Mode", Mode);
                    //  con.Open();

                    int wardValue = cmdPatientIPDwardDetails.ExecuteNonQuery();

                    // con.Close();
                }

                #endregion


                # region  Security Deposite

                if (obj.TPAStatus == "true")
                {
                    SqlCommand cmdSecurityDeposite = new SqlCommand("IUSecurityDeposite", con);
                    cmdSecurityDeposite.CommandType = CommandType.StoredProcedure;
                    cmdSecurityDeposite.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmdSecurityDeposite.Parameters.AddWithValue("@LocationID", LocationID);
                    cmdSecurityDeposite.Parameters.AddWithValue("@PatientRegNO", RegNo);

                    if (obj.chkIPDOPD == "IPD")
                    {
                        cmdSecurityDeposite.Parameters.AddWithValue("@OPDIPDID", obj.PatientIPDNO);
                    }
                    else
                    {
                        cmdSecurityDeposite.Parameters.AddWithValue("@OPDIPDID", obj.PatientOPDNO);
                    }

                    if (obj.SecurityDeposityID == "0" || obj.SecurityDeposityID == "")
                    {
                        cmdSecurityDeposite.Parameters.AddWithValue("@SecurityDeposityID", 0);
                        cmdSecurityDeposite.Parameters["@SecurityDeposityID"].Direction = ParameterDirection.Output;
                        cmdSecurityDeposite.Parameters.AddWithValue("@Mode", "Add");
                    }
                    else
                    {
                        cmdSecurityDeposite.Parameters.AddWithValue("@SecurityDeposityID", obj.SecurityDeposityID);
                        cmdSecurityDeposite.Parameters.AddWithValue("@Mode", "Edit");
                        // Mode = "Edit";
                    }

                    cmdSecurityDeposite.Parameters.AddWithValue("@PatientAccountRowID", 0);
                    if (obj.PaymentType != null)
                    {
                        cmdSecurityDeposite.Parameters.AddWithValue("@PaymentType", obj.PaymentType);
                    }
                    else
                    {
                        cmdSecurityDeposite.Parameters.AddWithValue("@PaymentType", "");
                    }
                    switch (obj.PaymentType)
                    {
                        case "Cheque":
                            cmdSecurityDeposite.Parameters.AddWithValue("@Number", obj.Number);
                            cmdSecurityDeposite.Parameters.AddWithValue("@Name", obj.Name);
                            cmdSecurityDeposite.Parameters.AddWithValue("@Date", obj.PaymentDate);
                            cmdSecurityDeposite.Parameters.AddWithValue("@Remark", obj.Remark);
                            break;
                        case "Debit Card":
                            cmdSecurityDeposite.Parameters.AddWithValue("@Number", obj.Number);
                            cmdSecurityDeposite.Parameters.AddWithValue("@Name", obj.Name);
                            cmdSecurityDeposite.Parameters.AddWithValue("@Date", obj.PaymentDate);
                            cmdSecurityDeposite.Parameters.AddWithValue("@Remark", obj.Remark);
                            break;
                        case "Credit Card":
                            cmdSecurityDeposite.Parameters.AddWithValue("@Number", obj.Number);
                            cmdSecurityDeposite.Parameters.AddWithValue("@Name", obj.Name);
                            cmdSecurityDeposite.Parameters.AddWithValue("@Date", obj.PaymentDate);
                            cmdSecurityDeposite.Parameters.AddWithValue("@Remark", obj.Remark);
                            break;
                        case "E-Money":
                            cmdSecurityDeposite.Parameters.AddWithValue("@Number", obj.Number);
                            cmdSecurityDeposite.Parameters.AddWithValue("@Name", obj.Name);
                            cmdSecurityDeposite.Parameters.AddWithValue("@Date", obj.PaymentDate);
                            cmdSecurityDeposite.Parameters.AddWithValue("@Remark", obj.Remark);
                            break;
                        case "EFT":
                            cmdSecurityDeposite.Parameters.AddWithValue("@Number", obj.Number);
                            cmdSecurityDeposite.Parameters.AddWithValue("@Name", obj.Name);
                            cmdSecurityDeposite.Parameters.AddWithValue("@Date", obj.PaymentDate);
                            cmdSecurityDeposite.Parameters.AddWithValue("@Remark", obj.Remark);
                            break;
                        default:
                            cmdSecurityDeposite.Parameters.AddWithValue("@Number", "");
                            cmdSecurityDeposite.Parameters.AddWithValue("@Name", "");
                            // PatientIPDDetails.Parameters.AddWithValue("@Date", "");//ucPatientOPD1.dtpRegDate.Text + " " + ucPatientOPD1.dtpTime.Text;
                            //PatientIPDDetails.Parameters.Add(new SqlParameter("@Date", DateTime.ParseExact(obj.AddmissionDate, "dd/MM/yyyy hh:mm:ss", null)));

                            cmdSecurityDeposite.Parameters.Add(new SqlParameter("@Date", DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), "dd/MM/yyyy hh:mm:ss", null)));

                            cmdSecurityDeposite.Parameters.AddWithValue("@Remark", "");
                            break;
                    }

                    if (obj.chkIPDOPD == "IPD")
                    {
                        cmdSecurityDeposite.Parameters.AddWithValue("@BillType", "IPDSecurityDeposit");
                    }
                    else
                    {
                        cmdSecurityDeposite.Parameters.AddWithValue("@BillType", "OPDSecurityDeposit");
                    }


                    //cmdSecurityDeposite.Parameters.AddWithValue("@PrintSecurityDeposite", obj.PrintSecurityDeposite);

                    if (obj.DrAmount == null)
                    {
                        cmdSecurityDeposite.Parameters.AddWithValue("@DrAmount", "0.00");
                    }
                    else
                    {
                        cmdSecurityDeposite.Parameters.AddWithValue("@DrAmount", obj.DrAmount);
                    }

                    if (obj.CrAmount == null)
                    {
                        cmdSecurityDeposite.Parameters.AddWithValue("@CrAmount", "0.00");
                    }
                    else
                    {
                        cmdSecurityDeposite.Parameters.AddWithValue("@CrAmount", obj.CrAmount);
                    }
                    cmdSecurityDeposite.Parameters.AddWithValue("@FinancialYearID", FinancialYearID);
                    cmdSecurityDeposite.Parameters.AddWithValue("@CreationID", UserID);
                    //  cmdSecurityDeposite.Parameters.AddWithValue("Mode", Mode);
                    //    con.Open();

                    int Value = cmdSecurityDeposite.ExecuteNonQuery();

                    //  con.Close();



                }
                if (Mode == "Edit")
                {
                    if (obj.TPAStatus == "false")
                    {


                        SqlCommand cmdSecurityDeposite = new SqlCommand("IUSecurityDeposite", con);
                        cmdSecurityDeposite.CommandType = CommandType.StoredProcedure;
                        cmdSecurityDeposite.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmdSecurityDeposite.Parameters.AddWithValue("@LocationID", LocationID);
                        cmdSecurityDeposite.Parameters.AddWithValue("@PatientRegNO", RegNo);

                        if (obj.chkIPDOPD == "IPD")
                        {
                            cmdSecurityDeposite.Parameters.AddWithValue("@OPDIPDID", obj.PatientIPDNO);
                        }
                        else
                        {
                            cmdSecurityDeposite.Parameters.AddWithValue("@OPDIPDID", obj.PatientOPDNO);
                        }

                        if (obj.SecurityDeposityID == "0" || obj.SecurityDeposityID == "")
                        {
                            cmdSecurityDeposite.Parameters.AddWithValue("@SecurityDeposityID", 0);
                            cmdSecurityDeposite.Parameters["@SecurityDeposityID"].Direction = ParameterDirection.Output;
                            cmdSecurityDeposite.Parameters.AddWithValue("@Mode", "Add");
                        }
                        else
                        {
                            cmdSecurityDeposite.Parameters.AddWithValue("@SecurityDeposityID", obj.SecurityDeposityID);
                            cmdSecurityDeposite.Parameters.AddWithValue("@Mode", "Edit");
                            // Mode = "Edit";
                        }

                        cmdSecurityDeposite.Parameters.AddWithValue("@PatientAccountRowID", 0);
                        if (obj.PaymentType != null)
                        {
                            cmdSecurityDeposite.Parameters.AddWithValue("@PaymentType", obj.PaymentType);
                        }
                        else
                        {
                            cmdSecurityDeposite.Parameters.AddWithValue("@PaymentType", "");
                        }
                        switch (obj.PaymentType)
                        {
                            case "Cheque":
                                cmdSecurityDeposite.Parameters.AddWithValue("@Number", obj.Number);
                                cmdSecurityDeposite.Parameters.AddWithValue("@Name", obj.Name);
                                cmdSecurityDeposite.Parameters.AddWithValue("@Date", obj.PaymentDate);
                                cmdSecurityDeposite.Parameters.AddWithValue("@Remark", obj.Remark);
                                break;
                            case "Debit Card":
                                cmdSecurityDeposite.Parameters.AddWithValue("@Number", obj.Number);
                                cmdSecurityDeposite.Parameters.AddWithValue("@Name", obj.Name);
                                cmdSecurityDeposite.Parameters.AddWithValue("@Date", obj.PaymentDate);
                                cmdSecurityDeposite.Parameters.AddWithValue("@Remark", obj.Remark);
                                break;
                            case "Credit Card":
                                cmdSecurityDeposite.Parameters.AddWithValue("@Number", obj.Number);
                                cmdSecurityDeposite.Parameters.AddWithValue("@Name", obj.Name);
                                cmdSecurityDeposite.Parameters.AddWithValue("@Date", obj.PaymentDate);
                                cmdSecurityDeposite.Parameters.AddWithValue("@Remark", obj.Remark);
                                break;
                            case "E-Money":
                                cmdSecurityDeposite.Parameters.AddWithValue("@Number", obj.Number);
                                cmdSecurityDeposite.Parameters.AddWithValue("@Name", obj.Name);
                                cmdSecurityDeposite.Parameters.AddWithValue("@Date", obj.PaymentDate);
                                cmdSecurityDeposite.Parameters.AddWithValue("@Remark", obj.Remark);
                                break;
                            case "EFT":
                                cmdSecurityDeposite.Parameters.AddWithValue("@Number", obj.Number);
                                cmdSecurityDeposite.Parameters.AddWithValue("@Name", obj.Name);
                                cmdSecurityDeposite.Parameters.AddWithValue("@Date", obj.PaymentDate);
                                cmdSecurityDeposite.Parameters.AddWithValue("@Remark", obj.Remark);
                                break;
                            default:
                                cmdSecurityDeposite.Parameters.AddWithValue("@Number", "");
                                cmdSecurityDeposite.Parameters.AddWithValue("@Name", "");
                                // PatientIPDDetails.Parameters.AddWithValue("@Date", "");//ucPatientOPD1.dtpRegDate.Text + " " + ucPatientOPD1.dtpTime.Text;
                                //PatientIPDDetails.Parameters.Add(new SqlParameter("@Date", DateTime.ParseExact(obj.AddmissionDate, "dd/MM/yyyy hh:mm:ss", null)));

                                cmdSecurityDeposite.Parameters.Add(new SqlParameter("@Date", DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), "dd/MM/yyyy hh:mm:ss", null)));

                                cmdSecurityDeposite.Parameters.AddWithValue("@Remark", "");
                                break;
                        }

                        if (obj.chkIPDOPD == "IPD")
                        {
                            cmdSecurityDeposite.Parameters.AddWithValue("@BillType", "IPDSecurityDeposit");
                        }
                        else
                        {
                            cmdSecurityDeposite.Parameters.AddWithValue("@BillType", "OPDSecurityDeposit");
                        }


                        //cmdSecurityDeposite.Parameters.AddWithValue("@PrintSecurityDeposite", obj.PrintSecurityDeposite);

                        if (obj.DrAmount == null)
                        {
                            cmdSecurityDeposite.Parameters.AddWithValue("@DrAmount", "0.00");
                        }
                        else
                        {
                            cmdSecurityDeposite.Parameters.AddWithValue("@DrAmount", obj.DrAmount);
                        }

                        if (obj.CrAmount == null)
                        {
                            cmdSecurityDeposite.Parameters.AddWithValue("@CrAmount", "0.00");
                        }
                        else
                        {
                            cmdSecurityDeposite.Parameters.AddWithValue("@CrAmount", obj.CrAmount);
                        }
                        cmdSecurityDeposite.Parameters.AddWithValue("@FinancialYearID", FinancialYearID);
                        cmdSecurityDeposite.Parameters.AddWithValue("@CreationID", UserID);
                        //  cmdSecurityDeposite.Parameters.AddWithValue("Mode", Mode);
                        //    con.Open();

                        int Value = cmdSecurityDeposite.ExecuteNonQuery();

                    }
                }
                con.Close();

                #endregion
              

            }





            return true;
        }

            #endregion


        public string GetPatientPrintNo_ToRegNo(string PrintRegNO,string PatientType)
        {
            DataSet ds = new DataSet();
            Connect();
            SqlCommand cmd = new SqlCommand("select*from Patient where  Patient.PrintRegNO = "+ PrintRegNO +"  and  HospitalID="+HospitalID+" and LocationID="+LocationID+ " and RowStatus=0 and PatientType like '" + PatientType+"'", con);//Your data query goes here for searching the data
            // Note: @filter parameter must be there.
            SqlDataAdapter ad = new SqlDataAdapter();
            ad.SelectCommand = cmd;
            con.Open();
            ad.Fill(ds);
            string RegNo = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                RegNo= ds.Tables[0].Rows[0]["PatientRegNO"].ToString();
            }
            return RegNo;
        }

        public string GetPrintNo_ToRegNo(string PrintRegNO)
        {
            DataSet ds = new DataSet();
            Connect();
            SqlCommand cmd = new SqlCommand("select*from Patient where  Patient.PrintRegNO=" + PrintRegNO + " and  HospitalID=" + HospitalID + " and LocationID=" + LocationID + " and RowStatus=0 ", con);//Your data query goes here for searching the data
            // Note: @filter parameter must be there.
            SqlDataAdapter ad = new SqlDataAdapter();
            ad.SelectCommand = cmd;
            con.Open();
            ad.Fill(ds);
            string RegNo = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                RegNo = ds.Tables[0].Rows[0]["PatientRegNO"].ToString();
            }
            return RegNo;
        }
    }
}