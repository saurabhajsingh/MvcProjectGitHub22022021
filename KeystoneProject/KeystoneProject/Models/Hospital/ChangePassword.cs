using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Hospital
{
    public class ChangePassword
    {
       public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public int UserID { get; set; }
        public string username { get; set; }
        public int usernameID { get; set; }
        public string oldpswd { get; set; }
        public string newpswd { get; set; }
        public string CnfrmNewpswd { get; set; }
        public string Saurabh { get; set; }
        public string Singh { get; set; }


      
    }
}