using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using MinervaSystem.Base;
using MinervaSystem.Base.Models;
using MinervaSystem.Base.Models.ViewModels;
using System.Data.Entity.Core.Objects;

namespace MinervaSystem.Web.Controllers
{
    [Authorize(Roles = "System Administrators, Site Administrators")]
    public class SugerMillController : Controller
    {
        // GET: SugerMill
        public ActionResult Index()
        {
            return View();
        }
        #region ----- Suger Mill Information-----
        public ActionResult ManageSugerMill()
        {
            ViewBag.HeaderContent = new HeaderContentModel("Suger Mill",
                new BreadcrumbModel("Suger Mill", new List<LinkModel>() { new LinkModel("/SugerMill", "Site Actions") }));
            return View();
        }

        public ActionResult ManageMail()
        {
            ViewBag.HeaderContent = new HeaderContentModel("Mail Box",
                new BreadcrumbModel("Mail Box", new List<LinkModel>() { new LinkModel("/ManageMail", "Site Actions") }));
            return View();
        }
        public ActionResult ComposeMail()
        {
            ViewBag.HeaderContent = new HeaderContentModel("Mail Box",
                new BreadcrumbModel("Mail Box", new List<LinkModel>() { new LinkModel("/ManageMail", "Site Actions") }));
            return View();
        }
        public JsonResult GetAllSugerMills()
        {
            var sugerMills = ContextPerRequest.CurrentContext.SugerMill.OrderBy(a => a.Name)
                .Select(a => new
                {
                    a.Id,
                    a.Code,
                    a.Name,
                    a.EstablishDate,
                    a.Photo,
                    a.Email,
                    a.CellPhone,
                    a.Phone,
                    a.Address,
                    a.Division,
                    a.District,
                    a.Thana,
                    a.Village,
                    a.PostalCode,
                    a.Note
                }).ToList();
            return Json(new { aaData = sugerMills }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllSugerMillsforDdl()
        {
            var sugerMills = ContextPerRequest.CurrentContext.SugerMill.OrderBy(a => a.Name)
                .Select(a => new
                {
                    a.Id,
                    a.Name
                }).ToList();
            return Json(new { aaData = sugerMills }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSugerMillDetails(int sugerMillId)
        {
            var sugerMill = ContextPerRequest.CurrentContext.SugerMill.Find(Convert.ToInt64(sugerMillId));
            var Author = ContextPerRequest.GetUserNameById(sugerMill.Author);
            var Editor = ContextPerRequest.GetUserNameById(sugerMill.Editor);
            var json = new
            {
                sugerMill.Id,
                sugerMill.Name,
                sugerMill.EstablishDate,
                sugerMill.Photo,
                sugerMill.Email,
                sugerMill.CellPhone,
                sugerMill.Phone,
                sugerMill.Address,
                sugerMill.Division,
                sugerMill.District,
                sugerMill.Thana,
                sugerMill.Village,
                sugerMill.PostalCode,
                sugerMill.Note,
                Author,
                sugerMill.Modified,
                Editor
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddSugerMill(SugerMill model)
        {
            try
            {
                ContextPerRequest.CurrentContext.SugerMill.Add(model);
                ContextPerRequest.CurrentContext.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict, ex.Message);
            }
        }
        public ActionResult CreateSugerMill()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSugerMill(SugerMillViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SugerMill sugerMill = new SugerMill();
                    sugerMill.Name = model.Name;
                    sugerMill.EstablishDate = model.EstablishDate;
                    sugerMill.Photo = (model.PhotoFile == null) ?
                        model.Photo : new WebImage(model.PhotoFile.InputStream).Resize(125, 125).GetBytes();
                    sugerMill.Email = model.Email;
                    sugerMill.CellPhone = model.CellPhone;
                    sugerMill.Phone = model.Phone;
                    sugerMill.Address = model.Address;
                    sugerMill.Division = model.Division;
                    sugerMill.District = model.District;
                    sugerMill.Thana = model.Thana;
                    sugerMill.Village = model.Village;
                    sugerMill.PostalCode = model.PostalCode;
                    sugerMill.Note = model.Note;
                    ContextPerRequest.CurrentContext.SugerMill.Add(sugerMill);
                    ContextPerRequest.CurrentContext.SaveChanges();
                    return RedirectToAction("ManageSugerMill", "SugerMill");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(model);
        }
        public ActionResult UpdateSugerMill(Int64 id)
        {
            SugerMillViewModel model = new SugerMillViewModel();
            var sugerMill = ContextPerRequest.CurrentContext.SugerMill.Find(id);
            if (sugerMill != null)
            {
                model.Code = sugerMill.Code;
                model.Name = sugerMill.Name;
                model.EstablishDate = sugerMill.EstablishDate;
                model.Photo = sugerMill.Photo;
                model.Email = sugerMill.Email;
                model.CellPhone = sugerMill.CellPhone;
                model.Phone = sugerMill.Phone;
                model.Address = sugerMill.Address;
                model.Division = sugerMill.Division;
                model.District = sugerMill.District;
                model.Thana = sugerMill.Thana;
                model.Village = sugerMill.Village;
                model.PostalCode = sugerMill.PostalCode;
                model.Note = sugerMill.Note;
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSugerMill(SugerMillViewModel model)
        {
            try
            {
                var sugerMill = ContextPerRequest.CurrentContext.SugerMill.Find(model.Id);
                if (string.IsNullOrEmpty(sugerMill.Code)) model.Code = sugerMill.Id.ToString().PadLeft(8, '0');
                sugerMill.Name = model.Name;
                sugerMill.EstablishDate = model.EstablishDate;
                sugerMill.Photo = (model.PhotoFile == null) ?
                        model.Photo : new WebImage(model.PhotoFile.InputStream).Resize(125, 125).GetBytes();
                sugerMill.Email = model.Email;
                sugerMill.CellPhone = model.CellPhone;
                sugerMill.Phone = model.Phone;
                sugerMill.Address = model.Address;
                sugerMill.Division = model.Division;
                sugerMill.District = model.District;
                sugerMill.Thana = model.Thana;
                sugerMill.Village = model.Village;
                sugerMill.PostalCode = model.PostalCode;
                sugerMill.Note = model.Note;
                ContextPerRequest.CurrentContext.SaveChanges();
                return RedirectToAction("ManageSugerMill", "SugerMill");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(model);
        }
        public ActionResult DeleteSugerMill(int sugerMillId)
        {
            try
            {
                var sugerMill = ContextPerRequest.CurrentContext.SugerMill.Find(sugerMillId);
                ContextPerRequest.CurrentContext.SugerMill.Remove(sugerMill);
                ContextPerRequest.CurrentContext.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict, ex.Message);
            }
        }
        #endregion
        #region ----- Farmer Information-----
        public ActionResult ManageFarmers()
        {
            ViewBag.HeaderContent = new HeaderContentModel("Farmer Information",
                new BreadcrumbModel("Farmer Information", new List<LinkModel>() { new LinkModel("/SugerMill", "Site Actions") }));
            return View();
        }
        public JsonResult GetAllFarmers()
        {
            var farmers = ContextPerRequest.CurrentContext.Farmer.OrderBy(a => a.Name)
                .Select(a => new
                {
                    a.Id,
                    a.FarmerIdNo,
                    a.Name,
                    a.BirthDate,
                    a.Photo,
                    a.Email,
                    a.CellPhone,
                    a.Phone,
                    a.Address,
                    a.NationalIdNo,
                    a.Division,
                    a.District,
                    a.Thana,
                    a.Village,
                    a.PostalCode
                }).ToList();
            return Json(new { aaData = farmers }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetAllFarmers(FarmerSearch farmerSearch)
        {
            if (farmerSearch.MemberKey == null && farmerSearch.Name == null && farmerSearch.Address == null && farmerSearch.MobileNumber == null
                && farmerSearch.NationalIdNo == null && farmerSearch.TotalLand == null && farmerSearch.SugerMillId == null)
            {
                return GetAllFarmers();
            }
            else
            {
                var farmers = from s in ContextPerRequest.CurrentContext.Farmer
                              select s;
                if (farmerSearch.Name != null) farmers = farmers.Where(oh => oh.Name.Contains(farmerSearch.Name));
                if (farmerSearch.NationalIdNo != null) farmers = farmers.Where(oh => oh.NationalIdNo.Contains(farmerSearch.NationalIdNo));
                if (farmerSearch.MemberKey != null) farmers = farmers.Where(oh => oh.FarmerIdNo.Contains(farmerSearch.MemberKey));
                if (farmerSearch.Address != null) farmers = farmers.Where(oh => oh.Address.Contains(farmerSearch.Address));
                if (farmerSearch.MobileNumber != null) farmers = farmers.Where(oh => oh.CellPhone.Contains(farmerSearch.MobileNumber));

                var list = farmers.OrderBy(a => a.Name)
                .Select(a => new
                {
                    a.Id,
                    a.FarmerIdNo,
                    a.Name,
                    a.BirthDate,
                    a.Photo,
                    a.Email,
                    a.CellPhone,
                    a.Phone,
                    a.Address,
                    a.NationalIdNo,
                    a.Division,
                    a.District,
                    a.Thana,
                    a.Village,
                    a.PostalCode
                }).ToList();
                return Json(new { aaData = list }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult GetData(string name)
        {
            var farmers = ContextPerRequest.CurrentContext.Farmer.Where(oh => oh.Name.Contains(name))
            .Select(a => new
            {
                a.Id,
                a.Name,
                a.Address
            }).ToList();
            return Json(new { aaData = farmers }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFarmerDetails(int farmerId)
        {
            var farmer = ContextPerRequest.CurrentContext.Farmer.Find(Convert.ToInt64(farmerId));
            var Author = ContextPerRequest.GetUserNameById(farmer.Author);
            var Editor = ContextPerRequest.GetUserNameById(farmer.Editor);
            var json = new
            {
                farmer.Id,
                farmer.Name,
                farmer.BirthDate,
                farmer.Photo,
                farmer.Email,
                farmer.CellPhone,
                farmer.Phone,
                farmer.Address,
                farmer.NationalIdNo,
                farmer.Division,
                farmer.District,
                farmer.Thana,
                farmer.Village,
                farmer.PostalCode,
                Author,
                farmer.Modified,
                Editor
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddFarmer(Farmer model)
        {
            try
            {
                ContextPerRequest.CurrentContext.Farmer.Add(model);
                ContextPerRequest.CurrentContext.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict, ex.Message);
            }
        }
        public ActionResult CreateFarmer()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFarmer(FarmerViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Farmer farmer = new Farmer();
                    farmer.Name = model.Name;
                    farmer.BirthDate = model.BirthDate;
                    farmer.Photo = (model.PhotoFile == null) ?
                        model.Photo : new WebImage(model.PhotoFile.InputStream).Resize(125, 125).GetBytes();
                    farmer.Email = model.Email;
                    farmer.CellPhone = model.CellPhone;
                    farmer.Phone = model.Phone;
                    farmer.Address = model.Address;
                    farmer.NationalIdNo = model.NationalIdNo;
                    farmer.Division = model.Division;
                    farmer.District = model.District;
                    farmer.Thana = model.Thana;
                    farmer.Village = model.Village;
                    farmer.PostalCode = model.PostalCode;
                    farmer.EmergencyContact = model.EmergencyContact;
                    farmer.EmergencyContactPhone = model.EmergencyContactPhone;
                    farmer.Note = model.Note;

                    ContextPerRequest.CurrentContext.Farmer.Add(farmer);
                    ContextPerRequest.CurrentContext.SaveChanges();
                    return RedirectToAction("ManageFarmers", "SugerMill");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(model);
        }
        public ActionResult UpdateFarmer(Int64 id)
        {
            FarmerViewModel model = new FarmerViewModel();
            var farmer = ContextPerRequest.CurrentContext.Farmer.Find(id);
            if (farmer != null)
            {
                model.FarmerIdNo = farmer.FarmerIdNo;
                model.Name = farmer.Name;
                model.BirthDate = farmer.BirthDate;
                model.Photo = farmer.Photo;
                model.Email = farmer.Email;
                model.CellPhone = farmer.CellPhone;
                model.Phone = farmer.Phone;
                model.Address = farmer.Address;
                model.NationalIdNo = farmer.NationalIdNo;
                model.Division = farmer.Division;
                model.District = farmer.District;
                model.Thana = farmer.Thana;
                model.Village = farmer.Village;
                model.PostalCode = farmer.PostalCode;
                model.EmergencyContact = farmer.EmergencyContact;
                model.EmergencyContactPhone = farmer.EmergencyContactPhone;
                model.Note = farmer.Note;
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateFarmer(FarmerViewModel model)
        {
            try
            {
                var farmer = ContextPerRequest.CurrentContext.Farmer.Find(model.Id);
                if (string.IsNullOrEmpty(farmer.FarmerIdNo)) model.FarmerIdNo = farmer.Id.ToString().PadLeft(8, '0');
                farmer.Name = model.Name;
                farmer.BirthDate = model.BirthDate;
                farmer.Photo = (model.PhotoFile == null) ?
                        model.Photo : new WebImage(model.PhotoFile.InputStream).Resize(125, 125).GetBytes();
                farmer.Email = model.Email;
                farmer.CellPhone = model.CellPhone;
                farmer.Phone = model.Phone;
                farmer.Address = model.Address;
                farmer.NationalIdNo = model.NationalIdNo;
                farmer.Division = model.Division;
                farmer.District = model.District;
                farmer.Thana = model.Thana;
                farmer.Village = model.Village;
                farmer.PostalCode = model.PostalCode;
                farmer.EmergencyContact = model.EmergencyContact;
                farmer.EmergencyContactPhone = model.EmergencyContactPhone;
                farmer.Note = model.Note;
                ContextPerRequest.CurrentContext.SaveChanges();
                return RedirectToAction("ManageFarmers", "SugerMill");
                //return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(model);
        }
        public ActionResult DeleteFarmer(int farmerId)
        {
            try
            {
                var farmer = ContextPerRequest.CurrentContext.Farmer.Find(farmerId);
                ContextPerRequest.CurrentContext.Farmer.Remove(farmer);
                ContextPerRequest.CurrentContext.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict, ex.Message);
            }
        }
        #endregion
        #region ----- Supply Information-----
        public ActionResult ManagSupplyInformation()
        {
            ViewBag.HeaderContent = new HeaderContentModel("Supply Information",
                new BreadcrumbModel("Supply Information", new List<LinkModel>() { new LinkModel("/SugerMill", "Site Actions") }));
            return View();
        }
        public PartialViewResult FarmerListPopup()
        {
            return PartialView("FarmerListPopup.cshtml");
        }
        public JsonResult GetAllSupplyInformations()
        {
            var supplyInformations = ContextPerRequest.CurrentContext.SupplyInformation.OrderBy(a => a.Farmer.Name)
                    .Select(a => new
                    {
                        a.Id,
                        a.FarmerId,
                        a.Farmer.Name,
                        SugerMillName = a.SugerMill.Name,
                        CaneVariety = a.CaneVariety.ToString(),
                        PlantRatoon = a.PlantRatoon.ToString(),
                        a.LandArea,
                        a.EstimatedAmount,
                        a.DateofPlanting,
                        a.SupplyDate
                        //Author = ContextPerRequest.GetUserNameById(a.Author),
                        //Editor = ContextPerRequest.GetUserNameById(a.Editor)
                    }).ToList();
            return Json(new { aaData = supplyInformations }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetAllSupplyInformations(SupplyInformationSearch supplyInformationSearch)
        {
            //DateTime dt1 = supplyInformationSearch.DateofPlanting ? supplyInformationSearch.DateofPlanting
            DateTime dateofPlanting = supplyInformationSearch.DateofPlanting != null ? DateTime.ParseExact(supplyInformationSearch.DateofPlanting, "d", null) : new DateTime();
            DateTime supplyDate = supplyInformationSearch.SupplyDate != null ? DateTime.ParseExact(supplyInformationSearch.SupplyDate, "d", null) : new DateTime();
            //DateTime dateofPlanting = supplyInformationSearch.DateofPlanting !=null ? DateTime.ParseExact(supplyInformationSearch.DateofPlanting, "MM/dd/yyyy",
            //                           System.Globalization.CultureInfo.InvariantCulture):new DateTime();
            //DateTime supplyDate = supplyInformationSearch.SupplyDate != null ? DateTime.ParseExact(supplyInformationSearch.SupplyDate, "MM/dd/yyyy",
            //                           System.Globalization.CultureInfo.InvariantCulture) : new DateTime();

            var supplyInformations = from s in ContextPerRequest.CurrentContext.SupplyInformation
                                     select s;
            if (supplyInformationSearch.MemberKey != null) supplyInformations = supplyInformations.Where(oh => oh.Farmer.FarmerIdNo.Contains(supplyInformationSearch.MemberKey));
            if (supplyInformationSearch.Name != null) supplyInformations = supplyInformations.Where(oh => oh.Farmer.Name.Contains(supplyInformationSearch.Name));
            if (supplyInformationSearch.EstimatedAmount != null) supplyInformations = supplyInformations.Where(oh => oh.EstimatedAmount == supplyInformationSearch.EstimatedAmount);
            if (supplyInformationSearch.CaneVariety != null) supplyInformations = supplyInformations.Where(oh => oh.CaneVariety == (CaneVariety)supplyInformationSearch.CaneVariety);
            if (supplyInformationSearch.PlantRatoon != null) supplyInformations = supplyInformations.Where(oh => oh.PlantRatoon == (PlantRatoon)supplyInformationSearch.PlantRatoon);
            if (supplyInformationSearch.SugerMillId != null) supplyInformations = supplyInformations.Where(oh => oh.SugerMillId == supplyInformationSearch.SugerMillId);
            if (supplyInformationSearch.LandArea != null) supplyInformations = supplyInformations.Where(oh => oh.LandArea == supplyInformationSearch.LandArea);
            if (supplyInformationSearch.DateofPlanting != null) supplyInformations = supplyInformations.Where(oh => oh.DateofPlanting == dateofPlanting);
            if (supplyInformationSearch.SupplyDate != null) supplyInformations = supplyInformations.Where(oh => oh.SupplyDate == supplyDate);

            var list = supplyInformations.OrderBy(a => a.Farmer.Name)
            .Select(a => new
            {
                a.Id,
                a.FarmerId,
                a.Farmer.Name,
                SugerMillName = a.SugerMill.Name,
                CaneVariety = a.CaneVariety.ToString(),
                PlantRatoon = a.PlantRatoon.ToString(),
                a.LandArea,
                a.EstimatedAmount,
                a.DateofPlanting,
                a.SupplyDate
            }).ToList();
            return Json(new { aaData = list }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSupplyInformationDetails(int supplyInformationId)
        {
            var supplyInformation = ContextPerRequest.CurrentContext.SupplyInformation.Find(Convert.ToInt64(supplyInformationId));
            var Author = ContextPerRequest.GetUserNameById(supplyInformation.Author);
            var Editor = ContextPerRequest.GetUserNameById(supplyInformation.Editor);
            var json = new
            {
                supplyInformation.Id,
                supplyInformation.FarmerId,
                supplyInformation.Farmer.Name,
                SugerMillName = supplyInformation.SugerMill.Name,
                supplyInformation.CaneVariety,
                supplyInformation.PlantRatoon,
                supplyInformation.LandArea,
                supplyInformation.EstimatedAmount,
                supplyInformation.DateofPlanting,
                supplyInformation.SupplyDate,
                Author,
                Editor
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreateSupplyInformation()
        {
            SupplyInformationViewModel viewModel = new SupplyInformationViewModel();
            viewModel.AllSugerMills = ContextPerRequest.CurrentContext.SugerMill.
                   Select(u => new SelectListItem() { Value = u.Id.ToString(), Text = u.Name });
            return View(viewModel);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreateSupplyInformation(SupplyInformationViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SupplyInformation supplyInformation = new SupplyInformation();
                    supplyInformation.FarmerId = model.FarmerId;
                    supplyInformation.SugerMillId = model.SugerMillId;
                    supplyInformation.CaneVariety = model.CaneVariety;
                    supplyInformation.PlantRatoon = model.PlantRatoon;
                    supplyInformation.LandArea = model.LandArea;
                    supplyInformation.EstimatedAmount = model.EstimatedAmount;
                    supplyInformation.DateofPlanting = model.DateofPlanting;
                    supplyInformation.SupplyDate = model.SupplyDate;
                    ContextPerRequest.CurrentContext.SupplyInformation.Add(supplyInformation);
                    ContextPerRequest.CurrentContext.SaveChanges();
                    return RedirectToAction("ManagSupplyInformation", "SugerMill");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(model);
        }
        public ActionResult UpdateSupplyInformation(Int64 id)
        {
            SupplyInformationViewModel model = new SupplyInformationViewModel();
            var supplyInformation = ContextPerRequest.CurrentContext.SupplyInformation.Find(id);
            if (supplyInformation != null)
            {
                model.FarmerId = supplyInformation.FarmerId;
                model.FarmerKey = supplyInformation.Farmer.FarmerIdNo;
                model.SugerMillId = supplyInformation.SugerMillId;
                model.AllSugerMills = ContextPerRequest.CurrentContext.SugerMill.
                    Select(u => new SelectListItem() { Value = u.Id.ToString(), Text = u.Name });
                model.FarmerName = supplyInformation.Farmer.Name;
                model.CaneVariety = supplyInformation.CaneVariety;
                model.PlantRatoon = supplyInformation.PlantRatoon;
                model.LandArea = supplyInformation.LandArea;
                model.EstimatedAmount = supplyInformation.EstimatedAmount;
                model.DateofPlanting = supplyInformation.DateofPlanting;
                model.SupplyDate = supplyInformation.SupplyDate;
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSupplyInformation(SupplyInformationViewModel model)
        {
            try
            {
                var supplyInformation = ContextPerRequest.CurrentContext.SupplyInformation.Find(model.Id);
                supplyInformation.Farmer.Id = model.FarmerId;
                supplyInformation.SugerMillId = model.SugerMillId;
                supplyInformation.CaneVariety = model.CaneVariety;
                supplyInformation.PlantRatoon = model.PlantRatoon;
                supplyInformation.LandArea = model.LandArea;
                supplyInformation.EstimatedAmount = model.EstimatedAmount;
                supplyInformation.DateofPlanting = model.DateofPlanting;
                supplyInformation.SupplyDate = model.SupplyDate;
                ContextPerRequest.CurrentContext.SaveChanges();
                return RedirectToAction("ManagSupplyInformation", "SugerMill");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(model);
        }
        public ActionResult DeleteSupplyInformation(int supplyInformationId)
        {
            try
            {
                var supplyInformation = ContextPerRequest.CurrentContext.SupplyInformation.Find(supplyInformationId);
                ContextPerRequest.CurrentContext.SupplyInformation.Remove(supplyInformation);
                ContextPerRequest.CurrentContext.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict, ex.Message);
            }
        }
        #endregion


        #region ----- Supply Order/Collection Information-----
        public ActionResult ManageSupplyOrder()
        {
            ViewBag.HeaderContent = new HeaderContentModel("Supply Order",
                new BreadcrumbModel("Supply Order", new List<LinkModel>() { new LinkModel("/SugerMill", "Site Actions") }));
            return View();
        }

        public ActionResult ManageCollectionInformation()
        {
            ViewBag.HeaderContent = new HeaderContentModel("Collection Information",
                new BreadcrumbModel("Collection Information", new List<LinkModel>() { new LinkModel("/SugerMill", "Site Actions") }));
            return View();
        }
        //public PartialViewResult FarmerListPopup()
        //{
        //    return PartialView("FarmerListPopup.cshtml");
        //}

        public JsonResult GetAllSupplyOrders()
        {
            DateTime? dateofPlanting = System.DateTime.Now;
            var supplyInformations = ContextPerRequest.CurrentContext.SupplyInformation
                                    .Where(oh => ((oh.CaneVariety == (CaneVariety)1 && EntityFunctions.DiffMonths(oh.DateofPlanting, dateofPlanting) >= 12
                                                 && EntityFunctions.DiffMonths(oh.DateofPlanting, dateofPlanting) <= 14))
                                                 || (oh.CaneVariety == (CaneVariety)1 && EntityFunctions.DiffMonths(oh.DateofPlanting, dateofPlanting) == 12)).
                                                                        OrderBy(a => a.Farmer.Name)
                    .Select(a => new
                    {
                        a.Id,
                        a.FarmerId,
                        a.Farmer.Name,
                        SugerMillId = a.SugerMill.Id,
                        SugerMillName = a.SugerMill.Name,
                        CaneVariety = a.CaneVariety.ToString(),
                        PlantRatoon = a.PlantRatoon.ToString(),
                        a.LandArea,
                        a.EstimatedAmount,
                        a.DateofPlanting,
                        a.SupplyDate
                        //Author = ContextPerRequest.GetUserNameById(a.Author),
                        //Editor = ContextPerRequest.GetUserNameById(a.Editor)
                    }).ToList();
            return Json(new { aaData = supplyInformations }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetAllSupplyOrders(SupplyInformationSearch supplyInformationSearch)
        {
            //DateTime? currentDate = System.DateTime.Now;
            DateTime? dateofPlanting = supplyInformationSearch.DateofPlanting != null ? DateTime.ParseExact(supplyInformationSearch.DateofPlanting, "d", null) : System.DateTime.Now;
            var supplyInformations = from s in ContextPerRequest.CurrentContext.SupplyInformation
                                     select s;
            if (supplyInformationSearch.MemberKey != null) supplyInformations = supplyInformations.Where(oh => oh.Farmer.FarmerIdNo.Contains(supplyInformationSearch.MemberKey));
            if (supplyInformationSearch.Name != null) supplyInformations = supplyInformations.Where(oh => oh.Farmer.Name.Contains(supplyInformationSearch.Name));
            if (supplyInformationSearch.EstimatedAmount != null) supplyInformations = supplyInformations.Where(oh => oh.EstimatedAmount == supplyInformationSearch.EstimatedAmount);
            if (supplyInformationSearch.CaneVariety != null) supplyInformations = supplyInformations.Where(oh => (oh.CaneVariety == (CaneVariety)supplyInformationSearch.CaneVariety));
            if (supplyInformationSearch.PlantRatoon != null) supplyInformations = supplyInformations.Where(oh => oh.PlantRatoon == (PlantRatoon)supplyInformationSearch.PlantRatoon);
            if (supplyInformationSearch.SugerMillId != null) supplyInformations = supplyInformations.Where(oh => oh.SugerMillId == supplyInformationSearch.SugerMillId);
            if (supplyInformationSearch.LandArea != null) supplyInformations = supplyInformations.Where(oh => oh.LandArea == supplyInformationSearch.LandArea);


            supplyInformations = supplyInformations.Where(oh => ((oh.CaneVariety == (CaneVariety)1 && EntityFunctions.DiffMonths(oh.DateofPlanting, dateofPlanting) >= 12
                                             && EntityFunctions.DiffMonths(oh.DateofPlanting, dateofPlanting) <= 14))
                                             || (oh.CaneVariety == (CaneVariety)0 && EntityFunctions.DiffMonths(oh.DateofPlanting, dateofPlanting) >= 12));


            var list = supplyInformations.OrderBy(a => a.Farmer.Name)
            .Select(a => new
            {
                a.Id,
                a.FarmerId,
                a.Farmer.Name,
                MobileNo = a.Farmer.CellPhone,
                SugerMillId = a.SugerMill.Id,
                SugerMillName = a.SugerMill.Name,
                CaneVariety = a.CaneVariety.ToString(),
                PlantRatoon = a.PlantRatoon.ToString(),
                a.LandArea,
                a.EstimatedAmount,
                a.DateofPlanting,
                a.SupplyDate
            }).ToList();
            return Json(new { aaData = list }, JsonRequestBehavior.AllowGet);
        }

   
        public JsonResult GetAllCollectionInformations()
        {
            var supplyInformations = ContextPerRequest.CurrentContext.SupplyOrder.OrderBy(a => a.CollectionDate)
                    .Select(a => new
                    {
                        a.Id,
                        a.SugerMillId,
                        a.SupplyInformationId,
                        a.ZoneId,
                        a.ZoneManagerId,
                        a.Code,
                        MemberKey = a.SupplyInformation.Farmer.FarmerIdNo,
                        SugerMillName = a.SupplyInformation.SugerMill.Name,
                        FarmerName = a.SupplyInformation.Farmer.Name,
                        MobileNo = a.SupplyInformation.Farmer.CellPhone,
                        Address = a.SupplyInformation.Farmer.Address,
                        PlantRatoon = a.SupplyInformation.PlantRatoon.ToString(),
                        LandArea = a.SupplyInformation.LandArea,
                        EstimatedAmount = a.SupplyInformation.EstimatedAmount,
                        AmounttoCollect = a.EstimatedAmount,
                        a.CollectedAmount,
                        a.CollectionDate,
                        a.IsCollected,
                        a.Note
                    }).ToList();
            return Json(new { aaData = supplyInformations }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetAllCollectionInformations(SupplyInformationSearch supplyInformationSearch)
        {
            DateTime collectionDate = new DateTime();

            var supplyOrders = from s in ContextPerRequest.CurrentContext.SupplyOrder
                               select s;

            supplyOrders = supplyOrders.Where(oh => oh.CollectionDate == collectionDate);

            var list = supplyOrders.OrderBy(a => a.CollectionDate)
            .Select(a => new
            {
                a.Id,
                a.SugerMillId,
                a.SupplyInformationId,
                a.ZoneId,
                a.ZoneManagerId,
                a.Code,
                MemberKey = a.SupplyInformation.Farmer.FarmerIdNo,
                SugerMillName = a.SupplyInformation.SugerMill.Name,
                FarmerName = a.SupplyInformation.Farmer.Name,
                MobileNo = a.SupplyInformation.Farmer.CellPhone,
                Address = a.SupplyInformation.Farmer.Address,
                PlantRatoon = a.SupplyInformation.PlantRatoon.ToString(),
                LandArea = a.SupplyInformation.LandArea,
                EstimatedAmount = a.SupplyInformation.EstimatedAmount,
                AmounttoCollect = a.EstimatedAmount,
                a.CollectedAmount,
                a.CollectionDate,
                a.IsCollected,
                a.Note
            }).ToList();
            return Json(new { aaData = list }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSupplyOrderDetails(int supplyOrderId)
        {
            var SupplyOrder = ContextPerRequest.CurrentContext.SupplyOrder.Find(Convert.ToInt64(supplyOrderId));
            var Author = ContextPerRequest.GetUserNameById(SupplyOrder.Author);
            var Editor = ContextPerRequest.GetUserNameById(SupplyOrder.Editor);
            var json = new
            {
                SupplyOrder.Id,
                SupplyOrder.SugerMillId,
                SupplyOrder.SupplyInformationId,
                SupplyOrder.ZoneId,
                SupplyOrder.ZoneManagerId,
                SupplyOrder.Code,
                MemberKey = SupplyOrder.SupplyInformation.Farmer.FarmerIdNo,
                SugerMillName = SupplyOrder.SupplyInformation.SugerMill.Name,
                FarmerName = SupplyOrder.SupplyInformation.Farmer.Name,
                Address = SupplyOrder.SupplyInformation.Farmer.Address,
                SupplyOrder.CollectionDate,
                SupplyOrder.EstimatedAmount,
                SupplyOrder.CollectedAmount,
                SupplyOrder.IsCollected,
                SupplyOrder.Note,
                Author,
                Editor
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreateSupplyOrder(SupplyOrderRequest model)
        {
            try
            {
                DateTime dateofPlanting = DateTime.ParseExact(model.CollectionDate, "dd/MM/yyyy hh:mm tt", null);
                if (ModelState.IsValid)
                {
                    SupplyOrder supplyOrder = new SupplyOrder();
                    supplyOrder.SugerMillId = model.SugerMillId;
                    supplyOrder.SupplyInformationId = model.SupplyInformationId;
                    supplyOrder.CollectionDate = dateofPlanting;
                    supplyOrder.EstimatedAmount = model.EstimatedAmount;
                    supplyOrder.Note = model.Note;
                    ContextPerRequest.CurrentContext.SupplyOrder.Add(supplyOrder);
                    ContextPerRequest.CurrentContext.SaveChanges();

                    SupplyOrderResponseMsg response = new SupplyOrderResponseMsg();
                    response.status = 0;
                    response.url = BasicInformation.SmsUrl;
                    response.from = BasicInformation.From;
                    response.responseMsg = "Hi Sir, Your Collection Date:" + model.CollectionDate + ", Amount to Collect: " + model.EstimatedAmount.ToString();
                    response.mobileNo = BasicInformation.CountryCode + model.MobileNo;
                    response.authorization = BasicInformation.smsAuth;
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                SupplyOrderResponseMsg response = new SupplyOrderResponseMsg();
                response.status = 1;
                response.responseMsg = ex.Message;
                return Json(new { response }, JsonRequestBehavior.AllowGet);
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSupplyOrder(SupplyOrderRequest model)
        {
            try
            {
                DateTime dateofPlanting = DateTime.ParseExact(model.CollectionDate, "dd/MM/yyyy hh:mm tt", null);
                var supplyOrder = ContextPerRequest.CurrentContext.SupplyOrder.Find(model.Id);
                supplyOrder.SugerMillId = model.SugerMillId;
                supplyOrder.SupplyInformationId = model.SupplyInformationId;
                //supplyOrder.ZoneId = model.ZoneId;
                //supplyOrder.ZoneManagerId = model.ZoneManagerId;
                supplyOrder.CollectionDate = dateofPlanting;
                supplyOrder.EstimatedAmount = model.EstimatedAmount;
                supplyOrder.CollectedAmount = model.CollectedAmount;
                supplyOrder.IsCollected = model.IsCollected;
                supplyOrder.Note = model.Note;
                ContextPerRequest.CurrentContext.SaveChanges();
                SupplyOrderResponseMsg response = new SupplyOrderResponseMsg();
                response.status = 0;
                response.url = BasicInformation.SmsUrl;
                response.from = BasicInformation.From;
                response.responseMsg = "Hi Sir, Your Collection Date:" + model.CollectionDate + ", Amount to Collect: " + model.EstimatedAmount.ToString();
                response.mobileNo = BasicInformation.CountryCode + model.MobileNo;
                response.authorization = BasicInformation.smsAuth;

                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                SupplyOrderResponseMsg response = new SupplyOrderResponseMsg();
                response.status = 1;
                response.responseMsg = ex.Message;
                return Json(new { response }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DeleteSupplyOrder(int supplyOrderId)
        {
            try
            {
                var supplyOrder = ContextPerRequest.CurrentContext.SupplyOrder.Find(supplyOrderId);
                ContextPerRequest.CurrentContext.SupplyOrder.Remove(supplyOrder);
                ContextPerRequest.CurrentContext.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict, ex.Message);
            }
        }
        #endregion
    }

}