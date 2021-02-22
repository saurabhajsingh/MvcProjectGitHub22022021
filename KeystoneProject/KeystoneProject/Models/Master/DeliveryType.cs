using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Master
{
    public class DeliveryType
    {
        public int LocationID { get; set; }
        public int HospitalID { get; set; }

        public int DeliveryTypeID { get; set; }


        public string DeliveryType1 { get; set; }

        public DataSet StoreAllDeliveryType { get; set; }
        public string Mode { get; set; } 
    }
}