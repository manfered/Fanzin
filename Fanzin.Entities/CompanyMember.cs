using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanzin.Entities
{
    public class CompanyMember
    {
        public int CompanyMemberId { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        [StringLength(255)]
        public string JobTitle { get; set; }
        [Required]
        [StringLength(1000)]
        public string Message { get; set; }
        [Required]
        [StringLength(256)]
        public string ContactURL { get; set; }

        public virtual CompanyMemberPhoto CompanyMemberPhoto { get; set; }
    }
}
