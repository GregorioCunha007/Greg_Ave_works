using System;
using System.Reflection;
using System.Reflection.Emit;

namespace JsonzaiWithEmit
{
    class Student
    {
        public string name;
        public int number;

        public Student(string name, int number)
        {
            this.name = name;
            this.number = number;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Student gustafson = new Student("gustavo", 5044);
            //CreateSerializer(gustafson);
            LoadSerializer(gustafson);

        }

        static void CreateSerializer(object target)
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

            /* Cast the target object to a Specified type*/
            mthdIL.Emit(OpCodes.Ldarg_1);
            mthdIL.Emit(OpCodes.Castclass, target.GetType());
            mthdIL.Emit(OpCodes.Stloc_0);

            FieldInfo [] fields = target.GetType().GetFields();
            int size = fields.Length;

            mthdIL.Emit(OpCodes.Ldloc_1, size * 3 ); // + Nomes + \n por cada 
            mthdIL.Emit(OpCodes.Newarr, typeof(string));
            mthdIL.Emit(OpCodes.Stloc_2);
            int counter = 0;

            foreach( FieldInfo f in fields)
            {
                /* Get the field name*/
                mthdIL.Emit(OpCodes.Ldloc_2);
                mthdIL.Emit(OpCodes.Ldc_I4_S, counter++);
                mthdIL.Emit(OpCodes.Ldstr, f.Name + ":");
                mthdIL.Emit(OpCodes.Stelem_Ref);

                /* Get the field value*/
                mthdIL.Emit(OpCodes.Ldloc_2);
                mthdIL.Emit(OpCodes.Ldc_I4_S, counter++);
                mthdIL.Emit(OpCodes.Ldloc_0);
                mthdIL.Emit(OpCodes.Ldfld, f);

                // If int we gotta box
                Type tt = f.GetValue(target).GetType();
                if ( tt.IsPrimitive && tt != typeof(string))
                {
                    mthdIL.Emit(OpCodes.Box,tt);
                }
                mthdIL.Emit(OpCodes.Stelem_Ref);

                /* Add the \n */
                mthdIL.Emit(OpCodes.Ldloc_2);
                mthdIL.Emit(OpCodes.Ldc_I4_S, counter++);
                mthdIL.Emit(OpCodes.Ldc_I4_S, 10);
                mthdIL.Emit(OpCodes.Box,typeof(int));
                mthdIL.Emit(OpCodes.Stelem_Ref);

            }

            mthdIL.Emit(OpCodes.Ldloc_2);
            mthdIL.Emit(OpCodes.Call, typeof(System.String).GetMethod("Concat", new Type[1] { typeof(object) }));
            mthdIL.Emit(OpCodes.Stloc_1);
            mthdIL.Emit(OpCodes.Ldloc_1);
            mthdIL.Emit(OpCodes.Ret);

            Type t = tb.CreateType();

            object demo = Activator.CreateInstance(t);

            abuilder.Save(name.Name + ".dll");

        }

        static void LoadSerializer(object target)
        {
           Assembly loadedAsm = Assembly.
           LoadFrom(@"C:\Users\Pedro\Desktop\1\Isel\6 SEMESTRE\AVE\Greg_Ave_works\Serie 2\S2\JsonzaiWithEmit\bin\Debug\Serializer" + target.GetType().Name + ".dll");

            Type[] types = loadedAsm.GetTypes();

            object obj = Activator.CreateInstance(types[0]);

            MethodInfo[] methodsInDll = obj.GetType().GetMethods();
            methodsInDll[0].Invoke(obj, new object[] { target });
            
        }
    }
}
