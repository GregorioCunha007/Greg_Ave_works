using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISerializerInterface;
using System.Reflection;
using System.Reflection.Emit;

namespace JsonzaiWithEmit
{

    class Program
    {
        static void Main(string[] args)
        {
        }

        static void CreateSerializer(object target)
        {
            AssemblyBuilder abuilder;
            AssemblyName name = new AssemblyName("Serializer" + target.GetType().Name);
            ModuleBuilder mb = EmitUtils.CreateModuleBuilder(name, out abuilder);

            TypeBuilder tb = mb.DefineType(target.GetType().Name  + "Serializer");
            tb.AddInterfaceImplementation(typeof(ISerializer));

            FieldInfo[] fields = target.GetType().GetFields();

            for( )
        }

        static void LoadSerializer(object target)
        {
           Assembly loadedAsm = Assembly.
           LoadFrom(@"C:\Users\Pedro\Desktop\1\Isel\6 SEMESTRE\AVE\Greg_Ave_works\Serie 2\S2\JsonzaiWithEmit\bin\Debug\Serializer" + target.GetType().Name + ".dll");

            Type[] types = loadedAsm.GetTypes();

            object obj = Activator.CreateInstance(types[0]);

            MethodInfo[] methodsInDll = obj.GetType().GetMethods();

            foreach (MethodInfo m in methodsInDll)
            {
                m.Invoke(obj, new object[] { });
            }
        }
    }
}
