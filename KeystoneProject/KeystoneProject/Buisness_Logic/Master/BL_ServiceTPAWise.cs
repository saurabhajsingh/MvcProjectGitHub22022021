using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using KeystoneProject.Models.Master;
using System.Data.OleDb;
namespace KeystoneProject.Buisness_Logic.Master
{
    public class BL_ServiceTPAWise
    {
        int HospitalID = 0;
        int LocationID = 0;
        int UserID = 0;

        private SqlConnection con;
        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
            HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        }

     

        public DataSet SelectAllServiceTPAWise()
        {
            Connect();
            DataSet dsServiceTPAWise = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllServicesTPAWise", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                //cmd.CommandTimeout = 70000;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                dsServiceTPAWise = new DataSet();
                da.Fill(dsServiceTPAWise);
                con.Close();

                return dsServiceTPAWise;
            }
            catch (Exception)
            {
                return dsServiceTPAWise;
            }

        }
        public DataSet GetWardDetailsPivot()
        {
            List<string> add = new List<string>();
            Connect();
            DataSet ds = new DataSet();
            try
            {

                SqlCommand cmd = new SqlCommand("GetWardDetailsPivot", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                //cmd.CommandTimeout = 70000;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                ds = new DataSet();
                da.Fill(ds);
                
                //DataSet dscolumn = new DataSet();
                //dscolumn.Tables.Add(new DataTable());
                con.Close();
                //SqlParameter[] param = new SqlParameter[2];
                //param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                //param[0].Value = HospitalID;
                //param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                //param[1].Value = LocationID;
                //ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetWardDetailsPivot", param);
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    foreach (DataColumn dc in ds.Tables[0].Columns)
                //    {
                //        add.Add(dc.ColumnName);
                //        //add.Add(string,{dc.ColumnName
                //        //});


                //    }
                //}
                return ds;
            }
            catch (Exception exp)
            {
                return ds;
            }
            //catch (Exception ex)
            //{
            //    ex.Data.Add("returnValue", "0");
            //    ExceptionManager.Publish(ex);
            //    throw ex;
            //}

        }
        public DataSet GetServicesTPAWise(int ServicesTPAWiseID)
        {
            //            SqlConnection con = null;
            //#pragma warning disable CS0219 // The variable 'result' is assigned but its value is never used
            //            string result = "";
            //#pragma warning restore CS0219 // The variable 'result' is assigned but its value is never used

            //            DataSet ds = null;
            Connect();
            DataSet ds = new DataSet();

            try
            {
               // con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());

                SqlCommand cmd = new SqlCommand("GetServicesTPAWise", con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@HospitalID ",HospitalID);
                cmd.Parameters.AddWithValue("@LocationID ", LocationID);
                cmd.Parameters.AddWithValue("@ServicesTPAWiseID ", ServicesTPAWiseID);


                con.Open();

                SqlDataAdapter da = new SqlDataAdapter();

                da.SelectCommand = cmd;

                ds = new DataSet();

                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch
            {
                return ds;
            }
            finally
            {
                con.Close();
            }

        }

        public DataSet GetServicesTPAWiseDetail(int ServicesTPAWiseID)
        {
            SqlConnection con = null;
#pragma warning disable CS0219 // The variable 'result' is assigned but its value is never used
            string result = "";
#pragma warning restore CS0219 // The variable 'result' is assigned but its value is never used

            DataSet ds = null;
            try
            {
                Connect();
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());

                SqlCommand cmd = new SqlCommand("GetServicesDetailsTPAWise", con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@HospitalID ", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID ", LocationID);
                cmd.Parameters.AddWithValue("@ServicesTPAWiseID ", ServicesTPAWiseID);


                con.Open();

                SqlDataAdapter da = new SqlDataAdapter();

                da.SelectCommand = cmd;

                ds = new DataSet();

                da.Fill(ds);

                return ds;
            }
            catch
            {
                return ds;
            }
            finally
            {
                con.Close();
            }

        }

        public DataSet GetOrganisation(string filter)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select  OrganizationID,OrganizationName  from  Organization  where   HospitalID=" + HospitalID + " and LocationID=" + LocationID + " and OrganizationName  like  '%'+@filter+'%' and RowStatus=0 and OrganizationID !=0  and OrganizationType ='T.P.A' order by  OrganizationName asc", con);
            cmd.Parameters.AddWithValue("@filter", filter);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;

        }

        public DataSet GetUnit(string filter1)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select  UnitID,UnitName from  Unit  where UnitName  like '%'+@filter1+'%' and  HospitalID=" + HospitalID + " and LocationID=" + LocationID + " and RowStatus=0 order by  UnitName asc", con);
            cmd.Parameters.AddWithValue("@filter1", filter1);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;

        }


        #region Delete Services
        public int DeleteServicesTPAWise(int ServiceID, string ServiceType)
        {
            //string Table = string.Empty;
           
                //DataSet dsService = new DataSet();
                //dsService = SelectAllServicesbyID(ServiceID);
               // ServiceType = dsService.Tables[0].Rows[0]["ServiceType"].ToString();

                Connect();
                SqlCommand cmd = new SqlCommand("DeleteServicesTPAWise", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@ServiceTPAWiseID", ServiceID);
                cmd.Parameters.AddWithValue("@ServiceType", ServiceType);
                //cmd.Parameters.Add("@TableName", SqlDbType.NVarChar, 50);
                //cmd.Parameters["@TableName"].Direction = ParameterDirection.Output;
                con.Open();
                int i = cmd.ExecuteNonQuery();
               // Table = (string)cmd.Parameters["@TableName"].Value;
                con.Close();
                //if (i > 0)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}

            return i;
        }
        #endregion

        public DataSet GetServices(string filter2, string SvcID)
        {
            Connect();
            string id = "";
            if (SvcID == "")
            {
                id = "0";
            }
            else
            {
                id = SvcID;
            }
            //SqlCommand cmd = new SqlCommand("select ServicesTPAWise.ServicesTPAWiseID ,Services.ServiceName,Services.ServiceID from ServicesTPAWise left join Services on Services.ServiceID = ServicesTPAWise.ServiceID where  ServicesTPAWise.OrganizationID = @OrganizationID and ServicesTPAWise.RowStatus = 0 order by  Services.ServiceName asc", con);
            SqlCommand cmd = new SqlCommand("select ServiceID,ServiceName from Services where ServiceID in (select ServiceID from ServicesTPAWise where RowStatus = 0 and OrganizationID in (select OrganizationID from Organization where   HospitalID=" + HospitalID + " and LocationID=" + LocationID + " and OrganizationID=" + id + " ) and HospitalID=" + HospitalID + " and LocationID=" + LocationID + ") and ServiceName Like '%" + filter2 + "%' order by ServiceName asc", con);
            //cmd.Parameters.AddWithValue("@OrganizationID", SvcID);
            //cmd.Parameters.AddWithValue("@ServiceName", filter2);

            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;

        }



        public string Save(ServicesTPAWise ServicesTPAWise, ArrayWordDetailTpa TpaDetail)
        {
            bool flag = true;
            Connect();
            con.Open();
          //  ArrayWordDetailTpa TPAWiseDetail = new ArrayWordDetailTpa();
            SqlCommand cmd;
            string Arror = "Save";
            try
            {

                
                    DataSet dsTPAWise = new DataSet();
                    // dsTPAWise = GetServicesTPAWise();


                    cmd = new SqlCommand("IUServicesTPAWise", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd.Parameters.AddWithValue("@LocationID", LocationID);
                    cmd.Parameters.AddWithValue("@ServicesTPAWiseID", ServicesTPAWise.ServicesTPAWiseID);
                    // cmd.Parameters["@ServicesTPAWiseID"].Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@ServiceID", ServicesTPAWise.ServiceID);

                    cmd.Parameters.AddWithValue("@ReferenceCode", 1);


                    if (ServicesTPAWise.UnitID == null)
                        cmd.Parameters.AddWithValue("@UnitID", 0);
                    else
                        cmd.Parameters.AddWithValue("@UnitID", ServicesTPAWise.UnitID);

                    //cmd.Parameters.AddWithValue("@OrganizationID", Convert.ToInt32(dsTPAWise.Tables[0].Rows[0]["OrganizationID"].ToString()));
                    cmd.Parameters.AddWithValue("@OrganizationID", ServicesTPAWise.OrganizationID);
                    cmd.Parameters.AddWithValue("@GeneralCharges", ServicesTPAWise.GeneralCharges);
                    cmd.Parameters.AddWithValue("@EmergencyCharges", ServicesTPAWise.EmergencyCharges);
                    cmd.Parameters.AddWithValue("@ServiceType", ServicesTPAWise.ServiceType);
                    cmd.Parameters.AddWithValue("@RecommendedByDoctor", ServicesTPAWise.RecommendedByDoctor);

                    cmd.Parameters.AddWithValue("@HideInBilling", ServicesTPAWise.HideInBilling);
                    cmd.Parameters.AddWithValue("@CreationID", UserID);
                    cmd.Parameters.AddWithValue("@Mode", "Edit");

                    int ServicesTPAWiseID = cmd.ExecuteNonQuery();
                    ServicesTPAWise.ServicesTPAWiseID = Convert.ToInt32(cmd.Parameters["@ServicesTPAWiseID"].Value.ToString());
                    int ServicesTPAWiseIDs = Convert.ToInt32(cmd.Parameters["@ServicesTPAWiseID"].Value.ToString());

                    // }
                    con.Close();

                    
                    DataSet dsWardDetailsTPAWise = new DataSet();
                    dsWardDetailsTPAWise = GetServicesTPAWiseDetail(ServicesTPAWiseIDs);


                    for (int i = 0; i < TpaDetail.WardID1.Length; i++)
                    {
                        con.Open();
                        cmd = new SqlCommand("IUServicesDetailsTPAWise", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmd.Parameters.AddWithValue("@LocationID", LocationID);
                        cmd.Parameters.AddWithValue("@ServicesDetailsTPAWiseID", Convert.ToInt32(TpaDetail.ServicesDetailsTPAWiseID[i]));
                        //   cmd.Parameters["@ServicesDetailsTPAWiseID"].Direction = ParameterDirection.Output;
                        cmd.Parameters.AddWithValue("@ServicesTPAWiseID", Convert.ToInt32(ServicesTPAWise.ServicesTPAWiseID));
                            cmd.Parameters.AddWithValue("@WardID", Convert.ToInt32(TpaDetail.WardID1[i]));
                            cmd.Parameters.AddWithValue("@GeneralCharges", Convert.ToDecimal(TpaDetail.GeneralCharges1[i].ToString()));
                            cmd.Parameters.AddWithValue("@EmergencyCharges", Convert.ToDecimal(TpaDetail.EmergencyCharges1[i]));
                        cmd.Parameters.AddWithValue("@CreationID", UserID);
                        cmd.Parameters.AddWithValue("@Mode", "Edit");
                        int Row2 = cmd.ExecuteNonQuery();
                        con.Close();
                        if (Row2 > 0)
                        {
                            flag = true;
                        }
                        else
                        {
                            flag = false;
                        }
                    }
                   
                }
           
            catch (Exception ex)
            {
                Arror = ex.Message;
            }
           
            return Arror;
        }


        //-------------------------Save Excel Data



        #region Save & Edit
        public bool SaveExcel(DataSet ds)
        {
            int ServiceID = 0;
            //  ServicesCharges servicesCharges = new ServicesCharges();
            // ds= btnImport_Click("");

            bool flag = true;
            Connect();
            //  con.Open();

            SqlCommand cmd;

            try
            {
                #region ----------------------Add Services--------------------------------

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //if (dr["ServiceID"].ToString() == "")
                    //{
                    //    ServiceID = 0;
                    //}
                    //else
                    //{
                    //    ServiceID = Convert.ToInt32(dr["ServiceID"]);
                    //}
                    //string UntID = "";// GetUnitID(dr["UnitName"].ToString()).Tables[0].Rows[0]["UnitID"].ToString();
                    //if (UntID == "")
                    //{
                    //    UntID = "1";
                    //}
                    if (dr["ServiceName"].ToString() != "")
                    {
                        cmd = new SqlCommand("IUServicesTPAWise", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmd.Parameters.AddWithValue("@LocationID", LocationID);
                        cmd.Parameters.AddWithValue("@ServicesTPAWiseID", dr["ServicesTPAWiseID"]);
                        // cmd.Parameters["@ServicesTPAWiseID"].Direction = ParameterDirection.Output;
                        cmd.Parameters.AddWithValue("@ServiceID", dr["ServiceID"]);
                        cmd.Parameters.AddWithValue("@ReferenceCode", 1);
                            cmd.Parameters.AddWithValue("@UnitID", dr["UnitID"]);
                        //cmd.Parameters.AddWithValue("@OrganizationID", Convert.ToInt32(dsTPAWise.Tables[0].Rows[0]["OrganizationID"].ToString()));
                        cmd.Parameters.AddWithValue("@OrganizationID", dr["OrganizationID"]);
                        cmd.Parameters.AddWithValue("@GeneralCharges", dr["WardName"]);
                        cmd.Parameters.AddWithValue("@EmergencyCharges", dr["WardName"]);
                        cmd.Parameters.AddWithValue("@ServiceType", dr["ServiceType"]);
                        cmd.Parameters.AddWithValue("@RecommendedByDoctor", dr["RecommendedByDoctor"]);
                        cmd.Parameters.AddWithValue("@HideInBilling", dr["HideInBilling"]);
                        cmd.Parameters.AddWithValue("@CreationID", UserID);
                        cmd.Parameters.AddWithValue("@Mode", "Edit");

                        int ServicesTPAWiseID = cmd.ExecuteNonQuery();

                       
                        int ServicesTPAWiseIDs = Convert.ToInt32(cmd.Parameters["@ServicesTPAWiseID"].Value.ToString());

                        // }
                        con.Close();
                       
                #endregion

                        #region -------------------Add ServiceCharges-------------------

                           DataSet dsWardDetailsTPAWise = new DataSet();
                        dsWardDetailsTPAWise = GetServicesTPAWiseDetail(ServicesTPAWiseIDs);
                        Bl_Services bl_obj = new Bl_Services();
                        if (ServiceID > 0)
                        {

                            int ServiceChargesID = 0;
                           
                            DataSet dsServiceCharge = new DataSet();
                           // dsServiceCharge = SelectServiceChargesbyID(ServiceID);
                            dsServiceCharge.Reset();
                            dsServiceCharge = GetServicesTPAWise(ServicesTPAWiseID); 
                            con.Open();
                            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                            {
                                if (i > 9)
                                {

                                    string WordName = ds.Tables[0].Columns[i].ColumnName;
                                    string charg = dr[WordName].ToString();
                                    if (charg == "")
                                    {
                                        dr[WordName] = 0;
                                    }
                                    DataView dvService = new DataView(dsServiceCharge.Tables[0], " WardName = '" + WordName.ToString() + "' ", "", DataViewRowState.CurrentRows);
                                    if (dvService.ToTable().Rows.Count > 0)
                                    {
                                        cmd = new SqlCommand("IUServicesDetailsTPAWise", con);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                                        cmd.Parameters.AddWithValue("@LocationID", LocationID);
                                        cmd.Parameters.AddWithValue("@ServicesDetailsTPAWiseID", Convert.ToInt32(dvService.ToTable().Rows[0]["ServicesDetailsTPAWiseID"]));
                                        //   cmd.Parameters["@ServicesDetailsTPAWiseID"].Direction = ParameterDirection.Output;
                                        cmd.Parameters.AddWithValue("@ServicesTPAWiseID", Convert.ToInt32(ServicesTPAWiseIDs));
                                        cmd.Parameters.AddWithValue("@WardID", Convert.ToInt32(dvService.ToTable().Rows[0]["WardID"]));
                                        cmd.Parameters.AddWithValue("@GeneralCharges", Convert.ToDecimal(dr[WordName]));
                                        cmd.Parameters.AddWithValue("@EmergencyCharges", Convert.ToDecimal(dr[WordName]));
                                        cmd.Parameters.AddWithValue("@CreationID", UserID);
                                        cmd.Parameters.AddWithValue("@Mode", "Edit");
                                        int Row2 = cmd.ExecuteNonQuery();
                                    }
                                }
                            }
                            con.Close();
                        }
                        #endregion --------------------------------------

                        #region Add ServiceTPAWise

                        String Mode = "";
                        Mode = cmd.Parameters["@Mode"].Value.ToString();
                    }
                    //if (Mode == "Add")
                    //{
                    //    DataSet dsTPAWise = new DataSet();
                    //    dsTPAWise = GetServicesTPAWiseForOrganizationID();

                    //    for (int i = 0; i < dsTPAWise.Tables[0].Rows.Count; i++)
                    //    {
                    //        cmd = new SqlCommand("IUServicesTPAWise", con);
                    //        cmd.CommandType = CommandType.StoredProcedure;
                    //        cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    //        cmd.Parameters.AddWithValue("@LocationID", LocationID);
                    //        cmd.Parameters.AddWithValue("@ServicesTPAWiseID", 0);
                    //        cmd.Parameters["@ServicesTPAWiseID"].Direction = ParameterDirection.Output;

                    //        cmd.Parameters.AddWithValue("@HideInBilling", dsTPAWise.Tables[0].Rows[i]["HideInBilling"].ToString());
                    //        cmd.Parameters.AddWithValue("@ServiceID", ServicesMod.ServiceID);

                    //        cmd.Parameters.AddWithValue("@ReferenceCode", 1);


                    //        if (ServicesMod.UnitID == null)
                    //            cmd.Parameters.AddWithValue("@UnitID", 0);
                    //        else
                    //            cmd.Parameters.AddWithValue("@UnitID", ServicesMod.UnitID);

                    //        cmd.Parameters.AddWithValue("@OrganizationID", dsTPAWise.Tables[0].Rows[i]["OrganizationID"].ToString());
                    //        cmd.Parameters.AddWithValue("@GeneralCharges", ServicesMod.GeneralCharges);
                    //        cmd.Parameters.AddWithValue("@EmergencyCharges", ServicesMod.EmergencyCharges);
                    //        cmd.Parameters.AddWithValue("@ServiceType", ServicesMod.ServiceType);
                    //        cmd.Parameters.AddWithValue("@RecommendedByDoctor", ServicesMod.RecommendedByDoctor);
                    //        cmd.Parameters.AddWithValue("@CreationID", UserID);
                    //        cmd.Parameters.AddWithValue("@Mode", "Add");
                    //        int ServicesTPAWiseID = cmd.ExecuteNonQuery();

                    //        //if (ServicesTPAWiseID > 0)
                    //        //{
                    //        //    flag = true;
                    //        //}
                    //        //else
                    //        //{
                    //        //    flag = false;
                    //        //}
                    //    }
                    //#endregion
                    //#region Add ServicesTPAWiseDetailForOrganization

                    //    ServicesMod.ServiceID = Convert.ToInt32(cmd.Parameters["@ServiceID"].Value.ToString());
                    //    ServiceID = Convert.ToInt32(cmd.Parameters["@ServiceID"].Value.ToString());

                    //    DataSet dsWardDetailsTPAWise = new DataSet();
                    //    dsWardDetailsTPAWise = GetServicesTPAWiseDetailForOrganizationID();


                    //    for (int i = 0; i < dsWardDetailsTPAWise.Tables[0].Rows.Count; i++)
                    //    {

                    //        cmd = new SqlCommand("IUServicesDetailsTPAWise", con);
                    //        cmd.CommandType = CommandType.StoredProcedure;
                    //        cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    //        cmd.Parameters.AddWithValue("@LocationID", LocationID);
                    //        cmd.Parameters.AddWithValue("@ServicesDetailsTPAWiseID", 0);
                    //        cmd.Parameters["@ServicesDetailsTPAWiseID"].Direction = ParameterDirection.Output;

                    //        cmd.Parameters.AddWithValue("@ServicesTPAWiseID", Convert.ToInt32(dsWardDetailsTPAWise.Tables[0].Rows[i]["ServicesTPAWiseID"].ToString()));

                    //        if (servicesCharges.WardID == null)
                    //            cmd.Parameters.AddWithValue("@WardID", 0);
                    //        else
                    //            cmd.Parameters.AddWithValue("@WardID", Convert.ToInt32(dsWardDetailsTPAWise.Tables[0].Rows[i]["WardID"].ToString()));

                    //        cmd.Parameters.AddWithValue("@WardName", Convert.ToString(dsWardDetailsTPAWise.Tables[0].Rows[i]["WardName"].ToString()));

                    //        cmd.Parameters.AddWithValue("@GeneralCharges", Convert.ToDecimal(dsWardDetailsTPAWise.Tables[0].Rows[i]["GeneralCharges"].ToString()));
                    //        cmd.Parameters.AddWithValue("@EmergencyCharges", Convert.ToDecimal(dsWardDetailsTPAWise.Tables[0].Rows[i]["GeneralCharges"].ToString()));
                    //        cmd.Parameters.AddWithValue("@CreationID", UserID);
                    //        cmd.Parameters.AddWithValue("@Mode", "Add");
                    //        int Row2 = cmd.ExecuteNonQuery();

                    //        if (Row2 > 0)
                    //        {
                    //            flag = true;
                    //        }
                    //        else
                    //        {
                    //            flag = false;
                    //        }
                    //    }
                    //}





                    //-------------
                }

                        #endregion

                if (flag == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                flag = false;
            }
            con.Close();
            return flag;
        }

        #endregion

        public DataSet btnImport_Click(string path)
        {
            //SqlConnection conn = new SqlConnection(File.ReadAllText(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "\\srv.dll"));

            DataSet myDataSet = new DataSet();
            DataTable dataTable = new DataTable();
            string aa = path;
            //  string aa1 = @"D:\20-7-2019\KeystoneProject\KeystoneProject\Service_Excel/Service_Excel_DataDD_09_2019_05_11_55.xlsx";

            String excelConnString = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\"", aa);
            //Create Connection to Excel work book 
            using (OleDbConnection excelConnection = new OleDbConnection(excelConnString))
            {

                // con.Open();
                //Create OleDbCommand to fetch data from Excel 
                // SqlCommand cmd = new SqlCommand("Select * from ProductDetails", con);
                //cmd.ExecuteNonQuery();

                //OleDbCommand cmd1 = new OleDbCommand("Select * from [Sheet1$]", excelConnection);
                OleDbCommand cmd1 = new OleDbCommand("Select* from [Exported from Keystone]", excelConnection);
                {
                    excelConnection.Open();
                    OleDbDataAdapter dataAdapter = new OleDbDataAdapter("SELECT * FROM  [Sheet1$]", excelConnString);

                    dataAdapter.Fill(myDataSet, "ProductDetails");
                    dataTable = myDataSet.Tables["ProductDetails"];



                    SaveExcel(myDataSet);

                    using (SqlBulkCopy sqlBulk = new SqlBulkCopy(con))
                    {
                        excelConnection.Close();
                        con.Close();
                        //    sqlBulk.DestinationTableName = "ProductDetails";
                        //  sqlBulk.WriteToServer(dataTable);
                    }
                    // dReader.Close();

                    // MessageBox.Show("Record Imported Successfully");
                }
                //}
                // }
            }
            return myDataSet;
        }
    }
}