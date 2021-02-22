using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
namespace KeystoneProject.Models.FinancialAccount
{
    public class ChequeLayout
    {
        public HttpPostedFileWrapper ImageFile { get; set; }
        public int ChequeLayoutID = 0;
        public int iUserID { get; set; }
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public string ModuleID { get; set; }
        public string strFinancialYear { get; set; }
        public DataSet NewRights { get; set; }
        public DateTime StartDateGlobal { get; set; }
        public DateTime EndDateGlobal { get; set; }

        public DataSet dsChequeLayout { get; set; }
        public int BankID { get; set; }
        public int PayeeL { get; 
            
            
            set; }
        public int PayeeT
        {
            get;

            set;

        }
        public int DateL { get; set; }
        public int DateT { get; set; }
        public int AmtInWord1L { get; set; }
        public int AmtINWord1T { get; set; }
        public int AmtINWord2L { get; set; }
        public int AmtInWord2T { get; set; }
        public int AmountL { get; set; }
        public int AmountT { get; set; }
        public string PhotoPath { get; set; }
        public string cmbFieldName { get; set; }

        public string LeyOutName { get; set; }
        public string BackImage { get; set; }
        public string CheckLeyoutID { get; set; }
        public Boolean Flag { get; set; }
        public byte[] Image { get; set; }
        public int txtHieght { get; set; }
        public int txtWidth { get; set; }
    }
}