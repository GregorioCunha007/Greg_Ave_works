using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections;
using System.Text;

namespace JsonzaiWithEmit
{
    class Program
    {
        static void Main(string[] args)
        {
            Student gustafson = new Student(40641, "Toze");
            ISerializer k = CreateSerializer(gustafson);
            //LoadSerializer(gustafson);
            string xpto = k.Serialize(gustafson);
            int breaker = 0;

        }

        static ISerializer CreateSerializer(object target)
        {
            AssemblyBuilder abuilder;
            AssemblyName name = new AssemblyName("Serializer" + target.GetType().Name);
            ModuleBuilder mb = EmitUtils.CreateModuleBuilder(name, out abuilder);

            TypeBuilder tb = mb.DefineType(target.GetType().Name  + "Serializer");
            tb.AddInterfaceImplementation(typeof(ISerializer));

            MethodBuilder mthd = tb.DefineMethod(
                "Serialize",
                MethodAttributes.Public | MethodAttributes.Virtual,
                typeof(string),
                new Type[] { typeof(object) });

            ILGenerator mthdIL = mthd.GetILGenerator();

            BuildIL(mthdIL,target);

            Type t = tb.CreateType();

            object demo = Activator.CreateInstance(t);

            abuilder.Save(name.Name + ".dll");

            return (ISerializer)demo;

        }

        static void LoadSerializer(object target)
        {
           Assembly loadedAsm = Assembly.
           LoadFrom(@"C:\Users\Tozé\Documents\ISEL\AVE\Greg_Ave_works\Serie 2\S2\JsonzaiWithEmit\bin\Debug\Serializer" + target.GetType().Name + ".dll");

            Type[] types = loadedAsm.GetTypes();

            object obj = Activator.CreateInstance(types[0]);

            MethodInfo[] methodsInDll = obj.GetType().GetMethods();
            methodsInDll[0].Invoke(obj, new object[] { target });
            
        }


        static void BuildIL(ILGenerator mthdIL, object target)
        {
            /* Defining the types */
            mthdIL.DeclareLocal(target.GetType());
            mthdIL.DeclareLocal(typeof(StringBuilder));
            mthdIL.DeclareLocal(typeof(string));

            mthdIL.Emit(OpCodes.Newobj, typeof(StringBuilder).GetConstructor(new Type[] { }));
            mthdIL.Emit(OpCodes.Stloc_1);

            mthdIL.Emit(OpCodes.Ldloc_1);
            mthdIL.Emit(OpCodes.Ldstr, "{");
            mthdIL.Emit(OpCodes.Callvirt, typeof(StringBuilder).GetMethod("Append", new Type[] { typeof(string) }));
            mthdIL.Emit(OpCodes.Pop);

            BuildObject(mthdIL, target);

            mthdIL.Emit(OpCodes.Ldloc_1);
            mthdIL.Emit(OpCodes.Ldstr, "}");
            mthdIL.Emit(OpCodes.Callvirt, typeof(StringBuilder).GetMethod("Append", new Type[] { typeof(string) }));
            mthdIL.Emit(OpCodes.Pop);

            mthdIL.Emit(OpCodes.Ldloc_1);
            mthdIL.Emit(OpCodes.Callvirt, typeof(StringBuilder).GetMethod("ToString", new Type[] { }));
            mthdIL.Emit(OpCodes.Stloc_2);
            mthdIL.Emit(OpCodes.Ldloc_2);
            mthdIL.Emit(OpCodes.Ret);
        }

        static void BuildObject(ILGenerator mthdIL, object target)
        {
            /* Cast the target object to a Specified type*/
            mthdIL.Emit(OpCodes.Ldarg_1);
            mthdIL.Emit(OpCodes.Castclass, target.GetType());
            mthdIL.Emit(OpCodes.Stloc_0);

            if (target.GetType().IsArray || target.GetType() == typeof(string[]))
            {
                mthdIL.Emit(OpCodes.Ldloc_1);
                mthdIL.Emit(OpCodes.Ldstr, "[");
                mthdIL.Emit(OpCodes.Callvirt, typeof(StringBuilder).GetMethod("Append", new Type[] { typeof(string) }));
                mthdIL.Emit(OpCodes.Pop);
                IEnumerable seq = (IEnumerable)target;
                IEnumerator it = seq.GetEnumerator();
                while (it.MoveNext())
                {
                    if (it.Current.GetType().IsPrimitive)
                    {
                        mthdIL.Emit(OpCodes.Ldc_I4, (byte)it.Current);
                        if(it.Current.GetType() != typeof(string))
                        {
                            mthdIL.Emit(OpCodes.Box);
                        }
                        mthdIL.Emit(OpCodes.Callvirt, typeof(StringBuilder).GetMethod("Append", new Type[] { typeof(string) }));
                        mthdIL.Emit(OpCodes.Pop);
                    }
                    else
                    {
                        BuildObject(mthdIL, it.Current);
                    } 
                    mthdIL.Emit(OpCodes.Ldstr, ",");
                    mthdIL.Emit(OpCodes.Callvirt, typeof(StringBuilder).GetMethod("Append", new Type[] { typeof(string) }));
                    mthdIL.Emit(OpCodes.Pop);
                }
                mthdIL.Emit(OpCodes.Ldloc_1);
                mthdIL.Emit(OpCodes.Ldstr, "]");
                mthdIL.Emit(OpCodes.Callvirt, typeof(StringBuilder).GetMethod("Append", new Type[] { typeof(string) }));
                mthdIL.Emit(OpCodes.Pop);
            }
   
            else
            {
                FieldInfo[] fields = target.GetType().GetFields();

                foreach (FieldInfo f in fields)
                {
                    /* Get the field name*/
                    mthdIL.Emit(OpCodes.Ldloc_1);
                    mthdIL.Emit(OpCodes.Ldstr, f.Name + ":");
                    mthdIL.Emit(OpCodes.Callvirt, typeof(StringBuilder).GetMethod("Append", new Type[] { typeof(string) }));
                    mthdIL.Emit(OpCodes.Pop);

                    /* Get the field value*/
                    BuildObject(mthdIL, f.GetValue(target));

                    /* Add the \n */
                    mthdIL.Emit(OpCodes.Ldloc_1);
                    mthdIL.Emit(OpCodes.Ldc_I4_S, 10);
                    mthdIL.Emit(OpCodes.Box, typeof(int));
                    mthdIL.Emit(OpCodes.Callvirt, typeof(StringBuilder).GetMethod("Append", new Type[] { typeof(string) }));
                    mthdIL.Emit(OpCodes.Pop);

                }
            }
        }
    }
}
    