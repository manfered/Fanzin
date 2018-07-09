using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Fanzin.Entities
{
    public class CompanyService
    {
        public int CompanyServiceId { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        [StringLength(1000)]
        public string BriefDescription { get; set; }
        [Required]
        [StringLength(10000)]
        [AllowHtml]
        public string FullDescription { get; set; }

        public virtual ServicePhotoTitle ServicePhotoTitle { get; set; }

        public virtual ICollection<CompanyServicePhoto> CompanyServicePhoto { get; set; }
    }
}
