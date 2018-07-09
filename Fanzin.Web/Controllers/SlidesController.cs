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
    public class SlidesController : Controller
    {
        private SliderContextDb db = new SliderContextDb();

        // GET: Slides
        public async Task<ActionResult> Index()
        {
            var slides = db.Slides.Include(s => s.SlidePhoto);
            return View(await slides.ToListAsync());
        }

        // GET: Slides/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slide slide = await db.Slides.FindAsync(id);
            if (slide == null)
            {
                return HttpNotFound();
            }
            return View(slide);
        }

        // GET: Slides/Create
        public ActionResult Create()
        {
            ViewBag.SlideId = new SelectList(db.SlidePhotos, "SlidePhotoId", "FileName");
            return View();
        }

        // POST: Slides/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SlideId,Title,Description")] Slide slide)
        {
            if (ModelState.IsValid)
            {
                db.Slides.Add(slide);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.SlideId = new SelectList(db.SlidePhotos, "SlidePhotoId", "FileName", slide.SlideId);
            return View(slide);
        }

        // GET: Slides/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slide slide = await db.Slides.FindAsync(id);
            if (slide == null)
            {
                return HttpNotFound();
            }
            ViewBag.SlideId = new SelectList(db.SlidePhotos, "SlidePhotoId", "FileName", slide.SlideId);
            return View(slide);
        }

        // POST: Slides/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SlideId,Title,Description")] Slide slide)
        {
            if (ModelState.IsValid)
            {
                db.Entry(slide).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SlideId = new SelectList(db.SlidePhotos, "SlidePhotoId", "FileName", slide.SlideId);
            return View(slide);
        }

        // GET: Slides/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slide slide = await db.Slides.FindAsync(id);
            if (slide == null)
            {
                return HttpNotFound();
            }
            return View(slide);
        }

        // POST: Slides/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Slide slide = await db.Slides.FindAsync(id);

            //delete slide image file first
            if (slide.SlidePhoto != null)
            {
                PhotoFileManager manager = new PhotoFileManager(PhotoManagerType.Slide);
                manager.Delete(slide.SlidePhoto.FileName, false);

                //delete photo second
                db.SlidePhotos.Remove(slide.SlidePhoto);
            }

            

            //delete slide
            db.Slides.Remove(slide);
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
