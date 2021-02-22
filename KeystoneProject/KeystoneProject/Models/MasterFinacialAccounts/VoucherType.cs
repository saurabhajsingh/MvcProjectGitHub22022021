using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.MasterFinacialAccounts
{
    public class VoucherType
    {
        public int LocationID { get; set; }
        public int HospitalID { get; set; }
        public string VoucherTypeName { get; set; }
        public string Narration { get; set; }
        public string DebitmasterAcc { get; set; }
        public string EditmasterAcc { get; set; }
        public string CodeBlock { get; set; }
        public string MasterAc { get; set; }
        public string AccountName { get; set; }     
        public int VoucherTypeID { get; set; }
        public DataSet StoreAllVoucherType { get; set; }
        public string MasterAcID { get; set; }

        public object ReferenceCode { get; set; }

    }
}