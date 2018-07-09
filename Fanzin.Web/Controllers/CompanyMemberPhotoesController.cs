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
    public class CompanyMemberPhotoesController : Controller
    {
        private CompanyMemberContextDb db = new CompanyMemberContextDb();

        // GET: CompanyMemberPhotoes
        public async Task<ActionResult> Index()
        {
            var companyMemberPhotoes = db.CompanyMemberPhotoes.Include(c => c.CompanyMember);
            return View(await companyMemberPhotoes.ToListAsync());
        }

        // GET: CompanyMemberPhotoes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyMemberPhoto companyMemberPhoto = await db.CompanyMemberPhotoes.FindAsync(id);
            if (companyMemberPhoto == null)
            {
                return HttpNotFound();
            }
            return View(companyMemberPhoto);
        }

        // GET: CompanyMemberPhotoes/Create
        public ActionResult Create(int id)
        {
            //ViewBag.CompanyMemberPhotoId = new SelectList(db.CompanyMembers, "CompanyMemberId", "Name");
            ViewBag.CompanyMemberPhotoId = id;
            return View();
        }

        // POST: CompanyMemberPhotoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CompanyMemberPhotoId,FileName")] CompanyMemberPhoto companyMemberPhoto)
        {
            if (ModelState.IsValid)
            {
                db.CompanyMemberPhotoes.Add(companyMemberPhoto);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "CompanyMembers");
            }

            ViewBag.CompanyMemberPhotoId = new SelectList(db.CompanyMembers, "CompanyMemberId", "Name", companyMemberPhoto.CompanyMemberPhotoId);
            return View(companyMemberPhoto);
        }

        // GET: CompanyMemberPhotoes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyMemberPhoto companyMemberPhoto = await db.CompanyMemberPhotoes.FindAsync(id);
            if (companyMemberPhoto == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyMemberPhotoId = new SelectList(db.CompanyMembers, "CompanyMemberId", "Name", companyMemberPhoto.CompanyMemberPhotoId);
            return View(companyMemberPhoto);
        }

        // POST: CompanyMemberPhotoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CompanyMemberPhotoId,FileName")] CompanyMemberPhoto companyMemberPhoto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(companyMemberPhoto).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "CompanyMembers");
            }
            ViewBag.CompanyMemberPhotoId = new SelectList(db.CompanyMembers, "CompanyMemberId", "Name", companyMemberPhoto.CompanyMemberPhotoId);
            return View(companyMemberPhoto);
        }

        // GET: CompanyMemberPhotoes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyMemberPhoto companyMemberPhoto = await db.CompanyMemberPhotoes.FindAsync(id);
            if (companyMemberPhoto == null)
            {
                return HttpNotFound();
            }
            return View(companyMemberPhoto);
        }

        // POST: CompanyMemberPhotoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id, string deleteFileName)
        {
            PhotoFileManager manager = new PhotoFileManager(PhotoManagerType.Member);
            manager.Delete(deleteFileName, false);

            CompanyMemberPhoto companyMemberPhoto = await db.CompanyMemberPhotoes.FindAsync(id);
            db.CompanyMemberPhotoes.Remove(companyMemberPhoto);
            await db.SaveChangesAsync();
            return RedirectToAction("Index" , "CompanyMembers");
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
            PhotoFileManager manager = new PhotoFileManager(files, PhotoManagerType.Member);
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
            PhotoFileManager manager = new PhotoFileManager(PhotoManagerType.Member);

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
