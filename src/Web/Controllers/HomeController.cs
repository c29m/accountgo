//-----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="AccountGo">
// Copyright (c) AccountGo. All rights reserved.
// <author>Marvin Perez</author>
// <date>1/11/2015 9:48:38 AM</date>
// </copyright>
//-----------------------------------------------------------------------

using System.Web.Mvc;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly Services.Administration.IAdministrationService _administrationService;
        public HomeController(Services.Administration.IAdministrationService administrationService)
        {
            _administrationService = administrationService;
        }

        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (_administrationService.GetDefaultCompany() == null)
                Data.DbInitializerHelper.Initialize();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult PopupWIndowTest()
        {
            return PartialView("_PopupWindowTest");
        }
    }
}
