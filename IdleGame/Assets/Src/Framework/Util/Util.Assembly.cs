using System;
using System.Collections.Generic;
using System.Reflection;

namespace GameFrameWork.Util
{
    public static class AssemblyTool
    {
        private static readonly Assembly Assemblies = null;

        static AssemblyTool()
        {
            Assemblies = Assembly.GetExecutingAssembly();
        }

        public static Assembly GetAssembles()
        {
            return Assemblies;
        }
        public static Type[] FindTypeBase(Type baseType)
        {
            List<Type> result = new List<Type>();


            Type[] types = Assemblies.GetTypes();
            for (int j = 0; j < types.Length; j++)
            {
                var type = types[j];
                if (type.IsClass && !type.IsAbstract && baseType.IsAssignableFrom(type))
                {
                    result.Add(type);
                }
            }
            
            return result.ToArray();
        }
    }
}