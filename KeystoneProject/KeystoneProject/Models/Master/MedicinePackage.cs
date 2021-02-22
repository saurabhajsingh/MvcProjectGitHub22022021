using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Master
{
    public class MedicinePackage
    {
        public int LocationID { get; set; }
        public int HospitalID { get; set; }

        public string MedicinesID { get; set; }
        public string Medicines { get; set; }
        public int MedicineLibraryID { get; set; }

        public int PackagesDetailsID { get; set; }

        public string Package { get; set; }

        public int PackagesID { get; set; }
    }
}