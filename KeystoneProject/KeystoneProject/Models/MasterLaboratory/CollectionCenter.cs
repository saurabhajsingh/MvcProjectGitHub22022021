using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace KeystoneProject.Models.MasterLaboratory
{
    public class CollectionCenter
    {
        public int HospitalID
        {
            get;
            set;
        }
        public int LocationID
        {
            get;
            set;
        }

        public string CollectionID { get; set; }
        public string CollectionName { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string AdminInCharge { get; set; }
    }
}