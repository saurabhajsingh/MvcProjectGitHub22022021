using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
namespace KeystoneProject.Models.Master
{
    public class DiscountReason
    {
        public int DiscountReasonID { get; set; }

        public string DiscountReasonName { get; set; }

        public DataSet storeAllDiscount { get; set; }

        public string Mode { get; set; }
        public string Message { get; set; }
    }
}