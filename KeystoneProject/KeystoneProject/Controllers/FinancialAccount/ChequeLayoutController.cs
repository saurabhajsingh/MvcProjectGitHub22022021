using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Patient;
using KeystoneProject.Buisness_Logic.Patient;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
namespace KeystoneProject.Controllers.FinancialAccount
{
    public class ChequeLayoutController : Controller
    {
        private SqlConnection con;
        int HospitalID = Convert.ToInt32(System.Web.HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(System.Web.HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]);
        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }
        int PayeeL = 0;
        int PayeeT = 0;
        int DateL = 0;
        int DateT = 0;
        int AmtInWord1L = 0;
        int AmtINWord1T = 0;
        int AmtINWord2L = 0;
        int AmtInWord2T = 0;
        int AmountL = 0;
        int AmountT = 0;
        //
        // GET: /ChequeLayout/
            
        public ActionResult ChequeLayout()
        {
            Session["dsChequeLayout"] = null;
            Session["PayeeL"] = 1080;
            Session["PayeeT"] = 1080;
            Session["DateL"] = 8520;
            Session["DateT"] = 700;
            Session["AmtInWord1L"] = 1680;
            Session["AmtINWord1T"] = 1560;
            Session["AmtINWord2L"] = 840;
            Session["AmtInWord2T"] = 2040;
            Session["AmountL"] = 8760;
            Session["AmountT"] = 2040;
            return View();
        }

