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
using PagedList;

namespace Fanzin.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class ContactUsController : Controller
    {
        private ContactUsContextDb db = new ContactUsContextDb();

        // GET: ContactUs
        //public async Task<ActionResult> Index()
        //{
        //    return View(await db.ContactUses.ToListAsync());
        //}

        public ActionResult Index(int page = 1)
        {
            var model = db.ContactUses.OrderByDescending( c => c.SubmitDateTime).ToPagedList(page, 10);
            return View(model);
        }

        // GET: ContactUs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactUs contactUs = await db.ContactUses.FindAsync(id);
            if (contactUs == null)
            {
                return HttpNotFound();
            }

            //change state of message from unread to read
            contactUs.Read = true;
            db.Entry(contactUs).State = EntityState.Modified;
            await db.SaveChangesAsync();
            
            return View(contactUs);
        }

        // GET: ContactUs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContactUs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Create([Bind(Include = "ContactUsId,Name,Email,Tel,Message,")] ContactUs contactUs)
        {
            if (ModelState.IsValid)
            {
                //default values
                contactUs.Read = false;

                //setting the time based on IR time zone
                var info = TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time");
                DateTimeOffset localServerTime = DateTimeOffset.Now;
                DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
                contactUs.SubmitDateTime = localTime.DateTime;


                db.ContactUses.Add(contactUs);
                await db.SaveChangesAsync();
                //return RedirectToAction("Index");
                return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);
            }

            //return View(contactUs);
            return Json(new { status = "Failed" }, JsonRequestBehavior.AllowGet);
        }

        // GET: ContactUs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactUs contactUs = await db.ContactUses.FindAsync(id);
            if (contactUs == null)
            {
                return HttpNotFound();
            }
            return View(contactUs);
        }

        // POST: ContactUs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ContactUsId,Name,Email,Tel,Message,Read,SubmitDateTime")] ContactUs contactUs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contactUs).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(contactUs);
        }

        // GET: ContactUs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactUs contactUs = await db.ContactUses.FindAsync(id);
            if (contactUs == null)
            {
                return HttpNotFound();
            }
            return View(contactUs);
        }

        // POST: ContactUs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ContactUs contactUs = await db.ContactUses.FindAsync(id);
            db.ContactUses.Remove(contactUs);
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
