﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace KeystoneProject.Models.Master
{
    public class Advice
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        public int LocationID { get; set; }
        public int HospitalID { get; set; }

        public int AdviceID { get; set; }


        public string AdviceName { get; set; }

        
        public string ReferenceCode { get; set; }

        public string AdviceDescription { get; set; }

        public DataSet StoreAllAdvice { get; set; }
        public string Mode { get; set; } 
    }
}