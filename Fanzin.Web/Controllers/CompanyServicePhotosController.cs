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
    public class CompanyServicePhotosController : Controller
    {
        private CompanyServiceContextDb db = new CompanyServiceContextDb();

        // GET: CompanyServicePhotos
        public async Task<ActionResult> Index(int id)
        {
            ViewBag.CompanyServiceId = id;
            return View(await db.CompanyServicePhotos.ToListAsync());
        }

        // GET: CompanyServicePhotos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyServicePhoto companyServicePhoto = await db.CompanyServicePhotos.FindAsync(id);
            if (companyServicePhoto == null)
            {
                return HttpNotFound();
            }
            return View(companyServicePhoto);
        }

        // GET: CompanyServicePhotos/Create
        public ActionResult Create(int id)
        {
            ViewBag.CompanyServiceId = id;
            return View();
        }

        // POST: CompanyServicePhotos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CompanyServicePhotoId,FileName,Title")] CompanyServicePhoto companyServicePhoto , int CompanyServiceId)
        {
            if (ModelState.IsValid)
            {
                companyServicePhoto.CompanyService = db.CompanyServices.Find(CompanyServiceId);
                //companyServicePhoto.CompanyService.CompanyServiceId = CompanyServiceId;
                db.CompanyServicePhotos.Add(companyServicePhoto);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { id = CompanyServiceId });
            }

            return View(companyServicePhoto);
        }

        // GET: CompanyServicePhotos/Edit/5
        public async Task<ActionResult> Edit(int? id, int companyServiceId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyServicePhoto companyServicePhoto = await db.CompanyServicePhotos.FindAsync(id);
            if (companyServicePhoto == null)
            {
                return HttpNotFound();
            }
            ViewBag.companyServiceId = companyServiceId;
            return View(companyServicePhoto);
        }

        // POST: CompanyServicePhotos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CompanyServicePhotoId,FileName,Title")] CompanyServicePhoto companyServicePhoto, string companyservicesID)
        {
            if (ModelState.IsValid)
            {
                db.Entry(companyServicePhoto).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "CompanyServicePhotos", new { id = companyservicesID });
            }
            return View(companyServicePhoto);
        }

        // GET: CompanyServicePhotos/Delete/5
        public async Task<ActionResult> Delete(int? id, int companyServiceId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyServicePhoto companyServicePhoto = await db.CompanyServicePhotos.FindAsync(id);
            if (companyServicePhoto == null)
            {
                return HttpNotFound();
            }
            ViewBag.companyServiceId = companyServiceId;
            return View(companyServicePhoto);
        }

        // POST: CompanyServicePhotos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id, string deleteFileName, string companyservicesID)
        {
            //delete image file with thumbnail
            PhotoFileManager manager = new PhotoFileManager(PhotoManagerType.Service);
            manager.Delete(deleteFileName, true);

            CompanyServicePhoto companyServicePhoto = await db.CompanyServicePhotos.FindAsync(id);
            db.CompanyServicePhotos.Remove(companyServicePhoto);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "CompanyServicePhotos", new { id = companyservicesID });
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
            PhotoFileManager manager = new PhotoFileManager(files, PhotoManagerType.Service);
            return manager.Upload(true, 300);
        }

        public ActionResult AsyncDelete()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AsyncDelete(string deleteFileName)
        {
            PhotoFileManager manager = new PhotoFileManager(PhotoManagerType.Service);

            return manager.Delete(deleteFileName, true);
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
