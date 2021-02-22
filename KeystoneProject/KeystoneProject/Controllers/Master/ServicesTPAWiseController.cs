using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Hospital;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
//using GemBox.Spreadsheet;
namespace KeystoneProject.Controllers.Master
{
    public class ServicesTPAWiseController : Controller
    {
        ServicesTPAWise dsSer = new ServicesTPAWise();
        //
        // GET: /ServicesTPAWise/

        //DataSet ds = new DataSet();
        public ActionResult ServicesTPAWise()
        {
            Buisness_Logic.Master.BL_ServiceTPAWise objServiceTPAWise = new Buisness_Logic.Master.BL_ServiceTPAWise();
            
            dsSer.ds = objServiceTPAWise.GetWardDetailsPivot();

            return View(dsSer);
        }
        [HttpPost]
        public ActionResult ServicesTPAWise(ServicesTPAWise location,ArrayWordDetailTpa TpaDetail)
        {
            Buisness_Logic.Master.BL_ServiceTPAWise ServiceEditTPAWise = new Buisness_Logic.Master.BL_ServiceTPAWise();


            string Save = ServiceEditTPAWise.Save(location, TpaDetail);

            if (location.ServicesTPAWiseID > 0)
            {
                if (Save == "Save")
                {

                    TempData["Msg"] = "ServiceTPAWise Updated Successfully";
                    return RedirectToAction("ServicesTPAWise", "ServicesTPAWise");
                }
            }

            else
            { 
                if (Save == "Save")
                {

                    TempData["msg"] = "Privilege Card Saved Successfully";
                    return RedirectToAction("ServicesTPAWise", "ServicesTPAWise");
                }
            }

            //if (Save == "Save") // passing Value to DBClass from model
            //{
            //    TempData["Msg"] = "ServiceTPAWise Save Sussefully";
            //}
           

            return RedirectToAction("ServicesTPAWise", "ServicesTPAWise");
        }
        //public JsonResult DeleteServicesTPAWise(int ServiceID, string ServiceType)
        //{
        //    Buisness_Logic.Master.BL_ServiceTPAWise ServiceEditTPAWise = new Buisness_Logic.Master.BL_ServiceTPAWise();
        //    Services AddServiceMod = new Services();

        //    string _Del = null;
        //    try
        //    {
        //        int Dependency = ServiceEditTPAWise.DeleteServicesTPAWise(Convert.ToInt32(ServiceID), ServiceType);
        //        if (Dependency == 2)
        //        {
        //            _Del = "ServiceTPAWise Deleted Successfully";
        //        }
        //        else
        //        {
        //            _Del = "Error";
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    return new JsonResult { Data = _Del, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}

