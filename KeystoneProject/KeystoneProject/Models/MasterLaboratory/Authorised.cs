using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.MasterLaboratory
{
    public class Authorised
    {
        public int HospitalId { get; set; }
        public int LocationId { get; set; }
        public string UserId { get; set; }
        public HttpPostedFileWrapper ImageFile { get; set; }
        public string AuthoriserID { get; set; }
        public string AuthoriserName { get; set; }
        public string MobileNo { get; set; }
        public string Remark { get; set; }
        public string Signature { get; set; }
        public string Mode { get; set; }
        public int CreationID { get; set; }
    }
}