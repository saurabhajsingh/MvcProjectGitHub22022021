using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;

namespace KeystoneProject.Models.Master
{
    public class MedicineType
    {
        public int LocationID { get; set; }
        public int HospitalID { get; set; }

        public int MedicineTypeID { get; set; }

        public string MedicineTypeName { get; set; }

        public DataSet StoreAllMedicineType { get; set; }
        public string Mode { get; set; }
    }
}