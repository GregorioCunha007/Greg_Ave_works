using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Serie1
{
    abstract class Unit
    {

        public MethodInfo[] methods;
        public EventInfo[] events;
        public FieldInfo[] fields;
        public PropertyInfo[] properties;

        public Unit()
        {
            // Nothing to do
        }
    }
}
