using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fanzin.Web.Models
{
    public class HomePageModel
    {
        public IEnumerable<Fanzin.Entities.Slide> Slides { get; set; }
        public IEnumerable<Fanzin.Entities.CompanyService> Services { get; set; }
        public IEnumerable<Fanzin.Entities.AboutUs> AboutUses { get; set; }
        public Fanzin.Entities.ContactUs ContactUS { get; set; }
        public IEnumerable<Fanzin.Entities.CompanyMember> Members { get; set; }

    }
}