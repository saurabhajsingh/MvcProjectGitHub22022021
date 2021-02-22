using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;


namespace KeystoneProject.Models.Master
{
    public class TestMasterTPAWise 
    {
       
        public int LocationId { get; set; }
        public int HospitalId { get; set; }
        public DataSet ds { get; set; }

        public string OrganisationName { get; set; }
        public string OrganisationID { get; set; }

        public string TestName { get; set; }

       public string TestTPAWiseID { get; set; }

        public string TestID { get; set; }

        public string TPAName { get; set; }

        public string TPAId { get; set; }

       public string GeneralCharges { get; set; }

       public string EmergencyCharges { get; set; }

       public string RecommendedByDoctor { get; set; }
 
        public DataSet dsgrid { get; set; }

        public WardDetail_class [] ward{ get; set; }

        public string WardName { get; set; }
        public int WardId { get; set; }
      
        

        public int TestDetailsTPAWiseID { get; set; }

     
    }


}
