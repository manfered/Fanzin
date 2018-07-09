using Fanzin.Web.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using Fanzin.Web.Models;

namespace Fanzin.Web.Controllers
{
    public class HomeController : Controller
    {
        private SliderContextDb dbSlider = new SliderContextDb();
        private CompanyServiceContextDb dbCompany = new CompanyServiceContextDb();
        private AboutUsContextDb dbAbout = new AboutUsContextDb();
        private CompanyMemberContextDb dbMember = new CompanyMemberContextDb();
        public ActionResult Index()
        {

            HomePageModel model = new HomePageModel();
            model.Slides = dbSlider.Slides.Include(s => s.SlidePhoto);
            model.Services = dbCompany.CompanyServices.ToList();
            model.AboutUses = dbAbout.AboutUses.ToList();
            model.ContactUS = new Entities.ContactUs();
            model.Members = dbMember.CompanyMembers.ToList();


            ////var slides = dbSlider.Slides.Include(s => s.SlidePhoto);
            //ViewBag.Slides = dbSlider.Slides.Include(s => s.SlidePhoto);
            //ViewBag.CompanyServices = dbCompany.CompanyServices;
            ////return View(slides.ToList());
            return View(model);
        }
        
    }
}