        string PhotoPath = "";
        public JsonResult SaveImagePathReportsHeader(KeystoneProject.Models.FinancialAccount.ChequeLayout model)
        {
            String Path1 = "";
            string path = "";
            //PatientMRD_Model Model
            // PatientMRD_Model obj = new PatientMRD_Model();
            var file = model.ImageFile;
            if (file != null)
            {
                //var path = "";
                var filename = Path.GetFileName(file.FileName);
                var extention = Path.GetExtension(file.FileName);
                var filenamethoutextenction = Path.GetFileNameWithoutExtension(file.FileName);
                //  file.SaveAs(Server.MapPath("~/MRDFiles/" + file.FileName));
                //  Session["Paper"] = "/MRDFiles/" + file.FileName;

                path = Server.MapPath("~/") + "MRDFiles/" + file.FileName;

                file.SaveAs(path);
                // Session["Paper"] = path;
                ViewData["ImagePath"] = "/MRDFiles/" + file.FileName;
                path = "/MRDFiles/" + file.FileName;
                path = Server.MapPath("~/") + "MRDFiles/" + file.FileName;
              //  IUFillData(path, obj);
                string Name = AutoGanrateferecnCode();
                 Path1 = "" + PhotoPath + "" + Name + ".jpg";
               // file.SaveAs(@Path1);
                //return Path1;

               //  path = @"C:\Users\Kapsicumweb\Pictures\Koala.jpg";
            }
            return new JsonResult { Data = path, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

      
        public String AutoGanrateferecnCode()//string Firstword)
        {
            string Reference = "";
            Reference += "PI";//Firstword;
            Reference += (DateTime.Now.Year.ToString()).Substring(2, 2);
            Reference += DateTime.Now.Month.ToString();
            Reference += DateTime.Now.Day.ToString();
            Reference += DateTime.Now.Hour.ToString();
            Reference += DateTime.Now.Minute.ToString();
            Reference += DateTime.Now.Second.ToString();
            Reference += DateTime.Now.Millisecond.ToString();
            return Reference;
        }
        Buisness_Logic.FinancialAccount.BL_ChequeLayout objBL_ChequeLayout_R = new Buisness_Logic.FinancialAccount.BL_ChequeLayout();
       // DataSet dsChequeLayout = new DataSet();
        string Mode = "Add";
        private void IUFillData(string Path,  KeystoneProject.Models.FinancialAccount.ChequeLayout obj)
        {
            obj.dsChequeLayout = new DataSet();
            obj.dsChequeLayout = (DataSet)Session["dsChequeLayout"];
            if ((DataSet)Session["dsChequeLayout"] == null)
            {
                
              obj.dsChequeLayout = objBL_ChequeLayout_R.GetChequeLayout(HospitalID, LocationID, 0);
              DataRow dr = obj.dsChequeLayout.Tables[0].NewRow();
              
                    //if (ucChequeLayout1.chkHideBackGround.Checked == false)
                    //{
                   //System.Drawing.Image img = new System.Drawing.Bitmap(Path);
                   //  string Name = AutoGanrateferecnCode();
                   //  String Path1 = "MRDFiles/" + PhotoPath + "" + Name + ".jpg";
                   //   img.Save(Path1);
                   //  byte[] imgdata = System.IO.File.ReadAllBytes(Server.MapPath(Path));

              FileStream file3 = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.Read);
              byte[] stphoto3 = new byte[file3.Length];
              file3.Read(stphoto3, 0, System.Convert.ToInt32(file3.Length));
              file3.Close();


              dr["BackImage"] = stphoto3;
                       
                    //}
                    //else { dr["BackImage"] = null; }
                
                dr["HospitalID"] = HospitalID;
                dr["LocationID"] = LocationID;
                dr["ChequeLayoutID"] = 0;
                dr["BankID"] = obj.BankID;
                dr["ImageHieght"] = obj.txtHieght;
                dr["IMageWidth"] = obj.txtWidth;
                dr["Date_L"] = obj.DateL;
                dr["Date_T"] = obj.DateT;
                dr["Payee_L"] = obj.PayeeL;
                dr["Payee_T"] = obj.PayeeT;
                dr["AmtWord1_L"] = obj.AmtInWord1L;
                dr["AmtWord1_T"] = obj.AmtINWord1T;
                dr["AmtWord2_L"] = obj.AmtINWord2L;
                dr["AmtWord2_T"] = obj.AmtInWord2T;
                dr["Amt_L"] = obj.AmountL;
                dr["Amt_T"] = obj.AmountT;
                dr["CreationID"] = UserID;
                dr["Mode"] = this.Mode.ToString();
                obj.dsChequeLayout.Tables[0].Rows.Add(dr);
            }
            else
            {
                                   
                 //["ChequeLayoutID"] = 0;
                    obj.dsChequeLayout.Tables[0].Rows[0]["BankID"] = obj.BankID;
                    //dr["BackImage"] = "Santosh Gupta";
                    obj.dsChequeLayout.Tables[0].Rows[0]["ImageHieght"]=obj.txtHieght ;
                    obj.dsChequeLayout.Tables[0].Rows[0]["IMageWidth"]=obj.txtWidth ;
                   obj.dsChequeLayout.Tables[0].Rows[0]["Date_L"] = obj.DateL ;
                    obj.dsChequeLayout.Tables[0].Rows[0]["Date_T"]= obj.DateT ;
                   obj.dsChequeLayout.Tables[0].Rows[0]["Payee_L"]= obj.PayeeL ;
                    obj.dsChequeLayout.Tables[0].Rows[0]["Payee_T"]=obj.PayeeT ;
                    obj.dsChequeLayout.Tables[0].Rows[0]["AmtWord1_L"]= obj.AmtInWord1L;
                  obj.dsChequeLayout.Tables[0].Rows[0]["AmtWord1_T"] = obj.AmtINWord1T;
                     obj.dsChequeLayout.Tables[0].Rows[0]["AmtWord2_L"]=obj.AmtINWord2L;
                  obj.dsChequeLayout.Tables[0].Rows[0]["AmtWord2_T"] = obj.AmtInWord2T;
                    obj.dsChequeLayout.Tables[0].Rows[0]["Amt_L"] =obj.AmountL;
                     obj.dsChequeLayout.Tables[0].Rows[0]["Amt_T"]=obj.AmountT;

                   
            }
            Session["dsChequeLayout"] = (DataSet)obj.dsChequeLayout;
        }

        KeystoneProject.Models.FinancialAccount.ChequeLayout obj = new Models.FinancialAccount.ChequeLayout();

        
        public ActionResult btnDown_Click(string cmbFieldName, string txtPayeeTop,string Path,string Height,string wight)
        {
            obj.cmbFieldName = cmbFieldName;
            obj.txtHieght =int.Parse( wight);
            obj.txtWidth = int.Parse( Height);
            PayeeL = Convert.ToInt32(Session["PayeeL"]);
            PayeeT = Convert.ToInt32(Session["PayeeT"]);
            DateL = Convert.ToInt32(Session["DateL"]);
            DateT = Convert.ToInt32(Session["DateT"]);
            AmtInWord1L = Convert.ToInt32(Session["AmtInWord1L"]);
            AmtINWord1T = Convert.ToInt32(Session["AmtINWord1T"]);
            AmtINWord2L = Convert.ToInt32(Session["AmtINWord2L"]);
            AmtInWord2T = Convert.ToInt32(Session["AmtInWord2T"]);
            AmountL = Convert.ToInt32(Session["AmountL"]);
            AmountT = Convert.ToInt32(Session["AmountT"]);

            if (cmbFieldName == "PAYEE NAME")
            {

                obj.PayeeT+= PayeeT + 50;
                txtPayeeTop = obj.PayeeT.ToString();
                IUFillData(Path,obj);
             //   ViewReport();
            }
            else if (cmbFieldName == "DATE")
            {
                obj.DateT+= DateT + 50;
                DateT = obj.DateT;
                txtPayeeTop = obj.DateT.ToString();
                IUFillData(Path,obj);
                //ViewReport();
            }
            else if (cmbFieldName == "AMOUNT IN WORD -1")
            {
                obj.AmtINWord1T+= AmtINWord1T + 50;
                txtPayeeTop = obj.AmtINWord1T.ToString();
                IUFillData(Path,obj);
               // ViewReport();
            }
            else if (cmbFieldName == "AMOUNT IN WORD -2")
            {
                obj.AmtInWord2T+= AmtInWord2T + 50;
                txtPayeeTop= obj.AmtInWord2T.ToString();
                IUFillData(Path, obj);
                //ViewReport();
            }
            else if (cmbFieldName == "AMOUNT")
            {
                obj.AmountT+= AmountT + 50;
                txtPayeeTop = obj.AmountT.ToString();
                IUFillData(Path, obj);
                //ViewReport();
             //   Session[""]

            }

            Session["PayeeL"] = PayeeL;
            Session["PayeeT"] = PayeeT;
            Session["DateL"] = DateL;
            Session["DateT"] = DateT;
            Session["AmtInWord1L"] = AmtInWord1L;
            Session["AmtINWord1T"] = AmtINWord1T;
            Session["AmtINWord2L"] = AmtINWord2L;
            Session["AmtInWord2T"] = AmtInWord2T;
            Session["AmountL"] = AmountL;
            Session["AmountT"] = AmountT;
            return new JsonResult { Data = txtPayeeTop, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult btnLeft_Click(string cmbFieldName, string txtPayeeLeft, string Path, int Height, int wight)
        {
            PayeeL = Convert.ToInt32(Session["PayeeL"]);
            PayeeT = Convert.ToInt32(Session["PayeeT"]);
            DateL = Convert.ToInt32(Session["DateL"]);
            DateT = Convert.ToInt32(Session["DateT"]);
            AmtInWord1L = Convert.ToInt32(Session["AmtInWord1L"]);
            AmtINWord1T = Convert.ToInt32(Session["AmtINWord1T"]);
            AmtINWord2L = Convert.ToInt32(Session["AmtINWord2L"]);
            AmtInWord2T = Convert.ToInt32(Session["AmtInWord2T"]);
            AmountL = Convert.ToInt32(Session["AmountL"]);
            AmountT = Convert.ToInt32(Session["AmountT"]);
            obj.cmbFieldName = cmbFieldName;
            obj.txtHieght = Convert.ToInt32(Height);
            obj.txtWidth = Convert.ToInt32(wight);
            if (cmbFieldName == "PAYEE NAME")
            {
                obj.PayeeL+= PayeeL - 50;
                txtPayeeLeft = obj.PayeeL.ToString();
                IUFillData(Path, obj);
            }
            else if (cmbFieldName == "DATE")
            {
                obj.DateL+= DateL - 50;
                txtPayeeLeft = obj.DateL.ToString();
                IUFillData(Path, obj);
            }
            else if (cmbFieldName == "AMOUNT IN WORD -1")
            {
                obj.AmtInWord1L+= AmtInWord1L - 50;
                txtPayeeLeft = obj.AmtInWord1L.ToString();
                IUFillData(Path, obj);
            }
            else if (cmbFieldName == "AMOUNT IN WORD -2")
            {
                obj.AmtINWord2L+= AmtINWord2L - 50;
                txtPayeeLeft = obj.AmtINWord2L.ToString();
                IUFillData(Path, obj);
            }
            else if (cmbFieldName == "AMOUNT")
            {

                obj.AmountL+= AmountL - 50;
                txtPayeeLeft = obj.AmountL.ToString();
                IUFillData(Path, obj);
            }
            Session["PayeeL"] =obj. PayeeL;
            Session["PayeeT"] = obj.PayeeT;
            Session["DateL"] = obj.DateL;
            Session["DateT"] = obj.DateT;
            Session["AmtInWord1L"] = obj.AmtInWord1L;
            Session["AmtINWord1T"] = obj.AmtINWord1T;
            Session["AmtINWord2L"] = obj.AmtINWord2L;
            Session["AmtInWord2T"] = obj.AmtInWord2T;
            Session["AmountL"] = obj.AmountL;
            Session["AmountT"] = obj.AmountT;
            return new JsonResult { Data = txtPayeeLeft, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        public JsonResult btnUp_Click(string cmbFieldName, string txtPayeeTop, string Path, int Height, int wight)
        {
            PayeeL = Convert.ToInt32(Session["PayeeL"]);
            PayeeT = Convert.ToInt32(Session["PayeeT"]);
            DateL = Convert.ToInt32(Session["DateL"]);
            DateT = Convert.ToInt32(Session["DateT"]);
            AmtInWord1L = Convert.ToInt32(Session["AmtInWord1L"]);
            AmtINWord1T = Convert.ToInt32(Session["AmtINWord1T"]);
            AmtINWord2L = Convert.ToInt32(Session["AmtINWord2L"]);
            AmtInWord2T = Convert.ToInt32(Session["AmtInWord2T"]);
            AmountL = Convert.ToInt32(Session["AmountL"]);
            AmountT = Convert.ToInt32(Session["AmountT"]);
            obj.txtHieght = wight;
            obj.txtWidth = Height;
            obj.cmbFieldName = cmbFieldName;
            if (obj.cmbFieldName == "PAYEE NAME")
            {
                obj.PayeeT+= PayeeT - 50;
               txtPayeeTop = obj.PayeeT.ToString();
               IUFillData(Path, obj);
            }
            else if (cmbFieldName == "DATE")
            {
                obj.DateT+= DateT - 50;
                txtPayeeTop = obj.DateT.ToString();
                IUFillData(Path, obj);
            }
            else if (obj.cmbFieldName == "AMOUNT IN WORD -1")
            {
                obj.AmtINWord1T+= AmtINWord1T - 50;
                txtPayeeTop = obj.AmtINWord1T.ToString();
                IUFillData(Path, obj);
            }
            else if (obj.cmbFieldName == "AMOUNT IN WORD -2")
            {
                obj.AmtInWord2T+= AmtInWord2T - 50;
                txtPayeeTop = obj.AmtInWord2T.ToString();
                IUFillData(Path, obj);
            }
            else if (cmbFieldName == "AMOUNT")
            {
                obj.AmountT+= AmountT - 50;
                txtPayeeTop = obj.AmountT.ToString();
                IUFillData(Path, obj);
            }
            Session["PayeeL"] = PayeeL;
            Session["PayeeT"] = PayeeT;
            Session["DateL"] = DateL;
            Session["DateT"] = DateT;
            Session["AmtInWord1L"] = AmtInWord1L;
            Session["AmtINWord1T"] = AmtINWord1T;
            Session["AmtINWord2L"] = AmtINWord2L;
            Session["AmtInWord2T"] = AmtInWord2T;
            Session["AmountL"] = AmountL;
            Session["AmountT"] = AmountT;
            return new JsonResult { Data = txtPayeeTop, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        public JsonResult  btnRieght_Click(string cmbFieldName, string txtPayeeLeft, string Path, int Height, int wight)
        {
            PayeeL = Convert.ToInt32(Session["PayeeL"]);
            PayeeT = Convert.ToInt32(Session["PayeeT"]);
            DateL = Convert.ToInt32(Session["DateL"]);
            DateT = Convert.ToInt32(Session["DateT"]);
            AmtInWord1L = Convert.ToInt32(Session["AmtInWord1L"]);
            AmtINWord1T = Convert.ToInt32(Session["AmtINWord1T"]);
            AmtINWord2L = Convert.ToInt32(Session["AmtINWord2L"]);
            AmtInWord2T = Convert.ToInt32(Session["AmtInWord2T"]);
            AmountL = Convert.ToInt32(Session["AmountL"]);
            AmountT = Convert.ToInt32(Session["AmountT"]);
            obj.txtHieght = wight;
            obj.txtWidth = Height;
            obj.cmbFieldName = cmbFieldName;
            if (obj.cmbFieldName == "PAYEE NAME")
            {
                obj.PayeeL+= PayeeL + 50;
                txtPayeeLeft = obj.PayeeL.ToString();
                IUFillData(Path, obj);
            }
            else if ( obj.cmbFieldName == "DATE")
            {
                obj.DateL+= DateL + 50;
                txtPayeeLeft = obj.DateL.ToString();
                IUFillData(Path, obj);
            }
            else if (obj.cmbFieldName == "AMOUNT IN WORD -1")
            {
                obj.AmtInWord1L+= AmtInWord1L + 50;
                txtPayeeLeft = obj.AmtInWord1L.ToString();
                IUFillData(Path, obj);
            }
            else if (obj.cmbFieldName == "AMOUNT IN WORD -2")
            {
                obj.AmtINWord2L+= AmtINWord2L + 50;
                txtPayeeLeft = obj.AmtINWord2L.ToString();
                IUFillData(Path, obj);
            }
            else if (obj.cmbFieldName == "AMOUNT")
            {
                obj.AmountL+= AmountL + 50;
                txtPayeeLeft = obj.AmountL.ToString();
                IUFillData(Path, obj);
            }
            Session["PayeeL"] = PayeeL;
            Session["PayeeT"] = PayeeT;
            Session["DateL"] = DateL;
            Session["DateT"] = DateT;
            Session["AmtInWord1L"] = AmtInWord1L;
            Session["AmtINWord1T"] = AmtINWord1T;
            Session["AmtINWord2L"] = AmtINWord2L;
            Session["AmtInWord2T"] = AmtInWord2T;
            Session["AmountL"] = AmountL;
            Session["AmountT"] = AmountT;
            return new JsonResult { Data = txtPayeeLeft, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }





        void txtPayeeTop_Leave(string cmbFieldName, string txtPayeeTop)
        {
            if (cmbFieldName == "PAYEE NAME")
            {
                obj.PayeeT = Convert.ToInt32(txtPayeeTop);
            }
            else if (cmbFieldName == "DATE")
            {

                obj.DateT = Convert.ToInt32(txtPayeeTop);
            }
            else if (cmbFieldName == "AMOUNT IN WORD -1")
            {

                obj.AmtINWord1T = Convert.ToInt32(txtPayeeTop);
            }
            else if (cmbFieldName == "AMOUNT IN WORD -2")
            {

                obj.AmtInWord2T = Convert.ToInt32(txtPayeeTop);
            }
            else if (cmbFieldName == "AMOUNT")
            {

                obj.AmountT = Convert.ToInt32(txtPayeeTop);
            }
        }
        void txtPayeeLeft_Leave(string cmbFieldName, string txtPayeeLeft)
        {
            if (cmbFieldName == "PAYEE NAME")
            {
                obj.PayeeL = Convert.ToInt32(txtPayeeLeft);

            }
            else if (cmbFieldName == "DATE")
            {
                obj.DateL = Convert.ToInt32(txtPayeeLeft);

            }
            else if (cmbFieldName == "AMOUNT IN WORD -1")
            {
                obj.AmtInWord1L = Convert.ToInt32(txtPayeeLeft);

            }
            else if (cmbFieldName == "AMOUNT IN WORD -2")
            {
                obj.AmtINWord2L = Convert.ToInt32(txtPayeeLeft);

            }
            else if (cmbFieldName == "AMOUNT")
            {
                obj.AmountL = Convert.ToInt32(txtPayeeLeft);

            }
        }
        public JsonResult GetAllChequeLayout()
        {
            Buisness_Logic.FinancialAccount.BL_ChequeLayout Bl_obj = new Buisness_Logic.FinancialAccount.BL_ChequeLayout();

            List<KeystoneProject.Models.FinancialAccount.ChequeLayout> serchAdd = new List<Models.FinancialAccount.ChequeLayout>();
            DataSet ds=Bl_obj.GetAllChequeLayout(1,1);
            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                int ChequeLayoutID = Convert.ToInt32(dr["ChequeLayoutID"]);
                int BankID = Convert.ToInt32(dr["BankID"]);
            serchAdd.Add(new Models.FinancialAccount.ChequeLayout  {
                LeyOutName = dr["LayoutName"].ToString(),
                BackImage = dr["BackImage"].ToString(),
           ChequeLayoutID=ChequeLayoutID,
           BankID = BankID,

            });
            }
            return new JsonResult { Data = serchAdd, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult FillData(int ChequeLayoutID)
        {
            Buisness_Logic.FinancialAccount.BL_ChequeLayout Bl_obj = new Buisness_Logic.FinancialAccount.BL_ChequeLayout();
            List<KeystoneProject.Models.FinancialAccount.ChequeLayout> serchAdd = new List<Models.FinancialAccount.ChequeLayout>();
            DataSet dsChequeLayout = new DataSet();
            dsChequeLayout = objBL_ChequeLayout_R.GetChequeLayout(HospitalID, LocationID, ChequeLayoutID);
            if (dsChequeLayout.Tables[0].Rows.Count > 0)
            {

                obj.LeyOutName = dsChequeLayout.Tables[0].Rows[0]["LayoutName"].ToString();
                //ucChequeLayout1.txtHieght.Text = dsChequeLayout.Tables[0].Rows[0]["ImageHieght"].ToString();
                //ucChequeLayout1.txtWidth.Text = dsChequeLayout.Tables[0].Rows[0]["IMageWidth"].ToString();
              obj.BankID = Convert.ToInt32(dsChequeLayout.Tables[0].Rows[0]["BankID"].ToString());
               obj. Flag = true;
               Session["dsChequeLayout"] = (DataSet)dsChequeLayout;
               serchAdd.Add(obj);
            }
            return new JsonResult { Data = serchAdd, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

	}
}