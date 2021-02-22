using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace KeystoneProject.Models.Master
{
    public class Department 
    {
        
        public int LocationID { get; set; }
        public int HospitalID { get; set; }

        public int DepartmentID { get; set; }

        [Required(ErrorMessage = "DepartmentName is Required")]
        public string DepartmentName { get; set; }

        [Required(ErrorMessage = "Referencecode is Required")]
        public string ReferenceCode { get; set; }

        public string Description { get; set; }

        public DataSet StoreAllDepartment { get; set; }
        public string Mode { get; set; }    

    }
}
