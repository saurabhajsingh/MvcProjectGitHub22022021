using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using KeystoneProject.Buisness_Logic.Master;
using KeystoneProject.Models.Master;

namespace KeystoneProject.Buisness_Logic.Master
{
    public class BL_PrivilegeCard
    {

        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<PrivilegeCard> PrivilegeCardList = new List<PrivilegeCard>();

        private SqlConnection con;
        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public List<PrivilegeCard> GetAllPrivilageCardServiceGroup()
        {
            Connect();


            SqlCommand cmd = new SqlCommand("GetAllPrivilageCardServiceGroup", con);
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
                PrivilegeCardList.Add(
                    new PrivilegeCard
                    {
                      //  PrivilegeCardID = Convert.ToInt32(dr["PrivilegeCardID"]),
                       
                        ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"]),
                        ServiceGroupName = Convert.ToString(dr["ServiceGroupName"]),
                        DiscountPer = Convert.ToDecimal(dr["DiscountPer"]),
                    });
            }
            return PrivilegeCardList;
        }


        public List<PrivilegeCard> GetPrivilageCard(int PrivilegeCardID)
        {
            Connect();
            List<PrivilegeCard> PrivilegeCardList1 = new List<PrivilegeCard>();
             PrivilegeCard obj = new Models.Master.PrivilegeCard();
            SqlCommand cmd = new SqlCommand("GetPrivilageCard", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@PrivilegeCardID", PrivilegeCardID);
            DataSet ds = new DataSet();
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

           // DataTable dt = new DataTable();
            con.Open();
            sd.Fill(ds);
            con.Close();

            priviledgecardarray1[] objfill = new priviledgecardarray1[ds.Tables[0].Rows.Count];
            priviledgecardPriceDetail[] objPrice = new priviledgecardPriceDetail[ds.Tables[1].Rows.Count];
            priviledgecardServiceName[] objService = new priviledgecardServiceName[ds.Tables[3].Rows.Count];
            priviledgecardTestName[] objTest = new priviledgecardTestName[ds.Tables[4].Rows.Count];
            priviledgecardServiceGroup[] objServiceGroup = new priviledgecardServiceGroup[ds.Tables[2].Rows.Count];

            int j = 0;
            GetAllPrivilegeCard();
            DataView dvDoctorNAme = new DataView(dsAllPriviledgeCard.Tables[0], " PrivilegeCardID = '" + Convert.ToInt32(ds.Tables[0].Rows[0]["PrivilegeCardID"].ToString()) + "' ", "", DataViewRowState.CurrentRows);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                objfill[j] = new priviledgecardarray1();
                objfill[j].PrivilegeCardID = Convert.ToInt32(dr["PrivilegeCardID"].ToString());
                if (dvDoctorNAme.ToTable().Rows.Count > 0)
                {
                    objfill[j].CardName = dvDoctorNAme.ToTable().Rows[0]["CardName"].ToString();
                    objfill[j].Path = dvDoctorNAme.ToTable().Rows[0]["Path"].ToString();
                    objfill[j].Remark = dvDoctorNAme.ToTable().Rows[0]["Remark"].ToString();
                }

               
                j++;

            }
        obj.filldgv =objfill;






            //---------------------------------Price
              int i = 0;
             // PrivilegeCardList.Add(obj);
              foreach (DataRow dr in ds.Tables[1].Rows)
              {

                  objPrice[i] = new priviledgecardPriceDetail();

                  objPrice[i].PrivilegePriceDetailID = Convert.ToInt32(dr["PrivilegePriceDetailID"].ToString());
                  objPrice[i].PrivilegeCardID = Convert.ToInt32(dr["PrivilegeCardID"].ToString());
                   objPrice[i].PriceName = dr["PriceName"].ToString();
                   objPrice[i].Price = Convert.ToDecimal(dr["Price"].ToString());
                  i++;
              }
              obj.cardPriceDetailArray = objPrice;
            //  PrivilegeCardList.Add(obj);

            // --Servicegroup
        i = 0;
              foreach (DataRow dr in ds.Tables[2].Rows)
              {

                  objServiceGroup[i] = new priviledgecardServiceGroup();

                  objServiceGroup[i].ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"]);
                  objServiceGroup[i].ServiceGroupName =dr["ServiceGroupName"].ToString();
                  objServiceGroup[i].DiscountPer = Convert.ToDecimal(dr["DiscountPer"]);
                  objServiceGroup[i].PrivilegeServiceGroupID = Convert.ToInt32(dr["PrivilegeServiceGroupID"]);
                  //objServiceGroup[i].
                  //objfill[j].PrivilegeServiceGroupID = Convert.ToInt32(dr["PrivilegeServiceGroupID"].ToString());
                  //objfill[j].PrivilegePriceDetailID = Convert.ToInt32(dr["PrivilegePriceDetailID"].ToString());

                  //objfill[j].PrivilegeServiceDetailID = Convert.ToInt32(dr["PrivilegeServiceDetailID"].ToString());
                  //objfill[j].PrivilegeTestDetailID = Convert.ToInt32(dr["PrivilegeTestDetailID"].ToString());   

                  i++;
              }
              obj.ServiceGroupArray = objServiceGroup;




              //------------------Service
              i = 0;
              foreach (DataRow dr in ds.Tables[3].Rows)
              {

                  objService[i] = new priviledgecardServiceName();
                  objService[i].PrivilegeServiceDetailID = Convert.ToInt32(dr["PrivilegeServiceDetailID"].ToString());
                  objService[i].PrivilegeCardID = Convert.ToInt32(dr["PrivilegeCardID"].ToString());
                  objService[i].ServiceID1 = Convert.ToInt32(dr["ServiceID"].ToString());
                  objService[i].ServiceName1 = dr["ServiceName"].ToString();
                  objService[i].DiscountService = Convert.ToDecimal(dr["DiscountPer"].ToString());
                
                  i++;
              }
              obj.ServiceNameArray = objService;

             // PrivilegeCardList.Add(obj);
              i = 0;
              //----------------LAB
              foreach (DataRow dr in ds.Tables[4].Rows)
              {
                  objTest[i] = new priviledgecardTestName();
                  objTest[i].PrivilegeTestDetailID = Convert.ToInt32(dr["PrivilegeTestDetailID"].ToString());
                  objTest[i].PrivilegeCardID = Convert.ToInt32(dr["PrivilegeCardID"].ToString());
                  objTest[i].TestID1 = Convert.ToInt32(dr["TestID"].ToString());
                  objTest[i].TestName1 = dr["TestName"].ToString();
                  objTest[i].DiscountTest = Convert.ToDecimal(dr["DiscountPer"].ToString());
                  i++;
              }
              obj.TestNameArray = objTest;

              PrivilegeCardList1.Add(obj);
             // ds.Reset();
        

            
                //PrivilegeCardList.Add(
                //    new PrivilegeCard
                //    {
                //        PrivilegeCardID = Convert.ToInt32(ds.Tables[0].Rows[0]["PrivilegeCardID"]),
                     

                //    });

              return PrivilegeCardList1;
        }



