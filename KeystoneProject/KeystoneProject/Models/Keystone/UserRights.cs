using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
namespace KeystoneProject.Models.Keystone
{
    public class UserRights
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public string HospitalName { get; set; }
        public string LocationName { get; set; }
        public string FullName { get; set; }
        public string EmailID { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        //public int RoleID { get; set; }
        //public int UserID { get; set; }

        public int RoleID { get; set; }
        public int UserID { get; set; }

        public string RoleName { get; set; }
        public string LastPassword1 = "";
        public string LastPassword2 = "";
        public string LastPassword3 = "";
        public int EmployeeID = 0;
        public string IsAccountLocked { get; set; }
        public string ReferenceCode = "1";
        public bool RowInternal = false;
        public int CreationID { get; set; }
        public Nullable<DateTime> CreationDate { get; set; }
        public int LastModificationID { get; set; }
        public Nullable<DateTime> LastModificationDate { get; set; }
        public int RowStatus { get; set; }
        public DataSet GetAllUsers { get; set; }
        public DataSet GetUsersForUser { get; set; }
        public DataSet GetRoles { get; set; }

        public int UserRightsID { get; set; }

        public DateTime EffectiveDateFrom { get; set; }

        public DateTime EffectiveDateTo { get; set; }

        public int ModuleID { get; set; }

        //  public string UserID { get; set; }

        public int FormMode { get; set; }

        public int Rights { get; set; }

        public int IsAuthorised { get; set; }

        public string Mode { get; set; }

        //  public string RowInternal { get; set; },

        public DataSet GetEmptyUserForRoleRight { get; set; }
    }
}
