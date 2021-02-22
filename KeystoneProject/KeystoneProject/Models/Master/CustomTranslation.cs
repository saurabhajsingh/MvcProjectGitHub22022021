using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Master;


namespace KeystoneProject.Models.Master
{
    public class CustomTranslation
    {
        public int LocationID { get; set; }
        public int HospitalID { get; set; }
        public int TextID { get; set; }
        public string Text { get; set; }
        public string English { get; set; }
        public string Hindi { get; set; }
        public string Marathi { get; set; }
        public string French { get; set; }
        public string Gujarati { get; set; }
        public string Bengali { get; set;}
        public string Korean { get; set; }
        public string Tamil { get; set; }
        public string Urdu { get; set; }
        public string Mode { get; set; } 
        public string Telugu { get; set; }
        public string Kannada { get; set; }
        public string Malayalam { get; set; }
        public string Arabic { get; set; }
        public string Odia { get; set; }

    }
}