using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Serie1
{
    class ObjectUnit : Unit
    {
        private object contained;

        public ObjectUnit(object unit)
        {
            contained = unit;
        }

        public override string ToString()
        {

            GatherAllData();
            StringBuilder representation = new StringBuilder();
            representation.Append("{ ");
            foreach (FieldInfo f in fields)
            {

                if (f.FieldType.IsPrimitive || f.FieldType == typeof(string))
                {
                    representation.Append(f.Name + ":" + f.GetValue(contained));
                }
                else if (f.FieldType.IsArray)
                {
                    new ArrayUnit(f.GetValue(contained));
                }
                else if (f.FieldType.IsAssignableFrom(typeof(object)))
                {
                    new ObjectUnit(f.GetValue(contained));
                }

            }
            representation.Append("}");

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
