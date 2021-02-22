using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using KeystoneProject.Models.Master;
using Microsoft.ApplicationBlocks.Data;
using KeystoneProject.Models.Master;

namespace KeystoneProject.Buisness_Logic.Master
{
    public class BL_DoctorCommissionSetting
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        private SqlConnection con;
        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);

        }

        #region Save & Edit

        public bool Save(DoctorCommissionSetting obj, DoctorCommissiondgv objdgv,OPDDGV obj1,IPDDGV obj2,LAB obj3)
        {



            //  ServicesCharges servicesCharges = new ServicesCharges();


            bool flag = true;
            string[] docid = obj.doc.Split(',');
            string[] check = obj.CheckRow.Split(',');
            

            Connect();
            con.Open();
            SqlCommand cmd;
            for (int row = 0; row < docid.Length; row++)
            {

                if (docid[row] != "")
                {
                    if(obj.Mode=="Add")
                    {
                        obj.DoctorCommissionSettingID = 0;
                    }
                   
                    #region ----------------------Add Services--------------------------------

                    cmd = new SqlCommand("IUDoctorCommissionSetting", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd.Parameters.AddWithValue("@LocationID", LocationID);

                    if (obj.DoctorCommissionSettingID == 0)
                    {
                        cmd.Parameters.AddWithValue("@DoctorCommissionSettingID", 0);
                        cmd.Parameters["@DoctorCommissionSettingID"].Direction = ParameterDirection.Output;
                        cmd.Parameters.AddWithValue("@Mode", "Add");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@DoctorCommissionSettingID", obj.DoctorCommissionSettingID);
                        cmd.Parameters.AddWithValue("@Mode", "Edit");
                    }

                    if (obj.DoctorID == null)
                        cmd.Parameters.AddWithValue("@DoctorID", 0);
                    else
                        cmd.Parameters.AddWithValue("@DoctorID", docid[row]);
                    if (obj.CheckRow == null)
                        cmd.Parameters.AddWithValue("@CheckRow", 0);
                    else
                        cmd.Parameters.AddWithValue("@CheckRow", check[row]);

                    if (obj.CommissionTypeOPD == null)
                        cmd.Parameters.AddWithValue("@CommissionTypeOPD", 0);
                    else
                        cmd.Parameters.AddWithValue("@CommissionTypeOPD", obj.CommissionTypeOPD);
                    if (obj.FixedOPD == null)
                        cmd.Parameters.AddWithValue("@FixedOPD", 0);
                    else
                        cmd.Parameters.AddWithValue("@FixedOPD", obj.FixedOPD);
                    if (obj.CommissionTypeIPD == null)
                        cmd.Parameters.AddWithValue("@CommissionTypeIPD", 0);
                    else
                        cmd.Parameters.AddWithValue("@CommissionTypeIPD", obj.CommissionTypeIPD);
                    if (obj.FixedIPD == null)
                        cmd.Parameters.AddWithValue("@FixedIPD", 0);
                    else
                        cmd.Parameters.AddWithValue("@FixedIPD", obj.FixedIPD);



                    if (obj.FixedLabType == null)
                        cmd.Parameters.AddWithValue("@FixedLabPerRs", 0);
                    else
                        cmd.Parameters.AddWithValue("@FixedLabPerRs", obj.FixedLabType);
                    if (obj.FixedOPDRsType == null)
                        cmd.Parameters.AddWithValue("@FixedOPDPerRs", 0);
                    else
                        cmd.Parameters.AddWithValue("@FixedOPDPerRs", obj.FixedOPDRsType);
                    if (obj.FixedIPDType == null)
                        cmd.Parameters.AddWithValue("@FixedIPDPerRs", 0);
                    else
                        cmd.Parameters.AddWithValue("@FixedIPDPerRs", obj.FixedIPDType);
                    if (obj.CommissionTypeLab == null)
                        cmd.Parameters.AddWithValue("@CommissionTypeLab", 0);
                    else
                        cmd.Parameters.AddWithValue("@CommissionTypeLab", obj.CommissionTypeLab);
                    if (obj.FixedLab == null)
                        cmd.Parameters.AddWithValue("@FixedLab", 0);
                    else
                        cmd.Parameters.AddWithValue("@FixedLab", obj.FixedLab);


                    cmd.Parameters.AddWithValue("@CreationID", UserID);




                    int RowNo = cmd.ExecuteNonQuery();

                    if (RowNo > 0)
                    {
                        obj.DoctorCommissionSettingID = Convert.ToInt32(cmd.Parameters["@DoctorCommissionSettingID"].Value.ToString());
                    }

                    #endregion

                    #region -------------------Add IUDoctorCommissionSettingDetails-------------------

                  
                    if (obj.DoctorCommissionSettingID > 0)
                    {
                        int ServiceChargesID = 0;
                        string serGroup = "";
                        string serGroupIPD = "";
                         string serGroupLab = "";
                        // obj.DoctorCommissionSettingID = Convert.ToInt32(cmd.Parameters["@DoctorCommissionSettingID"].Value.ToString());

                        //-------OPD
                        if (obj1.ServiceIDdgv != null )
                        {
                            var ser = "";
                            for (int i = 0; i < obj1.ServiceIDdgv.Length; i++)
                            {
                                //if (obj1.ServiceIDdgv[i] == "0" && obj1.ServiceGroupIDdgv[i]!=null)
                                //{

                                //}
                                //List<DoctorCommissionSetting> serach = new List<DoctorCommissionSetting>();
                                serGroup = obj1.ServiceGroupIDdgv[i];
                                if (serGroup=="")
                                {
                                    ser = "%";
                                }
                                else
                                {
                                    ser = serGroup;
                                }
                                GetServiceGroup(ser, "OPD");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    if (obj1.ServiceIDdgv[i].ToString() == "0")
                                    {
                                        obj1.ServiceIDdgv[i] = "%";
                                        DataView dvService = new DataView(ds.Tables[0], " ServiceID like '" + obj1.ServiceIDdgv[i].ToString() + "' and ServiceGroupID = '" + obj1.ServiceGroupIDdgv[i].ToString() + "' ", "", DataViewRowState.CurrentRows);
                                        foreach (DataRow dr in dvService.ToTable().Rows)
                                        {

                                            obj1.ServiceIDdgv[i] = dr["ServiceID"].ToString();
                                            obj1.ServiceNamedgv[i] = dr["ServiceName"].ToString();
                                            obj1.ServiceGroupNamedgv[i] = dr["ServiceGroupName"].ToString();
                                            //    
                                            //}
                                            //BindServiceNameOPDIPD("%", "%", "OPD");
                                            //if (dsIPDOPD.Tables[0].Rows.Count > 0)
                                            //{
                                            //    if (obj1.ServiceIDdgv[i].ToString()=="0")
                                            //    {
                                            //        obj1.ServiceIDdgv[i] = "%";
                                            //    }
                                            //    DataView dvService = new DataView(dsIPDOPD.Tables[0], " ServiceID like '" + obj1.ServiceIDdgv[i].ToString() + "' and ServiceGroupID = '" + obj1.ServiceGroupIDdgv[i].ToString() + "' ", "", DataViewRowState.CurrentRows);
                                            //    if (dvService.ToTable().Rows.Count > 0)
                                            //    {
                                            //        obj1.ServiceIDdgv[i] = dvService.ToTable().Rows[0]["ServiceID"].ToString();
                                            //        obj1.ServiceNamedgv[i] = dvService.ToTable().Rows[0]["ServiceName"].ToString();
                                            //    }
                                            //}
                                            //BindServiceGroupNameOPDIPD("%", "OPD");
                                            //if (dsGroupIPDOPD.Tables[0].Rows.Count > 0)
                                            //{
                                            //    DataView dvServiceGroup = new DataView(dsGroupIPDOPD.Tables[0], " ServiceGroupID = '" + obj1.ServiceGroupIDdgv[i].ToString() + "' ", "", DataViewRowState.CurrentRows);
                                            //    if (dvServiceGroup.ToTable().Rows.Count > 0)
                                            //    {
                                            //        obj1.ServiceGroupNamedgv[i] = dvServiceGroup.ToTable().Rows[0]["ServiceGroupName"].ToString();
                                            //    }
                                            //}

                                            SqlCommand cmdopd = new SqlCommand("IUDoctorCommissionSettingDetails", con);
                                            cmdopd.CommandType = CommandType.StoredProcedure;
                                            cmdopd.Parameters.AddWithValue("@HospitalID", HospitalID);
                                            cmdopd.Parameters.AddWithValue("@LocationID", LocationID);
                                            cmdopd.Parameters.AddWithValue("@DoctorCommissionSettingDetailsID", 0);
                                            cmdopd.Parameters["@DoctorCommissionSettingDetailsID"].Direction = ParameterDirection.Output;
                                            cmdopd.Parameters.AddWithValue("@DoctorCommissionSettingID", obj.DoctorCommissionSettingID);
                                            cmdopd.Parameters.AddWithValue("@ServiceID", obj1.ServiceIDdgv[i].ToString());
                                            cmdopd.Parameters.AddWithValue("@ServiceName", obj1.ServiceNamedgv[i]);
                                            cmdopd.Parameters.AddWithValue("@ServiceGroupID", obj1.ServiceGroupIDdgv[i].ToString());
                                            cmdopd.Parameters.AddWithValue("@ServiceGroupName", obj1.ServiceGroupNamedgv[i]);
                                            cmdopd.Parameters.AddWithValue("@TypeOfServices", "OPD");
                                            cmdopd.Parameters.AddWithValue("@IncludeExclude", obj1.IEOPDdgv[i]);
                                            cmdopd.Parameters.AddWithValue("@CommissionRsPer", obj1.CommissionOPDRsTypedgv[i].ToString());
                                            cmdopd.Parameters.AddWithValue("@Commission", obj1.CommissionOPDdgv[i]);
                                            cmdopd.Parameters.AddWithValue("@CreationID", UserID);
                                            cmdopd.Parameters.AddWithValue("@Mode", "Add");
                                            ServiceChargesID = cmdopd.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        SqlCommand cmdopd = new SqlCommand("IUDoctorCommissionSettingDetails", con);
                                        cmdopd.CommandType = CommandType.StoredProcedure;
                                        cmdopd.Parameters.AddWithValue("@HospitalID", HospitalID);
                                        cmdopd.Parameters.AddWithValue("@LocationID", LocationID);
                                        cmdopd.Parameters.AddWithValue("@DoctorCommissionSettingDetailsID", 0);
                                        cmdopd.Parameters["@DoctorCommissionSettingDetailsID"].Direction = ParameterDirection.Output;
                                        cmdopd.Parameters.AddWithValue("@DoctorCommissionSettingID", obj.DoctorCommissionSettingID);
                                        cmdopd.Parameters.AddWithValue("@ServiceID", obj1.ServiceIDdgv[i].ToString());
                                        cmdopd.Parameters.AddWithValue("@ServiceName", obj1.ServiceNamedgv[i]);
                                        cmdopd.Parameters.AddWithValue("@ServiceGroupID", obj1.ServiceGroupIDdgv[i].ToString());
                                        cmdopd.Parameters.AddWithValue("@ServiceGroupName", obj1.ServiceGroupNamedgv[i]);
                                        cmdopd.Parameters.AddWithValue("@TypeOfServices", "OPD");
                                        cmdopd.Parameters.AddWithValue("@IncludeExclude", obj1.IEOPDdgv[i]);
                                        cmdopd.Parameters.AddWithValue("@CommissionRsPer", obj1.CommissionOPDRsTypedgv[i].ToString());
                                        cmdopd.Parameters.AddWithValue("@Commission", obj1.CommissionOPDdgv[i]);
                                        cmdopd.Parameters.AddWithValue("@CreationID", UserID);
                                        cmdopd.Parameters.AddWithValue("@Mode", "Add");
                                        ServiceChargesID = cmdopd.ExecuteNonQuery();
                                    }
                                       
                                    
                        }
                            
                        }
                        }
                        //-----------IPD
                        if (obj2.ServiceIDIPDdgv != null)
                        {
                            for (int j = 0; j < obj2.ServiceIDIPDdgv.Length; j++)
                            {
                                serGroupIPD = obj2.ServiceGroupIDIPDdgv[j];
                                if (serGroupIPD == "")
                                {
                                    serGroupIPD = "%";
                                }
                                GetServiceGroup(serGroupIPD, "IPD");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    if (obj2.ServiceIDIPDdgv[j].ToString() == "0")
                                    {
                                        obj2.ServiceIDIPDdgv[j] = "%";

                                        DataView dvService = new DataView(ds.Tables[0], " ServiceID like '" + obj2.ServiceIDIPDdgv[j].ToString() + "' and ServiceGroupID = '" + obj2.ServiceGroupIDIPDdgv[j].ToString() + "' ", "", DataViewRowState.CurrentRows);
                                        foreach (DataRow dr in dvService.ToTable().Rows)
                                        {

                                            obj2.ServiceIDIPDdgv[j] = dr["ServiceID"].ToString();
                                            obj2.ServiceNameIPDdgv[j] = dr["ServiceName"].ToString();
                                            obj2.ServiceGroupNameIPDdgv[j] = dr["ServiceGroupName"].ToString();
                                            //BindServiceNameOPDIPD("%", "%", "IPD");
                                            //if (dsIPDOPD.Tables[0].Rows.Count > 0)
                                            //{
                                            //    DataView dvService = new DataView(dsIPDOPD.Tables[0], " ServiceID = '" + obj2.ServiceIDIPDdgv[j].ToString() + "' ", "", DataViewRowState.CurrentRows);
                                            //    if (dvService.ToTable().Rows.Count > 0)
                                            //    {
                                            //        obj2.ServiceNameIPDdgv[j] = dvService.ToTable().Rows[0]["ServiceName"].ToString();
                                            //    }
                                            //}

                                            //BindServiceGroupNameOPDIPD("%", "IPD");
                                            //if (dsGroupIPDOPD.Tables[0].Rows.Count > 0)
                                            //{
                                            //    DataView dvService = new DataView(dsGroupIPDOPD.Tables[0], " ServiceGroupID = '" + obj2.ServiceGroupIDIPDdgv[j].ToString() + "' ", "", DataViewRowState.CurrentRows);
                                            //    if (dvService.ToTable().Rows.Count > 0)
                                            //    {
                                            //        obj2.ServiceGroupNameIPDdgv[j] = dvService.ToTable().Rows[0]["ServiceGroupName"].ToString();
                                            //    }
                                            //}

                                            cmd = new SqlCommand("IUDoctorCommissionSettingDetails", con);
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                                            cmd.Parameters.AddWithValue("@LocationID", LocationID);
                                            cmd.Parameters.AddWithValue("@DoctorCommissionSettingDetailsID", 0);
                                            cmd.Parameters["@DoctorCommissionSettingDetailsID"].Direction = ParameterDirection.Output;
                                            cmd.Parameters.AddWithValue("@DoctorCommissionSettingID", obj.DoctorCommissionSettingID);
                                            cmd.Parameters.AddWithValue("@ServiceID", obj2.ServiceIDIPDdgv[j].ToString());
                                            cmd.Parameters.AddWithValue("@ServiceName", obj2.ServiceNameIPDdgv[j]);
                                            cmd.Parameters.AddWithValue("@TypeOfServices", "IPD");
                                            cmd.Parameters.AddWithValue("@IncludeExclude", obj2.IEIPDdgv[j]);
                                            cmd.Parameters.AddWithValue("@CommissionRsPer", obj2.CommissionIPDTypedgv[j].ToString());
                                            cmd.Parameters.AddWithValue("@Commission", obj2.CommissionIPDdgv[j]);
                                            cmd.Parameters.AddWithValue("@ServiceGroupID", obj2.ServiceGroupIDIPDdgv[j].ToString());
                                            cmd.Parameters.AddWithValue("@ServiceGroupName", obj2.ServiceGroupNameIPDdgv[j]);
                                            cmd.Parameters.AddWithValue("@CreationID", UserID);
                                            cmd.Parameters.AddWithValue("@Mode", "Add");
                                            ServiceChargesID = cmd.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        cmd = new SqlCommand("IUDoctorCommissionSettingDetails", con);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                                        cmd.Parameters.AddWithValue("@LocationID", LocationID);
                                        cmd.Parameters.AddWithValue("@DoctorCommissionSettingDetailsID", 0);
                                        cmd.Parameters["@DoctorCommissionSettingDetailsID"].Direction = ParameterDirection.Output;
                                        cmd.Parameters.AddWithValue("@DoctorCommissionSettingID", obj.DoctorCommissionSettingID);
                                        cmd.Parameters.AddWithValue("@ServiceID", obj2.ServiceIDIPDdgv[j].ToString());
                                        cmd.Parameters.AddWithValue("@ServiceName", obj2.ServiceNameIPDdgv[j]);
                                        cmd.Parameters.AddWithValue("@TypeOfServices", "IPD");
                                        cmd.Parameters.AddWithValue("@IncludeExclude", obj2.IEIPDdgv[j]);
                                        cmd.Parameters.AddWithValue("@CommissionRsPer", obj2.CommissionIPDTypedgv[j].ToString());
                                        cmd.Parameters.AddWithValue("@Commission", obj2.CommissionIPDdgv[j]);
                                        cmd.Parameters.AddWithValue("@ServiceGroupID", obj2.ServiceGroupIDIPDdgv[j].ToString());
                                        cmd.Parameters.AddWithValue("@ServiceGroupName", obj2.ServiceGroupNameIPDdgv[j]);
                                        cmd.Parameters.AddWithValue("@CreationID", UserID);
                                        cmd.Parameters.AddWithValue("@Mode", "Add");
                                        ServiceChargesID = cmd.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                        //-----LAB
                        if (obj3.ServiceIDLabdgv != null)
                        {
                            for (int i = 0; i < obj3.ServiceIDLabdgv.Length; i++)
                            {

                                //BindServiceNameLAB("%");
                                //if (dsIPDOPD.Tables[0].Rows.Count > 0)
                                //{
                                //    if (obj3.ServiceIDLabdgv[i].ToString() == "0")
                                //    {
                                //        obj3.ServiceIDLabdgv[i] = "%";
                                //    }
                                //    DataView dvService = new DataView(dsLab.Tables[0], " ServiceID = '" + obj3.ServiceIDLabdgv[i].ToString() + "' ", "", DataViewRowState.CurrentRows);
                                //    if (dvService.ToTable().Rows.Count > 0)
                                //    {
                                //        obj3.ServiceNameLABdgv[i] = dvService.ToTable().Rows[0]["ServiceName"].ToString();
                                //    }
                                //}
                                serGroupLab = obj3.ServiceGroupIDLabdgv[i];
                                if (serGroupLab == "")
                                {
                                    serGroupLab = "%";
                                }
                                GetLabGroupName(serGroupLab);
                                if (dsGroupLab.Tables[0].Rows.Count > 0)
                                {
                                    if (obj3.ServiceIDLabdgv[i].ToString() == "0")
                                    {
                                        obj3.ServiceIDLabdgv[i] = "%";


                                        DataView dvService = new DataView(dsGroupLab.Tables[0], " ServiceID like '" + obj3.ServiceIDLabdgv[i].ToString() + "' and ServiceGroupID = '" + obj3.ServiceGroupIDLabdgv[i].ToString() + "' ", "", DataViewRowState.CurrentRows);
                                        foreach (DataRow dr in dvService.ToTable().Rows)
                                        {

                                            obj3.ServiceIDLabdgv[i] = dr["ServiceID"].ToString();
                                            obj3.ServiceNameLABdgv[i] = dr["ServiceName"].ToString();
                                            obj3.ServiceGroupNameLABdgv[i] = dr["ServiceGroupName"].ToString();
                                            //    }
                                            //}
                                            cmd = new SqlCommand("IUDoctorCommissionSettingDetails", con);
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                                            cmd.Parameters.AddWithValue("@LocationID", LocationID);
                                            cmd.Parameters.AddWithValue("@DoctorCommissionSettingDetailsID", 0);
                                            cmd.Parameters["@DoctorCommissionSettingDetailsID"].Direction = ParameterDirection.Output;
                                            cmd.Parameters.AddWithValue("@DoctorCommissionSettingID", obj.DoctorCommissionSettingID);
                                            cmd.Parameters.AddWithValue("@ServiceID", obj3.ServiceIDLabdgv[i].ToString());
                                            cmd.Parameters.AddWithValue("@ServiceName", obj3.ServiceNameLABdgv[i]);
                                            cmd.Parameters.AddWithValue("@ServiceGroupID", obj3.ServiceGroupIDLabdgv[i].ToString());
                                            cmd.Parameters.AddWithValue("@ServiceGroupName", obj3.ServiceGroupNameLABdgv[i]);
                                            cmd.Parameters.AddWithValue("@TypeOfServices", "LAB");
                                            cmd.Parameters.AddWithValue("@IncludeExclude", obj3.IELABdgv[i]);
                                            cmd.Parameters.AddWithValue("@CommissionRsPer", obj3.CommissionLABTypedgv[i].ToString());
                                            cmd.Parameters.AddWithValue("@Commission", obj3.CommissionLABdgv[i]);
                                            cmd.Parameters.AddWithValue("@CreationID", UserID);
                                            cmd.Parameters.AddWithValue("@Mode", "Add");
                                            ServiceChargesID = cmd.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        cmd = new SqlCommand("IUDoctorCommissionSettingDetails", con);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                                        cmd.Parameters.AddWithValue("@LocationID", LocationID);
                                        cmd.Parameters.AddWithValue("@DoctorCommissionSettingDetailsID", 0);
                                        cmd.Parameters["@DoctorCommissionSettingDetailsID"].Direction = ParameterDirection.Output;
                                        cmd.Parameters.AddWithValue("@DoctorCommissionSettingID", obj.DoctorCommissionSettingID);
                                        cmd.Parameters.AddWithValue("@ServiceID", obj3.ServiceIDLabdgv[i].ToString());
                                        cmd.Parameters.AddWithValue("@ServiceName", obj3.ServiceNameLABdgv[i]);
                                        cmd.Parameters.AddWithValue("@ServiceGroupID", obj3.ServiceGroupIDLabdgv[i].ToString());
                                        cmd.Parameters.AddWithValue("@ServiceGroupName", obj3.ServiceGroupNameLABdgv[i]);
                                        cmd.Parameters.AddWithValue("@TypeOfServices", "LAB");
                                        cmd.Parameters.AddWithValue("@IncludeExclude", obj3.IELABdgv[i]);
                                        cmd.Parameters.AddWithValue("@CommissionRsPer", obj3.CommissionLABTypedgv[i].ToString());
                                        cmd.Parameters.AddWithValue("@Commission", obj3.CommissionLABdgv[i]);
                                        cmd.Parameters.AddWithValue("@CreationID", UserID);
                                        cmd.Parameters.AddWithValue("@Mode", "Add");
                                        ServiceChargesID = cmd.ExecuteNonQuery();
                                    }
                                }
                    #endregion --------------------------------------


                            }
                        }
                    }
                }
            }
                    con.Close();
               
            return flag;
        }

        DataSet dsGroupIPDOPD = new DataSet();
        DataSet dsIPDOPD = new DataSet();
        DataSet dsLab = new DataSet();
        DataSet dsGroupLab = new DataSet();
        DataSet ds = new DataSet();
        #endregion


        public bool DeleteDoctorCommissionSetting( int DoctorCommissionSettingID)
        {
            List<DoctorCommissionSetting> serach = new List<DoctorCommissionSetting>();
            Connect();
            SqlCommand cmd = new SqlCommand("DeleteDoctorCommissionSetting", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@DoctorCommissionSettingID", DoctorCommissionSettingID);
           con.Open();
           int row = cmd.ExecuteNonQuery();
           con.Close();
            if(row>0)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public List<DoctorCommissionSetting> BindServiceNameOPDIPD(string ServiceName, string GroupID, string Type)
        {
            List<DoctorCommissionSetting> serach = new List<DoctorCommissionSetting>();
            Connect();
           // SqlCommand cmd = new SqlCommand("select  ServiceName,ServiceID,ServiceGroupID from Services where ServiceType in ('IPD','Both') AND ServiceName like '%' and  ServiceGroupID like '%'  and RowStatus=0 and HospitalID =1 and LocationID = ", con);
            SqlCommand cmd = new SqlCommand("select Convert(nvarchar(500),ServiceID)as ServiceID, ServiceName,ServiceGroupID,GeneralCharges,EmergencyCharges from Services  where  (ServiceType = 'OPD' or ServiceType = 'Both') and ServiceName like '" + ServiceName + "%' and ServiceGroupID like  '" + GroupID + "%'  and   HospitalID = " + HospitalID + " and LocationID = " + LocationID + " and RowStatus = 0 Order by ServiceName asc", con);

           // DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dsIPDOPD);
            con.Close();
            foreach (DataRow dr in dsIPDOPD.Tables[0].Rows)
            {
                if (Type == "IPD")
                {
                    serach.Add(new Models.Master.DoctorCommissionSetting
                    {
                        ServiceIDIPD = Convert.ToInt32(dr["ServiceID"].ToString()),
                        ServiceNameIPD = dr["ServiceName"].ToString(),
                        ServiceGroupID = dr["ServiceGroupID"].ToString()
                       // ServiceGroupName = dr["ServiceName"].ToString()

                    });
                }
                if (Type == "OPD")
                {
                    serach.Add(new Models.Master.DoctorCommissionSetting
                    {
                        ServiceID = Convert.ToInt32(dr["ServiceID"].ToString()),
                        ServiceName = dr["ServiceName"].ToString(),
                        ServiceGroupID = (dr["ServiceGroupID"].ToString()),
                        //ServiceName = dr["ServiceName"].ToString()
                    });
                }
            }
            return serach;
        }

        public List<DoctorCommissionSetting> GetServiceGroup(string serGroupID, string type)
        {
            List<DoctorCommissionSetting> search = new List<DoctorCommissionSetting>();
            
          
                Connect();
                SqlCommand cmd = new SqlCommand("GetServiceGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@ServiceGroupID", serGroupID);
                cmd.Parameters.AddWithValue("@ServiceType", type);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                //con.Close();



                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    search.Add(new Models.Master.DoctorCommissionSetting
                    {
                        ServiceGroupID = dr["ServiceGroupID"].ToString(),
                        ServiceGroupName = dr["ServiceGroupName"].ToString(),
                        ServiceName = dr["ServiceName"].ToString(),
                        ServiceID =Convert.ToInt32(dr["ServiceID"].ToString()),
                        Type = dr["ServiceType"].ToString(),
                       
                    });
                }



                    return search;
        }
        public List<DoctorCommissionSetting> BindServiceGroupNameOPDIPD(string ServiceGroupName, string Type)
        {
            List<DoctorCommissionSetting> serach = new List<DoctorCommissionSetting>();
            Connect();
            if(Type=="IPD")
            {
             SqlCommand cmd = new SqlCommand("select ServiceGroupID, ServiceGroupName from ServiceGroup  where  (ServiceType = 'Both' or ServiceType = 'IPD') and ServiceGroupName like '" + ServiceGroupName + "%'  and   HospitalID = " + HospitalID + " and LocationID = " + LocationID + " and RowStatus = 0 Order by ServiceGroupName asc", con);
             con.Open();
             SqlDataAdapter da = new SqlDataAdapter(cmd);
             da.Fill(dsGroupIPDOPD);
            }
            else
            {
                SqlCommand cmd = new SqlCommand("select ServiceGroupID, ServiceGroupName from ServiceGroup  where  (ServiceType = 'OPD' or ServiceType = 'Both') and ServiceGroupName like '" + ServiceGroupName + "%'  and   HospitalID = " + HospitalID + " and LocationID = " + LocationID + " and RowStatus = 0 Order by ServiceGroupName asc", con);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsGroupIPDOPD);
            }
           
            // DataSet ds = new DataSet();
           
            //con.Close();
            foreach (DataRow dr in dsGroupIPDOPD.Tables[0].Rows)
            {
                if (Type == "IPD")
                {
                    serach.Add(new Models.Master.DoctorCommissionSetting
                    {
                        ServiceGroupIDIPD = dr["ServiceGroupID"].ToString(),
                        ServiceGroupNameIPD = dr["ServiceGroupName"].ToString()

                    });
                }
                if (Type == "OPD")
                {
                    serach.Add(new Models.Master.DoctorCommissionSetting
                    {
                        ServiceGroupID = (dr["ServiceGroupID"].ToString()),
                        ServiceGroupName = dr["ServiceGroupName"].ToString()

                    });
                }
            }
            return serach;
        }
        public List<DoctorCommissionSetting> BindServiceGroupNameLAB(string ServiceGroupName)
        {
            List<DoctorCommissionSetting> serach = new List<DoctorCommissionSetting>();
            Connect();
            SqlCommand cmd = new SqlCommand("select  CategoryID as ServiceGroupID  ,CategoryName as ServiceGroupName  from Category  where   HospitalID = " + HospitalID + " and LocationID = " + LocationID + " and RowStatus = 0 and CategoryName like '" + ServiceGroupName + "%' Order by CategoryName  asc", con);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dsGroupLab);
            //con.Close();

            foreach (DataRow dr in dsGroupLab.Tables[0].Rows)
            {
                serach.Add(new Models.Master.DoctorCommissionSetting
                {
                    ServiceGroupIDLab = (dr["ServiceGroupID"].ToString()),
                    ServiceGroupNameLab = dr["ServiceGroupName"].ToString()

                });
            }
            return serach;
        }

        public List<DoctorCommissionSetting> GetLabGroupName(string ServiceGroupID)
        {
            List<DoctorCommissionSetting> serach = new List<DoctorCommissionSetting>();
            Connect();
            SqlCommand cmd = new SqlCommand("select  Category.CategoryID as ServiceGroupID, Convert(nvarchar(Max),TestMaster.TestID) as ServiceID ,TestMaster.TestName as ServiceName,Category.CategoryName as ServiceGroupName  from Category left join TestMaster on TestMaster.Category=Category.CategoryID  where   Category.HospitalID = 1 and Category.LocationID = 1 and Category.RowStatus = 0 and   TestMaster.HospitalID = " + HospitalID + " and TestMaster.LocationID = " + LocationID + " and TestMaster.RowStatus = 0 and Category.CategoryID like '" + ServiceGroupID + "'  Order by Category.CategoryName  asc", con);
           // DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dsGroupLab);
            //con.Close();

            foreach (DataRow dr in dsGroupLab.Tables[0].Rows)
            {
                serach.Add(new Models.Master.DoctorCommissionSetting
                {
                    ServiceGroupIDLab = (dr["ServiceGroupID"].ToString()),
                    ServiceGroupNameLab = dr["ServiceGroupName"].ToString(),
                    ServiceIDLab = Convert.ToInt32(dr["ServiceID"]),
                    ServiceNameLab = dr["ServiceName"].ToString(),

                });
            }
            return serach;
        }
        public List<DoctorCommissionSetting> BindServiceNameLAB(string ServiceName)
        {
            List<DoctorCommissionSetting> serach = new List<DoctorCommissionSetting>();
            Connect();
            SqlCommand cmd = new SqlCommand("select  TestID as ServiceID  ,TestName as ServiceName,Category,CategoryName,GeneralCharges,EmergencyCharges   from TestMaster  where   HospitalID = " + HospitalID + " and LocationID = " + LocationID + " and RowStatus = 0 and TestName like '" + ServiceName + "%' Order by TestName  asc", con);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dsLab);
            //con.Close();

            foreach (DataRow dr in dsLab.Tables[0].Rows)
            {
                serach.Add(new Models.Master.DoctorCommissionSetting
                {
                    ServiceIDLab = Convert.ToInt32(dr["ServiceID"].ToString()),
                    ServiceNameLab = dr["ServiceName"].ToString(),
                    ServiceGroupIDLab = dr["Category"].ToString()

                });
            }
            return serach;
        }

        public List<DoctorCommissionSetting> GetDoctor(string DoctorID)
        {
            List<DoctorCommissionSetting> serach = new List<DoctorCommissionSetting>();
                //string[] TestID = DoctorID.Split(',');
                //int a = 0;
                //for (int row = 0; row < TestID.Length; row++)
                //{

                //   // a = Convert.ToInt32(TestID[row].ToString());
                //}
                    Connect();
                    SqlCommand cmd = new SqlCommand("GetDoctor", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd.Parameters.AddWithValue("@LocationID", LocationID);
                    cmd.Parameters.AddWithValue("@DoctorID", DoctorID);
                    DataSet ds = new DataSet();
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();



                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        serach.Add(new Models.Master.DoctorCommissionSetting
                        {
                            Department = dr["DepartmentName"].ToString(),
                            DoctorType = dr["DoctorType"].ToString()

                        });
                    }



                    return serach;
                
        }

        public List<DoctorCommissionSetting> GetDoctorName(string prefix)
        {
            List<DoctorCommissionSetting> serach = new List<DoctorCommissionSetting>();
            Connect();

            SqlCommand cmd = new SqlCommand("Select DoctorID,DoctorType,Department.DepartmentName,DoctorPrintName from dbo.Doctor left join  Department on Doctor.DepartmentID=Department.DepartmentID where   Doctor.RowStatus = 0 and DoctorPrintName like '"+prefix+"%' and Doctor.HospitalID=" + HospitalID + " and  Doctor.LocationID=" + LocationID + " Order by DoctorPrintName asc", con);

            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();



            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                serach.Add(new Models.Master.DoctorCommissionSetting
                {
                    Department = dr["DepartmentName"].ToString(),
                    DoctorType = dr["DoctorType"].ToString(),
                    DoctorName = dr["DoctorPrintName"].ToString(),
                    DoctorID = dr["DoctorID"].ToString()
                });
            }



            return serach;
        }

        public DataSet GetDoctorCommissionSettingForDoctorDetail(int HospitalID, int LocationID, string DoctorID)
        {
            List<DoctorCommissionSetting> serach = new List<DoctorCommissionSetting>();
            Connect();
            SqlCommand cmd = new SqlCommand("GetDoctorCommissionSettingForDoctorDetail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@DoctorID", DoctorID);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();



            return ds;
        }




        public List<DoctorCommissionSetting> GetDoctorCommissionSetting(string DoctorID)
        {
            DoctorCommissionSetting obj = new Models.Master.DoctorCommissionSetting();
           // DoctorCommissionSetting obj = new Models.Master.DoctorCommissionSetting();
            List<DoctorCommissionSetting> serach = new List<DoctorCommissionSetting>();
            //string[] TestID = DoctorID.Split(',');
            //int a = 0;
            //for (int row = 0; row < TestID.Length; row++)
            //{

            //    a = Convert.ToInt32(TestID[row].ToString());


                Connect();
                SqlCommand cmd = new SqlCommand("GetDoctorCommissionSetting", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@DoctorID", DoctorID);
                DataSet ds = new DataSet();
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
                DoctorCommissiondgv[] objfill = new DoctorCommissiondgv[ds.Tables[0].Rows.Count];
                int j = 0;
                GetAllDoctorCommissionSetting();
                if (ds.Tables[0].Rows.Count > 0)
                {


                    DataView dvDoctorNAme = new DataView(dsDoctor.Tables[0], " DoctorID = '" + Convert.ToInt32(ds.Tables[0].Rows[0]["DoctorID"]) + "' ", "", DataViewRowState.CurrentRows);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        objfill[j] = new DoctorCommissiondgv();
                        objfill[j].DoctorID = Convert.ToInt32(dr["DoctorID"].ToString());
                        if (dvDoctorNAme.ToTable().Rows.Count > 0)
                        {
                            objfill[j].DoctorName = dvDoctorNAme.ToTable().Rows[0]["Name"].ToString();
                        }
                        objfill[j].DoctorCommissionSettingIDdv = Convert.ToInt32(dr["DoctorCommissionSettingID"].ToString());

                        objfill[j].FixedOPDRsType = dr["FixedOPDPerRs"].ToString();
                        objfill[j].FixedIPDType = dr["FixedIPDPerRs"].ToString();
                        objfill[j].FixedLabType = dr["FixedLabPerRs"].ToString();


                        //ucDoctorCommissionSetting1.txtDoctorName.Value = dsDoctorCommissionSetting.Tables[0].Rows[0]["D,octorID"].ToString();
                        objfill[j].CommissionOPDRsTypedgv = dr["CommissionTypeOPD"].ToString();
                        objfill[j].FixedOPDdgv = Convert.ToDecimal(dr["FixedOPD"].ToString());
                        objfill[j].CommissionIPDTypedgv = dr["CommissionTypeIPD"].ToString();
                        objfill[j].FixedIPDdgv = Convert.ToDecimal(dr["FixedIPD"].ToString());
                        objfill[j].CommissionLABTypedgv = dr["CommissionTypeLab"].ToString();
                        objfill[j].FixedLabdgv = Convert.ToDecimal(dr["FixedLab"].ToString());
                        j++;

                    }
                    obj.filldgv = objfill;

                    //--------------OPD 
                    int i = 0;


                    #region close
                    OPDDGV1[] objOPD = new OPDDGV1[ds.Tables[1].Rows.Count];
                    IPDDGV1[] objIPD = new IPDDGV1[ds.Tables[2].Rows.Count];
                    LAB1[] objLAb = new LAB1[ds.Tables[3].Rows.Count];

                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {

                        objOPD[i] = new OPDDGV1();

                        objOPD[i].ServiceIDdgv = dr["ServiceID"].ToString();
                        objOPD[i].ServiceNamedgv = dr["ServiceName"].ToString();
                        objOPD[i].ServiceGroupIDdgv = dr["ServiceGroupID"].ToString();
                        objOPD[i].ServiceGroupNamedgv = dr["ServiceGroupName"].ToString();
                        // objOPD[i].ServiceIDdgv = dr["ServiceID"].ToString();
                        objOPD[i].IEOPDdgv = dr["IncludeExclude"].ToString();
                        objOPD[i].CommissionOPDdgv = dr["Commission"].ToString();
                        objOPD[i].CommissionOPDRsTypedgv = dr["CommissionRsPer"].ToString();
                        i++;
                    }
                    obj.OPDdgv = objOPD;
                    //------------------IPD
                    i = 0;
                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        objIPD[i] = new IPDDGV1();

                        objIPD[i].ServiceNameIPDdgv = dr["ServiceName"].ToString();
                        objIPD[i].ServiceIDIPDdgv = dr["ServiceID"].ToString();
                        objIPD[i].ServiceGroupNameIPDdgv = dr["ServiceGroupName"].ToString();
                        objIPD[i].ServiceGroupIDIPDdgv = dr["ServiceGroupID"].ToString();
                        objIPD[i].IEIPDdgv = dr["IncludeExclude"].ToString();
                        objIPD[i].CommissionIPDdgv = dr["Commission"].ToString();
                        objIPD[i].CommissionIPDTypedgv = dr["CommissionRsPer"].ToString();
                        i++;
                    }
                    obj.IPDdgv = objIPD;
                    i = 0;
                    //----------------LAB
                    foreach (DataRow dr in ds.Tables[3].Rows)
                    {
                        objLAb[i] = new LAB1();
                        objLAb[i].ServiceNameLABdgv = dr["ServiceName"].ToString();
                        objLAb[i].ServiceIDLabdgv = dr["ServiceID"].ToString();
                        objLAb[i].ServiceGroupNameLABdgv = dr["ServiceGroupName"].ToString();
                        objLAb[i].ServiceGroupIDLabdgv = dr["ServiceGroupID"].ToString();
                        objLAb[i].IELABdgv = dr["IncludeExclude"].ToString();
                        objLAb[i].CommissionLABdgv = dr["Commission"].ToString();
                        objLAb[i].CommissionLABTypedgv = dr["CommissionRsPer"].ToString();
                        i++;
                    }
                    obj.LABdgv = objLAb;

                    #endregion
                    serach.Add(obj);
                    ds.Reset();
                    ds = GetDoctorCommissionSettingForDoctorDetail(HospitalID, LocationID, DoctorID);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        serach.Add(new Models.Master.DoctorCommissionSetting
                        {
                            Department = dr["DepartmentName"].ToString(),
                            DoctorType = dr["DoctorType"].ToString()

                        });
                    }

                }
            return serach;
        }

        DataSet dsDoctor = new DataSet();
        public List<DoctorCommissionSetting> GetAllDoctorCommissionSetting()
        {
            List<DoctorCommissionSetting> serach = new List<DoctorCommissionSetting>();
            Connect();
            SqlCommand cmd = new SqlCommand("GetAllDoctorCommissionSetting", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
         
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dsDoctor);
            con.Close();

            foreach (DataRow dr in dsDoctor.Tables[0].Rows)
            {
                serach.Add(new Models.Master.DoctorCommissionSetting
                {
                    DoctorName = dr["Name"].ToString(),
                    DoctorType = dr["DoctorType"].ToString(),
                    DoctorID = dr["DoctorID"].ToString()
                    //CheckRow = dr["CheckRow"].ToString()
                });
            }
            return serach;
        }

    }

}