        DataSet dsAllPriviledgeCard = new DataSet();
        public List<PrivilegeCard> GetAllPrivilegeCard()
        {
            Connect();


            SqlCommand cmd = new SqlCommand("GetAllPrivilegeCard", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
 

            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dsAllPriviledgeCard);
            con.Close();
            foreach (DataRow dr in dsAllPriviledgeCard.Tables[0].Rows)
            {
                PrivilegeCardList.Add(
                    new PrivilegeCard
                    {
                        PrivilegeCardID = Convert.ToInt32(dr["PrivilegeCardID"]),
                        CardName = Convert.ToString(dr["CardName"]),
                        Path = Convert.ToString(dr["Path"]),
                        Remark = Convert.ToString(dr["Remark"]),

                    });
            }
            return PrivilegeCardList;
        }

        public List<PrivilegeCard> GetPrivilageCardServiceGroup(int PrivilegeCardID)
        {
            Connect();


            SqlCommand cmd = new SqlCommand("GetPrivilageCardServiceGroup", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@PrivilegeCardID", PrivilegeCardID);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                PrivilegeCardList.Add(
                    new PrivilegeCard
                    {
                        PrivilegeCardID = Convert.ToInt32(dr["PrivilegeCardID"]),
                        CardName = Convert.ToString(dr["CardName"]),

                    });
            }
            return PrivilegeCardList;
        }

        public List<PrivilegeCard> GetAllPrivilegeCardDetails()
        {
            Connect();


            SqlCommand cmd = new SqlCommand("GetAllPrivilegeCardDetails", con);
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
                PrivilegeCardList.Add(
                    new PrivilegeCard
                    {
                        PrivilegeCardID = Convert.ToInt32(dr["PrivilegeCardID"]),
                        CardName = Convert.ToString(dr["CardName"]),

                    });
            }
            return PrivilegeCardList;
        }

        public List<PrivilegeCard> Services()
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select   ServiceID  , ServiceName,GeneralCharges,EmergencyCharges   from Services  where   HospitalID = " + HospitalID + " and LocationID = " + LocationID + " and RowStatus = 0 Order by ServiceName  asc", con);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                PrivilegeCardList.Add(
                    new PrivilegeCard
                    {
                        ServiceID = Convert.ToInt32(dr["ServiceID"]),
                        ServiceName = Convert.ToString(dr["ServiceName"]),

                    });
            }
            return PrivilegeCardList;
        }

        public List<PrivilegeCard> Test()
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select  TestID  ,TestName,GeneralCharges,EmergencyCharges   from TestMaster  where   HospitalID = " + HospitalID + " and LocationID = " + LocationID + " and RowStatus = 0 Order by TestName  asc", con);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                PrivilegeCardList.Add(
                    new PrivilegeCard
                    {
                        TestID = Convert.ToInt32(dr["TestID"]),
                        TestName = Convert.ToString(dr["TestName"]),

                    });
            }
            return PrivilegeCardList;
        }


        public string Save(PrivilegeCard objPriviledge,priviledgecardarray arrr)
        {
            bool flag = true;
            Connect();
            con.Open();
            int PrivilegeCardID = 0;
           
            try
            {
               

                    SqlCommand cmd = new SqlCommand("IUPrivilegeCard", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd.Parameters.AddWithValue("@LocationID", LocationID);
                    if (objPriviledge.PrivilegeCardID == 0)
                    {
                        cmd.Parameters.AddWithValue("@PrivilegeCardID", 0);
                        cmd.Parameters["@PrivilegeCardID"].Direction = ParameterDirection.Output;
                        cmd.Parameters.AddWithValue("@Mode", "Add");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@PrivilegeCardID", objPriviledge.PrivilegeCardID);
                        cmd.Parameters.AddWithValue("@Mode", "Edit");
                    }

                    cmd.Parameters.AddWithValue("@CardName", objPriviledge.CardName);
                    if (objPriviledge.Path == null)
                    {
                        cmd.Parameters.AddWithValue("@Path", "");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Path", objPriviledge.Path + ".docx");

                    }

                    cmd.Parameters.AddWithValue("@Remark", objPriviledge.Remark);
                    cmd.Parameters.AddWithValue("@CreationID", UserID);

                    PrivilegeCardID = cmd.ExecuteNonQuery();
                    objPriviledge.PrivilegeCardID = Convert.ToInt32(cmd.Parameters["@PrivilegeCardID"].Value.ToString());
                
             
                if (PrivilegeCardID > 0)
                {

                    
                //    string[] Price = arrr.Price.ToString().Split(',');
                   //if()
                   //{

                   //}
                    for (int a = 0; a < arrr.PriceName.Length; a++)
                    {
                        
                        if (arrr.PriceName[a].ToString()!= "")
                        {
                           

                            cmd = new SqlCommand("IUPrivilegePriceDetail", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmd.Parameters.AddWithValue("@LocationID", LocationID);
                            cmd.Parameters.AddWithValue("@PrivilegeCardID", objPriviledge.PrivilegeCardID);

                            if (arrr.PrivilegePriceDetailID[a].ToString() == "0" || arrr.PrivilegePriceDetailID.ToString() == null)
                            {
                                cmd.Parameters.AddWithValue("@PrivilegePriceDetailID", 0);
                                //cmd.Parameters["@PrivilegePriceDetailID"].Direction = ParameterDirection.Output;
                                cmd.Parameters.AddWithValue("@Mode", "Add");
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@PrivilegePriceDetailID", arrr.PrivilegePriceDetailID[a]);
                                cmd.Parameters.AddWithValue("@Mode", "Edit");
                            }

                          
                            cmd.Parameters.AddWithValue("@PriceName", arrr.PriceName[a]);
                            cmd.Parameters.AddWithValue("@Price", arrr.Price[a]);
                            cmd.Parameters.AddWithValue("@CreationID", UserID);

                            int PrivilegePriceDetailID = cmd.ExecuteNonQuery();
                        }
                    }
                   if(arrr.ServiceName1!= null)
                    {
                        for (int a = 0; a < arrr.ServiceName1.Length; a++)
                        {


                            cmd = new SqlCommand("IUPrivilegeServiceDetail", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmd.Parameters.AddWithValue("@LocationID", LocationID);

                            cmd.Parameters.AddWithValue("@PrivilegeCardID", objPriviledge.PrivilegeCardID);

                              cmd.Parameters.AddWithValue("@PrivilegeServiceDetailID", 0);
                                // cmd.Parameters["@PrivilegeServiceDetailID"].Direction = ParameterDirection.Output;
                                cmd.Parameters.AddWithValue("@Mode", "Add");
                           
                            cmd.Parameters.AddWithValue("@ServiceID", arrr.ServiceID1[a]);
                            cmd.Parameters.AddWithValue("@ServiceName", arrr.ServiceName1[a]);
                            cmd.Parameters.AddWithValue("@DiscountPer", arrr.DiscountService[a]);
                            cmd.Parameters.AddWithValue("@CreationID", UserID);

                            int PrivilegeServiceDetailID = cmd.ExecuteNonQuery();
                        }
                    }
                  

                    if(arrr.TestName1 != null )
                    {
                        for (int a = 0; a < arrr.TestName1.Length; a++)
                        {

                            cmd = new SqlCommand("IUPrivilegeTestDetail", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmd.Parameters.AddWithValue("@LocationID", LocationID);

                            cmd.Parameters.AddWithValue("@PrivilegeCardID", objPriviledge.PrivilegeCardID);

                                cmd.Parameters.AddWithValue("@PrivilegeTestDetailID", 0);
                                // cmd.Parameters["@PrivilegeTestDetailID"].Direction = ParameterDirection.Output;
                                cmd.Parameters.AddWithValue("@Mode", "Add");
                           
                            cmd.Parameters.AddWithValue("@TestID", arrr.TestID1[a]);
                            cmd.Parameters.AddWithValue("@TestName", arrr.TestName1[a]);
                            cmd.Parameters.AddWithValue("@DiscountPer", arrr.DiscountTest[a]);
                            cmd.Parameters.AddWithValue("@CreationID", UserID);

                            int PrivilegeTestDetailID = cmd.ExecuteNonQuery();
                        }
                    }
                  
                   if(arrr.ServiceGroupName != null)
                    {
                        for (int a = 0; a < arrr.ServiceGroupName.Length; a++)
                        {


                            cmd = new SqlCommand("IUPrivilegeServiceGroup", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmd.Parameters.AddWithValue("@LocationID", LocationID);

                            cmd.Parameters.AddWithValue("@PrivilegeCardID", objPriviledge.PrivilegeCardID);
                           
                                cmd.Parameters.AddWithValue("@PrivilegeServiceGroupID", 0);
                                // cmd.Parameters["@PrivilegeServiceGroupID"].Direction = ParameterDirection.Output;
                                cmd.Parameters.AddWithValue("@Mode", "Add");
                            
                            cmd.Parameters.AddWithValue("@ServiceGroupID", arrr.ServiceGroupID[a]);
                            cmd.Parameters.AddWithValue("@ServiceGroupName", arrr.ServiceGroupName[a]);
                            cmd.Parameters.AddWithValue("@DiscountPer", arrr.DiscountPer[a]);
                            cmd.Parameters.AddWithValue("@CreationID", UserID);

                            int PrivilegeServiceGroupID = cmd.ExecuteNonQuery();
                        }
                    }
                    
                }

                else
                {
                    flag = false;
                }



            }
            catch (Exception ex)
            {
                return ex.Message;
            }
           return "Save";
        }


        #region CheckPrivilegeCard

        //public bool CheckPrivilegeCard(int PrivilegeCardID, string PrivilegeCardName)
        //{
        //    bool NameExists;
        //    Connect();
        //    SqlCommand checkdoctor = new SqlCommand("CheckPrivilegeCard", con);
        //    checkdoctor.CommandType = CommandType.StoredProcedure;
        //    checkdoctor.Parameters.AddWithValue("@HospitalID", HospitalID);
        //    checkdoctor.Parameters.AddWithValue("@LocationID", LocationID);
        //    //checkdoctor.Parameters.AddWithValue("@DoctorID", 0);
        //    checkdoctor.Parameters.AddWithValue("@PrivilegeCardID", 0);
        //    checkdoctor.Parameters.AddWithValue("@CardName", PrivilegeCardName.ToUpper());
        //    checkdoctor.Parameters.Add("@NameExists", SqlDbType.Bit, 50);
        //    checkdoctor.Parameters["@NameExists"].Direction = ParameterDirection.Output;
        //    con.Open();
        //    int i = checkdoctor.ExecuteNonQuery();
        //    NameExists = (bool)checkdoctor.Parameters["@NameExists"].Value;
        //    con.Close();
        //    if (i >= 1)
        //    {
        //        return Convert.ToBoolean(NameExists);
        //    }
        //    else
        //    {
        //        return Convert.ToBoolean(NameExists);
        //    }
        //}
        public bool CheckPrivilegeCard(int PrivilegeCardID, string PrivilegeCardName)
        {
            Connect();


            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckPrivilegeCard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@PrivilegeCardID", PrivilegeCardID);
                cmd.Parameters.AddWithValue("@CardName", PrivilegeCardName);
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

        # region Delete PrivilegeCard Details


        public string DeletePrivilegeCard(int PrivilegeCardID)
        {
            string Table = string.Empty;
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("DeletePrivilegeCard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PrivilegeCardID", PrivilegeCardID);
                con.Open();
                int i = cmd.ExecuteNonQuery();
             
                con.Close();
                if (i > 0)
                {
                    return "Done";
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

        # endregion
    






    }
}