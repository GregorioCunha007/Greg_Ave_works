using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JsonzaiWithEmit
{
    [TestClass]
    class JsonzaiWithEmitTest
    {

        [TestMethod]
        public void TestJsonzaiWithEmitStudent()
        {
            Student student1 = new Student(40622, "Joao Carlos");

            /* Creates serializer and calls method */
            string expected_fields = "{\n\"student_number\":40622,\n\"student_name\":\"Joao Carlos\",\n\"disciplines\":[\"Ave\",\"SO\",\"Mpd\",\"Redes\",\"LS\"]\n}\n";
            string result_1 = Program.GetSerializer(student1).Serialize(student1);
            Assert.AreEqual(expected_fields, result_1);

            /* Didn't create serializer since it already existed on the SerializerLib */
            string result_2 = Program.GetSerializer(student1).Serialize(student1);
            Assert.AreEqual(expected_fields, result_2);
        }
    }

    class Program
    {

        static SerializerLib lib = new SerializerLib();

        static void Main(string[] args)
        {

            JsonzaiWithEmitTest testing = new JsonzaiWithEmitTest();

            testing.TestJsonzaiWithEmitStudent();
            Console.WriteLine("All test running smoothly ");
        }

        public static ISerializer GetSerializer(object target)
        {
            ISerializer embryo;
            if (!lib.Exists(target))
            {
                embryo = CreateSerializer(target);
                lib.Add(embryo);
            }
            else
            {
                embryo = LoadSerializer(target);
            }
            return embryo;
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

        static ISerializer LoadSerializer(object target)
        {
            Assembly loadedAsm = Assembly.
           LoadFrom(@"C:\Users\Tozé\Documents\ISEL\AVE\Greg_Ave_works\Serie 2\S2\JsonzaiWithEmit\bin\Debug\Serializer" + target.GetType().Name + ".dll");

            Type[] types = loadedAsm.GetTypes();

            object obj = Activator.CreateInstance(types[0]);
            return (ISerializer)obj;
        }


        static void BuildIL(ILGenerator mthdIL, object target)
        {
            /* Defining the types */
            mthdIL.DeclareLocal(target.GetType());
            mthdIL.DeclareLocal(typeof(StringBuilder));
            mthdIL.DeclareLocal(typeof(string));

            mthdIL.Emit(OpCodes.Newobj, typeof(StringBuilder).GetConstructor(new Type[] { }));
            mthdIL.Emit(OpCodes.Stloc_1);

            StartObj(mthdIL);

            NextLine(mthdIL);

            BuildObject(mthdIL, target);

            EndObj(mthdIL);

            NextLine(mthdIL);

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

            FieldInfo[] fields = target.GetType().GetFields();
            int count = 0;
            foreach (FieldInfo f in fields)
            {
                
                /* Get the field name*/
                FieldName(mthdIL, f);

                /* Call ToJson */
                mthdIL.Emit(OpCodes.Ldloc_1);   // StringBuilder
                mthdIL.Emit(OpCodes.Ldloc_0);   //Object
                mthdIL.Emit(OpCodes.Ldfld, f);
                if (f.GetValue(target).GetType().IsPrimitive)
                {
                    mthdIL.Emit(OpCodes.Box, f.GetValue(target).GetType());
                }
                mthdIL.Emit(OpCodes.Call, typeof(Serie1.Jsonfier).GetMethod("ToJson", new Type[] { typeof(object) }));
                mthdIL.Emit(OpCodes.Callvirt, typeof(StringBuilder).GetMethod("Append", new Type[] { typeof(string) }));
                mthdIL.Emit(OpCodes.Pop);

                mthdIL.Emit(OpCodes.Call, typeof(Serie1.Jsonfier).GetMethod("Reset", new Type[] { }));

                /* Add the ','(comma) and \n */
                if (count < fields.Length-1)
                {
                    Comma(mthdIL);
                }
                NextLine(mthdIL);
                ++count;
            }

            
        }

        static void StartObj(ILGenerator mthdIL)
        {
            mthdIL.Emit(OpCodes.Ldloc_1);
            mthdIL.Emit(OpCodes.Ldstr, "{");
            mthdIL.Emit(OpCodes.Callvirt, typeof(StringBuilder).GetMethod("Append", new Type[] { typeof(string) }));
            mthdIL.Emit(OpCodes.Pop);
        }

        static void EndObj(ILGenerator mthdIL)
        {
            mthdIL.Emit(OpCodes.Ldloc_1);
            mthdIL.Emit(OpCodes.Ldstr, "}");
            mthdIL.Emit(OpCodes.Callvirt, typeof(StringBuilder).GetMethod("Append", new Type[] { typeof(string) }));
            mthdIL.Emit(OpCodes.Pop);
        }

        static void NextLine(ILGenerator mthdIL)
        {
            mthdIL.Emit(OpCodes.Ldloc_1);
            mthdIL.Emit(OpCodes.Ldc_I4_S, 10);
            mthdIL.Emit(OpCodes.Callvirt, typeof(StringBuilder).GetMethod("Append", new Type[] { typeof(char) }));
            mthdIL.Emit(OpCodes.Pop);
        }

        static void Comma(ILGenerator mthdIL)
        {
            mthdIL.Emit(OpCodes.Ldloc_1);
            mthdIL.Emit(OpCodes.Ldc_I4_S, ',');
            mthdIL.Emit(OpCodes.Callvirt, typeof(StringBuilder).GetMethod("Append", new Type[] { typeof(char) }));
            mthdIL.Emit(OpCodes.Pop);
        }

        static void FieldName(ILGenerator mthdIL, FieldInfo f)
        {
            mthdIL.Emit(OpCodes.Ldloc_1);
            mthdIL.Emit(OpCodes.Ldstr, "\"" + f.Name + "\"" + ":");
            mthdIL.Emit(OpCodes.Callvirt, typeof(StringBuilder).GetMethod("Append", new Type[] { typeof(string) }));
            mthdIL.Emit(OpCodes.Pop);
        }
    }
}
    