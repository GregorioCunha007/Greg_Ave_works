using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISerializerInterface;

namespace SerializerLibrary
{
    class Serializers
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
    }
}
