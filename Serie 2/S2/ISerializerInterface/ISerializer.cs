using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISerializerInterface
{
    public interface ISerializer
    {
        string Serialize(object target);
    }
}
