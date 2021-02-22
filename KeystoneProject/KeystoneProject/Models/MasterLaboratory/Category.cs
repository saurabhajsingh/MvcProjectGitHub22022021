using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace KeystoneProject.Models.MasterLaboratory
{
    public class Category
    {
     
        public int LocationId { get; set; }
        public int HospitalId { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string HSNCode { get; set; }
        public int ParentCategoryID { get; set; }
        public string ParentCategoryName { get; set; }
        public string level { get; set; }
        public string Description { get; set; }
        public DataSet dsCategory { get; set; }
        public object ReferenceCode { get; set; }

       




      
    }
}
