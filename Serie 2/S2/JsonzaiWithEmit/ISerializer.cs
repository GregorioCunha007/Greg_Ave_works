using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonzaiWithEmit
{
    public interface ISerializer
    {
         string Serialize(object target);
    }
}
