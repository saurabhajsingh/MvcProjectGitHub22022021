using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using KeystoneProject.Buisness_Logic.MasterLaboratory;
using KeystoneProject.Models.Master;
using System.Net;

namespace KeystoneProject.Buisness_Logic.MasterLaboratory
{
    public class BL_Parameter
    {
        private SqlConnection con;
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

        List<Parameter> searchList = new List<Parameter>();
        Parameter objParameter = new Parameter();
        //BL_Parameter _Parameter = new BL_Parameter();
        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);

        }


        #region Delete
        public bool DeleteParameter(int id)
        {
            try
            {
                Connect();
                SqlParameter[] aParams = new SqlParameter[3];
                aParams[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                aParams[0].Value = HospitalID;
                aParams[1] = new SqlParameter("@ParameterID", SqlDbType.Int);
                aParams[1].Value = id;
                aParams[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                aParams[2].Value = LocationID;
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "DeleteParameter", aParams);
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

        public bool CheckParameter(string ParameterID, string ParameterName)
        {
            string t = "";
            if (ParameterID == null || ParameterID == "")
            {
                t = "0";
            }
            else
            {
                t = ParameterID;
            }

            Connect();
            bool Table;
            bool flag;
            try
            {
                SqlCommand checkHearder = new SqlCommand("CheckParameter", con);
                checkHearder.CommandType = CommandType.StoredProcedure;
                checkHearder.Parameters.AddWithValue("@HospitalID", HospitalID);
                checkHearder.Parameters.AddWithValue("@LocationID", LocationID);
                checkHearder.Parameters.AddWithValue("@ParameterID", t);
                checkHearder.Parameters.AddWithValue("@ParameterName", ParameterName);
                checkHearder.Parameters.Add("@NameExists", SqlDbType.Bit, 50);
                checkHearder.Parameters["@NameExists"].Direction = ParameterDirection.Output;
                con.Open();
                int i = checkHearder.ExecuteNonQuery();
                Table = (bool)checkHearder.Parameters["@NameExists"].Value;
                con.Close();
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
        public List<Parameter> ParameterFormulaID(string ParameterName)
        {


            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("select ParameterID,ParameterName from Parameter where ParameterName like ''+@ParameterName+'%' and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID = " + LocationID + "", con);

                cmd.Parameters.AddWithValue("@ParameterName", ParameterName);


                SqlDataAdapter ad = new SqlDataAdapter();
                DataTable dt = new DataTable();
                ad.SelectCommand = cmd;
                con.Open();
                ad.Fill(dt);
                con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    searchList.Add(
                        new Parameter
                        {
                            ParameterName = dr["ParameterName"].ToString(),
                            ParameterID = dr["ParameterID"].ToString(),

                        });
                }
            }
            catch (Exception ex)
            {
            }
            return searchList;

        }


        public List<string> GetUnitID(string Unit)
        {
            Connect();
            List<string> unitlist = new List<string>();

            SqlCommand cmd = new SqlCommand("select UnitName from Unit where UnitName like ''+@UnitName+'%' and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID = " + LocationID + "", con);
            cmd.Parameters.AddWithValue("@UnitName", Unit);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                unitlist.Add(Convert.ToString(dr["UnitName"]));

            }
            return unitlist;
        }



        public List<Parameter> GetParameterNormalRangeType()
        {
            Connect();
            SqlCommand cmd = new SqlCommand("GetParameterNormalRangeType", con);
            cmd.CommandType = CommandType.StoredProcedure;

            //cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            //cmd.Parameters.AddWithValue("@LocationID", LocationID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            con.Close();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(

                    new Parameter
                    {

                        DaysFrom = dr["DaysFrom"].ToString(),
                        DaysTo = dr["DaysTo"].ToString(),
                        Gender = dr["Gender"].ToString(),
                        ConvLow = dr["ConvLow"].ToString(),
                        ConvHigh = dr["ConvHigh"].ToString(),
                        ConvNormal = dr["ConvNormal"].ToString(),
                    });

            }

            return searchList;
        }


        public List<Parameter> GetParameterData(int ParameterID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("GetParameter", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@ParameterID", ParameterID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            con.Open();
            da.Fill(ds);
            con.Close();

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                searchList.Add(

                    new Parameter
                    {
                        ParameterID = item["ParameterID"].ToString(),
                        ParameterName = item["ParameterName"].ToString(),
                        PrintAs = item["PrintAs"].ToString(),
                        Alias = item["Alias"].ToString(),
                        Footer = WebUtility.HtmlDecode(item["Footer"].ToString()),
                        Method = item["Method"].ToString(),
                        Unit = item["Unit"].ToString(),
                        Precision = item["Precision"].ToString(),
                        Formula = item["Formula"].ToString(),
                        FormulaWithShortName = item["FormulaWithShortName"].ToString(),
                        SampleType = item["SampleType"].ToString(),


                    });
            }


            foreach (DataRow item in ds.Tables[1].Rows)
            {
                string high = "";
                string low = "";
                string norm = "";
                string def = "";
                if ((item["ConvHigh"]).ToString() == null || item["ConvHigh"].ToString() == "" || item["ConvHigh"].ToString() == "0")
                {
                    high = "0";
                }
                else
                {
                    high = item["ConvHigh"].ToString();
                }
                if ((item["ConvLow"]).ToString() == null || item["ConvLow"].ToString() == "" || item["ConvLow"].ToString() == "0")
                {
                    low = "0";
                }
                else
                {
                    low = item["ConvLow"].ToString();
                }
                if ((item["ConvNormal"]).ToString() == null || item["ConvNormal"].ToString() == "" || item["ConvNormal"].ToString() == "0")
                {
                    norm = "0";
                }
                else
                {
                    norm = item["ConvNormal"].ToString();
                }
                if ((item["Default"]).ToString() == null || item["Default"].ToString() == "" || item["Default"].ToString() == "0")
                {
                    def = "0";
                }
                else
                {
                    def = item["Default"].ToString();
                }

                searchList.Add(new Parameter
                {

                    //  ParameterID = item["NormalRangeID"].ToString(),
                    DaysFrom = item["DaysFrom"].ToString(),
                    DaysTo = item["DaysTo"].ToString(),
                    Sex = item["Gender"].ToString(),
                    //if(item["ConvHigh"].ToString()==null || item["ConvHigh"].ToString()== "0")
                    //{
                    //     high="";
                    //}
                    //else
                    //{
                    //     high = item["ConvHigh"].ToString();
                    //}
                    ConvHigh = high.ToString(),
                    ConvLow = low.ToString(),
                    ConvNormal = norm.ToString(),
                    Default = def.ToString(),

                });
            }

            foreach (DataRow item in ds.Tables[2].Rows)
            {
                searchList.Add(

                    new Parameter
                    {
                        HelpValueID = item["HelpValueID"].ToString(),
                        HelpValue = item["HelpValue"].ToString(),
                        ParameterID = item["ParameterID"].ToString(),
                    });
            }

            foreach (DataRow item in ds.Tables[3].Rows)
            {
                searchList.Add(new Parameter
                {

                    PrecisionID = item["PrecisionID"].ToString(),
                    ParameterID = item["ParameterID"].ToString(),
                    Precision = item["Precision"].ToString()


                });

            }

            foreach (DataRow item in ds.Tables[4].Rows)
            {
                searchList.Add(new Parameter
                {

                    ParameterID = item["ParameterID"].ToString(),
                    FormulaID = item["FormulaID"].ToString(),
                    Formula = item["Formula"].ToString(),
                    ParameterName = item["ParameterName"].ToString(),


                });

            }


            return searchList;

        }




        public List<Parameter> GetAllParameter()
        {
            Connect();

            SqlCommand cmd = new SqlCommand("GetAllParameter", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            con.Close();
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                searchList.Add(
                    new Parameter
                    {
                        ParameterID = item["ParameterID"].ToString(),
                        ParameterName = item["ParameterName"].ToString(),
                        PrintAs = item["PrintAs"].ToString(),
                        Alias = item["Alias"].ToString(),
                        Method = item["Method"].ToString(),
                        Formula = item["Formula"].ToString(),

                    });


            }
            return searchList;

        }




        #region BindSampleType
        public List<string[]> GetTestMasterForBindSampleType()
        {
            Connect();
            List<string[]> samplelist = new List<string[]>();
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("GetTestMasterForBindSampleType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                //  cmd.Parameters.AddWithValue("@ParameterID", ParameterID);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(dt);
                //samplelist.Add(new string[]
                //    {
                //       "----Select----", "----Select----"
                //    });
                foreach (DataRow dr in dt.Rows)
                {
                    samplelist.Add(new string[]
                    {

                        dr["SampleType"].ToString(),dr["SampleTypeID"].ToString()
                    });

                }
            }
            catch (Exception ex)
            {

            }
            return samplelist;
        }

        #endregion


        public bool save(Parameter paraobj)
        {
            string Mode = "";
            Connect();
            bool flag = false;


            try
            {

                con.Open();
                SqlCommand cmd = new SqlCommand("IUParameter", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                if (paraobj.ParameterID == "" || paraobj.ParameterID == null || paraobj.ParameterID == "0")
                {
                    cmd.Parameters.AddWithValue("@ParameterID", 0);
                    cmd.Parameters["@ParameterID"].Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Mode", "Add");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ParameterID", paraobj.ParameterID);
                    cmd.Parameters.AddWithValue("@Mode", "Edit");
                }

                cmd.Parameters.AddWithValue("@ParameterName", paraobj.ParameterName);
                cmd.Parameters.AddWithValue("@PrintAs", paraobj.PrintAs);
                cmd.Parameters.AddWithValue("@Alias", paraobj.Alias);
                if (paraobj.Description == null)
                {
                    cmd.Parameters.AddWithValue("@Footer", "");
                }
                else
                {
                    //cmd.Parameters.AddWithValue("@Footer", paraobj.Description);
                    if (paraobj.Description == "<p><br></p>" || paraobj.Description == "<h1><br></h1>" || paraobj.Description == "<h2><br></h2>" || paraobj.Description == "<h3><br></h3>")
                    {
                        cmd.Parameters.AddWithValue("@Footer", "");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Footer", (paraobj.Description));
                    }


                }
                cmd.Parameters.AddWithValue("@Method", paraobj.Method);
                cmd.Parameters.AddWithValue("@Unit", paraobj.Unit);
                cmd.Parameters.AddWithValue("@DiffCount", "");
                cmd.Parameters.AddWithValue("@SampleType", paraobj.SampleType);
                cmd.Parameters.AddWithValue("@Precision", "");
                cmd.Parameters.AddWithValue("@ReferenceCode", "");
                cmd.Parameters.AddWithValue("@Formula", paraobj.Formula);
                if (paraobj.Formula == "" || paraobj.Formula == null)
                {
                    cmd.Parameters.AddWithValue("@FormulaWithShortName", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@FormulaWithShortName", paraobj.FormulaWithShortName);
                }

                cmd.Parameters.AddWithValue("@Suffix", "");
                cmd.Parameters.AddWithValue("@SendSMS", "No");
                cmd.Parameters.AddWithValue("@GTTGraph", "No");
                cmd.Parameters.AddWithValue("@CreationID", UserID);
                //cmd.Parameters.AddWithValue("@Mode", Mode);
                //con.Open();
                int intResult = cmd.ExecuteNonQuery();

                paraobj.ParameterID = Convert.ToInt32(cmd.Parameters["@ParameterID"].Value).ToString();
                // con.Close();

                if (intResult > 0)
                {
                    if (paraobj.DaysFrom != null)
                    {
                        string[] DaysFrom11 = paraobj.DaysFrom.Split(',');
                        string[] DaysTo11 = paraobj.DaysTo.Split(',');
                        string[] Sex11 = paraobj.Sex.Split(',');
                        string[] ConvLow11 = paraobj.ConvLow.Split(',');
                        string[] ConvHigh11 = paraobj.ConvHigh.Split(',');
                        string[] ConvNormal11 = paraobj.ConvNormal.Split(',');
                        string[] Default11 = paraobj.Default.Split(',');
                        for (int count = 0; count < DaysFrom11.Length; count++)
                        {
                            SqlCommand cmd1 = new SqlCommand("IUParameterNormalRange", con);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                            cmd1.Parameters.AddWithValue("@ParameterID", paraobj.ParameterID);

                            //if (paraobj.NormalRangeID == null || paraobj.NormalRangeID == "")
                            //{
                            cmd1.Parameters.AddWithValue("@NormalRangeID", 0);
                            cmd1.Parameters.AddWithValue("@Mode", "Add");
                            //}
                            //else
                            //{
                            //    cmd1.Parameters.AddWithValue("@NormalRangeID", paraobj.NormalRangeID);
                            //    cmd1.Parameters.AddWithValue("@Mode", "Edit");
                            //}


                            cmd1.Parameters.AddWithValue("@DaysFrom", DaysFrom11[count]);
                            cmd1.Parameters.AddWithValue("@DaysTo", DaysTo11[count]);
                            cmd1.Parameters.AddWithValue("@Gender", Sex11[count]);
                            cmd1.Parameters.AddWithValue("@ConvLow", ConvLow11[count]);
                            cmd1.Parameters.AddWithValue("@ConvHigh", ConvHigh11[count]);
                            cmd1.Parameters.AddWithValue("@ConvNormal", ConvNormal11[count]);
                            cmd1.Parameters.AddWithValue("@Default", Default11[count]);

                            cmd1.Parameters.AddWithValue("@CreationID", UserID);
                            //cmd1.Parameters.AddWithValue("@Mode", paraobj.Mode);
                            // con.Open();
                            cmd1.ExecuteNonQuery();
                            // con.Close();



                        }

                    }
                }

                if (intResult > 0)
                {
                    if (paraobj.HelpValue != null)
                    {
                        string[] HelpValue11 = paraobj.HelpValue.Split(',');
                        //string[] ParameterID11 = paraobj.ParameterID.Split(',');

                        for (int i = 0; i < HelpValue11.Length; i++)
                        {
                            if (HelpValue11[i] != "")
                            {
                                SqlCommand cmd1 = new SqlCommand("IUParamaterHelpValue", con);
                                cmd1.CommandType = CommandType.StoredProcedure;
                                cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                                cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                                cmd1.Parameters.AddWithValue("@ParameterID", paraobj.ParameterID);

                                //if (paraobj.HelpValueID == null || paraobj.HelpValueID == "")
                                //{
                                cmd1.Parameters.AddWithValue("@HelpValueID", 0);
                                cmd1.Parameters.AddWithValue("@Mode", "Add");
                                //}
                                //else
                                //{
                                //    cmd1.Parameters.AddWithValue("@HelpValueID", paraobj.HelpValueID);
                                //    cmd1.Parameters.AddWithValue("@Mode", "Edit");

                                //}

                                cmd1.Parameters.AddWithValue("@HelpValue", HelpValue11[i]);
                                cmd1.Parameters.AddWithValue("@CreationID", UserID);

                                //  con.Open();
                                cmd1.ExecuteNonQuery();
                                // con.Close();
                            }
                        }
                    }

                }

                if (intResult > 0)
                {

                    if (paraobj.FormulaID != "" && paraobj.FormulaID != null)
                    {
                        string[] FormulaID11 = paraobj.FormulaID.Split(',');
                        string[] Formula11 = paraobj.Formulareference.Split(',');


                        for (int i = 0; i < Formula11.Length; i++)

                        {
                            if (Formula11[i] != "")
                            {
                                SqlCommand cmd1 = new SqlCommand("IUParameterFormula", con);
                                cmd1.CommandType = CommandType.StoredProcedure;
                                cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                                cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                                cmd1.Parameters.AddWithValue("@ParameterID", paraobj.ParameterID);
                                cmd1.Parameters.AddWithValue("@FormulaID", FormulaID11[i]);
                                cmd1.Parameters.AddWithValue("@RefName", Formula11[i]);
                                cmd1.Parameters.AddWithValue("@Formula", "");
                                cmd1.Parameters.AddWithValue("@ParameterName", "");
                                cmd1.Parameters.AddWithValue("@CreationID", UserID);
                                cmd1.Parameters.AddWithValue("@Mode", "Add");
                                //  con.Open();
                                cmd1.ExecuteNonQuery();
                                // con.Close();

                            }
                        }

                    }

                }

                if (intResult > 0)
                {
                    if (paraobj.Precision != "" && paraobj.Precision != null)
                    {
                        string[] Precision11 = paraobj.Precision.Split(',');
                        //string[] ParameterID11 = paraobj.ParameterID.Split(',');


                        for (int i = 0; i < Precision11.Length; i++)
                        {
                            if (Precision11[i] != "")
                            {
                                SqlCommand cmd1 = new SqlCommand("IUParmaterPrecision", con);
                                cmd1.CommandType = CommandType.StoredProcedure;
                                cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                                cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                                cmd1.Parameters.AddWithValue("@ParameterID", paraobj.ParameterID);

                                //if (paraobj.PrecisionID == null || paraobj.PrecisionID == "")
                                //{
                                cmd1.Parameters.AddWithValue("@PrecisionID", 0);
                                cmd1.Parameters.AddWithValue("@Mode", "Add");
                                //}
                                //else
                                //{
                                //    cmd1.Parameters.AddWithValue("@PrecisionID", paraobj.PrecisionID);
                                //    cmd1.Parameters.AddWithValue("@Mode", "Edit");

                                //}

                                cmd1.Parameters.AddWithValue("@Precision", Precision11[i]);
                                cmd1.Parameters.AddWithValue("@SubParameterID", "");
                                cmd1.Parameters.AddWithValue("@CreationID", UserID);

                                // con.Open();
                                cmd1.ExecuteNonQuery();
                                //  con.Close();

                            }
                        }

                    }
                }
                con.Close();

            }
            catch (Exception ex)
            {




            }


            return true;
        }




    }
}