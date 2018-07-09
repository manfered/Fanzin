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
    public class CompanyMembersController : Controller
    {
        private CompanyMemberContextDb db = new CompanyMemberContextDb();

        // GET: CompanyMembers
        public async Task<ActionResult> Index()
        {
            var companyMembers = db.CompanyMembers.Include(c => c.CompanyMemberPhoto);
            return View(await companyMembers.ToListAsync());
        }

        // GET: CompanyMembers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyMember companyMember = await db.CompanyMembers.FindAsync(id);
            if (companyMember == null)
            {
                return HttpNotFound();
            }
            return View(companyMember);
        }

        // GET: CompanyMembers/Create
        public ActionResult Create()
        {
            ViewBag.CompanyMemberId = new SelectList(db.CompanyMemberPhotoes, "CompanyMemberPhotoId", "FileName");
            return View();
        }

        // POST: CompanyMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CompanyMemberId,Name,JobTitle,Message,ContactURL")] CompanyMember companyMember)
        {
            if (ModelState.IsValid)
            {
                db.CompanyMembers.Add(companyMember);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CompanyMemberId = new SelectList(db.CompanyMemberPhotoes, "CompanyMemberPhotoId", "FileName", companyMember.CompanyMemberId);
            return View(companyMember);
        }

        // GET: CompanyMembers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyMember companyMember = await db.CompanyMembers.FindAsync(id);
            if (companyMember == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyMemberId = new SelectList(db.CompanyMemberPhotoes, "CompanyMemberPhotoId", "FileName", companyMember.CompanyMemberId);
            return View(companyMember);
        }

        // POST: CompanyMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CompanyMemberId,Name,JobTitle,Message,ContactURL")] CompanyMember companyMember)
        {
            if (ModelState.IsValid)
            {
                db.Entry(companyMember).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyMemberId = new SelectList(db.CompanyMemberPhotoes, "CompanyMemberPhotoId", "FileName", companyMember.CompanyMemberId);
            return View(companyMember);
        }

        // GET: CompanyMembers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyMember companyMember = await db.CompanyMembers.FindAsync(id);
            if (companyMember == null)
            {
                return HttpNotFound();
            }
            return View(companyMember);
        }

        // POST: CompanyMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CompanyMember companyMember = await db.CompanyMembers.FindAsync(id);

            //delete member profile photo first if existed
            if (companyMember.CompanyMemberPhoto != null)
            {
                PhotoFileManager manager = new PhotoFileManager(PhotoManagerType.Member);
                manager.Delete(companyMember.CompanyMemberPhoto.FileName, false);

                //delete photo second
                db.CompanyMemberPhotoes.Remove(companyMember.CompanyMemberPhoto);
            }


            db.CompanyMembers.Remove(companyMember);
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
