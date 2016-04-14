using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using System.Reflection;

namespace JsonzaiWithEmit
{
    class EmitUtils
    {
        public static ModuleBuilder CreateModuleBuilder(AssemblyName aName,out AssemblyBuilder ab)
        {
            ab = AppDomain.CurrentDomain.DefineDynamicAssembly(
                    aName,
                    AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder mb = ab.DefineDynamicModule(aName.Name, aName.Name + ".dll");
            return mb;
        }
    }
}
