using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Financial
{
    public class ChequePrint
    {

        public int ChequePrintID
        {
            get;
            set;
        }
        public int VoucherEntryID
        {
            get;
            set;
        }
        public string BankName
        {
            get;
            set;
        }
        public string BankID
        {
            get;
            set;
        }
        public string ChequeDate
        {
            get;
            set;
        }

        public string PayeeNameID
        {
            get;
            set;
        }
        public string PayeeName
        {
            get;
            set;
        }
        public string ChequeBookID
        {
            get;
            set;
        }
        public string BookName
        {
            get;
            set;
        }
        public string ID
        {
            get;
            set;
        }
        public string ChequeNo
        {
            get;
            set;
        }
        public string ChequeAmount
        {
            get;
            set;
        }
        public string TDS
        {
            get;
            set;
        }
        public string TDSAmt
        {
            get;
            set;
        }
        public string Narretion
        {
            get;
            set;
        }
    }
}