using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace KeystoneProject.Models.FinancialAccount
{
    public class VoucherEntry
    {
        public int LocationID { get; set; }
        public int HospitalID { get; set; }
        public string ReferenceCode { get; set; }
        public string VoucharEntryID { get; set; }
        public string AccountsID { get; set; }
        public string VoucharID { get; set; }
        public string VoucharName { get; set; }
        public string RefVoucharNo { get; set; }
        public string CurrentDate { get; set; }
       
        public string VoucharDate { get; set; }
        public string ChequeClearDate { get; set; }
        public string InvNo { get; set; }
        public string Narration { get; set; }
        public DataSet dsAllVoucher { get; set; }
        public string VoucherTypeName { get; set; }
        public string VoucherTypeID { get; set; }
        public string AccountName { get; set; }
        public string DebitMasterAccount { get; set; }
        public string EditMasterAccount { get; set; }
        public string VoucharAccountName { get; set; }
        public string VoucharDrAmount { get; set; }
        public string VoucharCrAmount { get; set; }
        public string AccountsIDTable { get; set; }
        public string VoucharType { get; set; }
        public string DrAmount { get; set; }
        public string CrAmount { get; set; }
        public string ChequeNo { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string VoucharEntryDetailID { get; set; }
        public DataSet dstable { get; set; }
        public string Mode { get; set; }
        public string BankId { get; set; }
        public string BankName { get; set; }
        public DataSet dsbank { get; set; }
        public string ChequeBookID { get; set; }
        public string BookName { get; set; }
        public string PayeeName { get; set; }
        public string ChequeNumber { get;set; }
        public string Id { get; set; }
        public string ChequeNo1 { get; set; }
        public string SchequeNo { get; set; }
    }
}