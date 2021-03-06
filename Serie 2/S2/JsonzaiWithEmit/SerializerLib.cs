﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonzaiWithEmit
{
    class SerializerLib
    {
      private LinkedList<ISerializer> serializers = new LinkedList<ISerializer>();

        public void Add(ISerializer serializer)
        {
            serializers.AddLast(serializer);
        }

        public int Size()
        {
            return serializers.Count;
        }

        public bool Exists(object target)
        {
            foreach(ISerializer s in serializers)
            {
                if (s.GetType().FullName.Equals(target.GetType().FullName + "Serializer"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
