using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace KeystoneProject.Models.Keystone
{
    public class Users
    {
        
             public string chbAuthorizationRights { get; set; }
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public int UserDetailsID { get; set; }
        public string HospitalName { get; set; }
        public int UserID { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string LastPassword1 { get; set; }
        public string LastPassword2 { get; set; }
        public string LastPassword3 { get; set; }
        public int EmployeeID { get; set; }
        public string FullName { get; set; }
        public string ReferenceCode { get; set; }
        public string EmailID { get; set; }
        public string MaskColor { get; set; }
        public string RowInternal { get; set; }
        public string IsAccountLocked { get; set; }
        public string ADSIObjectID { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string RoleName2 { get; set; }
        public DataSet StoreAllUsers { get; set; }
        public DataSet StoreAllHospital { get; set; }
        public string Mode { get; set; }

        public string Password2 { get; set; }
        public string RoleID2 { get; set; }
    }
}
