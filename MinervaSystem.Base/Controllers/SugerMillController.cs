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
                    a.StateId,
                    StateName = a.State != null ? a.State.Name : null,
                    a.DistrictId,
                    DistrictName = a.District != null ? a.District.Name : null,
                    a.UpazilaId,
                    UpazilaName = a.Upazila != null ? a.Upazila.Name : null,
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
                sugerMill.StateId,
                StateName = sugerMill.State != null ? sugerMill.State.Name : null,
                sugerMill.DistrictId,
                DistrictName = sugerMill.District != null ? sugerMill.District.Name : null,
                sugerMill.UpazilaId,
                UpazilaName = sugerMill.Upazila != null ? sugerMill.Upazila.Name : null,
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
                    sugerMill.StateId = model.StateId;
                    sugerMill.DistrictId = model.DistrictId;
                    sugerMill.UpazilaId = model.UpazilaId;
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
                model.StateId = sugerMill.StateId;
                model.DistrictId = sugerMill.DistrictId;
                model.UpazilaId = sugerMill.UpazilaId;
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
                sugerMill.StateId = model.StateId;
                sugerMill.DistrictId = model.DistrictId;
                sugerMill.UpazilaId = model.UpazilaId;
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
                    Zone = a.ZoneId.ToString(),
                    a.FarmerIdNo,
                    a.Name,
                    a.BirthDate,
                    a.Photo,
                    a.Email,
                    a.CellPhone,
                    a.Phone,
                    a.NationalIdNo,
                    a.StateId,
                    StateName = a.State != null ? a.State.Name : null,
                    a.DistrictId,
                    DistrictName = a.District != null ? a.District.Name : null,
                    a.UpazilaId,
                    UpazilaName = a.Upazila != null ? a.Upazila.Name : null,
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
                if (farmerSearch.MobileNumber != null) farmers = farmers.Where(oh => oh.CellPhone.Contains(farmerSearch.MobileNumber));

                var list = farmers.OrderBy(a => a.Name)
                .Select(a => new
                {
                    a.Id,
                    Zone = a.ZoneId.ToString(),
                    a.FarmerIdNo,
                    a.Name,
                    a.BirthDate,
                    a.Photo,
                    a.Email,
                    a.CellPhone,
                    a.Phone,
                    a.NationalIdNo,
                    a.StateId,
                    StateName = a.State != null ? a.State.Name : null,
                    a.DistrictId,
                    DistrictName = a.District != null ? a.District.Name : null,
                    a.UpazilaId,
                    UpazilaName = a.Upazila != null ? a.Upazila.Name : null,
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
                a.Name
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
                farmer.ZoneId,
                farmer.Name,
                farmer.BirthDate,
                farmer.Photo,
                farmer.Email,
                farmer.CellPhone,
                farmer.Phone,
                farmer.NationalIdNo,
                farmer.StateId,
                StateName =  farmer.State != null ? farmer.State.Name : null,
                farmer.DistrictId,
                DistrictName = farmer.District !=null ? farmer.District.Name : null,
                farmer.UpazilaId,
                UpazilaName =  farmer.Upazila != null ? farmer.Upazila.Name : null,
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
                    farmer.ZoneId = model.ZoneId;
                    farmer.BirthDate = model.BirthDate;
                    farmer.Photo = (model.PhotoFile == null) ?
                        model.Photo : new WebImage(model.PhotoFile.InputStream).Resize(125, 125).GetBytes();
                    farmer.Email = model.Email;
                    farmer.CellPhone = model.CellPhone;
                    farmer.Phone = model.Phone;
                    farmer.NationalIdNo = model.NationalIdNo;
                    farmer.StateId = model.StateId;
                    farmer.DistrictId = model.DistrictId;
                    farmer.UpazilaId = model.UpazilaId;
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
                model.ZoneId = farmer.ZoneId;
                model.Name = farmer.Name;
                model.BirthDate = farmer.BirthDate;
                model.Photo = farmer.Photo;
                model.Email = farmer.Email;
                model.CellPhone = farmer.CellPhone;
                model.Phone = farmer.Phone;
                model.NationalIdNo = farmer.NationalIdNo;
                model.StateId = farmer.StateId;
                model.DistrictId = farmer.DistrictId;
                model.UpazilaId = farmer.UpazilaId;
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
                farmer.ZoneId = model.ZoneId;
                if (string.IsNullOrEmpty(farmer.FarmerIdNo)) model.FarmerIdNo = farmer.Id.ToString().PadLeft(8, '0');
                farmer.Name = model.Name;
                farmer.BirthDate = model.BirthDate;
                farmer.Photo = (model.PhotoFile == null) ?
                        model.Photo : new WebImage(model.PhotoFile.InputStream).Resize(125, 125).GetBytes();
                farmer.Email = model.Email;
                farmer.CellPhone = model.CellPhone;
                farmer.Phone = model.Phone;
                farmer.NationalIdNo = model.NationalIdNo;
                farmer.StateId = model.StateId;
                farmer.DistrictId = model.DistrictId;
                farmer.UpazilaId = model.UpazilaId;
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
                        MobileNo = a.Farmer.CellPhone,
                        MemberKey = a.Farmer.FarmerIdNo,
                        SugerMillId = a.SugerMill.Id,
                        SugerMillName = a.SugerMill.Name,
                        Zone=a.Farmer.ZoneId.ToString(),
                        CaneVariety = a.CaneVariety.ToString(),
                        PlantRatoon = a.PlantRatoon.ToString(),
                        a.LandArea,
                        a.EstimatedAmount,
                        a.DateofPlanting,
                        a.SupplyDate
                    }).ToList();
            return Json(new { aaData = supplyInformations }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetAllSupplyInformations(SupplyInformationSearch supplyInformationSearch)
        {
            DateTime dateofPlanting = supplyInformationSearch.DateofPlanting != null ? DateTime.ParseExact(supplyInformationSearch.DateofPlanting, "d", null) : System.DateTime.Now;


            var supplyInformations = from s in ContextPerRequest.CurrentContext.SupplyInformation
                                     select s;
            if (supplyInformationSearch.SearchType != null && supplyInformationSearch.SearchType == "1")
                supplyInformations = supplyInformations.Where(oh => (oh.PlantRatoon == (PlantRatoon)1 && (EntityFunctions.DiffMonths(oh.DateofPlanting, dateofPlanting) >= 12
                                             && EntityFunctions.DiffMonths(oh.DateofPlanting, dateofPlanting) <= 14))
                                             || (oh.PlantRatoon == (PlantRatoon)0 && (EntityFunctions.DiffMonths(oh.DateofPlanting, dateofPlanting) >= 10 &&
                                             EntityFunctions.DiffMonths(oh.DateofPlanting, dateofPlanting) < 13)));


            //if (supplyInformationSearch.SearchType != null && supplyInformationSearch.SearchType == "1")
            //{
            //    supplyInformations = supplyInformations.Where(oh => (EntityFunctions.DiffMonths(oh.DateofPlanting, dateofPlanting) < 20));
            //    //supplyInformations = supplyInformations.Where(oh => (EntityFunctions.DiffMonths(dateofPlanting, oh.DateofPlanting) < 20));
            //}

            if (supplyInformationSearch.MemberKey != null) supplyInformations = supplyInformations.Where(oh => oh.Farmer.FarmerIdNo.Contains(supplyInformationSearch.MemberKey));
            if (supplyInformationSearch.Name != null) supplyInformations = supplyInformations.Where(oh => oh.Farmer.Name.Contains(supplyInformationSearch.Name));
            if (supplyInformationSearch.EstimatedAmount != null) supplyInformations = supplyInformations.Where(oh => oh.EstimatedAmount == supplyInformationSearch.EstimatedAmount);
            if (supplyInformationSearch.CaneVariety != null) supplyInformations = supplyInformations.Where(oh => oh.CaneVariety == (CaneVariety)supplyInformationSearch.CaneVariety);
            if (supplyInformationSearch.PlantRatoon != null) supplyInformations = supplyInformations.Where(oh => oh.PlantRatoon == (PlantRatoon)supplyInformationSearch.PlantRatoon);
            if (supplyInformationSearch.SugerMillId != null) supplyInformations = supplyInformations.Where(oh => oh.SugerMillId == supplyInformationSearch.SugerMillId);
            if (supplyInformationSearch.LandArea != null) supplyInformations = supplyInformations.Where(oh => oh.LandArea == supplyInformationSearch.LandArea);
            if (supplyInformationSearch.DateofPlanting != null) supplyInformations = supplyInformations.Where(oh => oh.DateofPlanting == dateofPlanting);
            
            var list = supplyInformations.OrderByDescending(a => a.DateofPlanting)
            .Select(a => new
            {
                a.Id,
                a.FarmerId,
                a.Farmer.Name,
                MobileNo = a.Farmer.CellPhone,
                MemberKey = a.Farmer.FarmerIdNo,
                SugerMillId = a.SugerMill.Id,
                SugerMillName = a.SugerMill.Name,
                Zone = a.Farmer.ZoneId.ToString(),
                CaneVariety = a.CaneVariety.ToString(),
                PlantRatoon = a.PlantRatoon.ToString(),
                a.LandArea,
                a.EstimatedAmount,
                a.DateofPlanting,
                a.SupplyDate
            }).ToList();

            SupplyOrderResponseMsg response = new SupplyOrderResponseMsg();
            if (list.Count > 0)
            {
                response.status = 0;
                response.url = BasicInformation.SmsUrl;
                response.from = BasicInformation.From;
                response.countryCode = BasicInformation.CountryCode;
                response.authorization = BasicInformation.smsAuth;
                //response.responseMsg = "Prio " + list[0].Name + ", Apner member id " + list[0].MemberKey + ". Apner chashkrito jomir poriman " + list[0].LandArea + " bigha ebong shomvabbo fosholer poriman " + list[0]. + "  ton. Apner tothho shothik vabe halnagad kora hoese. Akh shongroher poroborti tarikh apnake sms er maddhome janie dea hobe.";
            }
            var result = new { aaData = list, smsData = response };
            return Json(result, JsonRequestBehavior.AllowGet);
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

                    var farmer = ContextPerRequest.CurrentContext.Farmer.Find(model.FarmerId);
                    string responseMsg = "Prio " + farmer.Name + ", Apner member id " + farmer.FarmerIdNo + ". Apner chashkrito jomir poriman 5 bigha ebong shomvabbo fosholer poriman 3 ton. Apner tothho shothik vabe halnagad kora hoese. Akh shongroher poroborti tarikh apnake sms er maddhome janie dea hobe.";


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
        [HttpPost]
        public ActionResult DeleteSupplyInformation(IEnumerable<Int64> supplyIds)
        {
            try
            {
                var supplyInfoList = ContextPerRequest.CurrentContext.SupplyInformation.Where(x => supplyIds.Contains(x.Id)).ToList();
                foreach (var supplyInfo in supplyInfoList)
                {
                    var supplyOrderList = ContextPerRequest.CurrentContext.SupplyOrder.Where(x => x.SupplyInformationId==supplyInfo.Id).Take(1);
                    if (supplyOrderList.Count() == 0)
                    {
                        ContextPerRequest.CurrentContext.SupplyInformation.Remove(supplyInfo);
                        ContextPerRequest.CurrentContext.SaveChanges();
                    }
                }
                SupplyOrderResponseMsg response = new SupplyOrderResponseMsg();
                response.status = 0;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                SupplyOrderResponseMsg response = new SupplyOrderResponseMsg();
                response.status = 1;
                response.responseMsg = ex.Message;
                return Json(response, JsonRequestBehavior.AllowGet);
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
                        MemberKey = a.Farmer.FarmerIdNo,
                        SugerMillId = a.SugerMill.Id,
                        SugerMillName = a.SugerMill.Name,
                        Zone = a.Farmer.ZoneId.ToString(),
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
                MemberKey = a.Farmer.FarmerIdNo,
                SugerMillId = a.SugerMill.Id,
                Zone = a.Farmer.ZoneId.ToString(),
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

        public JsonResult GetTodaysCollectionOrders()
        {
            var supplyInformations = ContextPerRequest.CurrentContext.SupplyOrder.Where(a => (a.CollectionDate.HasValue && EntityFunctions.TruncateTime(a.CollectionDate.Value) == EntityFunctions.TruncateTime(DateTime.Today))).OrderBy(a => a.CollectionDate)
                    .Select(a => new
                    {
                        a.Id,
                        a.SugerMillId,
                        a.SupplyInformationId,
                        a.Code,
                        MemberKey = a.SupplyInformation.Farmer.FarmerIdNo,
                        SugerMillName = a.SupplyInformation.SugerMill.Name,
                        FarmerName = a.SupplyInformation.Farmer.Name,
                        Zone = a.SupplyInformation.Farmer.ZoneId.ToString(),
                        MobileNo = a.SupplyInformation.Farmer.CellPhone,
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
        public JsonResult GetAllCollectionInformations()
        {
            var supplyInformations = ContextPerRequest.CurrentContext.SupplyOrder.OrderBy(a => a.CollectionDate)
                    .Select(a => new
                    {
                        a.Id,
                        a.SugerMillId,
                        a.SupplyInformationId,
                        a.Code,
                        MemberKey = a.SupplyInformation.Farmer.FarmerIdNo,
                        SugerMillName = a.SupplyInformation.SugerMill.Name,
                        FarmerName = a.SupplyInformation.Farmer.Name,
                        Zone = a.SupplyInformation.Farmer.ZoneId.ToString(),
                        MobileNo = a.SupplyInformation.Farmer.CellPhone,
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
        public JsonResult GetAllSupplyOrderbySupplyInfoId(Int64 supplyInfoId)
        {

            var supplyOrders = ContextPerRequest.CurrentContext.SupplyOrder.Where(a => a.SupplyInformationId == supplyInfoId).OrderBy(a => a.CollectionDate)
                    .Select(a => new
                    {
                        a.Id,
                        a.SugerMillId,
                        a.SupplyInformationId,
                        a.Code,
                        MemberKey = a.SupplyInformation.Farmer.FarmerIdNo,
                        SugerMillName = a.SupplyInformation.SugerMill.Name,
                        FarmerName = a.SupplyInformation.Farmer.Name,
                        Zone = a.SupplyInformation.Farmer.ZoneId.ToString(),
                        MobileNo = a.SupplyInformation.Farmer.CellPhone,
                        PlantRatoon = a.SupplyInformation.PlantRatoon.ToString(),
                        LandArea = a.SupplyInformation.LandArea,
                        EstimatedAmount = a.SupplyInformation.EstimatedAmount,
                        AmounttoCollect = a.EstimatedAmount,
                        a.CollectedAmount,
                        a.CollectionDate,
                        a.IsCollected,
                        a.Note
                    }).ToList();
            SupplyOrderResponseMsg response = new SupplyOrderResponseMsg();
            if (supplyOrders.Count > 0)
            {
                response.status = 0;
                response.url = BasicInformation.SmsUrl;
                response.from = BasicInformation.From;
                response.countryCode = BasicInformation.CountryCode;
                response.authorization = BasicInformation.smsAuth;
            }
            var result = new { aaData = supplyOrders, smsData = response };
            return Json(new { aaData = supplyOrders }, JsonRequestBehavior.AllowGet);
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
                a.Code,
                MemberKey = a.SupplyInformation.Farmer.FarmerIdNo,
                SugerMillName = a.SupplyInformation.SugerMill.Name,
                FarmerName = a.SupplyInformation.Farmer.Name,
                Zone = a.SupplyInformation.Farmer.ZoneId.ToString(),
                MobileNo = a.SupplyInformation.Farmer.CellPhone,
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
                SupplyOrder.Code,
                MemberKey = SupplyOrder.SupplyInformation.Farmer.FarmerIdNo,
                SugerMillName = SupplyOrder.SupplyInformation.SugerMill.Name,
                FarmerName = SupplyOrder.SupplyInformation.Farmer.Name,
                Zone = SupplyOrder.SupplyInformation.Farmer.ZoneId.ToString(),
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
        public ActionResult CreateAllSupplyOrder(List<SupplyOrderRequest> listSupplyOrderRequest1)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var model in listSupplyOrderRequest1)
                    {
                        DateTime dateofPlanting = DateTime.ParseExact(model.CollectionDate, "dd/MM/yyyy hh:mm tt", null);
                        SupplyOrder supplyOrder = new SupplyOrder();
                        supplyOrder.SugerMillId = model.SugerMillId;
                        supplyOrder.SupplyInformationId = model.SupplyInformationId;
                        supplyOrder.CollectionDate = dateofPlanting;
                        supplyOrder.EstimatedAmount = model.EstimatedAmount;
                        supplyOrder.Note = model.Note;
                        ContextPerRequest.CurrentContext.SupplyOrder.Add(supplyOrder);
                    }
                    ContextPerRequest.CurrentContext.SaveChanges();
                    SupplyOrderResponseMsg response = new SupplyOrderResponseMsg();
                    response.status = 0;
                    response.url = BasicInformation.SmsUrl;
                    response.from = BasicInformation.From;
                    response.responseMsg = "Prio " + listSupplyOrderRequest1[0].MemberName + ", Apner member id " + listSupplyOrderRequest1[0].MemberKey + ". Apner chashkrito jomir poriman 5 bigha ebong shomvabbo fosholer poriman 3 ton. Apner tothho shothik vabe halnagad kora hoese. Akh shongroher poroborti tarikh apnake sms er maddhome janie dea hobe.";
                    response.mobileNo = BasicInformation.CountryCode + listSupplyOrderRequest1[0].MobileNo;
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

            return View(listSupplyOrderRequest1);
        }
        [HttpPost]
        public ActionResult PassThings(List<SupplyOrderRequest> things)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var model in things)
                    {
                        DateTime dateofPlanting = DateTime.ParseExact(model.CollectionDate, "dd/MM/yyyy hh:mm tt", null);
                        SupplyOrder supplyOrder = new SupplyOrder();
                        supplyOrder.SugerMillId = model.SugerMillId;
                        supplyOrder.SupplyInformationId = model.SupplyInformationId;
                        supplyOrder.CollectionDate = dateofPlanting;
                        supplyOrder.EstimatedAmount = model.EstimatedAmount;
                        supplyOrder.Note = model.Note;
                        ContextPerRequest.CurrentContext.SupplyOrder.Add(supplyOrder);
                    }
                    ContextPerRequest.CurrentContext.SaveChanges();
                    SupplyOrderResponseMsg response = new SupplyOrderResponseMsg();
                    response.status = 0;
                    response.url = BasicInformation.SmsUrl;
                    response.from = BasicInformation.From;
                    response.responseMsg = "Prio " + things[0].MemberName+ ", Apner member id " + things[0].MemberKey + ". Apner chashkrito jomir poriman 5 bigha ebong shomvabbo fosholer poriman 3 ton. Apner tothho shothik vabe halnagad kora hoese. Akh shongroher poroborti tarikh apnake sms er maddhome janie dea hobe.";
                   // response.responseMsg = "Hi Sir, Your Collection Date:" + things[0].CollectionDate + ", Amount to Collect: " + things[0].EstimatedAmount.ToString();
                    response.mobileNo = BasicInformation.CountryCode + things[0].MobileNo;
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

            return View(things);
        }

        [HttpPost]
        public ActionResult UpdateSupplyOrder(SupplyOrderRequest model)
        {
            try
            {
                DateTime dateofPlanting = DateTime.ParseExact(model.CollectionDate, "dd/MM/yyyy hh:mm tt", null);
                var supplyOrder = ContextPerRequest.CurrentContext.SupplyOrder.Find(model.Id);
                supplyOrder.SugerMillId = model.SugerMillId;
                supplyOrder.SupplyInformationId = model.SupplyInformationId;
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
                //response.responseMsg = "Hi Sir, Your Collection Date:" + model.CollectionDate + ", Amount to Collect: " + model.EstimatedAmount.ToString();
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
        [HttpPost]
        public ActionResult DeleteSupplyOrder(IEnumerable<Int64> schedulerIds)
        {
            try
            {
                var supplyOrderList = ContextPerRequest.CurrentContext.SupplyOrder.Where(x => schedulerIds.Contains(x.Id)).ToList();
                foreach (var supplyOrder in supplyOrderList)
                {
                    ContextPerRequest.CurrentContext.SupplyOrder.Remove(supplyOrder);
                    ContextPerRequest.CurrentContext.SaveChanges();
                }
                SupplyOrderResponseMsg response = new SupplyOrderResponseMsg();
                response.status = 0;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                SupplyOrderResponseMsg response = new SupplyOrderResponseMsg();
                response.status = 1;
                response.responseMsg = ex.Message;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region  Country/State/District/Upazilla
        [HttpPost]
        public JsonResult GetAllState(int countryId)
        {
            var stateList = ContextPerRequest.CurrentContext.State.Where(a => a.CountryId == countryId).OrderBy(a => a.Name)
                    .Select(a => new
                    {
                        a.Id,
                        a.Name,
                        a.BnName
                    }).ToList();
            return Json(new { aaData = stateList }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetAllCity(int stateId)
        {
            var districtList = ContextPerRequest.CurrentContext.District.Where(a => a.StateId == stateId).OrderBy(a => a.Name)
                    .Select(a => new
                    {
                        a.Id,
                        a.Name,
                        a.BnName
                    }).ToList();
            return Json(new { aaData = districtList }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetAllUpazilla(int cityId)
        {
            var upazillaList = ContextPerRequest.CurrentContext.Upazila.Where(a => a.DistrictId == cityId).OrderBy(a => a.Name)
                    .Select(a => new
                    {
                        a.Id,
                        a.Name,
                        a.BnName
                    }).ToList();
            return Json(new { aaData = upazillaList }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

}