using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
namespace KeystoneProject.Controllers.FinancialAccount
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        int ChequeLayoutID = 0;
        int iUserID;
        int HospitalID;
        int LocationID;
        string ModuleID = "";
        string strFinancialYear = null;
        DataSet NewRights = null;
        DateTime StartDateGlobal = new DateTime();
        DateTime EndDateGlobal = new DateTime();

        DataSet dsChequeLayout = new DataSet();

        int PayeeL = 1080;
        int PayeeT = 1080;
        int DateL = 8520;
        int DateT = 700;
        int AmtInWord1L = 1680;
        int AmtINWord1T = 1560;
        int AmtINWord2L = 840;
        int AmtInWord2T = 2040;
        int AmountL = 8760;
        int AmountT = 2040;
        string PhotoPath;
        Boolean Flag = false; 
 
        protected void Page_Load(object sender, EventArgs e)
        {
            txtChequeLayout.TextChanged += txtChequeLayout_TextChanged;

        }
        public void ChequeLayoutBind()
        {
            Buisness_Logic.FinancialAccount.BL_ChequeLayout Bl_obj = new Buisness_Logic.FinancialAccount.BL_ChequeLayout();
            DataSet ds = Bl_obj.GetAllChequeLayout(1, 1);

        }
        void txtChequeLayout_TextChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }


        void txtPayeeTop_Leave(object sender, EventArgs e)
        {
            if (cmbFieldName.Text == "PAYEE NAME")
            {
                PayeeT = Convert.ToInt32(txtPayeeTop.Text);
            }
            else if (cmbFieldName.Text == "DATE")
            {

                DateT = Convert.ToInt32(txtPayeeTop.Text);
            }
            else if (cmbFieldName.Text == "AMOUNT IN WORD -1")
            {

                AmtINWord1T = Convert.ToInt32(txtPayeeTop.Text);
            }
            else if (cmbFieldName.Text == "AMOUNT IN WORD -2")
            {

                AmtInWord2T = Convert.ToInt32(txtPayeeTop.Text);
            }
            else if (cmbFieldName.Text == "AMOUNT")
            {

                AmountT = Convert.ToInt32(txtPayeeTop.Text);
            }
        }

        void txtPayeeLeft_Leave(object sender, EventArgs e)
        {
            if (cmbFieldName.Text == "PAYEE NAME")
            {
                PayeeL = Convert.ToInt32(txtPayeeLeft.Text);

            }
            else if (cmbFieldName.Text == "DATE")
            {
                DateL = Convert.ToInt32(txtPayeeLeft.Text);

            }
            else if (cmbFieldName.Text == "AMOUNT IN WORD -1")
            {
                AmtInWord1L = Convert.ToInt32(txtPayeeLeft.Text);

            }
            else if (cmbFieldName.Text == "AMOUNT IN WORD -2")
            {
                AmtINWord2L = Convert.ToInt32(txtPayeeLeft.Text);

            }
            else if (cmbFieldName.Text == "AMOUNT")
            {
                AmountL = Convert.ToInt32(txtPayeeLeft.Text);

            }
        }
        void cmbFieldName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFieldName.Text == "PAYEE NAME")
            {
               txtPayeeLeft.Text = PayeeL.ToString();
               txtPayeeTop.Text = PayeeT.ToString();
            }
            else if (cmbFieldName.Text == "DATE")
            {
                txtPayeeLeft.Text = DateL.ToString();
                txtPayeeTop.Text = DateT.ToString();
            }
            else if (cmbFieldName.Text == "AMOUNT IN WORD -1")
            {
               txtPayeeLeft.Text = AmtInWord1L.ToString();
                txtPayeeTop.Text = AmtINWord1T.ToString();
            }
            else if (cmbFieldName.Text == "AMOUNT IN WORD -2")
            {
               txtPayeeLeft.Text = AmtINWord2L.ToString();
                txtPayeeTop.Text = AmtInWord2T.ToString();
            }
            else if (cmbFieldName.Text == "AMOUNT")
            {
                txtPayeeLeft.Text = AmountL.ToString();
                txtPayeeTop.Text = AmountT.ToString();
            }
        }
      
     
        
        void btnDown_Click(object sender, EventArgs e)
        {
            if (cmbFieldName.Text == "PAYEE NAME")
            {
                PayeeT = PayeeT + 50;
                txtPayeeTop.Text = PayeeT.ToString();
                ViewReport();
            }
            else if (cmbFieldName.Text == "DATE")
            {
                DateT = DateT + 50;
                txtPayeeTop.Text = DateT.ToString();
                ViewReport();
            }
            else if (cmbFieldName.Text == "AMOUNT IN WORD -1")
            {
                AmtINWord1T = AmtINWord1T + 50;
                txtPayeeTop.Text = AmtINWord1T.ToString();
                ViewReport();
            }
            else if (cmbFieldName.Text == "AMOUNT IN WORD -2")
            {
                AmtInWord2T = AmtInWord2T + 50;
               txtPayeeTop.Text = AmtInWord2T.ToString();
                ViewReport();
            }
            else if (cmbFieldName.Text == "AMOUNT")
            {
                AmountT = AmountT + 50;
                txtPayeeTop.Text = AmountT.ToString();
                ViewReport();
            }
        }
        private void ViewReport()
        {
            //IUFillData();
            //ChequePrint rpt1 = new ChequePrint();
            //rpt1.SetDataSource(dsChequeLayout);

            //BlobFieldObject objBlobFieldObject = (BlobFieldObject)rpt1.ReportDefinition.Sections[0].ReportObjects["BackImage1"];
            //objBlobFieldObject.Height = Convert.ToInt32(txtHieght.Text);
            //objBlobFieldObject.Width = Convert.ToInt32(txtWidth.Text);

            //TextObject objPayName = (TextObject)rpt1.ReportDefinition.Sections[0].ReportObjects["Payeename"];
            //objPayName.Left = PayeeL;
            //objPayName.Top = PayeeT;

            //TextObject objDate = (TextObject)rpt1.ReportDefinition.Sections[0].ReportObjects["Date"];
            //objDate.Left = DateL;
            //objDate.Top = DateT;

            //TextObject objAmountInWord1 = (TextObject)rpt1.ReportDefinition.Sections[0].ReportObjects["AmountInWord1"];
            //objAmountInWord1.Left = AmtInWord1L;
            //objAmountInWord1.Top = AmtINWord1T;

            //TextObject objAmountInWord2 = (TextObject)rpt1.ReportDefinition.Sections[0].ReportObjects["AmountInWord2"];
            //objAmountInWord2.Left = AmtINWord2L;
            //objAmountInWord2.Top = AmtInWord2T;

            //TextObject objAmount = (TextObject)rpt1.ReportDefinition.Sections[0].ReportObjects["Amount"];
            //objAmount.Left = AmountL;
            //objAmount.Top = AmountT;
            //crystalReportViewer1.ReportSource = rpt1;

        }

        protected void btnUp_Click_Click(object sender, EventArgs e)
        {
              
            if (cmbFieldName.Text == "PAYEE NAME")
            {
                PayeeT = PayeeT - 50;
                txtPayeeTop.Text = PayeeT.ToString();
                ViewReport();
            }
            else if (cmbFieldName.Text == "DATE")
            {
                DateT = DateT - 50;
                txtPayeeTop.Text = DateT.ToString();
                ViewReport();
            }
            else if (cmbFieldName.Text == "AMOUNT IN WORD -1")
            {
                AmtINWord1T = AmtINWord1T - 50;
                txtPayeeTop.Text = AmtINWord1T.ToString();
                ViewReport();
            }
            else if (cmbFieldName.Text == "AMOUNT IN WORD -2")
            {
                AmtInWord2T = AmtInWord2T - 50;
                txtPayeeTop.Text = AmtInWord2T.ToString();
                ViewReport();
            }
            else if (cmbFieldName.Text == "AMOUNT")
            {
                AmountT = AmountT - 50;
                txtPayeeTop.Text = AmountT.ToString();
                ViewReport();
            

        }
        }

        protected void btnRieght_Click_Click(object sender, EventArgs e)
        {
              
            if (cmbFieldName.Text == "PAYEE NAME")
            {
                PayeeL = PayeeL + 50;
                txtPayeeLeft.Text = PayeeL.ToString();
                ViewReport();
            }
            else if (cmbFieldName.Text == "DATE")
            {
                DateL = DateL + 50;
               txtPayeeLeft.Text = DateL.ToString();
                ViewReport();
            }
            else if (cmbFieldName.Text == "AMOUNT IN WORD -1")
            {
                AmtInWord1L = AmtInWord1L + 50;
                txtPayeeLeft.Text = AmtInWord1L.ToString();
                ViewReport();
            }
            else if (cmbFieldName.Text == "AMOUNT IN WORD -2")
            {
                AmtINWord2L = AmtINWord2L + 50;
                txtPayeeLeft.Text = AmtINWord2L.ToString();
                ViewReport();
            }
            else if (cmbFieldName.Text == "AMOUNT")
            {
                AmountL = AmountL + 50;
                txtPayeeLeft.Text = AmountL.ToString();
                ViewReport();
            }

        
        }

        protected void btnLeft_Click_Click(object sender, EventArgs e)
        {
           
            if (cmbFieldName.Text == "PAYEE NAME")
            {
                PayeeL = PayeeL - 50;
                txtPayeeLeft.Text = PayeeL.ToString();
                ViewReport();
            }
            else if (cmbFieldName.Text == "DATE")
            {
                DateL = DateL - 50;
                txtPayeeLeft.Text = DateL.ToString();
                ViewReport();
            }
            else if (cmbFieldName.Text == "AMOUNT IN WORD -1")
            {
                AmtInWord1L = AmtInWord1L - 50;
                txtPayeeLeft.Text = AmtInWord1L.ToString();
                ViewReport();
            }
            else if (cmbFieldName.Text == "AMOUNT IN WORD -2")
            {
                AmtINWord2L = AmtINWord2L - 50;
               txtPayeeLeft.Text = AmtINWord2L.ToString();
                ViewReport();
            }
            else if (cmbFieldName.Text == "AMOUNT")
            {

                AmountL = AmountL - 50;
               txtPayeeLeft.Text = AmountL.ToString();
                ViewReport();
            }
        
        }
    }
}