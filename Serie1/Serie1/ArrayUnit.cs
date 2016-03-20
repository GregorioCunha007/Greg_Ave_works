using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Serie1
{
    class ArrayUnit : Unit
    {

        private Array contained;

        public ArrayUnit(object unit)
        {
            contained = (Array) unit;
        }

        public override string ToString()
        {

            StringBuilder representation = new StringBuilder();
            representation.Append("[ ");
            foreach(object o in contained)
            {
                
                if(o.GetType().IsPrimitive || o.GetType() == typeof(string))
                {
                    representation.Append(o + ",");
                }
                else if(o.GetType().IsArray)
                {
                    representation.Append(new ArrayUnit(o).ToString() + ",");
                }
                else if(o.GetType().IsAssignableFrom(typeof(object)))
                {
                    representation.Append(new ObjectUnit(o).ToString()+ ",");
                }
              
            }
            representation.Remove(representation.Length - 1,1);
            representation.Append("]");

            return representation.ToString();
        }

        private void GatherAllData()
        {
            methods = contained.GetType().GetMethods();
            fields = contained.GetType().GetFields();
            events = contained.GetType().GetEvents();
            properties = contained.GetType().GetProperties();
        }

    }
}
