using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanzin.Entities
{
    public class CompanyMemberPhoto
    {
        [ForeignKey("CompanyMember")]
        public int CompanyMemberPhotoId { get; set; }
        [Required]
        [StringLength(255)]
        public string FileName { get; set; }
        
        public virtual CompanyMember CompanyMember { get; set; }
    }
}
