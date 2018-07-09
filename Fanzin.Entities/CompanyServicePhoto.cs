using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanzin.Entities
{
    public class CompanyServicePhoto : IPhoto
    {
        public int CompanyServicePhotoId { get; set; }

        [Required]
        [StringLength(255)]
        public string FileName { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        
        public virtual CompanyService CompanyService { get; set; }

    }
}
