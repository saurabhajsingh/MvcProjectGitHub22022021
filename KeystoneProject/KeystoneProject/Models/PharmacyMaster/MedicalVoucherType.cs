using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.PharmacyMaster
{
    public class MedicalVoucherType
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public int UserID { get; set; }
        public string voucherTypeName { get; set; }
        public int VoucherTypeID { get; set; }
        public string masterAccount { get; set; }
        public int MasterAcID { get; set; }
        public int AccountID { get; set; }
        public string narration { get; set; }
        public string codeblock { get; set; }
        public string debitMasterAccount { get; set; }
        public string editMasterAccount { get; set; }
        public string ReferenceCode { get; set; }
    }
}