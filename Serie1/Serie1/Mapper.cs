using System;
using System.Reflection;

namespace Serie1
{
    abstract class Mapper
    {
        public abstract MemberInfo[] m(object target);
        public abstract object GetInfoMember(object instance, MemberInfo m);
    }

    class Fields : Mapper
    {
        public override MemberInfo[] m(object target)
        {
            FieldInfo [] fields = target.GetType().GetFields();
            return fields;
        }

        public override object GetInfoMember(object instance, MemberInfo m)
        {
            FieldInfo f = (FieldInfo)m;
            return f.GetValue(instance);
        }
    }

    class Properties : Mapper
    {
        public override MemberInfo[] m(object target)
        {
            PropertyInfo[] props = target.GetType().GetProperties();
            return props;
        }

        public override object GetInfoMember(object instance, MemberInfo m)
        {
            PropertyInfo prop = (PropertyInfo)m;
            return prop.GetValue(instance);
        }
    }

    class Events : Mapper
    {
        public override MemberInfo[] m(object target)
        {
            EventInfo [] events = target.GetType().GetEvents();
            return events;
        }

        public override object GetInfoMember(object instance, MemberInfo m)
        {
            EventInfo eventt = (EventInfo) m;
            return eventt.EventHandlerType.ToString();
        }
    }

    class Methods : Mapper
    {
        public override MemberInfo[] m(object target)
        {
            MethodInfo [] methods = target.GetType().GetMethods();
            return methods;
        }

        public override object GetInfoMember(object instance, MemberInfo m)
        {
            MethodInfo method = (MethodInfo)m;
            return method.ReturnType.ToString();
        }
    }
}
