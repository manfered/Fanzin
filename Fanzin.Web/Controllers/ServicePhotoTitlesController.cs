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

namespace Fanzin.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class ServicePhotoTitlesController : Controller
    {
        private CompanyServiceContextDb db = new CompanyServiceContextDb();

        // GET: ServicePhotoTitles
        public async Task<ActionResult> Index()
        {
            var servicePhotoTitles = db.ServicePhotoTitles.Include(s => s.CompanyService);
            return View(await servicePhotoTitles.ToListAsync());
        }

        // GET: ServicePhotoTitles/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicePhotoTitle servicePhotoTitle = await db.ServicePhotoTitles.FindAsync(id);
            if (servicePhotoTitle == null)
            {
                return HttpNotFound();
            }
            return View(servicePhotoTitle);
        }

        // GET: ServicePhotoTitles/Create
        public ActionResult Create(int id)
        {
            //ViewBag.ServicePhotoTitleId = new SelectList(db.CompanyServices, "CompanyServiceId", "Name", id);
            ViewBag.ServicePhotoTitleId = id;
            return View();
        }

        // POST: ServicePhotoTitles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ServicePhotoTitleId,FileName,Title")] ServicePhotoTitle servicePhotoTitle)
        {
            if (ModelState.IsValid)
            {
                db.ServicePhotoTitles.Add(servicePhotoTitle);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "CompanyServices");
            }

            ViewBag.ServicePhotoTitleId = new SelectList(db.CompanyServices, "CompanyServiceId", "Name", servicePhotoTitle.ServicePhotoTitleId);
            return View(servicePhotoTitle);
        }

        // GET: ServicePhotoTitles/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicePhotoTitle servicePhotoTitle = await db.ServicePhotoTitles.FindAsync(id);
            if (servicePhotoTitle == null)
            {
                return HttpNotFound();
            }
            ViewBag.ServicePhotoTitleId = new SelectList(db.CompanyServices, "CompanyServiceId", "Name", servicePhotoTitle.ServicePhotoTitleId);
            return View(servicePhotoTitle);
        }

        // POST: ServicePhotoTitles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ServicePhotoTitleId,FileName,Title")] ServicePhotoTitle servicePhotoTitle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(servicePhotoTitle).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index" , "CompanyServices");
            }
            ViewBag.ServicePhotoTitleId = new SelectList(db.CompanyServices, "CompanyServiceId", "Name", servicePhotoTitle.ServicePhotoTitleId);
            return View(servicePhotoTitle);
        }

        // GET: ServicePhotoTitles/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicePhotoTitle servicePhotoTitle = await db.ServicePhotoTitles.FindAsync(id);
            if (servicePhotoTitle == null)
            {
                return HttpNotFound();
            }
            return View(servicePhotoTitle);
        }

        // POST: ServicePhotoTitles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id, string deleteFileName)
        {
            PhotoFileManager manager = new PhotoFileManager(PhotoManagerType.ServiceTitle);
            manager.Delete(deleteFileName, false);

            ServicePhotoTitle servicePhotoTitle = await db.ServicePhotoTitles.FindAsync(id);
            db.ServicePhotoTitles.Remove(servicePhotoTitle);
            await db.SaveChangesAsync();
            return RedirectToAction("Index" , "CompanyServices");
        }



        //upload async action
        public ActionResult AsyncUpload()
        {
            return View();
        }

        //upload async action post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AsyncUpload(IEnumerable<HttpPostedFileBase> files)
        {
            PhotoFileManager manager = new PhotoFileManager(files, PhotoManagerType.ServiceTitle);
            return manager.Upload(false, null);
        }

        public ActionResult AsyncDelete()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AsyncDelete(string deleteFileName)
        {
            PhotoFileManager manager = new PhotoFileManager(PhotoManagerType.ServiceTitle);

            return manager.Delete(deleteFileName,false);
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
