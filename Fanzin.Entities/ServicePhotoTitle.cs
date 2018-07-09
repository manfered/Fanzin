using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanzin.Entities
{
    public class ServicePhotoTitle : IPhoto
    {
        [ForeignKey("CompanyService")]
        public int ServicePhotoTitleId { get; set; }
        [Required]
        [StringLength(255)]
        public string FileName { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        public virtual CompanyService CompanyService { get; set; }
    }
}
