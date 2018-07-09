using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanzin.Entities
{
    public interface IPhoto
    {
        string Title { get; set; }
        string FileName { get; set; }
    }
}
