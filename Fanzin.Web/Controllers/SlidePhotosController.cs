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
    public class SlidePhotosController : Controller
    {
        private SliderContextDb db = new SliderContextDb();

        // GET: SlidePhotos
        public async Task<ActionResult> Index()
        {
            var slidePhotos = db.SlidePhotos.Include(s => s.Slide);
            return View(await slidePhotos.ToListAsync());
        }

        // GET: SlidePhotos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SlidePhoto slidePhoto = await db.SlidePhotos.FindAsync(id);
            if (slidePhoto == null)
            {
                return HttpNotFound();
            }
            return View(slidePhoto);
        }

        // GET: SlidePhotos/Create
        public ActionResult Create(int id)
        {
            //ViewBag.SlidePhotoId = new SelectList(db.Slides, "SlideId", "Title");
            ViewBag.SlidePhotoId = id;
            return View();
        }

        // POST: SlidePhotos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SlidePhotoId,FileName,Title")] SlidePhoto slidePhoto)
        {
            if (ModelState.IsValid)
            {
                db.SlidePhotos.Add(slidePhoto);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Slides");
            }

            ViewBag.SlidePhotoId = new SelectList(db.Slides, "SlideId", "Title", slidePhoto.SlidePhotoId);
            return View(slidePhoto);
        }

        // GET: SlidePhotos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SlidePhoto slidePhoto = await db.SlidePhotos.FindAsync(id);
            if (slidePhoto == null)
            {
                return HttpNotFound();
            }
            ViewBag.SlidePhotoId = new SelectList(db.Slides, "SlideId", "Title", slidePhoto.SlidePhotoId);
            return View(slidePhoto);
        }

        // POST: SlidePhotos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SlidePhotoId,FileName,Title")] SlidePhoto slidePhoto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(slidePhoto).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Slides");
            }
            ViewBag.SlidePhotoId = new SelectList(db.Slides, "SlideId", "Title", slidePhoto.SlidePhotoId);
            return View(slidePhoto);
        }

        // GET: SlidePhotos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SlidePhoto slidePhoto = await db.SlidePhotos.FindAsync(id);
            if (slidePhoto == null)
            {
                return HttpNotFound();
            }
            return View(slidePhoto);
        }

        // POST: SlidePhotos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id , string deleteFileName)
        {
            PhotoFileManager manager = new PhotoFileManager(PhotoManagerType.Slide);
            manager.Delete(deleteFileName,false);

            SlidePhoto slidePhoto = await db.SlidePhotos.FindAsync(id);
            db.SlidePhotos.Remove(slidePhoto);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Slides");
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
            PhotoFileManager manager = new PhotoFileManager(files, PhotoManagerType.Slide);
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
            PhotoFileManager manager = new PhotoFileManager(PhotoManagerType.Slide);

            return manager.Delete(deleteFileName, false);
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
