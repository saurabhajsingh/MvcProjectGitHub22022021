using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
namespace KeystoneProject.Models.Keystone
{
    public class Rights
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>

        public string[] ParentLevel0ID { get; set; }
        public string[] ParentLevel1ID { get; set; }
        public string[] ParentLevel2ID { get; set; }
        public string[] ParentLevel3ID { get; set; }
        public string[] ParentLevel4ID { get; set; }
        public string[] OLDRightID { get; set; }
        public string[] RolesRightID { get; set; }
        public string[] RightName { get; set; }
        public string RoleName { get; set; }
        public string[] beforAfterchk { get; set; }

    }
    public class RolesRights
    {
        
              public string LeafModuleName { get; set; }
        public string SubModuleName { get; set; }
        
        public int RightCode;
        public int ModuleIDchk { get; set; }
        public int ModuleID { get; set; }
        public int Level { get; set; }
        public int ParentModuleID { get; set; }
        public int DisplayOrder { get; set; }
        public string RoleID { get; set; }
        public string RoleRightID { get; set; }
        public string RoleName { get; set; }
        public string ModuleName { get; set; }
        public string Mode { get; set; }
        public DataSet GetRoleAndRights { get; set; }
        public DataSet GetRights { get; set; }
        public DataSet GetRoles { get; set; }
        public DataSet AssignRoleAndRights { get; set; }
        public DataSet GetHospitalLocation { get; set; }
    }
}

