using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace KeystoneProject.Models.Master
{
    public class Allergies
    {

        public int LocationID { get; set; }
        public int HospitalID { get; set; }

        public int AllergiesID { get; set; }


        public string AllergiesName { get; set; }


        public string ReferenceCode { get; set; }

        public string AllergiesDescription { get; set; }

        public DataSet StoreAllAllergies { get; set; }
        public string Mode { get; set; } 
    }
}