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
namespace KeystoneProject.Controllers.Master
{
    public class ServicesController : Controller
    {
        //
        // GET: /Services/
        public ActionResult Services()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Services(Services AddServiceMod, ServicesCharges obj1)

        {

            try
            {
                FormCollection formCollection = new FormCollection();

                KeystoneProject.Buisness_Logic.Master.Bl_Services Bl_obj = new Buisness_Logic.Master.Bl_Services();

                if (Request.Form["BillCharges"] == null)
                {
                    AddServiceMod.BillChargesOne = false;
                    AddServiceMod.BillCharges = false;
                }
                else
                {
                    string nm = Request.Form["BillCharges"].ToString();

                    if (nm == "Auto")
                    {
                        AddServiceMod.BillCharges = true;

                    }
                
                    if (nm == "OneTime")
                    {
                        AddServiceMod.BillChargesOne = true;

                    }

                }
                if (Request.Form["CheckedOutTime"] == "on")
                {
                    AddServiceMod.CheckedOutTime = "true";
                }
                else
                {
                    AddServiceMod.CheckedOutTime = "false";
                }

                if (Request.Form["CheckedOneDay"] == "on")
                {
                    AddServiceMod.CheckedOneDay = "true";
                }
                else
                {
                    AddServiceMod.CheckedOneDay = "false";
                }
                AddServiceMod.Mode = "Add";

                // if (Bl_obj.CheckServices(AddServiceMod.ServiceID, AddServiceMod.ServiceName, AddServiceMod.ServiceGroupID, AddServiceMod.UnitID, AddServiceMod.ServiceType))
                // {

                //if (Bl_obj.Save(AddServiceMod, obj1))
                //{
                //    // ViewBag.Message = "Services Added Successfully";
                //    ModelState.Clear();
                //    TempData["Msg"] = "Services Save Successfully";
                //    //     }
                //}

                if (Convert.ToString(AddServiceMod.ServiceID) == "")
                {
                    AddServiceMod.ServiceID = 0;
                }

                var id = AddServiceMod.ServiceID;

                if (Bl_obj.CheckServices(AddServiceMod.ServiceID,AddServiceMod.ServiceGroupID,AddServiceMod.ServiceName))
                {
                    if (Bl_obj.Save(AddServiceMod, obj1))
                    {

                        if (id > 0)
                        {

                            TempData["Msg"] = "Services Updated Successfully";
                            return RedirectToAction("Services", "Services");

                        }

                        else
                        {

                            TempData["Msg"] = "Services Saved Successfully";
                            return RedirectToAction("Services", "Services");

                        }
                        //if (id == 0)
                        //{
                        //    TempData["Msg"] = "Services Save Successfully";
                        //    return RedirectToAction("Services", "Services");
                        //}
                        //else
                        //{
                        //    TempData["Msg"] = "Services Updated Successfully";
                        //    return RedirectToAction("Services", "Services");
                        //}

                    }
                }
               else
                {
                    TempData["Msg"] = " Services Already Exist's";
                    return RedirectToAction("Services", "Services");
                }

                AddServiceMod.dsServicesCharges = Bl_obj.AllWardCharges();
                // return View(AddServiceMod);
                return RedirectToAction("Services", "Services");
            }
            catch (Exception ex)
            {
                TempData["Msg"] = ex.Message;
                return RedirectToAction("Services", "Services");

            }
        }
        public JsonResult EditServices(int ServiceID)
        {
            //Business_Logic.Master.HospitalMaster.BL_Services objDBService = new Business_Logic.Master.HospitalMaster.BL_Services(); //calling class DBdata
            KeystoneProject.Buisness_Logic.Master.Bl_Services objDBService = new Buisness_Logic.Master.Bl_Services();
            Services ServiceModl = new Services();
            List<Services> SearchList = new List<Services>();
            DataSet ds = objDBService.SelectAllServicesbyID(ServiceID);
            DataSet ds1 = objDBService.SelectServiceChargesbyID(ServiceID);
            ServiceModl.dsServicesCharges = new DataSet();
            ServiceModl.dsServicesTPAWise = new DataSet();
            ServiceModl.dsWardDetailsTPAWise = new DataSet();
            ServiceModl.ServiceGroupID = ds.Tables[0].Rows[0]["ServiceGroupID"].ToString();
            ServiceModl.ServiceID = Convert.ToInt32(ds.Tables[0].Rows[0]["ServiceID"].ToString());
            ServiceModl.dsServicesCharges = objDBService.SelectServiceChargesbyID(ServiceID);
            DataSet dss = objDBService.GetServiceGroupID("%", ServiceModl.ServiceGroupID);
            ServiceModl.UnitID = ds.Tables[0].Rows[0]["UnitID"].ToString();
            DataSet dsUnit = objDBService.GetUnit_id(ServiceModl.UnitID);

            if (ds.Tables[0].Rows[0]["BillCharges"].ToString() == "")
            {
                ds.Tables[0].Rows[0]["BillCharges"] = false;
            }
            if (ds.Tables[0].Rows[0]["BillAutoOne"].ToString() == "")
            {
                ds.Tables[0].Rows[0]["BillAutoOne"] = false;
            }
            if (ds.Tables[0].Rows[0]["CheckedOneDay"].ToString() == "")
            {
                ds.Tables[0].Rows[0]["CheckedOneDay"] = false;
            }
            if (ds.Tables[0].Rows[0]["CheckedOutTime"].ToString() == "")
            {
                ds.Tables[0].Rows[0]["CheckedOutTime"] = false;
            }
            if (ds.Tables[0].Rows[0]["CheckOutTime"].ToString() == "" || ds.Tables[0].Rows[0]["CheckOutTime"].ToString() == null)
            {
                ds.Tables[0].Rows[0]["CheckOutTime"] = "";
            }
            else
            {
                ds.Tables[0].Rows[0]["CheckOutTime"] = Convert.ToDateTime(ds.Tables[0].Rows[0]["CheckOutTime"]).ToString("hh:mm:ss");

            }

           

            SearchList.Add(new Services
            {
                CheckOutTime = ds.Tables[0].Rows[0]["CheckOutTime"].ToString(),
                CheckOneDayTime = ds.Tables[0].Rows[0]["CheckOneDayTime"].ToString(),
                ServiceName = ds.Tables[0].Rows[0]["ServiceName"].ToString(),
                ServiceGroupID = ds.Tables[0].Rows[0]["ServiceGroupID"].ToString(),
                UnitID = ds.Tables[0].Rows[0]["UnitID"].ToString(),
                GeneralCharges = Convert.ToDecimal(ds.Tables[0].Rows[0]["GeneralCharges"].ToString()),
                EmergencyCharges = Convert.ToDecimal(ds.Tables[0].Rows[0]["EmergencyCharges"].ToString()),
                ServiceType = ds.Tables[0].Rows[0]["ServiceType"].ToString(),
                RecommendedByDoctor = ds.Tables[0].Rows[0]["RecommendedByDoctor"].ToString(),
                HospitalHSNCode = ds.Tables[0].Rows[0]["HospitalHSNCode"].ToString(),
                BillCharges = Convert.ToBoolean(ds.Tables[0].Rows[0]["BillCharges"]),
                TPAHSNCode = ds.Tables[0].Rows[0]["TPAHSNCode"].ToString(),
                HSNCode = ds.Tables[0].Rows[0]["HSNCode"].ToString(),
                HideInBilling = ds.Tables[0].Rows[0]["HideInBilling"].ToString(),
                ServiceGroupName = ds.Tables[0].Rows[0]["ServiceGroupName"].ToString(),
                UnitName = ds.Tables[0].Rows[0]["UnitName"].ToString(),
                BillChargesOne = Convert.ToBoolean(ds.Tables[0].Rows[0]["BillAutoOne"]),
                CheckedOneDay = ds.Tables[0].Rows[0]["CheckedOneDay"].ToString(),
                CheckedOutTime = ds.Tables[0].Rows[0]["CheckedOutTime"].ToString(),
            });

            foreach (DataRow dr in ds1.Tables[0].Rows)
            {

                SearchList.Add(new Services
                {
                    WardID = Convert.ToInt32(dr["WardID"].ToString()),
                    WordName = dr["WardName"].ToString(),
                    GeneralCharges = Convert.ToDecimal(dr["GeneralCharges"]),
                    EmergencyCharges = Convert.ToDecimal(dr["EmergencyCharges"]),
                });
            }



            return new JsonResult { Data = SearchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetAllWard()
        {
            KeystoneProject.Buisness_Logic.Master.Bl_Services Bl_obj = new Buisness_Logic.Master.Bl_Services();
            Services AddServiceMod = new Services();

            AddServiceMod.dsServicesCharges = Bl_obj.GetAllWardName();


            List<Services> search = new List<Services>();
            foreach (DataRow dr in AddServiceMod.dsServicesCharges.Tables[0].Rows)
            {
                dr["GeneralCharges"] = 0;
                dr["EmergencyCharges"] = 0;
                search.Add(new Services
                {
                    WardID = Convert.ToInt16(dr["WardID"]),
                    WordName = dr["WardName"].ToString(),
                    GeneralCharges = Convert.ToDecimal(dr["GeneralCharges"]),
                    EmergencyCharges = Convert.ToDecimal(dr["EmergencyCharges"]),



                });
            }
            return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult GetUnitID(string prefix)
        {
            KeystoneProject.Buisness_Logic.Master.Bl_Services Bl_obj = new Buisness_Logic.Master.Bl_Services();
            Services AddServiceMod = new Services();
            DataSet ds = Bl_obj.GetUnitID(prefix);
            List<Services> SearchList = new List<Services>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                SearchList.Add(new Services
                {
                    UnitName = dr["UnitName"].ToString(),
                    UnitID = dr["UnitID"].ToString()

                });
            }
            return new JsonResult { Data = SearchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetServiceGrpRecord(string prefix)
        {
            KeystoneProject.Buisness_Logic.Master.Bl_Services Bl_obj = new Buisness_Logic.Master.Bl_Services();
            Services AddServiceMod = new Services();
            DataSet ds = Bl_obj.GetServiceGroupID(prefix, "%");
            List<Services> SearchList = new List<Services>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                SearchList.Add(new Services
                {
                    ServiceGroupName = dr["ServiceGroupName"].ToString(),
                    ServiceGroupID = dr["ServiceGroupID"].ToString()
                });
            }
            return new JsonResult { Data = SearchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult ShowAllServices()
        {
            KeystoneProject.Buisness_Logic.Master.Bl_Services Bl_obj = new Buisness_Logic.Master.Bl_Services();
            Services AddServiceMod = new Services();
            AddServiceMod.StoreAllServices = Bl_obj.SelectAllData();

            List<Services> SearchList = new List<Services>();
            foreach (DataRow dr in AddServiceMod.StoreAllServices.Tables[0].Rows)
            {

                SearchList.Add(new Services
                {

                    ServiceID = Convert.ToInt32(dr["ServiceID"].ToString()),
                    ServiceGroupName = dr["ServiceGroupName"].ToString(),
                    ServiceName = dr["ServiceName"].ToString(),
                    GeneralCharges = Convert.ToDecimal(dr["General Charges"]),
                    EmergencyCharges = Convert.ToDecimal(dr["Emergency Charges"]),
                    ServiceType = dr["ServiceType"].ToString(),
                });

            }



            return new JsonResult { Data = SearchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        #region Delete Services
        [HttpPost]
        public JsonResult DeleteServices(int ServiceID, string ServiceType)
        {

            string _Del = null;
            try
            {
                KeystoneProject.Buisness_Logic.Master.Bl_Services objdb = new Buisness_Logic.Master.Bl_Services();
                Services objSG = new Models.Master.Services();

                int DependaincyName = objdb.DeleteServices(Convert.ToInt32(ServiceID), ServiceType);

                if (DependaincyName == 1)
                {
                    _Del = "Services Deleted Successfully";
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
            //KeystoneProject.Buisness_Logic.Master.Bl_Services Bl_obj = new Buisness_Logic.Master.Bl_Services();
            //Services AddServiceMod = new Services();

            //string _Del = null;
            //try
            //{
            //    string Dependency = Bl_obj.DeleteServices(Convert.ToInt32(ServiceID), ServiceType);
            //    if (Dependency == "Delete")
            //    {
            //        _Del = "Services Deleted Successfully";
            //    }
            //    else
            //    {
            //        _Del = "You Delete First " + Dependency;
            //    }
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
            //return new JsonResult { Data = _Del, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        #endregion

        public HttpPostedFileWrapper ExcelFile { get; set; }

        public ActionResult DownLoadFile()
        {
            try
            {
                #region Exal
                byte[] bytes;
                string fileName = "", contentType = "", PaperPath = "";
                DataSet dswordchargeinrow = new DataSet();
                KeystoneProject.Buisness_Logic.Master.Bl_Services Bl_obj = new Buisness_Logic.Master.Bl_Services();
                Services AddServiceMod = new Services();
                AddServiceMod.StoreAllServices = Bl_obj.GetAllServicesExcelData();
                AddServiceMod.dsServicesCharges = Bl_obj.GetAllWardName();

                Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
                Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
                Excel.Range chartRange;
                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets["Sheet1"];
                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.ActiveSheet;

                app.get_Range("A2", "S2").Locked = true;

                string WardNAme;
                //worksheet.Name = "Exported from Keystone";
                int count = 0;
                for (int i = 1; i < AddServiceMod.StoreAllServices.Tables[0].Columns.Count + 1; i++)
                {
                    worksheet.Cells[1, i] = AddServiceMod.StoreAllServices.Tables[0].Columns[i - 1].ColumnName;

                    count = i;
                }
                int m = 0;
                for (int word = 0; word < AddServiceMod.dsServicesCharges.Tables[0].Rows.Count; word++)
                {
                    worksheet.Cells[1, count + word] = AddServiceMod.dsServicesCharges.Tables[0].Rows[m]["WardName"];
                    // worksheet.get_Range(worksheet.Cells[1, word], worksheet.Cells[4, 3]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Orange);
                    WardNAme = AddServiceMod.dsServicesCharges.Tables[0].Rows[m]["WardName"].ToString() + ",";
                    m++;
                }

                for (int i = 0; i < AddServiceMod.StoreAllServices.Tables[0].Rows.Count; i++)
                {
                    for (int j = 0; j < AddServiceMod.StoreAllServices.Tables[0].Columns.Count; j++)
                    {
                        // if (j < 2)
                        //  {
                        worksheet.Cells[i + 2, j + 1] = AddServiceMod.StoreAllServices.Tables[0].Rows[i][j].ToString();
                        // }

                        if (AddServiceMod.StoreAllServices.Tables[0].Rows[i]["ServiceID"].ToString() != "")
                        {

                            dswordchargeinrow = Bl_obj.SelectServiceChargesbyID(Convert.ToInt16(AddServiceMod.StoreAllServices.Tables[0].Rows[i]["ServiceID"]));

                            if (dswordchargeinrow.Tables[0].Rows.Count > 0)
                            {

                                foreach (DataRow drcharge in AddServiceMod.dsServicesCharges.Tables[0].Rows)
                                {
                                    int charge = 0;
                                    foreach (DataRow dr in dswordchargeinrow.Tables[0].Rows)
                                    {
                                        string WardName = drcharge["WardName"].ToString();
                                        string WardName1 = dr["WardName"].ToString();
                                        if (drcharge["WardName"].ToString() == dr["WardName"].ToString())
                                        {
                                            worksheet.Cells[i + 2, charge + 7] = dr["GeneralCharges"];

                                        }
                                        charge++;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
                Random obj = new Random();
                int a = obj.Next(1, 300);
                string DateCode = "Service_Excel_Data" + Bl_obj.ReffrenceExcel();
                fileName = Server.MapPath("~/") + "Service_Excel/" + DateCode;
                
                workbook.SaveAs(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                workbook.Close(true, Type.Missing, Type.Missing);

                string Path = fileName + ".xlsx";
                string filename = System.IO.Path.GetFileName(DateCode + ".xlsx");

                // string fullpath = System.IO.Path.Combine(Path, fileName);
                contentType = MimeMapping.GetMimeMapping(Path);
                File(Path, contentType, filename);
                // File(Path, contentType, filename);
                return new JsonResult { Data = "Done", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                //fileName = "Service_Excel_Data" + Bl_obj.ReffrenceCode()+".xlsx";
            }
            catch (Exception ex)
            {
                throw;
            }

          

            //      return File(FileById.FileContent, "/MRDFiles/pdf", FileById.FileName);

        }


        public JsonResult imageuplod(Doctor model)
        {
            string path = "";
            //PatientMRD_Model Model
            // PatientMRD_Model obj = new PatientMRD_Model();
            var file = model.ImageFile;
            if (file != null)
            {
                KeystoneProject.Buisness_Logic.Master.Bl_Services Bl_obj = new Buisness_Logic.Master.Bl_Services();
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