        public JsonResult DeleteServicesTPAWise(int ServiceID, string ServiceType)
        {
            string _Del = null;
            try
            {
                KeystoneProject.Buisness_Logic.Master.BL_ServiceTPAWise objdb = new Buisness_Logic.Master.BL_ServiceTPAWise();
                ServicesTPAWise objSG = new Models.Master.ServicesTPAWise();

                int DependaincyName = objdb.DeleteServicesTPAWise(Convert.ToInt32(ServiceID), ServiceType);

                if (DependaincyName == 1)
                {
                    _Del = "ServiceTPAWise Deleted Successfully";
                }
                else
                {
                    _Del = "Can not Delete";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new JsonResult { Data = _Del, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult EditServiceTPAWise(int ServicesTPAWiseID)
        {
            Buisness_Logic.Master.BL_ServiceTPAWise ServiceEditTPAWise = new Buisness_Logic.Master.BL_ServiceTPAWise();
            List<ServicesTPAWise> searchList = new List<ServicesTPAWise>();
            DataSet ds = ServiceEditTPAWise.GetServicesTPAWise(ServicesTPAWiseID);
            ServicesTPAWise AddServiceTPAWise = new ServicesTPAWise();
            AddServiceTPAWise.OrganizationName = ds.Tables[0].Rows[0]["OrganizationName"].ToString();
            AddServiceTPAWise.OrganizationID = ds.Tables[0].Rows[0]["OrganizationID"].ToString();

            AddServiceTPAWise.ServiceName = ds.Tables[0].Rows[0]["ServiceName"].ToString();
            AddServiceTPAWise.ServiceID = ds.Tables[0].Rows[0]["ServiceID"].ToString();


            AddServiceTPAWise.UnitName = ds.Tables[0].Rows[0]["UnitName"].ToString();
            AddServiceTPAWise.UnitID = ds.Tables[0].Rows[0]["UnitID"].ToString();



            AddServiceTPAWise.GeneralCharges = Convert.ToDecimal(ds.Tables[0].Rows[0]["GeneralCharges"].ToString());
            AddServiceTPAWise.EmergencyCharges = Convert.ToDecimal(ds.Tables[0].Rows[0]["EmergencyCharges"].ToString());
            AddServiceTPAWise.ServiceType = ds.Tables[0].Rows[0]["ServiceType"].ToString();
            AddServiceTPAWise.RecommendedByDoctor = ds.Tables[0].Rows[0]["RecommendedByDoctor"].ToString();
            AddServiceTPAWise.HideInBilling = ds.Tables[0].Rows[0]["HideInBilling"].ToString();
            AddServiceTPAWise.ServiceID = ds.Tables[0].Rows[0]["ServiceID"].ToString();
            AddServiceTPAWise.UnitID = ds.Tables[0].Rows[0]["UnitID"].ToString();
            AddServiceTPAWise.OrganizationID = ds.Tables[0].Rows[0]["OrganizationID"].ToString();

      //      DataSet ds1 = ServiceEditTPAWise.GetServicesTPAWiseDetail(ServicesTPAWiseID);
            AddServiceTPAWise.ServicesTPAWiseID = ServicesTPAWiseID;
           // AddServiceTPAWise.dsServiceTPAWiseCharges = ServiceEditTPAWise.GetServicesTPAWiseDetail(ServicesTPAWiseID);
            ds = ServiceEditTPAWise.GetServicesTPAWiseDetail(ServicesTPAWiseID);
            searchList.Add(AddServiceTPAWise);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new ServicesTPAWise
                {

                    WardName = dr["WardName"].ToString(),
                    GeneralCharges = Convert.ToDecimal(dr["GeneralCharges"]),
                    EmergencyCharges = Convert.ToDecimal(dr["EmergencyCharges"]),
                    WardID =Convert.ToInt32( dr["WardID"]),
                    ServicesDetailsTPAWiseID = Convert.ToInt32(dr["ServicesDetailsTPAWiseID"]),

                });
            }
            //return View(ServiceTPACharges);
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = 86753090 };

        }


        public JsonResult GetServices(string prefix, string OgrID)
        {
            Buisness_Logic.Master.BL_ServiceTPAWise Bl_obj = new Buisness_Logic.Master.BL_ServiceTPAWise();
            DataSet ds = Bl_obj.GetServices(prefix, OgrID);
            List<ServicesTPAWise> searchList = new List<ServicesTPAWise>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new ServicesTPAWise
                {

                    ServiceID = dr["ServiceID"].ToString(),
                    ServiceName = dr["ServiceName"].ToString()


                });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetOrganisation(string prefix)
        {
            Buisness_Logic.Master.BL_ServiceTPAWise Bl_obj = new Buisness_Logic.Master.BL_ServiceTPAWise();
            DataSet ds = Bl_obj.GetOrganisation(prefix);
            List<ServicesTPAWise> searchList = new List<ServicesTPAWise>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new ServicesTPAWise
                {

                    OrganizationID = dr["OrganizationID"].ToString(),
                    OrganizationName = dr["OrganizationName"].ToString()


                });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult SelectAllServiceTPAWise()
        {
            Buisness_Logic.Master.BL_ServiceTPAWise objServiceTPAWise = new Buisness_Logic.Master.BL_ServiceTPAWise();
            DataSet ds = new DataSet();
          

            ds = objServiceTPAWise.SelectAllServiceTPAWise();
            List<ServicesTPAWise> searchList = new List<ServicesTPAWise>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new ServicesTPAWise
                {
                                           OrganizationName = dr["OrganizationName"].ToString(),
                                             ServiceName = dr["ServiceName"].ToString(),
                                             UnitName = dr["UnitName"].ToString(),
                                             ServiceType = dr["ServiceType"].ToString(),
                                             ServicesTPAWiseID =Convert.ToInt32( dr["ServicesTPAWiseID"]),
                                             ServiceID =dr["ServiceID"].ToString(),
                                             OrganizationID = dr["OrganizationID"].ToString(),

                });
            }

            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = 86753090 };
        }



        public JsonResult GetServicesTPAWiseDetail(int ServicesTPAWiseID)
        {
            Buisness_Logic.Master.BL_ServiceTPAWise objServiceTPAWise = new Buisness_Logic.Master.BL_ServiceTPAWise();
            DataSet ds = new DataSet();


            ds = objServiceTPAWise.GetServicesTPAWiseDetail(ServicesTPAWiseID);
            List<ServicesTPAWise> searchList = new List<ServicesTPAWise>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new ServicesTPAWise
                {

                    WardName = dr["WardName"].ToString(),
                     GeneralCharges = Convert.ToDecimal(dr["GeneralCharges"]),
                     EmergencyCharges = Convert.ToDecimal(dr["EmergencyCharges"]),



                });
            }

            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = 86753090 };
        }
        public JsonResult GetWardDetailsPivot()
        {
            Buisness_Logic.Master.BL_ServiceTPAWise objServiceTPAWise = new Buisness_Logic.Master.BL_ServiceTPAWise();
            dsSer.ds = objServiceTPAWise.GetWardDetailsPivot();
            var modeladd = new List<dynamic>(dsSer.ds.Tables[0].Rows.Count);
            int a = 0;
            foreach (DataRow row in dsSer.ds.Tables[0].Rows)
            {
                var obj = (IDictionary<string, object>)new System.Dynamic.ExpandoObject();

                a = 0;
             
                foreach (DataColumn col in dsSer.ds.Tables[0].Columns)
                {
                    
                  
                    string Rowname = row[col].ToString();
                    //if (a > 3)
                    //{
                    //    if (row[col].ToString() == "")
                    //    {
                    //        row[col] = 0;
                    //    }
                      
                    //    //string aa = Convert.ToDecimal(row["TotalAmount"]).ToString();
                    //    //string aa1 = Convert.ToDecimal(row[col]).ToString();
                    //    //decimal TotalPer = Convert.ToDecimal(row["TotalAmount"]) / 100 * Convert.ToDecimal(row[col]);

                    //    Rowname = "(" + row[col].ToString() + "%)" + " " + "(Rs " + Math.Round(TotalPer, 2).ToString() + ")";
                    //}

                    obj.Add(col.ColumnName, Rowname.ToString());
                    a++;
                }

                modeladd.Add(obj);
            }
            return new JsonResult { Data = modeladd, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        //public FileResult DownLoadFile()
        //{
        //    //SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

        //    var workbook = new ExcelFile();
        //    var worksheet = workbook.Worksheets.Add("DataTable to Sheet");
        //    Services AddServiceMod = new Services();
        //    KeystoneProject.Buisness_Logic.Master.Bl_Services Bl_obj = new Buisness_Logic.Master.Bl_Services();
        //    AddServiceMod.dsServicesCharges = Bl_obj.GetWardDetailsPivot();
        //    // worksheet.AddServiceMo
        //    //string fileName = Server.MapPath("~/") + "Service_Excel/" + Bl_obj.ReffrenceExcel() + ".xlsx";
        //    //ExcelLibrary.DataSetHelper.CreateWorkbook(fileName, AddServiceMod.dsServicesCharges);
        //    worksheet.InsertDataTable(AddServiceMod.dsServicesCharges.Tables[0],
        //    new InsertDataTableOptions()
        //    {
        //        ColumnHeaders = true,
        //        //StartRow = 2
        //    });
        //    string fileName = Server.MapPath("~/") + "Service_Excel/" + Bl_obj.ReffrenceExcel() + ".xlsx";
        //    //  workbook.Save("DataTable to Sheet.xlsx");
        //    workbook.Save(fileName);
        //   // workbook.Close(true, Type.Missing, Type.Missing);

        //    return File("","","");
        //}
        //public FileResult DownLoadFile()
        //{
        //    #region Exal
        //    byte[] bytes;
        //    string fileName = "", contentType = "", PaperPath = "";
        //    DataSet dswordchargeinrow = new DataSet();
        //    Buisness_Logic.Master.BL_ServiceTPAWise objServiceTPAWise = new Buisness_Logic.Master.BL_ServiceTPAWise();
        //    Services AddServiceMod = new Services();
        //    KeystoneProject.Buisness_Logic.Master.Bl_Services Bl_obj = new Buisness_Logic.Master.Bl_Services();
        //    // AddServiceMod.StoreAllServices = objServiceTPAWise.SelectAllServiceTPAWise();
        //    AddServiceMod.dsServicesCharges = Bl_obj.GetWardDetailsPivot();

        //    Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
        //    Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
        //    Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
        //    Excel.Range chartRange;
        //    worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets["Sheet1"];
        //    worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.ActiveSheet;

        //    app.get_Range("A2", "S2").Locked = true;

        //    string WardNAme;
        //    //worksheet.Name = "Exported from Keystone";
        //    int count = 0;
        //    for (int i = 1; i < AddServiceMod.dsServicesCharges.Tables[0].Columns.Count + 1; i++)
        //    {
        //        string column_name = AddServiceMod.dsServicesCharges.Tables[0].Columns[i - 1].ColumnName;
        //        worksheet.Cells[1, i] = AddServiceMod.dsServicesCharges.Tables[0].Columns[i - 1].ColumnName;

        //        count = i;

        //        int m = 0;

        //    }
        //    for (int i = 0; i < AddServiceMod.dsServicesCharges.Tables[0].Rows.Count; i++)
        //    {
        //        for (int j = 0; j < AddServiceMod.dsServicesCharges.Tables[0].Columns.Count; j++)
        //        {
        //            // if (j < 2)
        //            //  {
        //            string name = AddServiceMod.dsServicesCharges.Tables[0].Rows[i][j].ToString();
        //            worksheet.Cells[i + 2, j + 1] = AddServiceMod.dsServicesCharges.Tables[0].Rows[i][j].ToString();
        //        }
        //    }

        //    //        if (AddServiceMod.StoreAllServices.Tables[0].Rows[i]["ServiceID"].ToString() != "")
        //    //        {

        //    //            dswordchargeinrow = objServiceTPAWise.GetServicesTPAWiseDetail(Convert.ToInt32( AddServiceMod.StoreAllServices.Tables[0].Rows[i]["ServiceID"].ToString()));

        //    //            if (dswordchargeinrow.Tables[0].Rows.Count > 0)
        //    //            {

        //    //                foreach (DataRow drcharge in AddServiceMod.dsServicesCharges.Tables[0].Rows)
        //    //                {
        //    //                    int charge = 0;
        //    //                    foreach (DataRow dr in dswordchargeinrow.Tables[0].Rows)
        //    //                    {
        //    //                        string WardName = drcharge["WardName"].ToString();
        //    //                        string WardName1 = dr["WardName"].ToString();
        //    //                        if (drcharge["WardName"].ToString() == dr["WardName"].ToString())
        //    //                        {
        //    //                            worksheet.Cells[i + 2, charge + 7] = dr["GeneralCharges"];

        //    //                        }
        //    //                        charge++;
        //    //                    }
        //    //                }
        //    //            }
        //    //        }
        //    //    }
        //    //}
        //    #endregion
        //    Random obj = new Random();
        //    int a = obj.Next(1, 300);
        //    string DateCode = "ServiceTPA_Excel_Data" + Bl_obj.ReffrenceExcel();
        //    fileName = Server.MapPath("~/") + "Service_Excel/" + DateCode;

        //    workbook.SaveAs(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        //    workbook.Close(true, Type.Missing, Type.Missing);

        //    string Path = fileName + ".xlsx";
        //    string filename = System.IO.Path.GetFileName(DateCode + ".xlsx");

        //    // string fullpath = System.IO.Path.Combine(Path, fileName);
        //    contentType = MimeMapping.GetMimeMapping(Path);
        //    //fileName = "Service_Excel_Data" + Bl_obj.ReffrenceCode()+".xlsx";


        //    return File(Path, contentType, filename);

        //    //      return File(FileById.FileContent, "/MRDFiles/pdf", FileById.FileName);

        //}

           // public JsonResult 
        public JsonResult imageuplod(Doctor model)
        {
            string path = "";
            //PatientMRD_Model Model
            // PatientMRD_Model obj = new PatientMRD_Model();
            var file = model.ImageFile;
            if (file != null)
            {
                KeystoneProject.Buisness_Logic.Master.BL_ServiceTPAWise Bl_obj = new Buisness_Logic.Master.BL_ServiceTPAWise();
                //var path = "";
                var filename = Path.GetFileName(file.FileName);
                var extention = Path.GetExtension(file.FileName);
                var filenamethoutextenction = Path.GetFileNameWithoutExtension(file.FileName);
                file.SaveAs(Server.MapPath("~/MRDFiles/" + file.FileName));
                //  Session["Paper"] = "/MRDFiles/" + file.FileName;
                path = Server.MapPath("~/") + "MRDFiles/" + filename;
                // path = Server.MapPath("~/") + "Service_Excel/" + file.FileName;
                DataSet ds = new DataSet();
                //  file.SaveAs(path);
                ds = Bl_obj.btnImport_Click(path);

                //
                // Session["Paper"] = path;
                // ViewData["ImagePath"] = path;
            }
            return new JsonResult { Data = path, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
	}
}