using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Fanzin.Entities;
using Fanzin.Web.DataContext;
using Fanzin.Web.Models;
using System.IO;

namespace Fanzin.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class CompanyServicesController : Controller
    {
        private CompanyServiceContextDb db = new CompanyServiceContextDb();

        // GET: CompanyServices
        public async Task<ActionResult> Index()
        {
            var companyServices = db.CompanyServices.Include(c => c.ServicePhotoTitle);
            return View(await companyServices.ToListAsync());
        }

        // GET: CompanyServices/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyService companyService = await db.CompanyServices.FindAsync(id);
            if (companyService == null)
            {
                return HttpNotFound();
            }
            return View(companyService);
        }

        // GET: CompanyServices/Create
        public ActionResult Create()
        {
            ViewBag.CompanyServiceId = new SelectList(db.ServicePhotoTitles, "ServicePhotoTitleId", "FileName");
            return View();
        }

        // POST: CompanyServices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CompanyServiceId,Name,BriefDescription,FullDescription")] CompanyService companyService)
        {
            if (ModelState.IsValid)
            {
                db.CompanyServices.Add(companyService);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CompanyServiceId = new SelectList(db.ServicePhotoTitles, "ServicePhotoTitleId", "FileName", companyService.CompanyServiceId);
            return View(companyService);
        }

        // GET: CompanyServices/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyService companyService = await db.CompanyServices.FindAsync(id);
            if (companyService == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyServiceId = new SelectList(db.ServicePhotoTitles, "ServicePhotoTitleId", "FileName", companyService.CompanyServiceId);
            return View(companyService);
        }

        // POST: CompanyServices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CompanyServiceId,Name,BriefDescription,FullDescription")] CompanyService companyService)
        {
            if (ModelState.IsValid)
            {
                db.Entry(companyService).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyServiceId = new SelectList(db.ServicePhotoTitles, "ServicePhotoTitleId", "FileName", companyService.CompanyServiceId);
            return View(companyService);
        }

        // GET: CompanyServices/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyService companyService = await db.CompanyServices.FindAsync(id);
            if (companyService == null)
            {
                return HttpNotFound();
            }
            return View(companyService);
        }

        // POST: CompanyServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CompanyService companyService = await db.CompanyServices.FindAsync(id);

            //delete company service title image file first
            PhotoFileManager manager = new PhotoFileManager(PhotoManagerType.ServiceTitle);
            manager.Delete(companyService.ServicePhotoTitle.FileName,false);

            //delete photo second
            db.ServicePhotoTitles.Remove(companyService.ServicePhotoTitle);

            //delete all the service images files & associated thumbnails
            //delete all the service images
            //delete all thumbnails
            manager = new PhotoFileManager(PhotoManagerType.Service);
            foreach (var item in companyService.CompanyServicePhoto.ToList())
            {
                manager.Delete(item.FileName, true);
                db.CompanyServicePhotos.Remove(item);
            }


            //delete service
            db.CompanyServices.Remove(companyService);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
