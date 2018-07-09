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

namespace Fanzin.Web.Controllers
{
    public class ServicesController : Controller
    {
        private CompanyServiceContextDb db = new CompanyServiceContextDb();

        // GET: Services
        public async Task<ActionResult> Index(int? id)
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
