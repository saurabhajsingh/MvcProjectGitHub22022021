using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeystoneProject.Models.Master
{
        public class WardDetail_class
    {
        public int WardId { get; set; }

        public string WardName { get; set; }

        public string GeneralCharges { get; set; }
        public string EmergencyCharges { get; set; }

        public int TestDetailsTPAWiseID { get; set; }
    }
}
