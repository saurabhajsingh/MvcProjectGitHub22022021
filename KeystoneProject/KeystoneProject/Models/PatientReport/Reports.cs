using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
namespace KeystoneProject.Models.PatientReport
{
    public class Reports 
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        public int HospitalID { get; set; }
        public int LocationID { get; set; }

        public DataSet dsDepartment
        {
            get;
            set;
        }
        public DataSet dscons
        {
            get;
            set;
        }
        public DataSet dsReferd
        {
            get;
            set;
        }
        public DataSet Financiar
        {
            get;
            set;
        }
        public string OPDNo
        {
            get;
            set;
        }
        public string IPDNO
        {
            get;
            set;
        }
        public string RegNo
        {
            get;
            set;
        }
        public string Patient
        {
            get;
            set;
        }
        public string Address
        {
            get;
            set;
        }

        public string DoctorName
        {
            get;
            set;
        }
        public string PatientType
        {
            get;
            set;
        }
        public string PatientName
        {
            get;
            set;
        }
        public string Mobile
        {
            get;
            set;
        }
        public string AddmissionDate
        {
            get;
            set;
        }


        public string DischargeDate
        {
            get;
            set;
        }

        public string ConsultantDoctor
        {
            get;
            set;
        }


        public string RefferdDoctor
        {
            get;
            set;
        }

        public string FinanceYear
        {
            get;
            set;
        }

        public string BillAmt
        {
            get;
            set;
        }
        public string BillAmount
        {
            get;
            set;
        }

        public string Discount
        {
            get;
            set;
        }

        public string PaidAmount
        {
            get;
            set;
        }

        public string BalanceAmount
        {
            get;
            set;
        }

        public string BillAmount1
        {
            get;
            set;
        }

        public string Discount1
        {
            get;
            set;
        }

        public string PaidAmount1
        {
            get;
            set;
        }

        public string BalanceAmount1
        {
            get;
            set;
        }
        public string VoucherID
        {
            get;
            set;
        }
        public string VoucherName
        {
            get;
            set;
        }
        public DataSet ds
        {
            get;
            set;
        }
        public string DrAmount
        {
            get;
            set;
        }
        public string CrAmount
        {
            get;
            set;
        }
        public string DrAmount1
        {
            get;
            set;
        }
        public string CrAmount1
        {
            get;
            set;
        }
        public string Date
        {
            get;
            set;
        }
        public string ChequeNo
        {
            get;
            set;
        }
        public string BankName
        {
            get;
            set;
        }
        public string VouheEntryDetailID
        {
            get;
            set;
        }
        public string AcountName
        {
            get;
            set;
        }
        public string Narration
        {
            get;
            set;
        }

        public string consAmt
        {
            get;
            set;
        }
    }
}
